#pragma once
#include "Board.h"
#include <fstream>

class User
{
private:
	Rules m_rule;
	std::string m_username = "";
	std::vector<std::vector<bool>>m_personalizedMap;
	std::vector<std::vector<bool>>m_unregularBoard;
	std::string m_neighborOptionName = "";
	std::string m_boardSize = "";
	std::string m_fileName = "";
	std::string m_exampleNumber = "";
	int16_t m_rowsNumber, m_columnsNumber;
	bool m_personalizedMapOption;

	std::string SetLight();
	void SetNeighborOption(std::ostream& output, bool&);
	void SetRowColumnNumber(std::ostream& output, int16_t opt);

	void WriteBoardToFile(std::vector<std::vector<bool>>&) const;
	void InitializeMap(std::vector<std::vector<bool>>&);
	void SolvableMap(int16_t, int16_t, std::vector<std::vector<bool>>&);

	int16_t RandomNumberGenerator(int16_t, int16_t);

public:
	User();
	User(const User& other);
	User& operator = (const User&);
	~User();

	void setUserName();
	void SetRules(std::ostream& output);
	void SetCustomMap();
	void SetTimeType(std::ostream& output);

	std::string getUserName();
	Rules GetRules() const;
	Board GetCustomBoard();
	Board GetUnregularBoard();
	std::string getBoardType();
	bool getPersonalizedMapOpt();

	void SelectBoardType();
	void CreatingUnregularBoard();

	bool IsLightColour(const std::string&);
	bool NotSameColor(const std::string&, const std::string&);
};

