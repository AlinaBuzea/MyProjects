#include "FinalWindow.h"

FinalWindow::FinalWindow()
{
}

FinalWindow::~FinalWindow()
{
}

void FinalWindow::setWindowProprieties(const WindowProprieties & other)
{
	m_winProprieties = other;
}

WindowProprieties FinalWindow::getWindowProprieties()
{
	return m_winProprieties;
}

void FinalWindow::setGameWon(bool gameWon)
{
	m_gameWon = gameWon;
}

bool FinalWindow::getGameWon()
{
	return m_gameWon;
}

void FinalWindow::setTimeSpentInGame(int timeSpent)
{
	m_timeSpentInGameValue = timeSpent;
}

int FinalWindow::getTimeSpentInGame()
{
	return m_timeSpentInGameValue;
}

Button FinalWindow::initializeButton(float coordX, float coordY, float buttonWidth, float buttonHeight, sf::Color buttonColor, sf::Color hoverColour, sf::Color pressedColour)
{
	Button button;
	button.setShapeOption(0);
	button.setShape(coordX, coordY, buttonWidth, buttonHeight);
	button.setButtonColour(buttonColor);
	button.setHoverColour(hoverColour);
	button.setPressedColour(pressedColour);
	return button;
}

void FinalWindow::initializeResult(int characterSize, float spaceBetweenElements, const std::string & message, sf::Color color)
{
	m_result.setCharacterSize(20);
	m_result.setFont(m_textFont);
	m_result.setFillColor(color);
	m_result.setString(message);
	m_result.setPosition(
		(m_winProprieties.getWidth() - m_result.getGlobalBounds().width) / 2.f,
		(m_winProprieties.getHeight() - m_result.getGlobalBounds().height) / 2.f - spaceBetweenElements - m_result.getCharacterSize()
	);
}

void FinalWindow::initializeTimeSpentInGame(int characterSize, float spaceBetweenElements, const std::string& message, sf::Color color)
{
	m_timeSpentInGame.setCharacterSize(20);
	m_timeSpentInGame.setFont(m_textFont);
	m_timeSpentInGame.setFillColor(color);
	m_timeSpentInGame.setString(message);
	m_timeSpentInGame.setPosition((m_winProprieties.getWidth() - m_timeSpentInGame.getGlobalBounds().width) / 2.f,
		m_result.getPosition().y - spaceBetweenElements - m_timeSpentInGame.getCharacterSize()
	);
}

sf::Font FinalWindow::defineFont(const std::string & FilePath)
{
	Logging logger(std::cout);
	sf::Font font;
	if (!font.loadFromFile(FilePath))
	{
		logger.Log("Font file not found!", Logging::Level::ERROR);
	}
	return font;
}

void FinalWindow::initializeElements(int designOption)
{
	sf::Color background_colour, buttonColor, hoverColor, textColor;
	m_designOption = designOption;
	if (designOption == 1)
	{
		background_colour = sf::Color(73, 114, 229);
		buttonColor = sf::Color(255, 51, 153);
		hoverColor = sf::Color(255, 102, 178);
		textColor = sf::Color::White;
	}
	else if (designOption == 2)
	{
		background_colour = sf::Color(229, 229, 220);
		buttonColor = sf::Color(198, 107, 61);
		hoverColor = sf::Color(198, 107, 61, 120);
		textColor = sf::Color(38, 73, 92);
	}
	else if (designOption == 3)
	{
		background_colour = sf::Color(156, 155, 228);
		buttonColor = sf::Color(213, 169, 247);
		hoverColor = sf::Color(213, 169, 247, 120);
		textColor = sf::Color(235, 225, 231);
	}
	else if (designOption == 4)
	{
		background_colour = sf::Color(235, 212, 226);
		buttonColor = sf::Color(222, 119, 185);
		hoverColor = sf::Color(222, 119, 185, 120);
		textColor = sf::Color::White;
	}
	else
	{
		background_colour = sf::Color(207, 223, 218);
		buttonColor = sf::Color(35, 100, 119);
		hoverColor = sf::Color(50, 174, 78);
		textColor = sf::Color(23, 48, 66);
	}

	float buttonWidth = 150.0f;
	float buttonHeight = 50.0f;
	float spaceBetweenElements = 20.0f;
	int characterSize = 20;

	WindowProprieties win_prop(700, 700, background_colour);
	m_winProprieties = win_prop;

	std::string timeSpentInGameMessage = "Time's up!";
	std::string resultMessage = "Game over!";
	m_textFont = defineFont("..\\COOPBL.TTF");

	if (!m_gameWon)
	{
		resultMessage = "You lost!";
		timeSpentInGameMessage = "Time's up!";
	}
	else
	{
		timeSpentInGameMessage = "You managed to do so just in " + std::to_string(m_timeSpentInGameValue) + " second(s)!";
		resultMessage = "You won!";
	}
	
	initializeResult(characterSize, spaceBetweenElements, resultMessage, textColor);
	initializeTimeSpentInGame(characterSize, spaceBetweenElements, timeSpentInGameMessage, textColor);

	m_tryAgainButton = initializeButton((m_winProprieties.getWidth() - buttonWidth) / 2.f,
		m_result.getPosition().y+m_result.getGlobalBounds().height + buttonHeight + spaceBetweenElements, buttonWidth, buttonHeight, buttonColor,
		hoverColor, buttonColor);
	m_tryAgainButton.setFont(defineFont("..\\COOPBL.TTF"));
	m_tryAgainButton.setText("Try Again");

	m_goToMenuButton = initializeButton((m_winProprieties.getWidth() - buttonWidth) / 2.f,
		m_tryAgainButton.getButtonShape().getPosition().y + buttonHeight + spaceBetweenElements, buttonWidth, buttonHeight, buttonColor,
		hoverColor, buttonColor);
	m_goToMenuButton.setFont(defineFont("..\\COOPBL.TTF"));
	m_goToMenuButton.setText("Go to Menu");
	
}

void FinalWindow::ShowWindow()
{
	Logging logger(std::cout);
	sf::RenderWindow window(sf::VideoMode(m_winProprieties.getWidth(), m_winProprieties.getHeight()), "Game over!");
	window.clear(m_winProprieties.getColor());

	while (window.isOpen())
	{
		sf::Event event;
		while (window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
				window.close();
		}

		window.clear(m_winProprieties.getColor());
		
		window.draw(m_timeSpentInGame);
		window.draw(m_result);
	
		sf::Vector2f mousePosition = static_cast<sf::Vector2f>(sf::Mouse::getPosition(window));
		m_tryAgainButton.update(mousePosition);
		m_tryAgainButton.drawButton(&window);
		if (m_tryAgainButton.isPressed())
		{
			logger.Log("\"Try again\" button was pressed.", Logging::Level::INFO);
			window.close();
			MainMenu menu(m_designOption);
			menu.Play();
		}

		m_goToMenuButton.update(mousePosition);
		m_goToMenuButton.drawButton(&window);
		if (m_goToMenuButton.isPressed())
		{
			logger.Log("\"Go to Menu\" button was pressed.", Logging::Level::INFO);
			window.close();
			MainMenu menu(m_designOption);
			menu.Menu();
		}

		window.display();
	}
}