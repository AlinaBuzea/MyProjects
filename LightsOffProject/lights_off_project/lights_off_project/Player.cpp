#include "Player.h"
#include "InputValidation.h"
#include "../Logging/Logging.h"
#include <random>
#include <algorithm>
#include <iostream>
#include <fstream>

Player::Player()
{
	m_optionUser = 1;
}

Player::Player(const Player& player)
{
	m_rule = player.m_rule;
	m_user = player.m_user;
	m_optionUser = player.m_optionUser;
	m_board = player.m_board;
}

Player& Player::operator=(const Player& player)
{
	if (this != &player)
	{
		m_rule = player.m_rule;
		m_user = player.m_user;
		m_optionUser = player.m_optionUser;
		m_board = player.m_board;
	}
	return *this;
}

int Player::RandomNumberGenerator(int start, int end)
{
	std::random_device rd;
	std::mt19937 mt(rd());
	std::uniform_int_distribution<int> dist(start, end);
	return dist(mt);
}

std::vector<std::vector<bool>> Player::CreatingBoardMatrix(std::ostream& output)
{
	Logging logger(output);
	std::string fileName;
	std::vector<std::vector<bool>>matrixBoard;
	int16_t nrOfColumns;
	int16_t nrOfRows;

	nrOfRows = m_rule.getNumberOfRows();
	nrOfColumns = m_rule.getNumberOfColumns();
	m_user.SelectBoardType();
	fileName = m_user.getBoardType();

	std::ifstream file(fileName);
	try {
		if (file.is_open())
		{
			for (int index = 0; index < nrOfRows; index++)
			{
				std::vector<bool>temp;
				for (int index1 = 0; index1 < nrOfColumns; index1++)
				{
					bool temp1;
					file >> temp1;
					temp.push_back(temp1);
				}
				matrixBoard.push_back(temp);
			}
			return matrixBoard;
		}
		else
		{
			throw "\nCouldn't find input file";
		}
	}
	catch (const char* message)
	{
		logger.Log(message, Logging::Level::ERROR);
	}
}

void Player::SetNeighbours(int option)
{
	switch (option)
	{
	case 1:
		m_rule.setHorizontal(true);
		m_rule.setVertical(false);
		m_rule.setMainDiagonal(false);
		m_rule.setSecondaryDiagonal(false);
		break;
	case 2:
		m_rule.setHorizontal(false);
		m_rule.setVertical(true);
		m_rule.setMainDiagonal(false);
		m_rule.setSecondaryDiagonal(false);
		break;
	case 3:
		m_rule.setHorizontal(false);
		m_rule.setVertical(false);
		m_rule.setMainDiagonal(true);
		m_rule.setSecondaryDiagonal(false);
		break;
	case 4:
		m_rule.setHorizontal(false);
		m_rule.setVertical(false);
		m_rule.setMainDiagonal(false);
		m_rule.setSecondaryDiagonal(true);
		break;
	case 5:
		m_rule.setHorizontal(true);
		m_rule.setVertical(true);
		m_rule.setMainDiagonal(false);
		m_rule.setSecondaryDiagonal(false);
		break;
	case 6:
		m_rule.setHorizontal(false);
		m_rule.setVertical(false);
		m_rule.setMainDiagonal(true);
		m_rule.setSecondaryDiagonal(true);
		break;
	case 7:
		m_rule.setHorizontal(true);
		m_rule.setVertical(true);
		m_rule.setMainDiagonal(true);
		m_rule.setSecondaryDiagonal(true);
		break;
	default:
		m_rule.setHorizontal(false);
		m_rule.setVertical(false);
		m_rule.setMainDiagonal(false);
		m_rule.setSecondaryDiagonal(false);
		break;
	}
}

void Player::showRules(const std::string& lightsOff, const std::string& lightsOn)
{
	//Neighbours
	std::cout << "Neighbours: \n";
	if (m_rule.getHorizontal() == true)
		std::cout << "	Horizontal\n";
	if (m_rule.getVertical() == true)
		std::cout << "	Vertical\n";
	if (m_rule.getMainDiagonal() == true)
		std::cout << "	MainDiagonal\n";
	if (m_rule.getSecondaryDiagonal() == true)
		std::cout << "	SecondaryDiagonal\n";

	//board size
	std::cout << "Board size: " << m_rule.getNumberOfRows() << "x" << m_rule.getNumberOfColumns() << "\n";

	//lights color
	std::cout << "Lights color: \n	Off: " << lightsOff << "\n";
	std::cout << "	On: " << lightsOn << "\n\n";
}

void Player::selectFastGame(std::ostream& output)
{
	Logging logger(output);
	int16_t fastModeOption;
	char fastModeOpt;
	std::cout << "\nDo you want to play in fast game mode? Press 0 for NO or 1 for YES\n";
	std::cin >> fastModeOpt;
	fastModeOption = fastModeOpt - '0';

	while (!ValidFastModeOption(fastModeOption))
	{
		std::cout << "Enter a valid option!\n";
		logger.Log("The player chose a non-existent option.", Logging::Level::WARNING);
		std::cin >> fastModeOpt;
		fastModeOption = fastModeOpt - '0';
	}

	if (fastModeOption == 1)
	{
		m_rule.setFastGame(true);
		logger.Log("The player chose the fast game.", Logging::Level::INFO);
	}
	else
	{
		m_rule.setFastGame(false);
		logger.Log("The player chose the normal game.", Logging::Level::INFO);
	}
}

void Player::SetRules(std::ostream& output)
{
	Logging logger(output);
	char optionUser;
	std::cout << "If you would like random rules press 1. \nIf you would like the standard game press 2.\n";
	std::cin >> optionUser;
	m_optionUser = optionUser - '0';

	while (!TestOption(m_optionUser))
	{
		logger.Log("The player chose a non-existent option.", Logging::Level::WARNING);
		std::cin >> optionUser;
		m_optionUser = optionUser - '0';
	}

	if (m_optionUser == 1)
	{
		logger.Log("The player chose random rules to be set.", Logging::Level::INFO);
		//stabilesc regulile random care au optiuni cu boolean
		int optionsRandom;
		optionsRandom = RandomNumberGenerator(0, 7);
		SetNeighbours(optionsRandom);

		//jucatorul stabileste daca vrea sa se joace in mod rapid sau nu
		selectFastGame(output);

		//stabilesc random cate linii si coloane sa aiba boardul din versiunile 3x3, 5x5, 7x7, 9x9
		int16_t numberOfRowsColumnsRandom;
		numberOfRowsColumnsRandom = 2 * (RandomNumberGenerator(0, 3)) + 3;
		m_rule.setNumberOfRows(numberOfRowsColumnsRandom);
		m_rule.setNumberOfColumns(numberOfRowsColumnsRandom);

		//aleg doua colori diferite random pentru becurile aprinse si stinse
		std::vector<std::string>colors = { "blue", "green", "cyan", "red", "magenta", "yellow", "white", "grey", "bright blue", "bright green", "bright cyan", "bright red", "bright magenta", "bright yellow", "bright white" };
		std::shuffle(colors.begin(), colors.end(), std::random_device());
		std::string lightsOffColorRandom = colors.at(0);
		m_rule.setLightsOffColour(lightsOffColorRandom);
		std::shuffle(colors.begin(), colors.end(), std::random_device());
		std::string lightsOnColorRandom = colors.at(0);
		while (!NotSameColor(lightsOffColorRandom, lightsOnColorRandom))
		{
			std::shuffle(colors.begin(), colors.end(), std::random_device());
			lightsOnColorRandom = colors.at(0);
		}
		m_rule.setLightsOnColour(lightsOnColorRandom);

		std::cout << "\nThe rules are as follows: \n\n";
		showRules(lightsOffColorRandom, lightsOnColorRandom);
	}
	else
	{
		logger.Log("The player chose to play the standard game.", Logging::Level::INFO);
		m_rule = Rules(true);
	}
}

void Player::setRules(Rules rule)
{
	m_rule = rule;
}

Rules Player::GetRules()
{
	return m_rule;
}

std::string Player::SelectRandomFile(std::string boardType)
{
	std::string inputFileName;
	int16_t numberOfFiles;
	int16_t randomNumber;
	std::ifstream myfile(boardType);
	myfile >> numberOfFiles;
	randomNumber = RandomNumberGenerator(1, numberOfFiles);
	while (randomNumber > 0)
	{
		myfile >> inputFileName;
		--randomNumber;
	}
	return inputFileName;
}

std::pair<std::string, std::string> Player::GetFileName()
{
	std::string inputFileName;
	std::string boardType;
	std::pair<std::string, std::string>files;

	//creating board_type name
	boardType.append(std::to_string(m_rule.getNumberOfRows()));
	boardType.append("x");
	boardType.append(std::to_string(m_rule.getNumberOfColumns()));
	boardType.append("_");
	if (m_rule.getHorizontal() == true)
		boardType.append("h");
	if (m_rule.getVertical() == true)
		boardType.append("v");
	if (m_rule.getMainDiagonal() == true)
		boardType.append("f");
	if (m_rule.getSecondaryDiagonal() == true)
		boardType.append("s");
	boardType.append(".txt");

	inputFileName = SelectRandomFile(boardType);

	files.first = boardType;
	files.second = inputFileName;
	return files;
}

void Player::SetBoard(std::ostream& output)
{
	Logging logger(output);
	if (m_optionUser == 0)
	{
		int16_t nrOfColumns;
		int16_t nrOfRows;

		nrOfRows = m_rule.getNumberOfRows();
		nrOfColumns = m_rule.getNumberOfColumns();

		Board auxBoard(nrOfRows, nrOfColumns, CreatingBoardMatrix(output));
		m_board = std::move(auxBoard);
		logger.Log("A board has been imported.", Logging::Level::INFO);
	}
	else
	{
		std::pair<std::string, std::string> inputFiles;
		inputFiles = GetFileName();
		m_board.setColumnsNumber(m_rule.getNumberOfColumns());
		m_board.setLinesNumber(m_rule.getNumberOfRows());
		m_board.readBoardFromFile(output, inputFiles.second, inputFiles.first);
		logger.Log("A board has been imported.", Logging::Level::INFO);
	}
}

void Player::setBoard(Board board)
{
	m_board = board;
}

Board Player::GetBoard()
{
	return m_board;
}

void Player::Play(std::optional<std::atomic<int>*> stopGame)
{
	int16_t coordX, coordY;
	char coordXAux, coordYAux;
	while (!m_board.allLightsAreOff())
	{
		if ((stopGame.has_value()) && (*stopGame.value() == 1))
		{
			std::cout << "\nSorry, time's up. Hope you'll be faster next time!\n";
			break;
		}

		std::cout << "Coordinates: \n";
		std::cin >> coordXAux >> coordYAux;
		coordX = coordXAux - '0';
		coordY = coordYAux - '0';
		CoordinateValidation(coordX, coordY, m_board);
		m_board.click(coordX, coordY, m_rule);
	}
}

bool Player::NotSameColor(const std::string& firstColor, const std::string& secondColor)
{
	if (firstColor == secondColor)
		return false;
	return true;
}