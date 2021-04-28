#include "Game.h"
#include "Player.h"
#include "Stopwatch.h"
#include "InputValidation.h"
#include "Timer.h"
#include "../Logging/Logging.h"
#include <iostream>
#include <fstream>

void Game::Intro(std::ostream& output)
{
	Logging logger(output);
	int16_t choice;
	char choiceAux;
	std::cout << "Welcome! Would you like to play the game, using the standard rules, or would you rather set it up first?\n";
	std::cout << "If you'd just like to play, please press 1.\n";
	std::cout << "If you'd like to manage the settings, please press 2.\n";
	std::cout << "If you started the game by accident, please press 3, to exit.\n";

	std::cin >> choiceAux;
	choice = choiceAux - '0';
	bool aux = TestChoice(choice);
	while (aux != true)
	{
		std::cin >> choiceAux;
		choice = choiceAux - '0';
		aux = TestChoice(choice);
	}
	logger.Log("The user selected option " + std::to_string(choice) + ".", Logging::Level::INFO);
	StartGame(output, choice);
}

void Game::Play(std::ostream& output, std::optional<User> settings, std::optional<bool> optionBoard)
{
	Player player;
	Rules rules;
	Board board;

	if (settings.has_value())
	{
		player.setRules(settings.value().GetRules());
	}
	else
	{
		ReadIntroText(output, "intro_text.txt");
		player.SetRules(output);
	}

	if (optionBoard.has_value() && optionBoard.value() == true)
	{
		std::cout << "\n you will play on the custom board you created\n";
		player.setBoard(settings.value().GetCustomBoard());
	}
	else if (settings.has_value() && player.GetRules().getNumberOfColumns() != player.GetRules().getNumberOfRows())
	{
		settings.value().SelectBoardType();
		player.setBoard(settings.value().GetUnregularBoard());
	}
	else
	{
		player.SetBoard(output);
	}

	rules = player.GetRules();
	board = player.GetBoard();

	if (rules.getFastGame() == true)
	{
		Timer timer;
		board.writeBoardOnConsole(rules.getLightsOnColour(), rules.getLightsOffColour());
		timer.useThreads(player);
	}
	else
	{
		Stopwatch stopwatch;
		board.setColumnsNumber(rules.getNumberOfColumns());
		board.setLinesNumber(rules.getNumberOfRows());
		board.writeBoardOnConsole(rules.getLightsOnColour(), rules.getLightsOffColour());
		stopwatch.setStartClock();
		player.Play(std::nullopt);
		stopwatch.setEndClock();
		std::cout << "\nAmazing, you won! And you managed to do so in only " << stopwatch.getDuration() << " seconds.";
	}
}

void Game::ManageSettings(std::ostream& output)
{
	Logging logger(output);
	User user;
	int16_t optionMap;
	char optionMapAux;
	bool optionBoard = false;

	std::cout << "\nSo, you chose to manage the settings. Great! I need you to choose a username.\nBut first, here's what you should pay attention to:\n";
	user.setUserName();
	logger.Log("The username was set to " + user.getUserName() + ".", Logging::Level::INFO);
	std::cout << "\n" << "Well, hello " << user.getUserName() << ". Now you will choose the rules for the game.\n";
	
	user.SetRules(output);

	if (user.getPersonalizedMapOpt())
	{
		std::cout << "\nHere's how this works: We will start with a 5x5 map, with all the lights off. You will choose the lights on which positions\n";
		std::cout << "you want to turn on, and that light and its neighbours (horizontally and vertically) are going to toggle their on/off state.\n";
		user.SetCustomMap();
		optionBoard = true;
		logger.Log("The personalized board has been saved.", Logging::Level::INFO);
	}

	std::cout << "\nGreat, you're all set. Now you can play, using the settings you just set.\n";
	Play(output, user, optionBoard);
}

void Game::StartGame(std::ostream& output, int16_t choice)
{
	Logging logger(output);
	try {
		switch (choice)
		{
		case 1:
			Play(output, std::nullopt, std::nullopt);
			break;
		case 2:
			ManageSettings(output);
			break;
		case 3:
			std::cout << "\nOkay, game stopped. Hope you'll play next time!";
			break;
		default:
			throw "The option the user entered was incorrect!";
		}
	}
	catch (const char* message)
	{
		logger.Log(message, Logging::Level::ERROR);
	}
}

void Game::ReadIntroText(std::ostream& output, std::string filePath)
{
	Logging logger(output);
	std::ifstream inputFile(filePath);
	try {
		if (!inputFile)
		{
			throw "\nCouldn't find the input file\n";
			return;
		}
	}
	catch (const char* message)
	{
		logger.Log(message, Logging::Level::ERROR);
	}

	while (!inputFile.eof())
	{
		char in;
		inputFile.get(in);
		std::cout << in;
	}
	std::cout << "\n";
}

bool Game::TestChoice(int16_t choice)
{
	if ((choice == 1) || (choice == 2) || (choice == 3))
	{
		return true;
	}
	std::cout << "\nSorry, wrong choice. Choose again: ";
	return false;
}

bool Game::TestInputOption(int16_t choice)
{
	if ((choice == 1) || (choice == 2))
	{
		return true;
	}
	std::cout << "\nSorry, wrong choice. Choose again: ";
	return false;
}
