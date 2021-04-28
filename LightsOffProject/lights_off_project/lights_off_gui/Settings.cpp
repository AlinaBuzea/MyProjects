#include "Settings.h"
#include "MainMenu.h"

Settings::Settings()
{
}

Settings::~Settings()
{
}

Button Settings::initializeButton(float coordX, float coordY, float buttonWidth, float buttonHeight, sf::Color buttonColor, sf::Color hoverColour, sf::Color pressedColour)
{
	Button button;
	button.setShapeOption(0);
	button.setShape(coordX, coordY, buttonWidth, buttonHeight);
	button.setButtonColour(buttonColor);
	button.setHoverColour(hoverColour);
	button.setPressedColour(pressedColour);
	return button;
}

sf::Text Settings::defineText(const std::string& text, int size, sf::Color colour, sf::Uint32 style, sf::Vector2f position)
{
	sf::Text title;
	title.setString(text);
	title.setCharacterSize(size);
	title.setFillColor(colour);
	title.setStyle(style);
	title.setPosition(position);
	return title;
}

sf::Font Settings::defineFont(const std::string & FilePath)
{
	Logging logger(std::cout);
	sf::Font font;
	if (!font.loadFromFile(FilePath))
	{
		logger.Log("Font file not found.", Logging::Level::ERROR);
	}
	return font;
}

void Settings::initializeElements(int design_option)
{
	float buttonWidth = 250.0f;
	float buttonHeight = 50.0f;
	float spaceBetweenElements = 20.0f;
	m_designOption = design_option;

	sf::Color buttonColor, hoverColor, backgroundColor;

	if (m_designOption == 1)
	{
		backgroundColor = sf::Color(73, 114, 229);
		buttonColor = sf::Color(255, 51, 153);
		hoverColor = sf::Color(255, 102, 178);
	}
	else if (m_designOption == 2)
	{
		backgroundColor = sf::Color(229, 229, 220);
		buttonColor = sf::Color(198, 107, 61);
		hoverColor = sf::Color(198, 107, 61, 120);
	}
	else if (m_designOption == 3)
	{
		backgroundColor = sf::Color(156, 155, 228);
		buttonColor = sf::Color(213, 169, 247);
		hoverColor = sf::Color(213, 169, 247, 120);
	}
	else if (m_designOption == 4)
	{
		backgroundColor = sf::Color(235, 212, 226);
		buttonColor = sf::Color(222, 119, 185);
		hoverColor = sf::Color(222, 119, 185, 120);
	}
	else
	{
		buttonColor = sf::Color(35, 100, 119);
		hoverColor = sf::Color(50, 174, 78);
		backgroundColor = sf::Color(207, 223, 218);
	}
	int characterSize = 20;
	
	WindowProprieties winProp(700, 700, backgroundColor);
	m_winProprieties = winProp;

	m_instructionsButton = initializeButton(630.f, 10.f, 50.f, 50.f, sf::Color(198, 107, 61, 0), hoverColor, sf::Color(198, 107, 61, 0));
	m_instructionsButton.setFont(defineFont("..\\COOPBL.TTF"));
	m_instructionsButton.setText("i");

	m_personalizedMapButton = initializeButton((m_winProprieties.getWidth() - buttonWidth) / 2.f,
		(m_winProprieties.getHeight() - buttonHeight) / 2.f, buttonWidth, buttonHeight, buttonColor, hoverColor, buttonColor);
	m_personalizedMapButton.setFont(defineFont("..\\COOPBL.TTF"));
	m_personalizedMapButton.setText("Personalized Map");

	m_setConfigurationsButton = initializeButton((m_winProprieties.getWidth() - buttonWidth) / 2.f,
		m_personalizedMapButton.getButtonShape().getPosition().y + buttonHeight + spaceBetweenElements, buttonWidth, buttonHeight, buttonColor,
		hoverColor, buttonColor);
	m_setConfigurationsButton.setFont(defineFont("..\\COOPBL.TTF"));
	m_setConfigurationsButton.setText("Set Configurations");

	m_backToMenuButton = initializeButton((m_winProprieties.getWidth() - buttonWidth) / 2.f,
		m_setConfigurationsButton.getButtonShape().getPosition().y + buttonHeight + spaceBetweenElements, buttonWidth, buttonHeight, buttonColor,
		hoverColor, buttonColor);
	m_backToMenuButton.setFont(defineFont("..\\COOPBL.TTF"));
	m_backToMenuButton.setText("Back to Menu");
}

void Settings::ShowWindow()
{
	Logging logger(std::cout);
	sf::RenderWindow window(sf::VideoMode(m_winProprieties.getWidth(), m_winProprieties.getHeight()), "Settings");
	window.clear(m_winProprieties.getColor());

	while (window.isOpen())
	{
		sf::Event event;
		while (window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
				window.close();
		}

		window.clear(m_winProprieties.getColor());

		sf::Vector2f mousePosition = static_cast<sf::Vector2f>(sf::Mouse::getPosition(window));

		m_instructionsButton.update(mousePosition);
		m_instructionsButton.drawButton(&window);
		if (m_instructionsButton.isPressed())
		{
			logger.Log("The user asked for some additional information.", Logging::Level::INFO);
			Instructions();
		}

		m_personalizedMapButton.update(mousePosition);
		m_personalizedMapButton.drawButton(&window);
		if (m_personalizedMapButton.isPressed())
		{
			logger.Log("The user pressed the \"Personalized map\" button.", Logging::Level::INFO);
			User pmUser;
			pmUser.setPersonalizedMapOption(true);
			PersonalizedMap persMap(pmUser);
			persMap.DrawMap(m_designOption);
			saveSettings(pmUser, persMap.getFileName());
		}

		m_setConfigurationsButton.update(mousePosition);
		m_setConfigurationsButton.drawButton(&window);
		if (m_setConfigurationsButton.isPressed())
		{
			logger.Log("The user pressed the \"Set Configurations\" button.", Logging::Level::INFO);
			SetConfigurations setConfig;
			setConfig.initializeElements(m_designOption);
			setConfig.showWindow();
		}

		m_backToMenuButton.update(mousePosition);
		m_backToMenuButton.drawButton(&window);
		if (m_backToMenuButton.isPressed())
		{
			logger.Log("The user pressed the \"Back to Menu\" button.", Logging::Level::INFO);
			window.close();
			MainMenu menu(m_designOption);
			menu.Menu();
		}

		window.display();
	}
}

void Settings::Instructions()
{
	sf::Color backgroundColour, textColour;
	if (m_designOption == 1)
	{
		backgroundColour = sf::Color(73, 114, 229);
		textColour = sf::Color::White;
	}
	else if (m_designOption == 2)
	{
		backgroundColour = sf::Color(229, 229, 220);
		textColour = sf::Color(38, 73, 92);
	}
	else if (m_designOption == 3)
	{
		backgroundColour = sf::Color(156, 155, 228);
		textColour = sf::Color(235, 225, 231);
	}
	else if (m_designOption == 4)
	{
		backgroundColour = sf::Color(235, 212, 226);
		textColour = sf::Color::White;
	}
	else
	{
		backgroundColour = sf::Color(207, 223, 218);
		textColour = sf::Color(23, 48, 66);
	}

	WindowProprieties winProp(555, 605, backgroundColour);
	sf::RenderWindow window2(sf::VideoMode(winProp.getWidth(), winProp.getHeight()), "Instructions", sf::Style::Close | sf::Style::Titlebar);
	window2.clear(winProp.getColor());

	std::string inputText = readInputFile("instructions_text.txt");
	sf::Text text = defineText(inputText, 20, textColour, sf::Text::Regular, sf::Vector2f(10.f, 0.f));
	sf::Font font = defineFont("..\\COOPBL.TTF");
	text.setFont(font);

	while (window2.isOpen())
	{
		sf::Event evnt;

		while (window2.pollEvent(evnt))
		{
			if (evnt.type == sf::Event::Closed)
				window2.close();
		}

		window2.clear(winProp.getColor());
		window2.draw(text);
		window2.display();
	}
}

void Settings::saveSettings(User pmUser, const std::string& personalizedMapFileName)
{
	Logging logger(std::cout);
	std::ofstream fileOut("Settings.txt");
	try {
		if (fileOut.is_open())
		{
			fileOut << "5 5\n";
			fileOut << "hv\n" << pmUser.ConvertColourToString(pmUser.getLightsOnColor()) << "\n"
				<< pmUser.ConvertColourToString(pmUser.getLightsOffColor()) << "\nnormal game\n1\n";
			if (pmUser.getPersonalizedMapOption())
			{
				fileOut << personalizedMapFileName;
			}
			fileOut.close();
			logger.Log("The settings have been saved.", Logging::Level::INFO);
		}
		else
		{
			throw (std::string)"This file can not be opened";
		}
	}
	catch (std::string message)
	{
		logger.Log(message, Logging::Level::ERROR);
	}
	
}

std::string Settings::readInputFile(const std::string& filePath)
{
	Logging logger(std::cout);
	std::ifstream inputFile(filePath);

	if (!inputFile)
	{
		logger.Log("Couldn't read the input file.", Logging::Level::ERROR);
		return "Error reading file";
	}

	std::string inputText;

	inputText.assign((std::istreambuf_iterator<char>(inputFile)),
		(std::istreambuf_iterator<char>()));

	inputFile.close();

	return inputText;
}