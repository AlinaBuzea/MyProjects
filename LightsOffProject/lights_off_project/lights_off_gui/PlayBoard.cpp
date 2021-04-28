#include "PlayBoard.h"
#include "Timer.h"
#include "MainMenu.h"
#include "FinalWindow.h"
#include <chrono>
#include <thread>

bool PlayBoard::AllLightsAreOff()
{
	bool lightIsOn = false;
	for (auto iter : m_lights)
		for (auto iter1 : iter)
			if (iter1 == true)
				lightIsOn = true;
	if (lightIsOn)
		return false;
	return true;
}

Button PlayBoard::initializeButton(int coordX, int coordY, int buttonWidth, int buttonHeight, sf::Color buttonColour, sf::Color hoverColour, sf::Color pressedColour, bool lightOn)
{
	Button button;
	button.setShapeOption(1);
	button.setShape(coordX, coordY, buttonWidth, buttonHeight);
	if (!lightOn)
	{
		button.setButtonColour(buttonColour);
		button.setHoverColour(hoverColour);
		button.setPressedColour(pressedColour);
	}
	else
	{
		button.setButtonColour(pressedColour);
		button.setHoverColour(buttonColour);
		button.setPressedColour(buttonColour);
	}
	return button;
}

sf::Font PlayBoard::defineFont(std::string filePath)
{
	Logging logger(std::cout);
	sf::Font font;
	if (!font.loadFromFile(filePath))
	{
		logger.Log("Font file not found.", Logging::Level::ERROR);
	}
	return font;
}

void PlayBoard::Click(const int& row, const int& column)
{
	Change(row, column);
	if (m_user.getHorizontal())
	{
		if (column > 0)
			Change(row, column - 1);
		if (column < m_user.getNumberOfColumns() - 1)
			Change(row, column + 1);
	}
	if (m_user.getVertical())
	{
		if (row > 0)
			Change(row - 1, column);
		if (row < m_user.getNumberOfRows() - 1)
			Change(row + 1, column);
	}
	if (m_user.getMainDiagonal())
	{
		if (column > 0 && row > 0)
			Change(row - 1, column - 1);
		if (column < m_user.getNumberOfColumns() - 1 && row < m_user.getNumberOfRows() - 1)
			Change(row + 1, column + 1);
	}
	if (m_user.getSecondaryDiagonal())
	{
		if (row > 0 && column < m_user.getNumberOfColumns() - 1)
			Change(row - 1, column + 1);
		if (row < m_user.getNumberOfRows() - 1 && column>0)
			Change(row + 1, column - 1);
	}
	UpdateButtons();
}

void PlayBoard::Change(const int& row, const int& column)
{
	if (m_lights[row][column])
		m_lights[row][column] = false;
	else
		m_lights[row][column] = true;
}

void PlayBoard::UpdateButtons()
{
	for (int index = 0; index < m_user.getNumberOfRows(); index++)
		for (int index1 = 0; index1 < m_user.getNumberOfColumns(); index1++)
		{
			if (!m_lights[index][index1])
			{
				m_gameBoard[index][index1].setButtonColour(m_lightsOffColour);
				m_gameBoard[index][index1].setHoverColour(m_lightsOnColour);
				m_gameBoard[index][index1].setPressedColour(m_lightsOnColour);
			}
			else
			{
				m_gameBoard[index][index1].setButtonColour(m_lightsOnColour);
				m_gameBoard[index][index1].setHoverColour(m_lightsOffColour);
				m_gameBoard[index][index1].setPressedColour(m_lightsOffColour);
			}
		}
}

PlayBoard::PlayBoard(User user)
{
	Logging logger(std::cout);

	m_user = user;
	std::string text = " ";

	m_lightsOffColour = m_user.getLightsOffColor();
	m_lightsOnColour = m_user.getLightsOnColor();

	std::ifstream file(user.chooseBoard()); //user.getFileName()

	int radius = 25;
	int width = radius * 2 + 10;

	float coordX;
	float coordY = (700 - user.getNumberOfRows() * width) / 2;
	bool light;

	try {
		if (file.is_open())
		{
			logger.Log("A board was selected.", Logging::Level::INFO);
			for (int index = 0; index < m_user.getNumberOfRows(); index++)
			{
				std::vector<Button>temp;
				std::vector<bool>temp1;
				coordX = (700 - user.getNumberOfColumns() * width) / 2;
				for (int index1 = 0; index1 < m_user.getNumberOfColumns(); index1++)
				{
					file >> light;
					Button button = initializeButton(coordX, coordY, radius, radius, m_lightsOffColour, m_lightsOnColour, m_lightsOnColour, light);
					temp.push_back(button);
					temp1.push_back(light);
					coordX += width;
				}
				m_gameBoard.push_back(temp);
				m_lights.push_back(temp1);
				coordY += width;
			}
		}
		else
		{
			throw "Couldn't find input file!";
		}
	}
	catch (const char* message)
	{
		logger.Log(message, Logging::Level::ERROR);
	}
}

void PlayBoard::DrawBoard(int designOption)
{
	Logging logger(std::cout);
	sf::Color backgroundColour, buttonColour, hoverColour;
	if (designOption == 1)
	{
		backgroundColour = sf::Color(73, 114, 229);
		buttonColour = sf::Color(255, 51, 153);
		hoverColour = sf::Color(255, 102, 178);
	}
	else if (designOption == 2)
	{
		backgroundColour = sf::Color(229, 229, 220);
		buttonColour = sf::Color(198, 107, 61);
		hoverColour = sf::Color(198, 107, 61, 120);
	}
	else if (designOption == 3)
	{
		backgroundColour = sf::Color(156, 155, 228);
		buttonColour = sf::Color(213, 169, 247);
		hoverColour = sf::Color(213, 169, 247, 120);
	}
	else if (designOption == 4)
	{
		backgroundColour = sf::Color(235, 212, 226);
		buttonColour = sf::Color(222, 119, 185);
		hoverColour = sf::Color(222, 119, 185, 120);
	}
	else
	{
		backgroundColour = sf::Color(207, 223, 218);
		buttonColour = sf::Color(35, 100, 119);
		hoverColour = sf::Color(50, 174, 78);
	}

	WindowProprieties winProp(700, 700, backgroundColour);
	sf::RenderWindow window(sf::VideoMode(winProp.getWidth(), winProp.getHeight()), "Game", sf::Style::Titlebar);
	window.clear(winProp.getColor());

	window.setActive(true);

	sf::Font font = defineFont("..\\COOPBL.TTF");
	Button buttonBack(0, 600.f, 630.f, 80.f, 50.f, sf::Color(0, 0, 0, 0), hoverColour, sf::Color(0, 0, 0, 0), "Back", font);

	Timer timer(m_user.getFastGame());
	timer.Initialize();
	if (m_user.getNumberOfColumns() == m_user.getNumberOfRows() && m_user.getNumberOfColumns() % 2 == 1)
	{
		timer.setBoardType(m_user.getNumberOfColumns());
	}
	else
	{
		timer.setBoardType(2);
	}

	while (window.isOpen())
	{
		sf::Event evnt;
		while (window.pollEvent(evnt))
		{
			if (evnt.type == sf::Event::Closed)
				window.close();
		}
		window.clear(winProp.getColor());

		timer.update(&window);

		sf::Vector2f mousePosition = static_cast<sf::Vector2f>(sf::Mouse::getPosition(window));

		buttonBack.update(mousePosition);
		if (buttonBack.isPressed())
		{
			logger.Log("The user pressed the \"Back\" button.", Logging::Level::INFO);
			window.close();
			MainMenu menu(designOption);
			menu.Menu();
		}
		buttonBack.drawButton(&window);

		int row = 0;
		int column = 0;
		for (auto iter : m_gameBoard)
		{
			column = 0;
			for (auto iter1 : iter)
			{
				iter1.update(mousePosition);
				iter1.drawButton(&window);
				if (iter1.isPressed())
				{
					Click(row, column);
				}
				column++;
			}
			row++;
		}

		if (AllLightsAreOff())
		{
			window.close();
			FinalWindow finalWin;
			finalWin.setTimeSpentInGame(timer.getSeconds());
			finalWin.setGameWon(true);
			finalWin.initializeElements(designOption);
			finalWin.ShowWindow();
		}

		if (timer.getStopGame() == true)
		{
			logger.Log("The timer has stopped, and therefore the game is over.", Logging::Level::INFO);
			window.clear(winProp.getColor());
			timer.drawTimeIsUp(&window);
			window.display();
			std::this_thread::sleep_until(std::chrono::system_clock::now() + std::chrono::seconds(2));
			window.close();
			FinalWindow finalWin;
			finalWin.setTimeSpentInGame(timer.getSeconds());
			finalWin.setGameWon(false);
			finalWin.initializeElements(designOption);
			finalWin.ShowWindow();
		}
		window.display();
	}
}