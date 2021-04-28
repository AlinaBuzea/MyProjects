#include "Logging.h"

std::string Logging::ConvertLevelToString(Level level)
{
	switch (level) {
	case Level::INFO:
		return "INFO";
	case Level::WARNING:
		return "WARNING";
	case Level::ERROR:
		return "ERROR";
	default:
		return "";
	}
}

// Get current date/time, format is YYYY-MM-DD.HH:mm:ss
const std::string Logging::currentDateTime()
{
	time_t     now = time(0);
	struct tm  tstruct;
	char       buf[80];
#pragma warning(suppress : 4996)
	tstruct = *localtime(&now);
	strftime(buf, sizeof(buf), "%Y-%m-%d.%X ", &tstruct);

	return buf;
}

Logging::Logging(std::ostream& outFlux)
	: m_outFlux{ outFlux }
{
}

void Logging::Log(const std::string& message, Level level)
{
	Log(message.c_str(), level);
}

void Logging::Log(const char* message, Level level)
{
	m_outFlux << "[" << ConvertLevelToString(level) << "] " << currentDateTime() << " " << message << "\n";
}
