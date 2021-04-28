#pragma once

#include <SFML/Graphics.hpp>
#include <iostream>
#include "WindowProprieties.h"
#include "Button.h"
#include "MainMenu.h"
#include "../Logging/Logging.h"
class FinalWindow
{
public:
	FinalWindow();
	~FinalWindow();

	void setWindowProprieties(const WindowProprieties& other);
	WindowProprieties getWindowProprieties();

	void setGameWon(bool gameWon);
	bool getGameWon();

	void setTimeSpentInGame(int timeSpent);
	int getTimeSpentInGame();

	Button initializeButton(float coordX, float coordY, float buttonWidth, float buttonHeight, sf::Color buttonColor, sf::Color hoverColour,
		sf::Color pressedColour);

	void initializeResult(int characterSize, float spaceBetweenElements, const std::string& message, sf::Color color);
	void initializeTimeSpentInGame(int characterSize, float spaceBetweenElements, const std::string& message, sf::Color color);

	sf::Font defineFont(const std::string& FilePath);
	void initializeElements(int designOption);
	void ShowWindow();
	
private:

	bool m_gameWon;
	int m_timeSpentInGameValue;
	sf::Text m_timeSpentInGame;
	sf::Text m_result;
	sf::Font m_textFont;
	WindowProprieties m_winProprieties;
	Button m_tryAgainButton;
	Button m_goToMenuButton;
	int m_designOption;
};
