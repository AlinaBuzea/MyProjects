#pragma once
#include <string>

class Rules
{
public:
	enum class Colours : int16_t
	{
		blue = 1,
		green = 2,
		cyan = 3,
		red = 4,
		magenta = 5,
		yellow = 6,
		white = 7,
		grey = 8,
		bright_blue = 9,
		bright_green = 10,
		bright_cyan = 11,
		bright_red = 12,
		bright_magenta = 13,
		bright_yellow = 14,
		bright_white = 15
	};

	Colours ConvertStringToColour(const std::string&);
	int16_t ConvertColourToInt(Colours);

public:

	Rules();
	Rules(bool);
	Rules(const Rules&);
	Rules(bool, bool, bool, bool, bool, int16_t, int16_t, Colours, Colours);
	Rules& operator = (const Rules&);

	friend bool operator==(const Rules& other1, const Rules& other2);

	void setHorizontal(bool);
	void setVertical(bool);
	void setMainDiagonal(bool);
	void setSecondaryDiagonal(bool);
	void setFastGame(bool);

	void setNumberOfRows(int16_t);
	void setNumberOfColumns(int16_t);

	void setLightsOnColour(const std::string&);
	void setLightsOffColour(const std::string&);

	int16_t getNumberOfRows() const;
	int16_t getNumberOfColumns() const;

	bool getHorizontal() const;
	bool getVertical() const;
	bool getMainDiagonal() const;
	bool getSecondaryDiagonal() const;
	bool getFastGame() const;

	int16_t getLightsOnColour();
	int16_t getLightsOffColour();

private:

	//neighbours
	bool m_horizontal;
	bool m_vertical;
	bool m_mainDiagonal;
	bool m_secondaryDiagonal;

	//board dimensions
	int16_t m_numberOfRows;
	int16_t m_numberOfColumns;

	//fast game
	bool m_fastGame;

	//colours
	Colours m_lightsOnColour;
	Colours m_lightsOffColour;
};