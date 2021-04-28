#pragma once
#include <optional>
#include "User.h"

class Game
{
public:
	void Intro(std::ostream& outFlux);
	bool TestChoice(int16_t);
	bool TestInputOption(int16_t);

private:
	void ReadIntroText(std::ostream& output, std::string filePath);
	void Play(std::ostream& output, std::optional<User> settings, std::optional<bool> optionBoard);
	void ManageSettings(std::ostream& output);
	void StartGame(std::ostream& output, int16_t choice);
};

