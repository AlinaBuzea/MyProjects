#include "InputValidation.h"
#include <random>
#include <regex>
#include <iostream>
#include "User.h"

bool IsNumber(const std::string& number)
{
	std::regex numbers_range{ "^[0-9]$" };
	if (std::regex_match(number, numbers_range))
	{
		return true;
	}
	return false;
}

bool IsBoardSize(const std::string& number)
{
	std::regex numbers_range{ "^[3-9]$" };
	if (std::regex_match(number, numbers_range))
	{
		return true;
	}
	return false;
}

void IsCustomMapCoordinates(int16_t& coordX, int16_t& coordY)
{
	char coordXAux, coordYAux;
	while (coordX >= 5 || coordX < 0 || coordY >= 5 || coordY < 0)
	{
		std::cout << "Please enter valid coordinates!\n";
		std::cout << "CoordX: ";
		std::cin >> coordXAux;
		std::cout << "CoordY: ";
		std::cin >> coordYAux;
		coordX = coordXAux - '0';
		coordY = coordYAux - '0';
	}
}

bool IsCorrectUsername(const std::string& username)
{
	/*
	Username requirements:
	-alphanumeric characters (a-zA-Z0-9), lowercase, or uppercase.
	- allowed of the dot (.), underscore (_), and hyphen (-).
	- dot, underscore, or hyphen must not be the first or last character and can not appear consecutively
	- number of characters must be between 5 to 20.*/

	std::regex usernameFormat{ "^[a-zA-Z0-9]([._-](?![._-])|[a-zA-Z0-9]){3,18}[a-zA-Z0-9]$" };

	if (std::regex_match(username, usernameFormat))
	{
		std::cout << "You chose your username wisely. Great job!\n";
		return true;
	}
	std::cout << "Incorrect username! Please enter again: \n";
	return false;
}

void CoordinateValidation(int16_t& coordX, int16_t& coordY, Board board)
{
	char coordXAux, coordYAux;
	while (coordX >= board.getLinesNumber() || coordX < 0 || coordY >= board.getColumnsNumber() || coordY < 0)
	{
		std::cout << "Please enter valid coordinates!\n";
		std::cout << "CoordX: ";
		std::cin >> coordXAux;
		std::cout << "CoordY: ";
		std::cin >> coordYAux;
		coordX = coordXAux - '0';
		coordY = coordYAux - '0';
	}
}

bool ValidFastModeOption(int16_t fastModeOption)
{
	if (fastModeOption != 0 && fastModeOption != 1)
	{
		return false;
	}
	return true;
}

bool TestOption(int16_t choice)
{
	if ((choice == 1) || (choice == 2))
	{
		return true;
	}
	std::cout << "\nSorry, wrong choice. Choose again: ";
	return false;
}

