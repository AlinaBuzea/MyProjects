#include "Button.h"
#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>

Button::Button(int shapeOption, float coordX, float coordY, float width, float height, sf::Color buttonColour, sf::Color hoverColour, sf::Color pressedColour, const std::string& text, sf::Font font)
{
	m_shapeOption = shapeOption;
	m_font = font;
	m_text.setFont(m_font);
	m_text.setString(text);
	m_text.setFillColor(sf::Color::Black);
	m_text.setCharacterSize(20);

	if (shapeOption == 0)
	{
		m_shape.setPosition(sf::Vector2f(coordX, coordY));
		m_shape.setSize(sf::Vector2f(width, height));
		m_shape.setFillColor(buttonColour);
		m_text.setPosition(
			m_shape.getPosition().x + (m_shape.getGlobalBounds().width / 2.f) - m_text.getGlobalBounds().width / 2.f,
			m_shape.getPosition().y + (m_shape.getGlobalBounds().height / 2.f) - m_text.getGlobalBounds().height / 2.f
		);
	}

	else if (shapeOption == 1)
	{
		m_boardShape.setPosition(sf::Vector2f(coordX, coordY));
		m_boardShape.setRadius(width);
		m_boardShape.setFillColor(buttonColour);
		m_text.setPosition(
			m_boardShape.getPosition().x + (m_boardShape.getGlobalBounds().width / 2.f) - m_text.getGlobalBounds().width / 2.f,
			m_boardShape.getPosition().y + (m_boardShape.getGlobalBounds().height / 2.f) - m_text.getGlobalBounds().height / 2.f
		);
	}

	m_buttonColour = buttonColour;
	m_hoverColour = hoverColour;
	m_pressedColour = pressedColour;

	m_doingNothing = true;
	m_hover = false;
	m_pressed = false;
}

Button::Button(const Button& button)
{
	m_boardShape = button.m_boardShape;
	m_buttonColour = button.m_buttonColour;
	m_doingNothing = button.m_doingNothing;
	m_font = button.m_font;
	m_hover = button.m_hover;
	m_hoverColour = button.m_hoverColour;
	m_pressedColour = button.m_pressedColour;
	m_pressed = button.m_pressed;
	m_shape = button.m_shape;
	m_shapeOption = button.m_shapeOption;
	m_text = button.m_text;
}

Button::~Button()
{
}

Button& Button::operator=(const Button& button)
{
	if (this != &button)
	{
		m_boardShape = button.m_boardShape;
		m_buttonColour = button.m_buttonColour;
		m_doingNothing = button.m_doingNothing;
		m_font = button.m_font;
		m_hover = button.m_hover;
		m_hoverColour = button.m_hoverColour;
		m_pressedColour = button.m_pressedColour;
		m_pressed = button.m_pressed;
		m_shape = button.m_shape;
		m_shapeOption = button.m_shapeOption;
		m_text = button.m_text;
	}
	return *this;
}

Button& Button::operator=(Button&& button)
{
	if (this != &button)
	{
		m_boardShape = button.m_boardShape;
		m_buttonColour = button.m_buttonColour;
		m_doingNothing = button.m_doingNothing;
		m_font = button.m_font;
		m_hover = button.m_hover;
		m_hoverColour = button.m_hoverColour;
		m_pressedColour = button.m_pressedColour;
		m_pressed = button.m_pressed;
		m_shape = button.m_shape;
		m_shapeOption = button.m_shapeOption;
		m_text = button.m_text;
	}
	new(&button) Button;
	return *this;
}

Button::Button(Button&& button)
{
	*this = std::move(button);
}

sf::Text Button::getButtonText()
{
	return m_text;
}

sf::RectangleShape Button::getButtonShape()
{
	return m_shape;
}

sf::CircleShape Button::getButtonBoardShape()
{
	return m_boardShape;
}

void Button::update(sf::Vector2f& mousePosition)
{
	m_doingNothing = true;
	m_hover = false;
	m_pressed = false;

	//hover
	if ((m_shapeOption == 0 && m_shape.getGlobalBounds().contains(mousePosition)) || (m_shapeOption == 1 && m_boardShape.getGlobalBounds().contains(mousePosition)))
	{
		m_hover = true;
		m_doingNothing = false;
		m_pressed = false;

		//pressed
		if (sf::Mouse::isButtonPressed(sf::Mouse::Left))
		{
			m_pressed = true;
			m_hover = false;
			m_doingNothing = false;
		}
	}

	if (m_doingNothing == true)
	{
		if(m_shapeOption == 0)
			m_shape.setFillColor(m_buttonColour);
		if(m_shapeOption == 1)
			m_boardShape.setFillColor(m_buttonColour);
	}
	else if (m_hover == true)
	{
		if (m_shapeOption == 0)
			m_shape.setFillColor(m_hoverColour);
		if (m_shapeOption == 1)
			m_boardShape.setFillColor(m_hoverColour);
	}
	else if (m_pressed == true)
	{
		if (m_shapeOption == 0)
			m_shape.setFillColor(m_pressedColour);
		if (m_shapeOption == 1)
			m_boardShape.setFillColor(m_pressedColour);
	}
}

void Button::drawButton(sf::RenderWindow* target)
{
	if (m_shapeOption == 0)
		target->draw(m_shape);
	if (m_shapeOption == 1)
		target->draw(m_boardShape);
	target->draw(m_text);
}

bool Button::isPressed() const
{
	if (m_pressed == true)
		return true;
	return false;
}

void Button::setShapeOption(int shapeOption)
{
	m_shapeOption = shapeOption;
}

void Button::setShape(float coordX, float coordY, float width, float height)
{
	if (m_shapeOption == 0)
	{
		m_shape.setPosition(sf::Vector2f(coordX, coordY));
		m_shape.setSize(sf::Vector2f(width, height));
		m_shape.setFillColor(m_buttonColour);
	}
	else if (m_shapeOption == 1)
	{
		m_boardShape.setPosition(sf::Vector2f(coordX, coordY));
		m_boardShape.setRadius(width);
		m_boardShape.setFillColor(m_buttonColour);
	}
}

void Button::setText(const std::string& text)
{
	m_text.setFont(m_font);
	m_text.setString(text);
	m_text.setFillColor(sf::Color::Black);
	m_text.setCharacterSize(20);

	if (m_shapeOption == 0)
	{
		m_text.setPosition(
			m_shape.getPosition().x + (m_shape.getGlobalBounds().width / 2.f) - m_text.getGlobalBounds().width / 2.f,
			m_shape.getPosition().y + (m_shape.getGlobalBounds().height / 2.f) - m_text.getGlobalBounds().height / 2.f
		);
	}
	else if (m_shapeOption == 1)
	{
		m_text.setPosition(
			m_boardShape.getPosition().x + (m_boardShape.getGlobalBounds().width / 2.f) - m_text.getGlobalBounds().width / 2.f,
			m_boardShape.getPosition().y + (m_boardShape.getGlobalBounds().height / 2.f) - m_text.getGlobalBounds().height / 2.f
		);
	}
}

void Button::setFont(sf::Font font)
{
	m_font = font;
}

void Button::setButtonColour(sf::Color colour)
{
	m_buttonColour = colour;
}

void Button::setHoverColour(sf::Color colour)
{
	m_hoverColour = colour;
}

void Button::setPressedColour(sf::Color colour)
{
	m_pressedColour = colour;
}

void Button::setPressed(bool pressed)
{
	m_pressed = pressed;
}

void Button::setHover(bool hover)
{
	m_hover = hover;
}

void Button::setDoingNothing(bool doingNothing)
{
	m_doingNothing = doingNothing;
}
