import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.util.Random;

public class Brick {

	private int m_coordX;
	private int m_coordY;
	private int m_brick_width;
	private int m_brick_length;
	private Color m_color;
	private boolean m_visible;
	
	public Brick()
	{
		m_coordX=20;
		m_coordY=40;
		m_brick_width=20;
		m_brick_length=40;
		Random generate_random_number= new Random();
		int red_shade=generate_random_number.nextInt(256);
		int green_shade=generate_random_number.nextInt(256);
		int blue_shade=generate_random_number.nextInt(256);
		
		m_color=new Color(red_shade,green_shade,blue_shade);
		m_visible=true;
		
	}
	public Brick(int coordX, int coordY, int brick_width, int brick_length, boolean visible)
	{
		m_coordX=coordX;
		m_coordY=coordY;
		m_brick_width=brick_width;
		m_brick_length=brick_length;
		Random generate_random_number= new Random();
		int red_shade=generate_random_number.nextInt(256);
		int green_shade=generate_random_number.nextInt(256);
		int blue_shade=generate_random_number.nextInt(256);
		
		m_color=new Color(red_shade,green_shade,blue_shade);
		m_visible=visible;
	}
	public int getCoordX()
	{
		return m_coordX;
	}
	public void setCoordX(int coordX)
	{
		m_coordX=coordX;
	}
	public int getCoordY()
	{
		return m_coordY;
	}
	public void setCoordY(int coordY)
	{
		m_coordY=coordY;
	}
	public int getBrickWidth()
	{
		return m_brick_width;
	}
	public void setCrickWidth(int brick_width)
	{
		m_brick_width=brick_width;
	}
	public int getBrickLength()
	{
		return m_brick_length;
	}
	public void setBrickLength(int brick_length)
	{
		m_brick_length=brick_length;
	}
	public Color getColor()
	{
		return m_color;
	}
	public void setColor(Color color)
	{
		m_color=color;
	}
	public boolean getVisible()
	{
		return m_visible;
	}
	public void setVisible(boolean visible)
	{
		m_visible=visible;
	}
	
	public void drawBrick(Graphics g)
	{
		if(m_visible)
		{
			g.setColor(m_color);
			g.fillRect(m_coordX, m_coordY, m_brick_length, m_brick_width);
			g.setColor(new Color(0,0,102));
			g.drawRect(m_coordX, m_coordY, m_brick_length, m_brick_width);
		}
	}
	
	public void hideBrick(Graphics g)
	{
		if(m_visible)
		{
			g.setColor(new Color(0,0,102));
			g.fillRect(m_coordX, m_coordY, m_brick_length, m_brick_width);
			m_visible=false;
		}
	}

}
