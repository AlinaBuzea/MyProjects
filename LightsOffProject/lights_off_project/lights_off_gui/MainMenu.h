#pragma once

#include <string>
#include "Button.h"
#include "WindowProprieties.h"
#include "../Logging/Logging.h"
class MainMenu
{
private:
	int m_designOption;

	sf::Font defineFont(const std::string& filePath);
	sf::Text defineText(const std::string& title, int size, sf::Color colour, sf::Uint32 style, sf::Vector2f position);
	std::string readInputFile(const std::string& filePath);
	void resetFile(const std::string& settings, const std::string& standardSettings);
	void setButtonColours(Button& button, sf::Color buttonColour, sf::Color hoverColour);
	void Instructions();

public:
	MainMenu();
	MainMenu(int designOption);
	void Menu();
	void Play();
	void Setting();
};

