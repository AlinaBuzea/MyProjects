#include "User.h"
#include <fstream>
#include "PersonalizedMap.h"

User::User()
{
	m_numberOfRows = 5;
	m_numberOfColumns = 5;
	m_horizontal = true;
	m_vertical = true;
	m_mainDiagonal = false;
	m_secondaryDiagonal = false;
	m_lightsOnColor = sf::Color::Yellow;
	m_lightsOffColor = sf::Color::Black;
	m_personalizedMapOption = false;
	m_fastGameOption = false;
	m_designOption = 3;
}

User::User(bool readSettingsFromFile)
{
	Logging logger(std::cout);
	if (readSettingsFromFile)
	{
		std::ifstream fileIn("Settings.txt");
		try {
			if (fileIn.is_open())
			{
				std::string line;
				std::getline(fileIn, line);

				m_numberOfRows = line[0] - '0';
				m_numberOfColumns = line[2] - '0';

				std::getline(fileIn, line);
				m_horizontal = false;
				m_vertical = false;
				m_mainDiagonal = false;
				m_secondaryDiagonal = false;

				for (char character : line)
				{
					switch (character)
					{
					case 'h': m_horizontal = true;
						break;

					case 'v': m_vertical = true;
						break;

					case 'f': m_mainDiagonal = true;
						break;

					case 's': m_secondaryDiagonal = true;
					}
				}

				std::getline(fileIn, line);
				try
				{
					m_lightsOnColor = ConvertStringToColour(line);
				}
				catch (const char* message)
				{
					logger.Log(message, Logging::Level::ERROR);
				}

				std::getline(fileIn, line);
				try
				{
					m_lightsOffColor = ConvertStringToColour(line);
				}
				catch (const char* message)
				{
					logger.Log(message, Logging::Level::ERROR);
				}

				std::getline(fileIn, line);
				try
				{
					std::string gameOption = line;
					if (gameOption == "normal game")
					{
						m_fastGameOption = false;
					}
					else
					{
						m_fastGameOption = true;
					}
				}
				catch (std::string)
				{
					std::cout << "The string you entered wasn't correct.\n";
				}

				std::getline(fileIn, line);
				try
				{
					int personalizedMapOption = line[0] - '0';
					if (personalizedMapOption == 1)
					{
						m_personalizedMapOption = true;
					}
					else
					{
						m_personalizedMapOption = false;
					}
				}
				catch (std::string)
				{
					std::cout << "The string you entered wasn't correct.\n";
				}

				fileIn.close();
			}
			else
			{
				throw (std::string)"This file can not be opened!";
			}
		}
		catch (std::string message)
		{
			logger.Log("Couldn't find the input file!", Logging::Level::ERROR);
		}
	}
	else
	{
		m_numberOfRows = 5;
		m_numberOfColumns = 5;
		m_horizontal = true;
		m_vertical = true;
		m_mainDiagonal = false;
		m_secondaryDiagonal = false;
		m_lightsOnColor = sf::Color::White;
		m_lightsOffColor = sf::Color::Yellow;
	}
	m_designOption = 3;
}

User::~User()
{
}

void User::setNumberOfRows(int16_t numberOfRows)
{
	m_numberOfRows = numberOfRows;
}

int16_t User::getNumberOfRows()
{
	return m_numberOfRows;
}

void User::setNumberOfColumns(int16_t numberOfColumns)
{
	m_numberOfColumns = numberOfColumns;
}

int16_t User::getNumberOfColumns()
{
	return m_numberOfColumns;
}

void User::setHorizontal(bool horizontal)
{
	m_horizontal = horizontal;
}

bool User::getHorizontal()
{
	return m_horizontal;
}

void User::setVertical(bool vertical)
{
	m_vertical = vertical;
}

bool User::getVertical()
{
	return m_vertical;
}

void User::setMainDiagonal(bool mainDiagonal)
{
	m_mainDiagonal = mainDiagonal;
}

bool User::getMainDiagonal()
{
	return m_mainDiagonal;
}

void User::setSecondaryDiagonal(bool secondaryDiagonal)
{
	m_secondaryDiagonal = secondaryDiagonal;
}

bool User::getSecondaryDiagonal()
{
	return m_secondaryDiagonal;
}

void User::setLightsOnColor(sf::Color color)
{
	m_lightsOnColor = color;
}

sf::Color User::getLightsOnColor()
{
	return m_lightsOnColor;
}

void User::setLightsOffColor(sf::Color color)
{
	m_lightsOffColor = color;
}

sf::Color User::getLightsOffColor()
{
	return m_lightsOffColor;
}

void User::setFastGame(bool fastGame)
{
	m_fastGameOption = fastGame;
}

bool User::getFastGame()
{
	return  m_fastGameOption;
}

void User::setPersonalizedMapOption(bool option)
{
	m_personalizedMapOption = option;
}

bool User::getPersonalizedMapOption()
{
	return m_personalizedMapOption;
}

std::string User::getFileName()
{
	return m_finalFileName;
}

void User::setDesignOption(int designOption)
{
	m_designOption = designOption;
}


sf::Color User::ConvertStringToColour(const std::string& color)
{
	if (color == "sf::Color::Blue")
		return sf::Color::Blue;
	else if (color == "sf::Color::Green")
		return sf::Color::Green;
	else if (color == "sf::Color::Cyan")
		return sf::Color::Cyan;
	else if (color == "sf::Color::Red")
		return sf::Color::Red;
	else if (color == "sf::Color::Magenta")
		return sf::Color::Magenta;
	else if (color == "sf::Color::Yellow")
		return sf::Color::Yellow;
	else if (color == "sf::Color::White")
		return sf::Color::White;
	else if (color == "sf::Color::Black")
		return sf::Color::Black;
		
	throw (std::string)"Error: Could not convert string to sf::Color!";
}

std::string User::ConvertColourToString(sf::Color color)
{
	if (color == sf::Color::Blue)
		return (std::string)"sf::Color::Blue";
	else if (color == sf::Color::Green)
		return (std::string)"sf::Color::Green";
	else if (color == sf::Color::Cyan)
		return (std::string)"sf::Color::Cyan";
	else if (color == sf::Color::Red)
		return (std::string)"sf::Color::Red";
	else if (color == sf::Color::Magenta)
		return (std::string)"sf::Color::Magenta";
	else if (color == sf::Color::Yellow)
		return (std::string)"sf::Color::Yellow";
	else if (color == sf::Color::White)
		return (std::string)"sf::Color::White";
	else if (color == sf::Color::Black)
		return (std::string)"sf::Color::Black";

	throw (std::string)"Error: Could not convert sf::Color to string!";
}

std::string User::chooseBoard()
{
	Logging logger(std::cout);
	std::string boardConfiguration;
	if (m_personalizedMapOption)
	{
		return "5x5_hv_mp.txt";
	}
	else if ((m_numberOfRows == m_numberOfColumns) && m_numberOfRows % 2 == 1)
	{
		boardConfiguration = std::to_string(m_numberOfRows) + "x" + std::to_string(m_numberOfColumns)+"_";
		if (m_horizontal)
		{
			boardConfiguration += "h";
		}
		if (m_vertical)
		{
			boardConfiguration += "v";
		}
		if (m_mainDiagonal)
		{
			boardConfiguration += "f";
		}
		if (m_secondaryDiagonal)
		{
			boardConfiguration += "s";
		}
		int16_t numberOfFiles;
		try
		{
			numberOfFiles = ExtractNumberOfFilesFromFile(boardConfiguration + ".txt");
			boardConfiguration += "_" + std::to_string(RandomNumberGenerator(1, numberOfFiles)) +".txt";

		}
		catch (std::string message)
		{
			logger.Log(message, Logging::Level::ERROR);
		}
	}
	else
	{
		PersonalizedMap personalizedMap(this);
		personalizedMap.DrawMap(m_designOption);
		saveSettings();
		return personalizedMap.getFileName();
	}
	return boardConfiguration;
}

int16_t User::RandomNumberGenerator(int16_t start, int16_t end)
{
	std::random_device rd;
	std::mt19937 mt(rd());
	std::uniform_int_distribution<int> dist(start, end);
	return dist(mt);
}

int16_t User::ExtractNumberOfFilesFromFile(const std::string & filePath)
{
	std::ifstream fileIn(filePath);
	if (fileIn.is_open())
	{
		int number;
		fileIn >> number;
		fileIn.close();
		return number;
	}
	else
	{
		throw (std::string)"This file can not be opened!";
	}
	return 0;
}
void User::saveSettings()
{
	Logging logger(std::cout);
	std::ofstream fileOut("Settings.txt");
	try {
		if (fileOut.is_open())
		{
			fileOut << getNumberOfRows() << " " << getNumberOfColumns() << "\n";
			if (getHorizontal())
			{
				fileOut << "h";
			}
			if (getVertical())
			{
				fileOut << "v";
			}
			if (getMainDiagonal())
			{
				fileOut << "f";
			}
			if (getSecondaryDiagonal())
			{
				fileOut << "s";
			}
			try {
				fileOut << "\n" << ConvertColourToString(getLightsOnColor()) << "\n"
					<< ConvertColourToString(getLightsOffColor()) << "\n";
			}
			catch (std::string message)
			{
				logger.Log(message, Logging::Level::ERROR);
			}

			if (getFastGame())
			{
				fileOut << "fast game\n";
			}
			else
			{
				fileOut << "normal game\n";
			}


			fileOut << getPersonalizedMapOption() << "\n" << m_finalFileName;
			fileOut.close();
		}
		else
		{
			throw (std::string)"This file can not be opened";
		}
	}
	catch (std::string message)
	{
		logger.Log(message, Logging::Level::ERROR);
	}
}
