#include "WindowProprieties.h"

WindowProprieties::WindowProprieties()
{
	w_height = 700;
	w_width = 700 ;
	w_color = sf::Color(73, 114, 229);
}

WindowProprieties::WindowProprieties(uint16_t width, uint16_t height, sf::Color color)
 :w_width{ width }, w_height { height }, w_color { color }
{
	
}

WindowProprieties::WindowProprieties(const WindowProprieties & other)
{
	*this = other;	
}

void WindowProprieties::setWidth(uint16_t width)
{
	w_width = width;
}

uint16_t WindowProprieties::getWidth()
{
	return w_width;
}

void WindowProprieties::setHeight(uint16_t height)
{
	w_height = height;
}

uint16_t WindowProprieties::getHeight()
{
	return w_height;
}

void WindowProprieties::setColor(const sf::Color& color)
{
	w_color = color;
}

sf::Color WindowProprieties::getColor()
{
	return w_color;
}

WindowProprieties::~WindowProprieties()
{
}
