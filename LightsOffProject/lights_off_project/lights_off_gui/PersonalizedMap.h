#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include <fstream>
#include <string>
#include "Button.h"
#include "User.h"
#include "../Logging/Logging.h"
class PersonalizedMap
{
private:
	User m_user;
	std::string m_fileName;
	std::vector<std::vector<Button>>m_gameBoard;
	std::vector<std::vector<bool>>m_lights;

	Button initializeButton(int coordX, int coordY, int buttonWidth, int buttonHeight, sf::Color buttonColour, sf::Color hoverColour, sf::Color pressedColour, bool shape);
	void Click(const int& row, const int& column);
	void Change(const int& row, const int& column);
	void UpdateButtons();
	sf::Font defineFont(std::string filePath);
	void Save();

public:
	PersonalizedMap(User user);
	void DrawMap(int designOption);
	std::string getFileName();
};

