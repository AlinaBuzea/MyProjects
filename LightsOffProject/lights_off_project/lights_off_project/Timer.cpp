#include "Timer.h"
#include <thread>

constexpr int COUNTDOWN = 5;

Timer::Timer()
{
}

Timer::Timer(const Timer& other)
{
	m_startClock = other.m_startClock;
	m_endClock = other.m_endClock;
}

Timer& Timer::operator=(const Timer& other)
{
	if (this != &other)
	{
		m_startClock = other.m_startClock;
		m_endClock = other.m_endClock;
	}

	return *this;
}

void Timer::setStartClock()
{
	m_startClock = std::chrono::system_clock::now();
}

void Timer::setEndClock()
{
	m_endClock = m_startClock + std::chrono::seconds(COUNTDOWN);
}

std::chrono::time_point<std::chrono::system_clock> Timer::getCurrentTime()
{
	return std::chrono::system_clock::now();
}

std::chrono::time_point<std::chrono::system_clock> Timer::getEndClock()
{
	return m_endClock;
}

void Timer::Countdown(std::atomic<int>* stopGame)
{
	setStartClock();
	setEndClock();
	while (getCurrentTime() < m_endClock)
	{
		*stopGame = 0;
	}
	*stopGame = 1;
}

void Timer::useThreads(Player& player)
{
	std::atomic<int> stopGame;
	stopGame = 0;

	std::thread countdownThread(&Timer::Countdown, this, &stopGame);
	std::thread gameThread(&Player::Play, player, &stopGame);

	countdownThread.join();
	gameThread.join();
}