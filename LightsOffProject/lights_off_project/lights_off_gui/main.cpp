#include <SFML/Graphics.hpp>
#include <iostream>
#include "WindowProprieties.h"
#include "Button.h"
#include "MainMenu.h"
#include "Timer.h"
#include "../Logging/Logging.h"
#include <SFML/Audio.hpp>
 
int main()
{
	Logging logger(std::cout);
	logger.Log("The app started running.", Logging::Level::INFO);

	sf::Music music;
	if (!music.openFromFile("11 Wake Me Up When September Ends.flac")) //04 Boulevard of Broken Dreams.flac, 10. Mirror Mirror.flac
		logger.Log("Couldn't find music", Logging::Level::ERROR);

	sf::Font font;
	try
	{
		if (!font.loadFromFile("..\\COOPBL.TTF"))
		{
			throw "Can't find font.";
		}
	}
	catch (const char* message)
	{
		logger.Log(message, Logging::Level::ERROR);
	};

	sf::RenderWindow window(sf::VideoMode(660, 150), "Music", sf::Style::Titlebar);
	Button buttonYes(0, 160.f, 80.f, 90.f, 50.f, sf::Color(35, 100, 119), sf::Color(50, 174, 78), sf::Color(35, 100, 119), "Yes", font);
	Button buttonNo(0, 410.f, 80.f, 90.f, 50.f, sf::Color(35, 100, 119), sf::Color(50, 174, 78), sf::Color(35, 100, 119), "No", font);

	sf::Text title;
	title.setString("Do you want to listen to a song while you use this app?");
	title.setCharacterSize(20);
	title.setFillColor(sf::Color(23, 48, 66));
	title.setStyle(sf::Text::Bold);
	title.setPosition(10.f, 10.f);
	title.setFont(font);
	
	window.clear(sf::Color(207, 223, 218));

	while (window.isOpen())
	{
		sf::Event evnt;
		while (window.pollEvent(evnt))
		{
			if (evnt.type == sf::Event::Closed)
			{
				window.close();
			}
		}
		sf::Vector2f mousePosition = static_cast<sf::Vector2f>(sf::Mouse::getPosition(window));

		window.clear(sf::Color(207, 223, 218));
		window.draw(title);

		buttonYes.update(mousePosition);
		if (buttonYes.isPressed())
		{
			logger.Log("The user wanted some music.", Logging::Level::INFO);
			music.play();
			window.close();
		}
		buttonYes.drawButton(&window);

		buttonNo.update(mousePosition);
		if (buttonNo.isPressed())
		{
			logger.Log("The user wanted no music.", Logging::Level::INFO);
			window.close();
		}
		buttonNo.drawButton(&window);

		window.display();
	}

	MainMenu menu;
	menu.Menu();

	return 0;
}