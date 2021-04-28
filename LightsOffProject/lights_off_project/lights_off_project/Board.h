#pragma once
#include "Rules.h"
#include <vector>

class Board
{
public:
	Board();
	Board(uint16_t linesNumber, uint16_t columnsNumber, const std::vector<std::vector<bool>>& board);
	Board(const Board& other);
	Board(Board&& other);
	Board& operator=(const Board& other);
	Board& operator=(Board&& other);
	~Board();

	uint16_t getLinesNumber()const;
	void setLinesNumber(uint16_t linesNumber);
	uint16_t getColumnsNumber()const;
	void setColumnsNumber(uint16_t columnsNumber);
	std::vector<std::vector<bool>> getBoardMatrix()const;

	friend bool operator==(const Board& other1, const Board& other2);

	void readBoardFromFile(std::ostream& output, std::string inputFileName, std::string boardType);
	void click(uint16_t line, uint16_t column, Rules neighbours);
	void writeBoardOnConsole(int16_t lightsOnColor, int16_t lightsOffColor);
	void modifyBoardElement(uint16_t line, uint16_t column);
	bool allLightsAreOff();

private:
	uint16_t m_linesNumber, m_columnsNumber;
	std::vector<std::vector<bool>> m_board;
};
