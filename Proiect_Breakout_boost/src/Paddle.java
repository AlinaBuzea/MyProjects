import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.event.KeyEvent;


public class Paddle {

	private int m_coordX;
	private int m_coordY;
	private int m_paddle_width;
	private int m_paddle_length;
	private int m_paddle_move;
	
	
	public Paddle()
	{
		m_coordX=160;
		m_coordY=440;
		m_paddle_width=20;
		m_paddle_length=80;
		m_paddle_move=20;
	}
	public Paddle(int coordX, int coordY, int paddle_width, int paddle_length, int paddle_move)
	{
		m_coordX=coordX;
		m_coordY=coordY;
		m_paddle_width=paddle_width;
		m_paddle_length=paddle_length;
		m_paddle_move=paddle_move;
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
	public int getPaddleWidth()
	{
		return m_paddle_width;
	}
	public void setPaddleWidth(int paddle_width)
	{
		m_paddle_width=paddle_width;
	}
	public int getPaddleLength()
	{
		return m_paddle_length;
	}
	public void setPaddleLength(int paddle_length)
	{
		m_paddle_length=paddle_length;
	}
	public int getPaddleMove()
	{
		return m_paddle_move;
	}
	public void setPaddleMove(int paddle_move)
	{
		m_paddle_move=paddle_move;
	}
	
	public boolean moveRight(BoardGeometry board)
	{
		if(m_coordX<=(board.getBoardMaxX()-getPaddleMove()-getPaddleLength()))
		{
			m_coordX+=getPaddleMove();
			return true;
		}
		return false;
	}
	
	public boolean moveLeft(BoardGeometry board)
	{
		if(m_coordX>=(board.getBoardMinX() + getPaddleMove()))
		{
			m_coordX-=getPaddleMove();
			return true;
		}
		return false;
	}
	
	public boolean moveRightWithBall(BoardGeometry board, Ball ball)
	{
		if(m_coordX<=(board.getBoardMaxX()-getPaddleMove()-getPaddleLength()))
		{
			m_coordX+=getPaddleMove();
			ball.setCenterX(ball.getCenterX()+2*ball.getBallRadius());
			return true;
		}
		return false;
	}
	
	public boolean moveLeftWithBall(BoardGeometry board, Ball ball)
	{
		if(m_coordX>=(board.getBoardMinX() + getPaddleMove()))
		{
			m_coordX-=getPaddleMove();
			ball.setCenterX(ball.getCenterX()-2*ball.getBallRadius());
			return true;
		}
		return false;
	}
	public void drawPaddle(Graphics g)
	{
		g.setColor(new Color(40,174,223));
		g.fillRect(m_coordX,m_coordY, m_paddle_length, m_paddle_width);
		
	}
}


