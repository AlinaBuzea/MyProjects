#include "MainMenu.h"
#include "Timer.h"
#include "FinalWindow.h"
#include "Settings.h"
#include "PlayBoard.h"
#include "User.h"
#include <fstream>
#include <chrono>
#include <thread>

sf::Font MainMenu::defineFont(const std::string& filePath)
{
	Logging logger(std::cout);
	sf::Font font;
	try
	{
		if (!font.loadFromFile(filePath))
		{
			throw "Can't find font.";
		}
	}
	catch (const char* message)
	{
		logger.Log(message, Logging::Level::ERROR);
	};
	return font;
}

sf::Text MainMenu::defineText(const std::string& text, int size, sf::Color colour, sf::Uint32 style, sf::Vector2f position)
{
	sf::Text title;
	title.setString(text);
	title.setCharacterSize(size);
	title.setFillColor(colour);
	title.setStyle(style);
	title.setPosition(position);
	return title;
}

std::string MainMenu::readInputFile(const std::string& filePath)
{
	Logging logger(std::cout);
	std::ifstream inputFile(filePath);

	if (!inputFile)
	{
		logger.Log("Couldn't find " + filePath + " input file!", Logging::Level::ERROR);
		return "Error reading file!";
	}

	std::string inputText;

	inputText.assign((std::istreambuf_iterator<char>(inputFile)),
		(std::istreambuf_iterator<char>()));

	inputFile.close();

	return inputText;
}

void MainMenu::resetFile(const std::string& settings, const std::string& standardSettings)
{
	Logging logger(std::cout);
	std::ofstream outputFile(settings);
	std::string inputText = readInputFile(standardSettings);

	if (!outputFile)
	{
		logger.Log("Couldn't find " + settings + " input file!", Logging::Level::ERROR);
		return;
	}

	outputFile << inputText;
	outputFile.close();
}

void MainMenu::setButtonColours(Button& button, sf::Color buttonColour, sf::Color hoverColour)
{
	button.setButtonColour(buttonColour);
	button.setHoverColour(hoverColour);
	button.setPressedColour(buttonColour);
}

MainMenu::MainMenu()
{
	m_designOption = 5;
}

MainMenu::MainMenu(int designOption)
{
	m_designOption = designOption;
}

void MainMenu::Menu()
{
	Logging logger(std::cout);
	sf::Color buttonColour, hoverColour, backgroundColour, textColour;
	if (m_designOption == 1)
	{
		backgroundColour = sf::Color(73, 114, 229);
		buttonColour = sf::Color(255, 51, 153);
		hoverColour = sf::Color(255, 102, 178);
		textColour = sf::Color::White;
	}
	else if (m_designOption == 2)
	{
		backgroundColour = sf::Color(229, 229, 220);
		textColour = sf::Color(38, 73, 92);
		buttonColour = sf::Color(198, 107, 61);
		hoverColour = sf::Color(198, 107, 61, 120);
	}
	else if (m_designOption == 3)
	{
		backgroundColour = sf::Color(156, 155, 228);
		textColour = sf::Color(235, 225, 231);
		buttonColour = sf::Color(213, 169, 247);
		hoverColour = sf::Color(213, 169, 247, 120);
	}
	else if (m_designOption == 4)
	{
		backgroundColour = sf::Color(235, 212, 226);
		textColour = sf::Color::White;
		buttonColour = sf::Color(222, 119, 185);
		hoverColour = sf::Color(222, 119, 185, 120);
	}
	else
	{
		backgroundColour = sf::Color(207, 223, 218);
		textColour = sf::Color(23, 48, 66);
		buttonColour = sf::Color(35, 100, 119);
		hoverColour = sf::Color(50, 174, 78);
	}

	WindowProprieties winProp(700, 700, backgroundColour);
	sf::RenderWindow window(sf::VideoMode(winProp.getWidth(), winProp.getHeight()), "Menu", sf::Style::Close | sf::Style::Titlebar);
	window.clear(winProp.getColor());

	sf::Font font = defineFont("..\\COOPBL.TTF");
	sf::Text title = defineText("Welcome to Lights Off!", 40, textColour, sf::Text::Bold, sf::Vector2f(100.f, 20.f));
	title.setFont(font);

	sf::Text designOption = defineText("Design option: ", 20, textColour, sf::Text::Bold, sf::Vector2f(10.f, 643.f));
	designOption.setFont(font);
	Button option1(0, 190.f, 630.f, 40.f, 40.f, sf::Color(0, 172, 214, 0), hoverColour, sf::Color(0, 172, 214, 0), "1", font); //Alina
	Button option2(0, 230.f, 630.f, 40.f, 40.f, sf::Color(0, 172, 214, 0), hoverColour, sf::Color(0, 172, 214, 0), "2", font); //Timea
	Button option3(0, 270.f, 630.f, 40.f, 40.f, sf::Color(0, 172, 214, 0), hoverColour, sf::Color(0, 172, 214, 0), "3", font); //Ildiko
	Button option4(0, 310.f, 630.f, 40.f, 40.f, sf::Color(0, 172, 214, 0), hoverColour, sf::Color(0, 172, 214, 0), "4", font); //Amalia
	Button option5(0, 350.f, 630.f, 40.f, 40.f, sf::Color(0, 172, 214, 0), hoverColour, sf::Color(0, 172, 214, 0), "5", font); //Silviu

	Button buttonPlay(0, 275.f, 415.f, 150.f, 70.f, buttonColour, hoverColour, buttonColour, "Play", font);
	Button buttonSettings(0, 275.f, 275.f, 150.f, 70.f, buttonColour, hoverColour, buttonColour, "Settings", font);
	Button buttonInfo(0, 630.f, 630.f, 50.f, 50.f, sf::Color(0, 172, 214, 0), hoverColour, sf::Color(0, 172, 214, 0), "i", font);

	while (window.isOpen())
	{
		sf::Event evnt;
		while (window.pollEvent(evnt))
		{
			if (evnt.type == sf::Event::Closed)
			{
				resetFile("Settings.txt", "settings_backup.txt");
				window.close();
			}
		}

		window.clear(winProp.getColor());
		window.draw(title);
		window.draw(designOption);

		sf::Vector2f mousePosition = static_cast<sf::Vector2f>(sf::Mouse::getPosition(window));

		option1.update(mousePosition);
		if (option1.isPressed())
		{
			logger.Log("Design option number 1 selected.", Logging::Level::INFO);
			m_designOption = 1;
			winProp.setColor(sf::Color(73, 114, 229));
			title.setFillColor(sf::Color::White);
			designOption.setFillColor(sf::Color::White);
			buttonColour = sf::Color(255, 51, 153);
			hoverColour = sf::Color(255, 102, 178);
			setButtonColours(buttonPlay, buttonColour, hoverColour);
			setButtonColours(buttonSettings, buttonColour, hoverColour);
			setButtonColours(buttonInfo, sf::Color(255, 51, 153, 0), hoverColour);
			setButtonColours(option1, sf::Color(255, 51, 153, 0), hoverColour);
			setButtonColours(option2, sf::Color(255, 51, 153, 0), hoverColour);
			setButtonColours(option3, sf::Color(255, 51, 153, 0), hoverColour);
			setButtonColours(option4, sf::Color(0, 172, 214, 0), hoverColour);
			setButtonColours(option5, sf::Color(0, 172, 214, 0), hoverColour);
		}
		option1.drawButton(&window);

		option2.update(mousePosition);
		if (option2.isPressed())
		{
			logger.Log("Design option number 2 selected.", Logging::Level::INFO);
			m_designOption = 2;
			winProp.setColor(sf::Color(229, 229, 220));
			title.setFillColor(sf::Color(38, 73, 92));
			designOption.setFillColor(sf::Color(38, 73, 92));
			buttonColour = sf::Color(198, 107, 61);
			hoverColour = sf::Color(198, 107, 61, 120);
			setButtonColours(buttonPlay, buttonColour, hoverColour);
			setButtonColours(buttonSettings, buttonColour, hoverColour);
			setButtonColours(buttonInfo, sf::Color(198, 107, 61, 0), hoverColour);
			setButtonColours(option1, sf::Color(198, 107, 61, 0), hoverColour);
			setButtonColours(option2, sf::Color(198, 107, 61, 0), hoverColour);
			setButtonColours(option3, sf::Color(198, 107, 61, 0), hoverColour);
			setButtonColours(option4, sf::Color(0, 172, 214, 0), hoverColour);
			setButtonColours(option5, sf::Color(0, 172, 214, 0), hoverColour);
		}
		option2.drawButton(&window);

		option3.update(mousePosition);
		if (option3.isPressed())
		{
			logger.Log("Design option number 3 selected.", Logging::Level::INFO);
			m_designOption = 3;
			winProp.setColor(sf::Color(156, 155, 228));
			title.setFillColor(sf::Color(235, 225, 231));
			designOption.setFillColor(sf::Color(235, 225, 231));
			buttonColour = sf::Color(213, 169, 247);
			hoverColour = sf::Color(213, 169, 247, 120);
			setButtonColours(buttonPlay, buttonColour, hoverColour);
			setButtonColours(buttonSettings, buttonColour, hoverColour);
			setButtonColours(buttonInfo, sf::Color(213, 169, 247, 0), hoverColour);
			setButtonColours(option1, sf::Color(213, 169, 247, 0), hoverColour);
			setButtonColours(option2, sf::Color(213, 169, 247, 0), hoverColour);
			setButtonColours(option3, sf::Color(213, 169, 247, 0), hoverColour);
			setButtonColours(option4, sf::Color(213, 169, 247, 0), hoverColour);
			setButtonColours(option5, sf::Color(213, 169, 247, 0), hoverColour);
		}
		option3.drawButton(&window);

		option4.update(mousePosition);
		if (option4.isPressed())
		{
			logger.Log("Design option number 4 selected.", Logging::Level::INFO);
			m_designOption = 4;
			winProp.setColor(sf::Color(235, 212, 226));
			title.setFillColor(sf::Color::White);
			designOption.setFillColor(sf::Color::White);
			buttonColour = sf::Color(222, 119, 185);
			hoverColour = sf::Color(222, 119, 185, 120);
			setButtonColours(buttonPlay, buttonColour, hoverColour);
			setButtonColours(buttonSettings, buttonColour, hoverColour);
			setButtonColours(buttonInfo, sf::Color(217, 164, 198, 0), hoverColour);
			setButtonColours(option1, sf::Color(217, 164, 198, 0), hoverColour);
			setButtonColours(option2, sf::Color(217, 164, 198, 0), hoverColour);
			setButtonColours(option3, sf::Color(217, 164, 198, 0), hoverColour);
			setButtonColours(option4, sf::Color(217, 164, 198, 0), hoverColour);
			setButtonColours(option5, sf::Color(217, 164, 198, 0), hoverColour);
		}
		option4.drawButton(&window);

		option5.update(mousePosition);
		if (option5.isPressed())
		{
			logger.Log("Design option number 5 selected.", Logging::Level::INFO);
			m_designOption = 5;
			winProp.setColor(sf::Color(207, 223, 218));
			title.setFillColor(sf::Color(23, 48, 66)); 
			designOption.setFillColor(sf::Color(23, 48, 66));
			buttonColour = sf::Color(35, 100, 119); 
			hoverColour = sf::Color(50, 174, 78);
			setButtonColours(buttonPlay, buttonColour, hoverColour);
			setButtonColours(buttonSettings, buttonColour, hoverColour);
			setButtonColours(buttonInfo, sf::Color(0, 172, 214, 0), hoverColour);
			setButtonColours(option1, sf::Color(0, 172, 214, 0), hoverColour);
			setButtonColours(option2, sf::Color(0, 172, 214, 0), hoverColour);
			setButtonColours(option3, sf::Color(0, 172, 214, 0), hoverColour);
			setButtonColours(option4, sf::Color(0, 172, 214, 0), hoverColour);
			setButtonColours(option5, sf::Color(0, 172, 214, 0), hoverColour);
		}
		option5.drawButton(&window);

		buttonPlay.update(mousePosition);
		if (buttonPlay.isPressed())
		{
			logger.Log("\"Play\" button was pressed.", Logging::Level::INFO);
			window.close();
			Play();
		}
		buttonPlay.drawButton(&window);

		buttonSettings.update(mousePosition);
		if (buttonSettings.isPressed())
		{
			logger.Log("\"Settings\" button was pressed.", Logging::Level::INFO);
			window.close();
			Setting();
		}
		buttonSettings.drawButton(&window);

		buttonInfo.update(mousePosition);
		if (buttonInfo.isPressed())
		{
			logger.Log("The user asked for some additional information.", Logging::Level::INFO);
			Instructions();
		}
		buttonInfo.drawButton(&window);

		window.display();
	}
}

void MainMenu::Play()
{
	User user(true);
	user.setDesignOption(m_designOption);
	PlayBoard board(user);
	board.DrawBoard(m_designOption);
}

void MainMenu::Setting()
{
	Settings settings;
	settings.initializeElements(m_designOption);
	settings.ShowWindow();
}

void MainMenu::Instructions()
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

	WindowProprieties winProp(700, 700, backgroundColour);
	sf::RenderWindow window2(sf::VideoMode(450, 450), "How to play", sf::Style::Close | sf::Style::Titlebar);
	window2.clear(winProp.getColor());

	std::string input_text = readInputFile("intro_text.txt");
	sf::Text text = defineText(input_text, 20, textColour, sf::Text::Regular, sf::Vector2f(10.f, 0.f));
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