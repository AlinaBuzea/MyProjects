#include "Rules.h"
#include <iostream>

Rules::Colours Rules::ConvertStringToColour(const std::string& colour)
{
	try {
		if (colour == "blue")
			return Rules::Colours::blue;
		else if (colour == "green")
			return Rules::Colours::green;
		else if (colour == "cyan")
			return Rules::Colours::cyan;
		else if (colour == "red")
			return Rules::Colours::red;
		else if (colour == "magenta")
			return Rules::Colours::magenta;
		else if (colour == "yellow")
			return Rules::Colours::yellow;
		else if (colour == "white")
			return Rules::Colours::white;
		else if (colour == "grey")
			return Rules::Colours::grey;
		else if (colour == "bright blue")
			return Rules::Colours::bright_blue;
		else if (colour == "bright green")
			return Rules::Colours::bright_green;
		else if (colour == "bright cyan")
			return Rules::Colours::bright_cyan;
		else if (colour == "bright red")
			return Rules::Colours::bright_red;
		else if (colour == "bright magenta")
			return Rules::Colours::bright_magenta;
		else if (colour == "bright yellow")
			return Rules::Colours::bright_yellow;
		else if (colour == "bright white")
			return Rules::Colours::bright_white;
		throw "Error: Could not convert from string to Colour!";
	}
	catch (const char*)
	{
		std::cout << "The string you entered wasn't correct.\n";
	}
}

int16_t Rules::ConvertColourToInt(Colours colour)
{
	try {
		if (colour == Rules::Colours::blue)
			return static_cast<int16_t>(Rules::Colours::blue);
		else if (colour == Rules::Colours::green)
			return static_cast<int16_t>(Rules::Colours::green);
		else if (colour == Rules::Colours::cyan)
			return static_cast<int16_t>(Rules::Colours::cyan);
		else if (colour == Rules::Colours::red)
			return static_cast<int16_t>(Rules::Colours::red);
		else if (colour == Rules::Colours::magenta)
			return static_cast<int16_t>(Rules::Colours::magenta);
		else if (colour == Rules::Colours::yellow)
			return static_cast<int16_t>(Rules::Colours::yellow);
		else if (colour == Rules::Colours::white)
			return static_cast<int16_t>(Rules::Colours::white);
		else if (colour == Rules::Colours::grey)
			return static_cast<int16_t>(Rules::Colours::grey);
		else if (colour == Rules::Colours::bright_blue)
			return static_cast<int16_t>(Rules::Colours::bright_blue);
		else if (colour == Rules::Colours::bright_green)
			return static_cast<int16_t>(Rules::Colours::bright_green);
		else if (colour == Rules::Colours::bright_cyan)
			return static_cast<int16_t>(Rules::Colours::bright_cyan);
		else if (colour == Rules::Colours::bright_red)
			return static_cast<int16_t>(Rules::Colours::bright_red);
		else if (colour == Rules::Colours::bright_magenta)
			return static_cast<int16_t>(Rules::Colours::bright_magenta);
		else if (colour == Rules::Colours::bright_yellow)
			return static_cast<int16_t>(Rules::Colours::bright_yellow);
		else if (colour == Rules::Colours::bright_white)
			return static_cast<int16_t>(Rules::Colours::bright_white);
		throw "Couldn't convert colour to int.\n";
	}
	catch (const char*)
	{
		std::cout << "Error converting colour to int";
	}
}

Rules::Rules()
{
}

Rules::Rules(bool standardGame)
{
	m_horizontal = true;
	m_vertical = true;
	m_mainDiagonal = false;
	m_secondaryDiagonal = false;

	m_numberOfRows = 5;
	m_numberOfColumns = 5;

	m_fastGame = false;

	m_lightsOffColour = Colours::white;
	m_lightsOnColour = Colours::yellow;
}

Rules::Rules(const Rules& rule)
{
	m_horizontal = rule.m_horizontal;
	m_vertical = rule.m_vertical;
	m_mainDiagonal = rule.m_mainDiagonal;
	m_secondaryDiagonal = rule.m_secondaryDiagonal;
	m_fastGame = rule.m_fastGame;

	m_numberOfRows = rule.m_numberOfRows;
	m_numberOfColumns = rule.m_numberOfColumns;

	m_lightsOffColour = rule.m_lightsOffColour;
	m_lightsOnColour = rule.m_lightsOnColour;
}

Rules::Rules(bool horizontal, bool vertical, bool mainDiagonal, bool secondaryDiagonal, bool fastGame, \
	int16_t numberOfRows, int16_t numberOfColumns, Colours lightsOffColour, Colours lightsOnColour)
	:m_horizontal{ horizontal }, m_vertical{ vertical }, m_mainDiagonal{ mainDiagonal }, m_secondaryDiagonal{ secondaryDiagonal }, \
	m_fastGame{ fastGame }, m_numberOfRows{ numberOfRows }, m_numberOfColumns{ numberOfColumns }, \
	m_lightsOffColour{ lightsOffColour }, m_lightsOnColour{ lightsOnColour }
{
}

Rules& Rules::operator=(const Rules& rule)
{
	if (this != &rule)
	{
		m_horizontal = rule.m_horizontal;
		m_vertical = rule.m_vertical;
		m_mainDiagonal = rule.m_mainDiagonal;
		m_secondaryDiagonal = rule.m_secondaryDiagonal;
		m_fastGame = rule.m_fastGame;

		m_numberOfRows = rule.m_numberOfRows;
		m_numberOfColumns = rule.m_numberOfColumns;

		m_lightsOffColour = rule.m_lightsOffColour;
		m_lightsOnColour = rule.m_lightsOnColour;
	}
	return *this;
}

void Rules::setHorizontal(bool horizontal)
{
	m_horizontal = horizontal;
}

void Rules::setVertical(bool vertical)
{
	m_vertical = vertical;
}

void Rules::setMainDiagonal(bool mainDiagonal)
{
	m_mainDiagonal = mainDiagonal;
}

void Rules::setSecondaryDiagonal(bool secondaryDiagonal)
{
	m_secondaryDiagonal = secondaryDiagonal;
}

void Rules::setFastGame(bool fastGame)
{
	m_fastGame = fastGame;
}

void Rules::setNumberOfRows(int16_t numberOfRows)
{
	m_numberOfRows = numberOfRows;
}

void Rules::setNumberOfColumns(int16_t numberOfColumns)
{
	m_numberOfColumns = numberOfColumns;
}

void Rules::setLightsOnColour(const std::string& colour)
{
	m_lightsOnColour = ConvertStringToColour(colour);
}

void Rules::setLightsOffColour(const std::string& colour)
{
	m_lightsOffColour = ConvertStringToColour(colour);
}

int16_t Rules::getNumberOfRows() const
{
	return m_numberOfRows;
}

int16_t Rules::getNumberOfColumns() const
{
	return m_numberOfColumns;
}

bool Rules::getHorizontal() const
{
	return m_horizontal;
}

bool Rules::getVertical() const
{
	return m_vertical;
}

bool Rules::getMainDiagonal() const
{
	return m_mainDiagonal;
}

bool Rules::getSecondaryDiagonal() const
{
	return m_secondaryDiagonal;
}

bool Rules::getFastGame() const
{
	return m_fastGame;
}

int16_t Rules::getLightsOnColour()
{
	return ConvertColourToInt(m_lightsOnColour);
}

int16_t Rules::getLightsOffColour()
{
	return ConvertColourToInt(m_lightsOffColour);
}

bool operator==(const Rules& other1, const Rules& other2)
{
	if (other1.m_fastGame != other2.m_fastGame)
		return false;
	if (other1.m_horizontal != other2.m_horizontal)
		return false;
	if (other1.m_vertical != other2.m_vertical)
		return false;
	if (other1.m_mainDiagonal != other2.m_mainDiagonal)
		return false;
	if (other1.m_secondaryDiagonal != other2.m_secondaryDiagonal)
		return false;
	if (other1.m_lightsOffColour != other2.m_lightsOffColour)
		return false;
	if (other1.m_lightsOnColour != other2.m_lightsOnColour)
		return false;
	if (other1.m_numberOfColumns != other2.m_numberOfColumns)
		return false;
	if (other1.m_numberOfRows != other2.m_numberOfRows)
		return false;
	return true;
}
