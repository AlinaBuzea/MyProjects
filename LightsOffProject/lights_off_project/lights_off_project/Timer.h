#pragma once
#include "Player.h"
#include <chrono>

class Timer
{
private:
	//Class template std::chrono::time_point represents a point in time. 
	//It is implemented as if it stores a value of type Duration indicating 
	//the time interval from the start of the Clock's epoch.
	std::chrono::time_point<std::chrono::system_clock> m_startClock;
	std::chrono::time_point<std::chrono::system_clock> m_endClock;

public:
	Timer();
	Timer(const Timer&);
	Timer& operator = (const Timer&);

	void setStartClock();
	void setEndClock();
	std::chrono::time_point<std::chrono::system_clock> getCurrentTime();
	std::chrono::time_point<std::chrono::system_clock> getEndClock();
	void Countdown(std::atomic<int>*);
	void useThreads(Player& player);
};