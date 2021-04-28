#include "User.h"
#include "InputValidation.h"
#include "../Logging/Logging.h"
#include <array>
#include <algorithm>
#include <fstream>
#include <iostream>
#include <random>

User::User()
{
}

User::User(const User& user)
{
	m_rule = user.m_rule;
	m_username = user.m_username;
	m_rowsNumber = user.m_rowsNumber;
	m_columnsNumber = user.m_columnsNumber;
	m_personalizedMapOption = user.m_personalizedMapOption;

	if (user.m_personalizedMap.size() > 0)
	{
		for (uint16_t index = 0; index < m_rowsNumber; index++)
		{
			std::vector<bool> line;
			for (uint16_t index1 = 0; index1 < m_columnsNumber; index1++)
			{
				line.push_back(user.m_personalizedMap[index][index1]);
			}
			m_personalizedMap.push_back(line);
		}
	}
}

User& User::operator=(const User& user)
{
	if (this != &user)
	{
		m_rule = user.m_rule;
		m_username = user.m_username;
		m_rowsNumber = user.m_rowsNumber;
		m_columnsNumber = user.m_columnsNumber;
		m_personalizedMapOption = user.m_personalizedMapOption;

		if (user.m_personalizedMap.size() > 0)
		{
			for (uint16_t index = 0; index < m_rowsNumber; index++)
			{
				std::vector<bool> line;
				for (uint16_t index1 = 0; index1 < m_columnsNumber; index1++)
				{
					line.push_back(user.m_personalizedMap[index][index1]);
				}
				m_personalizedMap.push_back(line);
			}
		}
	}
	return *this;
}

User::~User()
{
}

void User::setUserName()
{
	std::string username;

	std::cout << "Username requirements:\n";
	std::cout << "   ->Number of characters must be between 5 to 20.\n";
	std::cout << "   ->Can contain lowercase and  uppercase alphanumeric characters.\n";
	std::cout << "   ->Allowed of . - _\n";
	std::cout << "   -> dot, underscore, or hyphen must not be the first or last character and does not appear consecutively\n";
	
	std::cout << "Enter a username:\n";
	std::cin >> username;

	bool ok;
	ok = IsCorrectUsername(username);
	while (ok == false)
	{
		std::getline(std::cin, username);
		ok = IsCorrectUsername(username);
	} 

	m_username = username;
}

std::string User::getUserName()
{
	return m_username;
}

std::string User::SetLight()
{
	bool ok = true;
	std::string lights;
	do {
		std::getline(std::cin, lights);
		ok = IsLightColour(lights);
	} while (ok != true);
	return lights;
}

void User::SetNeighborOption(std::ostream& output, bool& ok)
{
	Logging logger(output);
	int16_t neighbourOption;
	std::string neighbourOpt;
	std::cin >> neighbourOpt;
	while (!IsNumber(neighbourOpt))
	{
		std::cout << "\nThe character you entered is not an option. Please enter a number: ";
		logger.Log(m_username + " did not enter a number.", Logging::Level::WARNING);
		std::cin >> neighbourOpt;
	}
	neighbourOption = std::stoi(neighbourOpt);

	switch (neighbourOption)
	{
	case 1:
	{
		m_rule.setHorizontal(true);
		ok = true;
		m_neighborOptionName = "_h_";
		logger.Log(m_username + " chose option 1 as the neighbourhood.", Logging::Level::INFO);
		break;
	}
	case 2:
	{
		m_rule.setVertical(true);
		m_neighborOptionName = "_v_";
		ok = true;
		logger.Log(m_username + " chose option 2 as the neighbourhood.", Logging::Level::INFO);
		break;
	}
	case 3:
	{
		m_rule.setVertical(true);
		m_rule.setHorizontal(true);
		m_neighborOptionName = "_hv_";
		ok = true;
		logger.Log(m_username + " chose option 3 as the neighbourhood.", Logging::Level::INFO);
		break;
	}
	case 4:
	{
		m_rule.setMainDiagonal(true);
		m_neighborOptionName = "_f_";
		ok = true;
		logger.Log(m_username + " chose option 4 as the neighbourhood.", Logging::Level::INFO);
		break;
	}
	case 5:
	{
		m_rule.setSecondaryDiagonal(true);
		m_neighborOptionName = "_s_";
		ok = true;
		logger.Log(m_username + " chose option 5 as the neighbourhood.", Logging::Level::INFO);
		break;
	}
	case 6:
	{
		m_rule.setVertical(true);
		m_rule.setHorizontal(true);
		m_rule.setMainDiagonal(true);
		m_rule.setSecondaryDiagonal(true);
		m_neighborOptionName = "_hvfs_";
		ok = true;
		logger.Log(m_username + " chose option 6 as the neighbourhood.", Logging::Level::INFO);
		break;
	}
	case 7:
	{
		m_rule.setMainDiagonal(true);
		m_rule.setSecondaryDiagonal(true);
		m_neighborOptionName = "_fs_";
		ok = true;
		logger.Log(m_username + " chose option 7 as the neighbourhood.", Logging::Level::INFO);
		break;
	}
	default:
	{
		std::cout << "The key you entered does not exist!\n";
		logger.Log(m_username + " chose a non-existent option as the neighbourhood.", Logging::Level::WARNING);
		ok = false;
	}
	}
}

void User::SetRowColumnNumber(std::ostream& output, int16_t opt)
{
	Logging logger(output);
	std::int16_t number;
	std::string numberIn;

	if(opt == 1)
		std::cout << "\nPlease enter the number of columns for the board: ";
	else
		std::cout << "\nPlease enter the number of rows for the board: ";
	std::cin >> numberIn;

	while (!IsBoardSize(numberIn))
	{
		logger.Log(m_username + " entered a value that can't be accepted as the number of rows or columns.", Logging::Level::WARNING);
		std::cin >> numberIn;
	}
	number = std::stoi(numberIn);

	if (opt == 1)
	{
		logger.Log(m_username + " set the number of columns as " + numberIn + ".", Logging::Level::INFO);
		m_rule.setNumberOfColumns(number);
	}
	else
	{
		logger.Log(m_username + " set the number of rows as " + numberIn + ".", Logging::Level::INFO);
		m_rule.setNumberOfRows(number);
	}
}

void User::SetRules(std::ostream& output)
{
	Logging logger(output);
	int16_t option;
	char opt;
	std::cout << "\nIf you would like to create a personalized map, you will have to play by the standard rules.\n";
	std::cout << "If you would like to create a personalized map, press 1, if not, press 2. \n";
	std::cin >> opt;

	option = opt - '0';
	while (TestOption(option) == false)
	{
		logger.Log(m_username + " chose a non-existent option.", Logging::Level::WARNING);
		std::cin >> opt;
		option = opt - '0';
	}

	if (option == 1)
	{
		logger.Log(m_username + " chose to create a personalized board.", Logging::Level::INFO);
		Rules rule(true);
		m_rule = rule;
		m_personalizedMapOption = true;
	}
	else
	{
		logger.Log(m_username + " chose not to create a personalized board.", Logging::Level::INFO);
		m_personalizedMapOption = false;

		//setting the LIGHTS ON and OFF colour
		std::string lightsOn, lightsOff;
		std::array<std::string, 15>colours = { "blue", "green", "cyan", "red", "magenta", "yellow", "white", "grey", "bright blue", "bright green", "bright cyan", "bright red", "bright magenta", "bright yellow", "bright white" };

		std::cout << "\nThese are the colours you can choose from: \n";
		for (auto colour : colours)
		{
			std::cout << " - " << colour << "\n";
		}

		do {
			std::cout << "Which colour would you like the lights that are ON to be marked with? \n -> ";
			lightsOn = SetLight();
			std::cout << "Which colour would you like the lights that are OFF to be marked with? \n -> ";
			lightsOff = SetLight();
		} while (!NotSameColor(lightsOn, lightsOff) == true);
		m_rule.setLightsOffColour(lightsOff);
		logger.Log(m_username + " wants the lights that are off to be marked with " + lightsOff + ".", Logging::Level::INFO);
		m_rule.setLightsOnColour(lightsOn);
		logger.Log(m_username + " wants the lights that are on to be marked with " + lightsOn + ".", Logging::Level::INFO);

		//Neighbours
		std::cout << "\nPlease select the rules you want to play by:\n";
		std::cout << "1->Left-Right.\n";//h
		std::cout << "2->Up-Down.\n";//o
		std::cout << "3->Left-Right, Up-Down.\n";//ho
		std::cout << "4->Maind Diagonal.\n";//f
		std::cout << "5->Secondary Diagonal.\n";//s
		std::cout << "6->Left-Right, Up-Down, Maind Diagonal, Secondary Diagonal.\n";//hofs
		std::cout << "7->Maind Diagonal,Secondary Diagonal.\n";//fs

		bool rightOption = true;
		do {
			SetNeighborOption(output, rightOption);
		} while (rightOption == false);

		std::cout << "\nThe size of the board must be MxN, with M and N between [3,9].\n";
		SetRowColumnNumber(output, 1);
		SetRowColumnNumber(output, 2);

		SetTimeType(output);
	}
}

Rules User::GetRules() const
{
	return m_rule;
}

Board User::GetCustomBoard()
{
	Board customBoard(5, 5, m_personalizedMap);
	return customBoard;
}

Board User::GetUnregularBoard()
{
	Board unregularBoard (m_rowsNumber, m_columnsNumber, m_unregularBoard);
	return unregularBoard;
}

void User::WriteBoardToFile(std::vector<std::vector<bool>>& matrix)const
{
	std::string file = m_fileName;
	std::ofstream fileOut(file);

	if (fileOut.is_open())
	{
		if (m_rowsNumber != m_columnsNumber)
		{
			fileOut << 1;
			fileOut << "\n";
		}

		for (int row = 0; row < m_rowsNumber; row++)
		{
			for (int column = 0; column < m_columnsNumber; column++)
			{
				fileOut << matrix[row][column] << " ";
			}
			fileOut << "\n";
		}
		fileOut.close();
	}
	else
		std::cout << "Unable to open file";
}

void User::InitializeMap(std::vector<std::vector<bool>>& matrix)
{
	for (int row = 0; row < m_rowsNumber; row++)
	{
		std::vector<bool>aux;
		for (int column = 0; column < m_columnsNumber; column++)
		{
			aux.push_back(false);
		}
		matrix.push_back(aux);
	}
}

void User::SolvableMap(int16_t coordX, int16_t coordY, std::vector<std::vector<bool>>& matrix)
{
	if ((coordX >= 0) && (coordX < m_rowsNumber) && (coordY >= 0) && (coordY < m_columnsNumber))
	{
		matrix[coordX][coordY] = !matrix[coordX][coordY];
	}
}

void User::SetCustomMap()
{
	Rules rule(true);
	m_rowsNumber = rule.getNumberOfRows();
	m_columnsNumber = rule.getNumberOfColumns();

	std::cout << "\nHow many times do you want to toggle the lights on and off state?\n";
	std::cout << "You have to do it at least once, and at most 9 times.\nEnter a number : ";
	int lightsOnNumber;
	std::string lightsOnNumberAux;
	std::cin >> lightsOnNumberAux;

	while (!IsNumber(lightsOnNumberAux))
	{
		std::cout << "\nThe number you entered is not in the [1-9] interval. Please enter a new number: ";
		std::cin >> lightsOnNumberAux;
	}
	lightsOnNumber = std::stoi(lightsOnNumberAux);

	InitializeMap(m_personalizedMap);

	std::cout << "\nInclude the lights coordinates:\n";
	int16_t coordX, coordY;
	char coordXAux, coordYAux;
	for (int16_t light = 0; light < lightsOnNumber; light++)
	{
		std::cout << "\ncoordX: ";
		std::cin >> coordXAux;
		coordX = coordXAux - '0';
		std::cout << "\ncoordY: ";
		std::cin >> coordYAux;
		coordY = coordYAux - '0';
		IsCustomMapCoordinates(coordX, coordY);

		SolvableMap(coordX, coordY, m_personalizedMap);
		SolvableMap(coordX - 1, coordY, m_personalizedMap);
		SolvableMap(coordX + 1, coordY, m_personalizedMap);
		SolvableMap(coordX, coordY - 1, m_personalizedMap);
		SolvableMap(coordX, coordY + 1, m_personalizedMap);
	}

	m_fileName = "personalized_map.txt";
	WriteBoardToFile(m_personalizedMap);
}

int16_t User::RandomNumberGenerator(int16_t start, int16_t end)
{
	std::random_device rd;
	std::mt19937 mt(rd());
	std::uniform_int_distribution<int> dist(start, end);
	return dist(mt);
}

void User::CreatingUnregularBoard()
{
	InitializeMap(m_unregularBoard);
	int16_t numberOfOperations, coordX, coordY;
	numberOfOperations = RandomNumberGenerator(1, m_columnsNumber * m_rowsNumber);
	for (int16_t operation = 0; operation < numberOfOperations; operation++)
	{
		coordX = RandomNumberGenerator(0, m_rowsNumber - 1);
		coordY = RandomNumberGenerator(0, m_columnsNumber - 1);

		SolvableMap(coordX, coordY, m_unregularBoard);

		if (m_rule.getVertical())
		{
			SolvableMap(coordX - 1, coordY, m_unregularBoard);
			SolvableMap(coordX + 1, coordY, m_unregularBoard);
		}
		if (m_rule.getHorizontal())
		{
			SolvableMap(coordX, coordY - 1, m_unregularBoard);
			SolvableMap(coordX, coordY + 1, m_unregularBoard);
		}
		if (m_rule.getMainDiagonal())
		{
			SolvableMap(coordX - 1, coordY - 1, m_unregularBoard);
			SolvableMap(coordX + 1, coordY + 1, m_unregularBoard);
		}
		if (m_rule.getSecondaryDiagonal())
		{
			SolvableMap(coordX - 1, coordY + 1, m_unregularBoard);
			SolvableMap(coordX + 1, coordY - 1, m_unregularBoard);
		}
	}
}

void User::SelectBoardType()
{
	if ((m_rule.getNumberOfRows() == m_rule.getNumberOfColumns()) && m_rule.getNumberOfRows() % 2 == 1)
	{
		m_boardSize = std::to_string(m_rule.getNumberOfRows()) + "x" + std::to_string(m_rule.getNumberOfColumns());
		m_exampleNumber = std::to_string(RandomNumberGenerator(1, 6));
		m_fileName = m_boardSize + m_neighborOptionName + m_exampleNumber + ".txt";
	}
	else
	{
		m_rowsNumber = m_rule.getNumberOfRows();
		m_columnsNumber = m_rule.getNumberOfColumns();
		CreatingUnregularBoard();
	}
}

std::string User::getBoardType()
{
	return m_fileName;
}

bool User::getPersonalizedMapOpt()
{
	return m_personalizedMapOption;
}

void User::SetTimeType(std::ostream& output)
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
		logger.Log(m_username + " chose a non-existent option.", Logging::Level::WARNING);
		std::cin >> fastModeOpt;
		fastModeOption = fastModeOpt - '0';
	}

	if (fastModeOption == 1)
	{
		m_rule.setFastGame(true);
		logger.Log(m_username + " chose the fast game.", Logging::Level::INFO);
	}
	else
	{
		m_rule.setFastGame(false);
		logger.Log(m_username + " chose the normal game.", Logging::Level::INFO);
	}
}

bool User::IsLightColour(const std::string& colour)
{
	try {
		if (colour == "blue")
			return true;
		else if (colour == "green")
			return true;
		else if (colour == "cyan")
			return true;
		else if (colour == "red")
			return true;
		else if (colour == "magenta")
			return true;
		else if (colour == "yellow")
			return true;
		else if (colour == "white")
			return true;
		else if (colour == "grey")
			return true;
		else if (colour == "bright blue")
			return true;
		else if (colour == "bright green")
			return true;
		else if (colour == "bright cyan")
			return true;
		else if (colour == "bright red")
			return true;
		else if (colour == "bright magenta")
			return true;
		else if (colour == "bright yellow")
			return true;
		else if (colour == "bright white")
			return true;
		throw "Invalid option. Please enter a valid option!\n";
	}
	catch (const char*)
	{
		return false;
	}
}

bool User::NotSameColor(const std::string& firstColor, const std::string& secondColor)
{
	if (firstColor == secondColor)
		return false;
	return true;
}