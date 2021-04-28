#pragma once

#include <SFML/Graphics.hpp>
#include <cstdint>

class WindowProprieties
{
public:
	WindowProprieties();
	WindowProprieties(uint16_t , uint16_t, sf::Color);
	WindowProprieties(const WindowProprieties&);
	void setWidth(uint16_t);
	uint16_t getWidth();
	void setHeight(uint16_t); 
	uint16_t getHeight();
	void setColor(const sf::Color&);
	sf::Color getColor();
	~WindowProprieties();

private:
	int w_height;
	int w_width;
	sf::Color w_color;
};
