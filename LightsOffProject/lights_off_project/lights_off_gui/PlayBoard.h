#pragma once
#include <iostream>
#include "Button.h"
#include "WindowProprieties.h"
#include "User.h"
#include <vector>
#include <fstream>
#include "../Logging/Logging.h"
class PlayBoard
{
private:
	std::vector<std::vector<Button>>m_gameBoard ;
	std::vector<std::vector<bool>>m_lights;
	User m_user;
	sf::Color m_lightsOffColour;
	sf::Color m_lightsOnColour;
	//Logging logger(std::cout);

	bool AllLightsAreOff();
	Button initializeButton(int coordX, int coordY, int buttonWidth, int buttonHeight, sf::Color buttonColour, sf::Color hoverColour, sf::Color pressedColour, bool lightOn);
	sf::Font defineFont(std::string filePath);
	void Click(const int& row, const int& column);
	void Change(const int& row, const int& column);
	void UpdateButtons();

public:
	PlayBoard(User user);
	void DrawBoard(int designOption);
};