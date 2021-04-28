#pragma once
#include <optional>
#include <utility>
#include <atomic>
#include "User.h"

class Player
{
private:
	Rules m_rule;
	User m_user;
	int16_t m_optionUser;
	Board m_board;

	std::string SelectRandomFile(std::string boardType);
	std::pair<std::string, std::string> GetFileName();
	int RandomNumberGenerator(int start, int end);
	std::vector<std::vector<bool>> CreatingBoardMatrix(std::ostream& output);
	void SetNeighbours(int option);
	void showRules(const std::string& lightsOff, const std::string& lightsOn);
	void selectFastGame(std::ostream& output);

public:
	Player();
	Player(const Player&);
	Player& operator = (const Player&);

	void SetRules(std::ostream& output);
	void SetBoard(std::ostream& output);
	void setRules(Rules rule);
	void setBoard(Board board);

	Rules GetRules();
	Board GetBoard();

	void Play(std::optional<std::atomic<int>*>);
	bool NotSameColor(const std::string&, const std::string&);
};