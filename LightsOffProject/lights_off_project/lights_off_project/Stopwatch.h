#pragma once
#include <chrono>

class Stopwatch
{
private:
	//Class template std::chrono::time_point represents a point in time. 
	//It is implemented as if it stores a value of type Duration indicating 
	//the time interval from the start of the Clock's epoch.
	std::chrono::time_point<std::chrono::steady_clock> m_startClock;
	std::chrono::time_point<std::chrono::steady_clock> m_endClock;

public:
	Stopwatch();
	Stopwatch(const Stopwatch&);
	Stopwatch& operator = (const Stopwatch&);

	void setStartClock();
	void setEndClock();
	int getDuration();
};

