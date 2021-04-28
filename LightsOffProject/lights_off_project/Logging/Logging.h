#pragma once
#include <iostream>
#include <fstream>
#include <string>

#ifdef LOGGING_EXPORTS
#define LOGGING_API __declspec(dllexport)
#else
#define LOGGING_API __declspec(dllimport)
#endif

class LOGGING_API Logging
{
public:
	enum class Level {
		INFO,
		WARNING,
		ERROR,
	};

	Logging(std::ostream& outFlux);

	void Log(const std::string& message, Level level = Level::ERROR);

	void Log(const char* message, Level level = Level::ERROR);

private:
	std::string ConvertLevelToString(Level level);

	const std::string currentDateTime();

	std::ostream& m_outFlux;
};

