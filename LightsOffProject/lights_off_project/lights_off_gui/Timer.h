#pragma once
#include <SFML/Graphics.hpp>

const int FAST_GAME_DEFAULT = 20;
const int FAST_GAME_9 = 30;
const int FAST_GAME_7 = 23;
const int FAST_GAME_5 = 15;
const int FAST_GAME_3 = 7;

class Timer
{
private: 
	bool m_fastGame;
	bool m_stopGame;

	int m_seconds;
	int m_minutes;
	int m_hours;

	int m_boardType;

	sf::Text m_timeText;
	sf::Font m_font;

	sf::Time m_time;
	sf::Clock m_clock;

private:
	std::string setStr();
	sf::Font setFont(const std::string& filePath);
	void updateStopGame();
	
public:
	Timer(bool fastGame);
	Timer() = default;
	Timer(const Timer& timer);
	~Timer();
	  
	void update(sf::RenderWindow* window);
	void Initialize();

	void setBoardType(int boardType);

	sf::Text getTime();
	int getSeconds();
	bool getStopGame();
	void drawTimeIsUp(sf::RenderWindow* window);
};

