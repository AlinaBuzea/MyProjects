#include "pch.h"
#include "CppUnitTest.h"
#include "../lights_off_project/Rules.h"
#include "../lights_off_project/Board.h"
#include "../lights_off_project/Player.h"
#include "../lights_off_project/User.h"
#include "../lights_off_project/Game.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace UnitTests
{
	TEST_CLASS(LightsOffUnitTests)
	{
	public:

		//Rules tests
		TEST_METHOD(ConstructorStandardGame)
		{
			Rules rule(true);

			Assert::IsTrue(true == rule.getHorizontal());
			Assert::IsTrue(true == rule.getVertical());
			Assert::IsTrue(false == rule.getMainDiagonal());
			Assert::IsTrue(false == rule.getSecondaryDiagonal());
			Assert::IsTrue(false == rule.getFastGame());

			Assert::IsTrue(5 == rule.getNumberOfRows());
			Assert::IsTrue(5 == rule.getNumberOfColumns());

			Assert::IsTrue((uint16_t)Rules::Colours::bright_white == rule.getLightsOffColour());
			Assert::IsTrue((uint16_t)Rules::Colours::bright_yellow == rule.getLightsOnColour());
		}

		TEST_METHOD(Constructor)
		{
			Rules rule(true, false, true, false, false, 7, 7, Rules::Colours::bright_white, Rules::Colours::bright_yellow);

			Assert::IsTrue(true == rule.getHorizontal());
			Assert::IsTrue(false == rule.getVertical());
			Assert::IsTrue(true == rule.getMainDiagonal());
			Assert::IsTrue(false == rule.getSecondaryDiagonal());
			Assert::IsTrue(false == rule.getFastGame());

			Assert::IsTrue(7 == rule.getNumberOfRows());
			Assert::IsTrue(7 == rule.getNumberOfColumns());

			Assert::IsTrue((uint16_t)Rules::Colours::bright_white == rule.getLightsOffColour());
			Assert::IsTrue((uint16_t)Rules::Colours::bright_yellow == rule.getLightsOnColour());
		}

		TEST_METHOD(convertStringToColours)
		{
			Rules rule;

			Assert::AreEqual(Rules::Colours::blue, rule.ConvertStringToColour("blue"));
			Assert::AreEqual(Rules::Colours::bright_blue, rule.ConvertStringToColour(static_cast<std::string>("bright blue")));
			Assert::AreEqual(Rules::Colours::bright_white, rule.ConvertStringToColour(static_cast<std::string>("bright white")));
			Assert::ExpectException<const char*>([&]() {
				rule.ConvertStringToColour(static_cast<std::string>("pink"));
				});
		}

		TEST_METHOD(ConvertColourToInt)
		{
			Rules rule;

			Assert::AreEqual(static_cast<int16_t>(1), rule.ConvertColourToInt(Rules::Colours::blue));
			Assert::AreEqual(static_cast<int16_t>(9), rule.ConvertColourToInt(Rules::Colours::bright_blue));
			Assert::AreEqual(static_cast<int16_t>(15), rule.ConvertColourToInt(Rules::Colours::bright_white));
		}

		//Board tests
		TEST_METHOD(TestBoardAllLightsAreOff)
		{
			uint16_t lines_no = 5, columns_no = 5;
			std::vector<std::vector<bool>> board = { {false, false, false, false, false},
													{false, false, false, false, false},
													{false, false, false, false, false},
													{false, false, false, false, false},
													{false, false, false, false, false} };
			Board b(lines_no, columns_no, board);

			bool result = b.allLightsAreOff();
			bool expected_result = true;
			Assert::AreEqual(expected_result, result);
		}

		TEST_METHOD(TestBoardAllLightsAreOff_SomeLightsAreOn)
		{
			uint16_t lines_no = 5, columns_no = 5;
			std::vector<std::vector<bool>> board = { {false, false, false, false, false},
													{false, false, false, false, false},
													{false, false, false, true, false},
													{false, false, false, false, false},
													{true, false, false, false, false} };
			Board b(lines_no, columns_no, board);

			bool result = b.allLightsAreOff();
			bool expected_result = false;
			Assert::AreEqual(expected_result, result);
		}

		TEST_METHOD(TestClickChangeLights)
		{
			uint16_t lines_no = 5, columns_no = 5;
			uint16_t line = 4, column = 1;
			std::vector<std::vector<bool>> board = { {false, false, false, false, false},
													{false, false, false, false, false},
													{false, false, false, false, false},
													{false, false, false, false, false},
													{false, false, false, false, false} };

			Board b_result(lines_no, columns_no, board);
			Rules rule;
			rule.setHorizontal(true);
			rule.setVertical(true);
			b_result.click(line, column, rule);
			std::vector<std::vector<bool>> board_expected_result = { {false, false, false, false, false},
													{false, false, false, false, false},
													{false, false, false, false, false},
													{false, true, false, false, false},
													{true, true, true, false, false} };
			Board b_expected_result(lines_no, columns_no, board_expected_result);
			Assert::AreEqual(b_expected_result, b_result);
		}

		TEST_METHOD(setLinesNumber)
		{
			Board b;
			uint16_t lines_no = 6;
			b.setLinesNumber(lines_no);
			uint16_t result = b.getLinesNumber();
			uint16_t expected_result = 6;

			Assert::AreEqual(expected_result, result);
		}

		TEST_METHOD(setColumnsNumber)
		{
			Board b;
			uint16_t columns_no = 3;
			b.setColumnsNumber(columns_no);
			uint16_t result = b.getColumnsNumber();
			uint16_t expected_result = 3;

			Assert::AreEqual(expected_result, result);
		}

		TEST_METHOD(getBoardMatrix)
		{
			uint16_t lines_no = 3, columns_no = 6;
			std::vector<std::vector<bool>> board = { {false, false, false, false, false, false},
													{false, true, false, true, false, false},
													{true, false, false, false, false, false} };

			Board b_expected_result(lines_no, columns_no, board);
			std::vector<std::vector<bool>> result = b_expected_result.getBoardMatrix();
			Board b_result(lines_no, columns_no, result);
			Assert::AreEqual(b_expected_result, b_result);
		}

		TEST_METHOD(exceptions)
		{
			Board board;
			Rules rule;
			Assert::ExpectException<std::string>([&]() {
				board.readBoardFromFile("InexistentFile.txt", "5X5");
				});

			Assert::ExpectException<std::string>([&]() {
				board.writeBoardOnConsole(0, 16);
				});

			Assert::ExpectException<std::string>([&]() {
				board.writeBoardOnConsole(2, 2);
				});

			Assert::ExpectException<std::string>([&]() {
				board.click(7, 2, rule);
				});
		}

		//Player tests
		TEST_METHOD(setRules)
		{
			Rules rule(true);
			Player player;
			player.setRules(rule);
			Assert::AreEqual(rule, player.GetRules());
		}

		TEST_METHOD(setBoard)
		{
			uint16_t rows = 5, columns = 5;
			std::vector<std::vector<bool>> board = { {false, false, false, false, false},
													{false, true, false, true, false},
													{false, false, false, false, false},
													{false, false, true, false, true},
													{false, true, false, false, false} };
			Board b(rows, columns, board);

			Player player;
			player.setBoard(b);
			Assert::AreEqual(b, player.GetBoard());
		}

		//InputValidation tests
		TEST_METHOD(IsLightColour)
		{
			User user;
			bool expected_result = true;
			bool actual_result = user.IsLightColour("cyan");
			Assert::AreEqual(expected_result, actual_result);
		}

		TEST_METHOD(IsLightColour_false) 
		{
			User user;
			bool expected_result = false;
			bool actual_result = user.IsLightColour("pink");
			Assert::AreEqual(expected_result, actual_result);
		}

		TEST_METHOD(NotSameColor)
		{
			Player player;
			std::string first_color = "blue";
			std::string second_color = "red";
			bool result = player.NotSameColor(first_color, second_color);
			bool expected_result = true;
			Assert::AreEqual(expected_result, result);
		}

		TEST_METHOD(NotSameColor_SameColor)
		{
			Player player;
			std::string first_color = "blue";
			std::string second_color = "blue";
			bool result = player.NotSameColor(first_color, second_color);
			bool expected_result = false;
			Assert::AreEqual(expected_result, result);
		}

		TEST_METHOD(TestChoice)
		{
			Game game;
			bool expected_result = false;
			bool actual_result;
			int16_t choice = 4;
			actual_result = game.TestChoice(choice);
			Assert::AreEqual(expected_result, actual_result);
		}

		TEST_METHOD(TestChoice_ExpectTrue)
		{
			Game game;
			bool expected_result = true;
			bool actual_result;
			int16_t choice = 2;
			actual_result = game.TestChoice(choice);
			Assert::AreEqual(expected_result, actual_result);
		}

		TEST_METHOD(TestInputOption)
		{
			Game game;
			bool expected_result = false;
			bool actual_result;
			int16_t option = 4;
			actual_result = game.TestInputOption(option);
			Assert::AreEqual(expected_result, actual_result);
		}

		TEST_METHOD(TestInputOption_ExpectTrue)
		{
			Game game;
			bool expected_result = true;
			bool actual_result;
			int16_t option = 1;
			actual_result = game.TestInputOption(option);
			Assert::AreEqual(expected_result, actual_result);
		}
	};
}
