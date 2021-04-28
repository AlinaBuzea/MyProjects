#include "Board.h"
#include "../Logging/Logging.h"
#include <fstream>
#include <Windows.h>
#include <iostream>

const int WHITE = 15;
Board::Board()
{
	m_linesNumber = 0;
	m_columnsNumber = 0;
}

Board::~Board()
{

}

Board::Board(uint16_t linesNumber, uint16_t columnsNumber, const std::vector<std::vector<bool>>& board)
	:m_linesNumber{ linesNumber }, m_columnsNumber{ columnsNumber }
{
	for (std::vector<bool> currentLine : board)
	{
		std::vector<bool> line;
		for (bool field : currentLine)
		{
			line.push_back(field);
		}
		m_board.push_back(line);
	}

}

Board::Board(const Board& other)
{
	*this = other;
}

Board::Board(Board&& other)
{
	*this = std::move(other);
}

Board& Board::operator=(const Board& other)
{
	if (this != &other)
	{
		m_board.clear();
		m_linesNumber = other.m_linesNumber;
		m_columnsNumber = other.m_columnsNumber;
		for (std::vector<bool> otherLine : other.m_board)
		{
			std::vector<bool> line;
			for (bool field : otherLine)
			{
				line.push_back(field);
			}
			m_board.push_back(line);
		}
	}
	return *this;
}

Board& Board::operator=(Board&& other)
{
	if (this != &other)
	{
		m_linesNumber = other.m_linesNumber;
		m_columnsNumber = other.m_columnsNumber;
		for (std::vector<bool> other_line : other.m_board)
		{
			std::vector<bool> line;
			for (bool field : other_line)
			{
				line.push_back(field);
			}
			m_board.push_back(line);
		}
	}
	new(&other) Board;

	return *this;
}

uint16_t Board::getLinesNumber()const
{
	return m_linesNumber;
}

void Board::setLinesNumber(uint16_t lines_number)
{
	m_linesNumber = lines_number;
}

uint16_t Board::getColumnsNumber()const
{
	return m_columnsNumber;
}

void Board::setColumnsNumber(uint16_t columns_number)
{
	m_columnsNumber = columns_number;
}

std::vector<std::vector<bool>> Board::getBoardMatrix() const
{
	return m_board;
}

void Board::readBoardFromFile(std::ostream& output, std::string input_file_name, std::string board_type)
{
	Logging logger(output);
	std::ifstream file_in(input_file_name);
	try {
		if (file_in.is_open())
		{
			m_linesNumber = board_type[0] - '0';
			m_columnsNumber = board_type[2] - '0';
			int16_t element;
			for (int16_t index = 0; index < m_linesNumber; index++)
			{
				std::vector<bool> line;
				for (int16_t index1 = 0; index1 < m_columnsNumber; index1++)
				{
					file_in >> element;
					line.push_back((bool)element);
				}
				m_board.push_back(line);
			}
			file_in.close();
		}
		else
		{
			throw std::string("The file can not be opened!");
		}
	}
	catch (std::string message)
	{
		logger.Log(message, Logging::Level::WARNING);
	}
}

void Board::click(uint16_t line, uint16_t column, Rules myRules)
{
	if (line < m_linesNumber && column < m_columnsNumber)
	{
		modifyBoardElement(line, column);
		if (myRules.getVertical())
		{
			if (line - 1 > -1)
			{
				modifyBoardElement(line - 1, column);
			}
			if (line + 1 < m_linesNumber)
			{
				modifyBoardElement(line + 1, column);
			}
		}
		if (myRules.getHorizontal())
		{
			if (column - 1 > -1)
			{
				modifyBoardElement(line, column - 1);
			}
			if (column + 1 < m_columnsNumber)
			{
				modifyBoardElement(line, column + 1);
			}
		}
		if (myRules.getMainDiagonal())
		{
			if (line - 1 > -1 && column - 1 > -1)
			{
				modifyBoardElement(line - 1, column - 1);
			}
			if (line + 1 < m_linesNumber && column + 1 < m_columnsNumber)
			{
				modifyBoardElement(line + 1, column + 1);
			}
		}
		if (myRules.getSecondaryDiagonal())
		{
			if (line - 1 > -1 && column + 1 < m_columnsNumber)
			{
				modifyBoardElement(line - 1, column + 1);
			}
			if (line + 1 < m_linesNumber && column - 1 > -1)
			{
				modifyBoardElement(line + 1, column - 1);
			}
		}
	}
	else
	{
		throw std::string("Coordinates out of bounds");
	}

	writeBoardOnConsole(myRules.getLightsOnColour(), myRules.getLightsOffColour());
}

bool Board::allLightsAreOff()
{
	for (std::vector<bool> line : m_board)
	{
		for (bool field : line)
		{
			if (field)
			{
				return false;
			}
		}
	}
	return true;
}

void Board::writeBoardOnConsole(int16_t lightsOnColor, int16_t lightsOffColor)
{
	try {
		HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
		if (lightsOnColor > 0 && lightsOnColor < 16 && lightsOffColor> 0 && lightsOffColor < 16)
		{
			if (lightsOnColor != lightsOffColor)
			{
				for (uint16_t index = 0; index < m_linesNumber; index++)
				{
					for (uint16_t index1 = 0; index1 < m_columnsNumber; index1++)
					{
						if (m_board[index][index1])
						{
							SetConsoleTextAttribute(hConsole, lightsOnColor);
							std::cout << index << index1 << " ";
						}
						else
						{
							SetConsoleTextAttribute(hConsole, lightsOffColor);
							std::cout << index << index1 << " ";
						}
					}
					std::cout << "\n";
				}
			}
			else
			{
				throw std::string("Lights_off and Lights_on have the same color");
			}

		}
		else
		{
			throw std::string("Colors out of bounds");
		}
		SetConsoleTextAttribute(hConsole, WHITE);
	}
	catch (std::string)
	{
		std::cout << "\nAn error occured.";
	}
}

void Board::modifyBoardElement(uint16_t line, uint16_t column)
{
	m_board[line][column] = !m_board[line][column];
}

bool operator==(const Board& other1, const Board& other2)
{
	if (other1.m_linesNumber != other2.m_linesNumber || other1.m_columnsNumber != other2.m_columnsNumber)
	{
		return false;
	}

	for (int contor = 0; contor < other1.m_linesNumber; contor++)
	{
		for (int contor1 = 0; contor1 < other1.m_columnsNumber; contor1++)
		{
			if (other1.m_board[contor][contor1] != other2.m_board[contor][contor1])
			{
				return false;
			}
		}
	}
	return true;
}
