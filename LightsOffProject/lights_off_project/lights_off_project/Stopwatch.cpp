#include "Stopwatch.h"

Stopwatch::Stopwatch()
{
}

Stopwatch::Stopwatch(const Stopwatch& other)
{
	m_startClock = other.m_startClock;
	m_endClock = other.m_endClock;
}

Stopwatch& Stopwatch::operator=(const Stopwatch& other)
{
	if (this != &other)
	{
		m_startClock = other.m_startClock;
		m_endClock = other.m_endClock;
	}
	return *this;
}

void Stopwatch::setStartClock()
{
	m_startClock = std::chrono::steady_clock::now(); //Returns a time point representing the current point in time
}

void Stopwatch::setEndClock()
{
	m_endClock = std::chrono::steady_clock::now();
}

int Stopwatch::getDuration()
{
	return std::chrono::duration_cast<std::chrono::seconds>(m_endClock - m_startClock).count();
}
