#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include "WindowProprieties.h"
#include "Button.h"
#include "SetConfigurations.h"
#include "PersonalizedMap.h"
#include "User.h"
#include "../Logging/Logging.h"
class Settings
{
public:
	Settings();
	~Settings();

	Button initializeButton(float coordX, float coordY, float buttonWidth, float buttonHeight, sf::Color buttonColor, sf::Color hoverColour,
		sf::Color pressedColour);

	sf::Text defineText(const std::string& title, int size, sf::Color colour, sf::Uint32 style, sf::Vector2f position);
	sf::Font defineFont(const std::string& FilePath);
	void initializeElements(int designOption);
	void ShowWindow();
	void Instructions();
private:
	void saveSettings(User mpUser, const std::string& personalizedMapFileName);
	std::string readInputFile(const std::string& filePath);

private:

	WindowProprieties m_winProprieties;
	Button m_backToMenuButton;
	Button m_personalizedMapButton;
	Button m_setConfigurationsButton;
	Button m_instructionsButton;
	int m_designOption;
};