#include "SetConfigurations.h"

const int DIMENSIONS_NUMBER = 7;

SetConfigurations::SetConfigurations()
{
}

SetConfigurations::~SetConfigurations()
{
}

void SetConfigurations::setFont()
{
	m_font = defineFont("..\\COOPBL.TTF");
}

sf::Font SetConfigurations::getFont()
{
	return m_font;
}

sf::Font SetConfigurations::defineFont(const std::string & FilePath)
{
	Logging logger(std::cout);
	sf::Font font;
	if (!font.loadFromFile(FilePath))
	{
		logger.Log("Font file not found.", Logging::Level::ERROR);
	}
	return font;
}

sf::Text SetConfigurations::initializeText(int coordX, int coordY, int characterSize, const std::string & message, sf::Color color)
{
	sf::Text textLabel;
	textLabel.setCharacterSize(characterSize);
	textLabel.setFont(m_font);
	textLabel.setFillColor(color);
	textLabel.setString(message);
	textLabel.setPosition(coordX, coordY);
	return textLabel;
}

Button SetConfigurations::initializeButton(int coordX, int coordY, int buttonWidth, int buttonHeight, sf::Color buttonColor, sf::Color hoverColour, sf::Color pressedColour)
{
	Button button;
	button.setShapeOption(0);
	button.setShape(coordX, coordY,buttonWidth, buttonHeight);
	button.setButtonColour(buttonColor);
	button.setHoverColour(hoverColour);
	button.setPressedColour(pressedColour);
	return button;
}

void SetConfigurations::initializeElements(int designOption)
{
	Logging logger(std::cout);
	int buttonHeight = 30 ;
	int buttonWidth = 30;
	int coordXFirstFromLine = 10;
	int coordYFirstFromLine = 10;
	int spaceCoordX = 5;
	int spaceCoordY = 10;
	int characterSizeOfCathegoryTitle = 20;
	int characterSizeOfChoiceName = 15;
	m_designOption = designOption;
	setFont();

	sf::Color backgroundColour, saveButtonColour, saveHoverColour, textColour;
	if (designOption == 1)
	{
		backgroundColour = sf::Color(73, 114, 229);
		textColour = sf::Color::White;
		m_pressed = sf::Color::Green;
		m_unpressed = sf::Color::Cyan;
		saveButtonColour = sf::Color(255, 51, 153);
		saveHoverColour = sf::Color(255, 102, 178);
	}
	else if (designOption == 2)
	{
		backgroundColour = sf::Color(229, 229, 220);
		textColour = sf::Color(38, 73, 92);
		m_unpressed = sf::Color(198, 107, 61);
		m_pressed = sf::Color(198, 107, 61, 120);
		saveButtonColour = sf::Color(198, 107, 61);
		saveHoverColour = sf::Color(198, 107, 61, 120);
	}
	else if (designOption == 3)
	{
		backgroundColour = sf::Color(156, 155, 228);
		textColour = sf::Color(235, 225, 231);
		m_unpressed = sf::Color(213, 169, 247);
		m_pressed = sf::Color(213, 169, 247, 120);
		saveButtonColour = sf::Color(213, 169, 247);
		saveHoverColour = sf::Color(213, 169, 247, 120);
	}
	else if (designOption == 4)
	{
		backgroundColour = sf::Color(235, 212, 226);
		textColour = sf::Color::White;
		m_unpressed = sf::Color(222, 119, 185);
		m_pressed = sf::Color(217, 164, 198);
		saveButtonColour = sf::Color(248, 47, 185);
		saveHoverColour = sf::Color(248, 47, 185, 120);
	}
	else
	{
		backgroundColour = sf::Color(207, 223, 218);
		textColour = sf::Color(23, 48, 66);
		m_pressed = sf::Color(50, 174, 78);
		m_unpressed = sf::Color(35, 100, 119);
		saveButtonColour = sf::Color(35, 100, 119);
		saveHoverColour = sf::Color(50, 174, 78);
	}

	WindowProprieties winProp(700, 700, backgroundColour);
	m_winProprieties = winProp;

	m_chooseDimensions = initializeText(coordXFirstFromLine, coordYFirstFromLine,characterSizeOfCathegoryTitle,"Choose dimensions for the board:", textColour);
	coordYFirstFromLine += characterSizeOfCathegoryTitle + spaceCoordY;
	m_rowsNumber = initializeText(coordXFirstFromLine , coordYFirstFromLine + (buttonHeight - characterSizeOfChoiceName) / 2, characterSizeOfChoiceName, "Lines no. : ", textColour);

	for (int index = 0; index < DIMENSIONS_NUMBER; index++)
	{
		Button setRowsNoButton = initializeButton( coordXFirstFromLine +m_rowsNumber.getGlobalBounds().width+ spaceCoordX +buttonWidth*index + 
								spaceCoordX*index,coordYFirstFromLine, buttonWidth, buttonHeight, m_unpressed, m_pressed,
								m_pressed);

		m_rowsNumberPossibleValues.push_back(setRowsNoButton);
	}

	coordYFirstFromLine += buttonHeight + spaceCoordY;
	m_columnsNumber = initializeText(coordXFirstFromLine, coordYFirstFromLine + (buttonHeight - characterSizeOfChoiceName) / 2, characterSizeOfChoiceName, "Columns no. : ", textColour);

	for (int index = 0; index < DIMENSIONS_NUMBER; index++)
	{
		Button setColumnsNoButton = initializeButton(coordXFirstFromLine + m_columnsNumber.getGlobalBounds().width + spaceCoordX + 
									(buttonWidth + spaceCoordX) * index, coordYFirstFromLine, buttonWidth, buttonHeight, 
									m_unpressed, m_pressed, m_pressed);
		
		m_columnsNumberPossibleValues.push_back(setColumnsNoButton);
	}

	coordYFirstFromLine += 2*(buttonHeight + spaceCoordY);
	m_chooseNeighbours = initializeText(coordXFirstFromLine,coordYFirstFromLine, characterSizeOfCathegoryTitle, "Choose neighbours: ", textColour);
	coordYFirstFromLine += characterSizeOfCathegoryTitle + spaceCoordY;

	std::ifstream fileIn("NeighboursList.txt");
	try {
		if (fileIn.is_open())
		{
			std::string line;
			int index = 0;
			while (std::getline(fileIn, line))
			{
				Button neighbour = initializeButton(coordXFirstFromLine, coordYFirstFromLine + (buttonHeight + spaceCoordY) * index,
					buttonWidth, buttonHeight, m_unpressed, m_pressed, m_pressed);
				sf::Text neighbour_choice = initializeText(coordXFirstFromLine + buttonWidth + spaceCoordX,
					coordYFirstFromLine + (buttonHeight - characterSizeOfChoiceName) / 2 + (buttonHeight + spaceCoordY) * index,
					characterSizeOfChoiceName, line, textColour);

				m_neighboursPossibleValues.push_back(neighbour);
				m_neighbours.push_back(neighbour_choice);
				index++;
			}
			fileIn.close();
		}
		else
		{
			throw "The NeighboursList.txt file can not pe opened!";
		}
	}
	catch (const char* message)
	{
		logger.Log(message, Logging::Level::ERROR);
	}

	coordYFirstFromLine += m_neighbours.size() * (buttonHeight + spaceCoordY);
	m_chooseFastGame = initializeText(coordXFirstFromLine, coordYFirstFromLine, characterSizeOfCathegoryTitle, "Do you want to play fast game?", textColour);
	coordYFirstFromLine += characterSizeOfCathegoryTitle + spaceCoordY;
	
	m_yesFastGame = initializeButton(coordXFirstFromLine, coordYFirstFromLine, buttonWidth, buttonHeight, m_unpressed, m_pressed, m_pressed);
	m_noFastGame = initializeButton(coordXFirstFromLine + buttonWidth + spaceCoordX, coordYFirstFromLine, buttonWidth, buttonHeight, m_unpressed, m_pressed, m_pressed);

	coordXFirstFromLine = m_chooseDimensions.getGlobalBounds().width + 2 * (buttonWidth + spaceCoordX);
	coordYFirstFromLine = 10;
	m_ChooseLightsColors = initializeText(coordXFirstFromLine, coordYFirstFromLine, characterSizeOfCathegoryTitle, "Choose colors for lights: ", textColour);
	coordXFirstFromLine += buttonWidth;
	coordYFirstFromLine += m_ChooseLightsColors.getGlobalBounds().height + spaceCoordY;

	m_lightsOn = initializeText(coordXFirstFromLine, coordYFirstFromLine, characterSizeOfChoiceName, "On:", textColour);
	m_lightsOff = initializeText(coordXFirstFromLine + m_ChooseLightsColors.getGlobalBounds().width - 3* buttonWidth, coordYFirstFromLine, 
					characterSizeOfChoiceName, "Off:", textColour);
	
	coordYFirstFromLine += m_lightsOn.getGlobalBounds().height + spaceCoordY;


	std::ifstream fileInColors("Colors.txt");
	try {
		if (fileInColors.is_open())
		{
			std::string line;
			int index = 0;
			while (std::getline(fileInColors, line))
			{
				Button setLightsOnButton = initializeButton(coordXFirstFromLine, coordYFirstFromLine + (buttonWidth + spaceCoordY) * index,
					buttonWidth, buttonHeight, m_unpressed, m_pressed, m_pressed);
				Button setLightsOffButton = initializeButton(coordXFirstFromLine + m_ChooseLightsColors.getGlobalBounds().width - 3 * buttonWidth,
					coordYFirstFromLine + (buttonWidth + spaceCoordY) * index, buttonWidth, buttonHeight,
					m_unpressed, m_pressed, m_pressed);
				sf::Text light_name = initializeText(coordXFirstFromLine + 2 * (buttonWidth + spaceCoordX),
					coordYFirstFromLine + (buttonHeight - characterSizeOfChoiceName) / 2 + (buttonHeight + spaceCoordY) * index,
					characterSizeOfChoiceName, line, textColour);

				m_lightsOnColorPossibleValues.push_back(setLightsOnButton);
				m_LightsOffColorPossibleValues.push_back(setLightsOffButton);
				m_lightsColorNames.push_back(light_name);
				index++;
			}
			fileInColors.close();
		}
		else
		{
			throw (std::string)"The Colors.txt file can not pe opened!";
		}
	}
	catch (std::string message)
	{
		logger.Log(message, Logging::Level::ERROR);
	}

	coordYFirstFromLine += (m_lightsOnColorPossibleValues.at(m_lightsOnColorPossibleValues.size() - 1).getButtonShape().getGlobalBounds().height
							+spaceCoordY)* m_LightsOffColorPossibleValues.size();

	m_equalColors = initializeText(coordXFirstFromLine-buttonWidth, coordYFirstFromLine, characterSizeOfChoiceName, "You can't choose the same color!", sf::Color::Red);
	m_equalColors.setFillColor(sf::Color::Red);

	coordYFirstFromLine += buttonHeight;
	coordXFirstFromLine += m_ChooseLightsColors.getGlobalBounds().width - buttonWidth;
	buttonHeight = 50;
	buttonWidth = 100;
	coordXFirstFromLine -= buttonWidth;
	m_saveOptions = initializeButton(coordXFirstFromLine, coordYFirstFromLine, buttonWidth, buttonHeight, saveButtonColour,
		saveHoverColour, saveButtonColour);

	m_showErrorEqualColors = false;
	
	m_winProprieties.setWidth(730);
}

void SetConfigurations::showWindow()
{
	Logging logger(std::cout);
	sf::RenderWindow window(sf::VideoMode(m_winProprieties.getWidth(), m_winProprieties.getHeight()),"Set Configurations");
	window.clear(m_winProprieties.getColor());

	while (window.isOpen())
	{
		sf::Event event;
		while (window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
			{
				updateUser();
				m_user.saveSettings();
				window.close();
			}
		}
		window.clear(m_winProprieties.getColor());

		sf::Vector2f mousePosition = static_cast<sf::Vector2f>(sf::Mouse::getPosition(window));
		
		window.draw(m_chooseDimensions);
		window.draw(m_rowsNumber);

		for (int index = 0; index < DIMENSIONS_NUMBER; index++)
		{
			m_rowsNumberPossibleValues.at(index).setFont(defineFont("..\\COOPBL.TTF"));
			m_rowsNumberPossibleValues.at(index).setText(std::to_string(index + 3));

			m_rowsNumberPossibleValues.at(index).update(mousePosition);
			m_rowsNumberPossibleValues.at(index).drawButton(&window);

			if (m_rowsNumberPossibleValues.at(index).isPressed())
			{
				for (int index1 = 0; index1 < DIMENSIONS_NUMBER; index1++)
				{
					m_rowsNumberPossibleValues.at(index1).setButtonColour(m_unpressed);
				}
				m_rowsNumberPossibleValues.at(index).setButtonColour(m_pressed);
			}
		}

		window.draw(m_columnsNumber);

		for (int index = 0; index < DIMENSIONS_NUMBER; index++)
		{
			m_columnsNumberPossibleValues.at(index).setFont(defineFont("..\\COOPBL.TTF"));
			m_columnsNumberPossibleValues.at(index).setText(std::to_string(index + 3));

			m_columnsNumberPossibleValues.at(index).update(mousePosition);
			m_columnsNumberPossibleValues.at(index).drawButton(&window);

			if (m_columnsNumberPossibleValues.at(index).isPressed())
			{
				for (int index1 = 0; index1 < DIMENSIONS_NUMBER; index1++)
				{
					m_columnsNumberPossibleValues.at(index1).setButtonColour(m_unpressed);
				}
				m_columnsNumberPossibleValues.at(index).setButtonColour(m_pressed);
			}
		}

		window.draw(m_chooseNeighbours);

		for (int index = 0; index < m_neighboursPossibleValues.size(); index++)
		{
			m_neighboursPossibleValues.at(index).update(mousePosition);
			m_neighboursPossibleValues.at(index).drawButton(&window);
			window.draw(m_neighbours.at(index));

			if (m_neighboursPossibleValues.at(index).isPressed())
			{
				for (int index1 = 0; index1 < m_neighboursPossibleValues.size(); index1++)
				{
					m_neighboursPossibleValues.at(index1).setButtonColour(m_unpressed);
				}
				m_neighboursPossibleValues.at(index).setButtonColour(m_pressed);
			}
		}

		window.draw(m_chooseFastGame);
		m_yesFastGame.setFont(defineFont("..\\COOPBL.TTF"));
		m_yesFastGame.setText("yes");
		m_yesFastGame.update(mousePosition);
		m_yesFastGame.drawButton(&window);
		if (m_yesFastGame.isPressed())
		{
			m_yesFastGame.setButtonColour(m_pressed);
			m_noFastGame.setButtonColour(m_unpressed);
		}

		m_noFastGame.setFont(defineFont("..\\COOPBL.TTF"));
		m_noFastGame.setText("no");
		m_noFastGame.update(mousePosition);
		m_noFastGame.drawButton(&window);
		if (m_noFastGame.isPressed())
		{
			m_noFastGame.setButtonColour(m_pressed);
			m_yesFastGame.setButtonColour(m_unpressed);
		}


		window.draw(m_ChooseLightsColors);
		window.draw(m_lightsOn);
		window.draw(m_lightsOff);

		for (int index = 0; index < m_lightsColorNames.size(); index++)
		{
			m_lightsOnColorPossibleValues.at(index).update(mousePosition);
			m_lightsOnColorPossibleValues.at(index).drawButton(&window);

			if (m_lightsOnColorPossibleValues.at(index).isPressed())
			{
				if (m_LightsOffColorPossibleValues.at(index).getButtonShape().getFillColor() != m_pressed)
				{
					m_showErrorEqualColors = false;

					for (int index1 = 0; index1 < m_lightsColorNames.size(); index1++)
					{
						m_lightsOnColorPossibleValues.at(index1).setButtonColour(m_unpressed);
					}
					m_lightsOnColorPossibleValues.at(index).setButtonColour(m_pressed);
				}
				else
				{
					m_showErrorEqualColors = true;
				}
			}

			m_LightsOffColorPossibleValues.at(index).update(mousePosition);
			m_LightsOffColorPossibleValues.at(index).drawButton(&window);
			if (m_LightsOffColorPossibleValues.at(index).isPressed())
			{
				if (m_lightsOnColorPossibleValues.at(index).getButtonShape().getFillColor() != m_pressed)
				{
					m_showErrorEqualColors = false;
					for (int index1 = 0; index1 < m_lightsColorNames.size(); index1++)
					{
						m_LightsOffColorPossibleValues.at(index1).setButtonColour(m_unpressed);
					}
					m_LightsOffColorPossibleValues.at(index).setButtonColour(m_pressed);
				}
				else
				{
					m_showErrorEqualColors = true;
				}
			}
			window.draw(m_lightsColorNames.at(index));
		}
		if (m_showErrorEqualColors)
		{
			logger.Log("The user tried to choose the same colours for the lights on and off state!", Logging::Level::WARNING);
			window.draw(m_equalColors);
		}

		m_saveOptions.setFont(defineFont("..\\COOPBL.TTF"));
		m_saveOptions.setText("Save");
		m_saveOptions.update(mousePosition);
		m_saveOptions.drawButton(&window);

		if (m_saveOptions.isPressed())
		{
			updateUser();
			m_user.saveSettings();
			logger.Log("The user set new configurations.", Logging::Level::INFO);
			window.close();
		}
		window.display();
	}
}

void SetConfigurations::updateUser()
{
	Logging logger(std::cout);
	int index = 0;
	while (index < m_rowsNumberPossibleValues.size() && m_rowsNumberPossibleValues.at(index).getButtonShape().getFillColor() != m_pressed)
		index++;
	
	if (index < m_rowsNumberPossibleValues.size())
	{
		m_user.setNumberOfRows(index + 3);
		logger.Log("The number of rows has been set as " + std::to_string(index + 3), Logging::Level::INFO);
	}

	index = 0;
	while (index < m_columnsNumberPossibleValues.size() && m_columnsNumberPossibleValues.at(index).getButtonShape().getFillColor() != m_pressed)
		index++;
	if (index < m_columnsNumberPossibleValues.size())
	{
		m_user.setNumberOfColumns(index + 3);
		logger.Log("The number of columns has been set as " + std::to_string(index + 3), Logging::Level::INFO);
	}

	index = 0;
	while (index < m_neighboursPossibleValues.size() && m_neighboursPossibleValues.at(index).getButtonShape().getFillColor() != m_pressed)
		index++;

	if (index < m_neighboursPossibleValues.size())
	{
		if (static_cast<std::string>(m_neighbours.at(index).getString()) == "Left-right-up-down")
		{
			m_user.setHorizontal(true);
			m_user.setVertical(true);
			logger.Log("The neighbourhood has been set as \"Left - right - up - down\".", Logging::Level::INFO);

		}
		else if (static_cast<std::string>(m_neighbours.at(index).getString()) == "Left-right")
		{
			m_user.setHorizontal(true);
			m_user.setVertical(false);
			logger.Log("The neighbourhood has been set as \"Left-right\".", Logging::Level::INFO);
		}
		else if (static_cast<std::string>(m_neighbours.at(index).getString()) == "Up-down")
		{
			m_user.setVertical(true);
			m_user.setHorizontal(false);
			logger.Log("The neighbourhood has been set as \"Up-down\".", Logging::Level::INFO);
		}
		else if (static_cast<std::string>(m_neighbours.at(index).getString()) == "Main diagonal")
		{
			m_user.setMainDiagonal(true);
			m_user.setHorizontal(false);
			m_user.setVertical(false);
			logger.Log("The neighbourhood has been set as \"Main diagonal\".", Logging::Level::INFO);
		}
		else if (static_cast<std::string>(m_neighbours.at(index).getString()) == "Secondary diagonal")
		{
			m_user.setSecondaryDiagonal(true);
			m_user.setHorizontal(false);
			m_user.setVertical(false);
			logger.Log("The neighbourhood has been set as \"Secondary diagonal\".", Logging::Level::INFO);
		}
		else if (static_cast<std::string>(m_neighbours.at(index).getString()) == "Left-right-up-down and diagonals")
		{
			m_user.setHorizontal(true);
			m_user.setVertical(true);
			m_user.setMainDiagonal(true);
			m_user.setSecondaryDiagonal(true);
			logger.Log("The neighbourhood has been set as \"Left-right-up-down and diagonals\".", Logging::Level::INFO);
		}
	}
	
	index = 0;
	while (index < m_lightsOnColorPossibleValues.size() && m_lightsOnColorPossibleValues.at(index).getButtonShape().getFillColor() != m_pressed)
		index++;

	if (index < m_lightsOnColorPossibleValues.size())
	{
		m_user.setLightsOnColor(m_user.ConvertStringToColour("sf::Color::" + static_cast<std::string>(m_lightsColorNames.at(index).getString())));
		logger.Log("The colour of the lights that are on is " + static_cast<std::string>(m_lightsColorNames.at(index).getString()), Logging::Level::INFO);
	}

	index = 0;
	while (index < m_LightsOffColorPossibleValues.size() && m_LightsOffColorPossibleValues.at(index).getButtonShape().getFillColor() != m_pressed)
		index++;
	if (index < m_LightsOffColorPossibleValues.size())
	{
		m_user.setLightsOffColor(m_user.ConvertStringToColour("sf::Color::" + static_cast<std::string>(m_lightsColorNames.at(index).getString())));
		logger.Log("The colour of the lights that are off is " + static_cast<std::string>(m_lightsColorNames.at(index).getString()), Logging::Level::INFO);
	}

	if (m_yesFastGame.getButtonShape().getFillColor() == m_pressed)
	{
		m_user.setFastGame(true);
		logger.Log("The fast game mode has been chosen.", Logging::Level::INFO);
	}
	else
	{
		m_user.setFastGame(false);
		logger.Log("The normal game mode has been chosen.", Logging::Level::INFO);
	}
}