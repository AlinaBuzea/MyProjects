#pragma once
#include "Board.h"

bool IsNumber(const std::string&);
void IsCustomMapCoordinates(int16_t&, int16_t&);
bool IsBoardSize(const std::string&);
bool IsCorrectUsername(const std::string&);

void CoordinateValidation(int16_t& coordX, int16_t& coordY, Board board);
bool ValidFastModeOption(int16_t fastModeOption);

bool TestOption(int16_t);