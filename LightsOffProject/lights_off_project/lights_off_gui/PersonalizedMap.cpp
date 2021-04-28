#include "PersonalizedMap.h"

Button PersonalizedMap::initializeButton(int coordX, int coordY, int buttonWidth, int buttonHeight, sf::Color buttonColour, sf::Color hoverColour, sf::Color pressedColour, bool shape)
{
	Button button;
	button.setShapeOption(shape);
	button.setShape(coordX, coordY, buttonWidth, buttonHeight);
	button.setButtonColour(buttonColour);
	button.setHoverColour(hoverColour);
	button.setPressedColour(pressedColour);
	return button;
}

void PersonalizedMap::Click(const int& row, const int& column)
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

void PersonalizedMap::Change(const int& row, const int& column)
{
	if (m_lights[row][column])
		m_lights[row][column] = false;
	else
		m_lights[row][column] = true;
}

void PersonalizedMap::UpdateButtons()
{
	for (int index = 0; index < m_user.getNumberOfRows(); index++)
	{
		for (int index1 = 0; index1 < m_user.getNumberOfColumns(); index1++)
		{
			if (!m_lights[index][index1])
			{
				m_gameBoard[index][index1].setButtonColour(m_user.getLightsOffColor());
				m_gameBoard[index][index1].setHoverColour(m_user.getLightsOnColor());
				m_gameBoard[index][index1].setPressedColour(m_user.getLightsOnColor());
			}
			else
			{
				m_gameBoard[index][index1].setButtonColour(m_user.getLightsOnColor());
				m_gameBoard[index][index1].setHoverColour(m_user.getLightsOffColor());
				m_gameBoard[index][index1].setPressedColour(m_user.getLightsOffColor());
			}
		}
	}
}

sf::Font PersonalizedMap::defineFont(std::string filePath)
{
	Logging logger(std::cout);
	sf::Font font;
	if (!font.loadFromFile(filePath))
	{
		logger.Log("Font file not found.", Logging::Level::ERROR);
	}
	return font;
}

void PersonalizedMap::Save()
{
	m_fileName.append(std::to_string(m_user.getNumberOfRows()));
	m_fileName.append("x");
	m_fileName.append(std::to_string(m_user.getNumberOfColumns()));
	m_fileName.append("_");
	if (m_user.getHorizontal())
		m_fileName.append("h");
	if (m_user.getVertical())
		m_fileName.append("v");
	if (m_user.getMainDiagonal())
		m_fileName.append("f");
	if (m_user.getSecondaryDiagonal())
		m_fileName.append("s");
	m_fileName.append("_mp.txt");

	std::ofstream file(m_fileName);
	for (auto iter : m_lights)
	{
		for (auto iter1 : iter)
			file << iter1<<" ";
		file << "\n";
	}
}

PersonalizedMap::PersonalizedMap(User user)
{
	m_user = user;

	int radius = 25;
	int width = radius * 2 + 10;

	float coordX;
	float coordY = (700 - user.getNumberOfRows() * width) / 2;
	bool light = false;

	for (int index = 0; index < m_user.getNumberOfRows(); index++)
	{
		std::vector<Button>temp;
		std::vector<bool>temp1;
		coordX = (700 - user.getNumberOfColumns() * width) / 2;
		for (int index1 = 0; index1 < m_user.getNumberOfColumns(); index1++)
		{
			Button button = initializeButton(coordX, coordY, radius, radius, m_user.getLightsOffColor(), m_user.getLightsOnColor(), m_user.getLightsOnColor(), 1);
			temp.push_back(button);
			temp1.push_back(light);
			coordX += width;
		}
		m_gameBoard.push_back(temp);
		m_lights.push_back(temp1);
		coordY += width;
	}
}

void PersonalizedMap::DrawMap(int designOption)
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

	sf::RenderWindow window(sf::VideoMode(700, 700), "Create Board", sf::Style::Close | sf::Style::Titlebar);
	window.clear(backgroundColour);
	window.setActive(true);
	Button saver = initializeButton(600, 650, 70, 30, buttonColour, hoverColour, buttonColour, 0);
	saver.setFont(defineFont("..\\COOPBL.TTF"));
	saver.setText("Save");
	while (window.isOpen())
	{
		sf::Event evnt;
		while (window.pollEvent(evnt))
		{
			if (evnt.type == sf::Event::Closed)
				window.close();

			window.clear(backgroundColour);

			sf::Vector2f mousePosition = static_cast<sf::Vector2f>(sf::Mouse::getPosition(window));
			saver.update(mousePosition);
			saver.drawButton(&window);
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
			if (saver.isPressed())
			{
				Save();
				logger.Log("The user created a personalized map.", Logging::Level::INFO);
				window.close();
			}
			window.display();
		}
	}
}

std::string PersonalizedMap::getFileName()
{
	return m_fileName;
}
