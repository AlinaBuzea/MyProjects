#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
class Button
{
private: 
	sf::RectangleShape m_shape;
	sf::CircleShape m_boardShape;
	sf::Text m_text;
	sf::Font m_font;

	sf::Color m_buttonColour;
	sf::Color m_hoverColour;
	sf::Color m_pressedColour;

	int m_shapeOption;  //0-rectangle, 1-circle
	bool m_pressed;
	bool m_hover;
	bool m_doingNothing;

public:
	Button(int shape, float coordX, float coordY, float width, float height, sf::Color buttonColour, sf::Color hoverColour, sf::Color pressedColour, const std::string& text, sf::Font font);
	Button() = default;
	Button(const Button& button);
	Button(Button&& button);
	~Button();

	Button& operator =(const Button& button);
	Button& operator =(Button&& button);

	sf::Text getButtonText();
	sf::RectangleShape getButtonShape();
	sf::CircleShape getButtonBoardShape();

	void update(sf::Vector2f& mousePosition);
	void drawButton(sf::RenderWindow* target);
	bool isPressed() const;

	void setShapeOption(int shapeOption);
	void setShape(float coordX, float coordY, float width, float height);
	void setText(const std::string& text);
	void setFont(sf::Font);
	void setButtonColour(sf::Color colour);
	void setHoverColour(sf::Color colour);
	void setPressedColour(sf::Color colour);
	void setPressed(bool pressed);
	void setHover(bool hover);
	void setDoingNothing(bool doingNothing);
};

