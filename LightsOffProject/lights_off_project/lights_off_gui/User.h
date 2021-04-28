#pragma once

#include <SFML/Graphics.hpp>
#include <iostream>
#include <fstream>
#include <string>
#include <random>
#include "../Logging/Logging.h"

class User
{
public:

	User();
	User(bool);
	~User();

	void setNumberOfRows(int16_t);
	int16_t getNumberOfRows();
	void setNumberOfColumns(int16_t);
	int16_t getNumberOfColumns();
	void setHorizontal(bool);
	bool getHorizontal();
	void setVertical(bool);
	bool getVertical();
	void setMainDiagonal(bool);
	bool getMainDiagonal();
	void setSecondaryDiagonal(bool);
	bool getSecondaryDiagonal();

	void setLightsOnColor(sf::Color);
	sf::Color getLightsOnColor();
	void setLightsOffColor(sf::Color);
	sf::Color getLightsOffColor();

	void setFastGame(bool option);
	bool getFastGame();
	void setPersonalizedMapOption(bool option);
	bool getPersonalizedMapOption();
	std::string getFileName();

	void setDesignOption(int designOption);

	sf::Color ConvertStringToColour(const std::string& color);
	std::string ConvertColourToString(sf::Color color);
	std::string chooseBoard();
	void saveSettings();

private:
	int16_t RandomNumberGenerator(int16_t start, int16_t end);
	int16_t ExtractNumberOfFilesFromFile(const std::string & filePath);

private:
	int16_t m_numberOfRows;
	int16_t m_numberOfColumns;
	bool m_horizontal;
	bool m_vertical;
	bool m_mainDiagonal;
	bool m_secondaryDiagonal;

	sf::Color m_lightsOnColor;
	sf::Color m_lightsOffColor;

	bool m_fastGameOption;
	bool m_personalizedMapOption;

	std::string m_finalFileName;
	int m_designOption;
};