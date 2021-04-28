#include "Game.h"
#include "InputValidation.h"
#include <iostream>
#include "../Logging/Logging.h"

std::string readInputFile(const std::string& filePath)
{
	std::ifstream inputFile(filePath);

	if (!inputFile)
	{
		return "Error reading file!";
	}

	std::string inputText;

	inputText.assign((std::istreambuf_iterator<char>(inputFile)),
		(std::istreambuf_iterator<char>()));

	inputFile.close();

	return inputText;
}

void resetFile(const std::string& log, const std::string& oldLog)
{
	Logging logger(std::cout);
	std::ofstream outputFile(oldLog);
	std::string inputText = readInputFile(log);

	if (!outputFile)
	{
		return;
	}

	outputFile << inputText;
	outputFile.close();
}

int readOption()
{
	char play;
	int16_t playNum;
	std::cout << "\nPress 1 to start the game. Press 2 to leave. ";
	std::cin >> play;
	playNum = play - '0';

	while (TestOption(playNum) == false)
	{
		std::cin >> play;
		playNum = play - '0';
	}
	return playNum;
}

int main()
{
	Game game;
	int16_t option;

	std::ofstream logOutput("log.txt", std::ios::app);
	Logging logger(logOutput);

	option = readOption();

	while (option == 1)
	{
		logger.Log("The game started.", Logging::Level::INFO);
		game.Intro(logOutput);
		option = readOption();
	}

	logger.Log("The user chose to exit the game.", Logging::Level::INFO);
	logOutput.close();

	resetFile("log.txt", "log_old.txt");
	remove("log.txt");
	return 0;
}