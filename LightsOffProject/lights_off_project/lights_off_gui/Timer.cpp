#include "Timer.h"
#include "Button.h"
#include "MainMenu.h"
#include "FinalWindow.h"
#include <iostream>
#include <string>
#include <chrono>
#include <thread>

std::string Timer::setStr()
{
	std::string clock;
	clock = "";

	if (m_hours < 10)
	{
		clock = clock + "0" + std::to_string(m_hours);
	}
	else
	{
		clock = clock + std::to_string(m_hours);
	}
	clock = clock + ":";

	if (m_minutes < 10)
	{
		clock = clock + "0" + std::to_string(m_minutes);
	}
	else
	{
		clock = clock + std::to_string(m_minutes);
	}
	clock = clock + ":";

	if (m_seconds < 10)
	{
		clock = clock + "0" + std::to_string(m_seconds);
	}
	else
	{
		clock = clock + std::to_string(m_seconds);
	}

	return clock;
}

sf::Font Timer::setFont(const std::string& filePath)
{
	sf::Font font;
	try
	{
		if (!font.loadFromFile(filePath))
		{
			throw "Can't find font.";
		}
	}
	catch (const char*)
	{
	};
	return font;
}

void Timer::updateStopGame()
{
	if (m_boardType == 3 && m_seconds >= FAST_GAME_3)
	{
		m_stopGame = true;
	}

	else if (m_boardType == 5 && m_seconds >= FAST_GAME_5)
	{
		m_stopGame = true;
	}

	else if (m_boardType == 7 && m_seconds >= FAST_GAME_7)
	{
		m_stopGame = true;
	}

	else if (m_boardType == 9 && m_seconds >= FAST_GAME_9)
	{
		m_stopGame = true;
	}
	else if (m_seconds >= FAST_GAME_DEFAULT)
	{
		m_stopGame = true;
	}
}

Timer::Timer(bool fastGame)
{
	m_fastGame = fastGame;
	m_hours = 0;
	m_minutes = 0;
	m_seconds = 0;
	m_boardType = 0;
	m_stopGame = false;
	m_timeText.setString("");
	m_font = setFont("..\\COOPBL.TTF");
}

Timer::Timer(const Timer& timer)
{
	m_fastGame = timer.m_fastGame;
	m_hours = timer.m_hours;
	m_minutes = timer.m_minutes;
	m_seconds = timer.m_seconds;
	m_stopGame = timer.m_stopGame;
	m_timeText = timer.m_timeText;
	m_font = timer.m_font;
	m_boardType = timer.m_boardType;
}

Timer::~Timer()
{
}

void Timer::update(sf::RenderWindow* window)
{	
	m_time = m_clock.getElapsedTime();
	m_seconds = m_time.asSeconds();
	m_hours = m_seconds / 3600;
	m_minutes = (m_seconds - (m_hours * 3600)) / 60;
	m_seconds = m_seconds - (m_hours * 3600 + m_minutes * 60);

	m_timeText.setString(setStr());
	m_timeText.setPosition((0.0 + (window->getSize().x / 2.f) - m_timeText.getGlobalBounds().width / 2.f), 10);
	window->draw(m_timeText);

	if (m_fastGame == true)
	{
		updateStopGame();
	}
}

void Timer::Initialize()
{
	m_timeText.setString("");
	m_timeText.setFont(m_font);
	m_timeText.setCharacterSize(60);
	#pragma warning(suppress : 4996)
	m_timeText.setColor(sf::Color::Black);
}

void Timer::setBoardType(int boardType)
{
	m_boardType = boardType;
}

sf::Text Timer::getTime()
{
	return m_timeText;
}

int Timer::getSeconds()
{
	return m_seconds;
}

bool Timer::getStopGame()
{
	return m_stopGame;
}

void Timer::drawTimeIsUp(sf::RenderWindow* window)
{
	sf::Text text;
	text.setString("Time's up!");
	text.setFont(m_font);
	text.setCharacterSize(60);
	#pragma warning(suppress : 4996)
	text.setColor(sf::Color::Black);
	text.setPosition(
		(0.0 + (window->getSize().x / 2.f) - m_timeText.getGlobalBounds().width / 2.f),
		(0.0 + (window->getSize().y / 2.f) - m_timeText.getGlobalBounds().height / 2.f)
	);
	window->draw(text);
}
