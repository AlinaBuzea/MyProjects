#pragma once

#include <SFML/Graphics.hpp>
#include <vector>
#include "User.h"
#include "Button.h"
#include "WindowProprieties.h"
#include "../Logging/Logging.h"
class SetConfigurations
{
public:

	SetConfigurations();
	~SetConfigurations();

	void setFont();
	sf::Font getFont();
	sf::Font defineFont(const std::string &);

	sf::Text initializeText(int coordX, int coordY, int characterSize,  const std::string & message, sf::Color color);
	Button initializeButton(int coordX, int coordY, int buttonWidth, int buttonHeight, sf::Color buttonColour, sf::Color hoverColour, 
							sf::Color pressedColour);
	void initializeElements(int designOption);
	void showWindow();
	void updateUser();

private:
	User m_user;
	WindowProprieties m_winProprieties;
	sf::Font m_font;
	Button m_saveOptions;
	sf::Color m_pressed ;
	sf::Color m_unpressed;

	sf::Text m_chooseDimensions;
	sf::Text m_rowsNumber;
	sf::Text m_columnsNumber;
	std::vector<Button> m_rowsNumberPossibleValues;
	std::vector<Button> m_columnsNumberPossibleValues;

	sf::Text m_ChooseLightsColors, m_lightsOn, m_lightsOff;
	std::vector<Button> m_LightsOffColorPossibleValues;
	std::vector<Button> m_lightsOnColorPossibleValues;
	std::vector<sf::Text> m_lightsColorNames;
	sf::Text m_equalColors;
	bool m_showErrorEqualColors;

	sf::Text m_chooseNeighbours;
	std::vector<Button> m_neighboursPossibleValues;
	std::vector<sf::Text> m_neighbours;

	sf::Text m_chooseFastGame;
	Button m_yesFastGame;
	Button m_noFastGame;

	int m_designOption;
};