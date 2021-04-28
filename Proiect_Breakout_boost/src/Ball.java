import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.util.Vector;

public class Ball 
{
	private int m_centerX;
	private int m_centerY;
	private int m_ball_radius;
	private int m_stepX;
	private int m_stepY;
	private double m_stepTime;
	
	
	public Ball()
	{
		m_centerX=200;
		m_centerY=430;
		m_ball_radius=10;
		m_stepX = -m_ball_radius;
		m_stepY = -m_ball_radius;
		m_stepTime = 150;
	}
	public Ball(int centerX, int centerY, int ball_radius, int stepX, int stepY, int stepTime)
	{
		m_centerX=centerX;
		m_centerY=centerY;
		m_ball_radius=ball_radius;
		m_stepX = stepX;
		m_stepY = stepY;
		m_stepTime = stepTime;
	}
	public int getCenterX()
	{
		return m_centerX;
	}
	public void setCenterX(int centerX)
	{
		m_centerX=centerX;
	}
	public int getCenterY()
	{
		return m_centerY;
	}
	public void setCenterY(int centerY)
	{
		m_centerY=centerY;
	}
	public int getBallRadius()
	{
		return m_ball_radius;
	}
	public void setBallRadius(int radius)
	{
		m_ball_radius=radius;
	}
	public int getStepX()
	{
		return m_stepX;
	}
	public void setStepX(int stepX)
	{
		m_stepX=stepX;
	}
	public int getStepY()
	{
		return m_stepY;
	}
	public void setStepY(int stepY)
	{
		m_stepY=stepY;
	}
	public double getStepTime()
	{
		return m_stepTime;
	}
	public void setStepTime(double stepTime)
	{
		m_stepTime=stepTime;
	}
	
	public void stopMovement()
	{
		setStepX(0);
		setStepY(0);
	}
	
	public void drawBall(Graphics g)
	{
		g.setColor(new Color(255,51,153));
		g.fillOval(m_centerX - m_ball_radius + 1, m_centerY - m_ball_radius + 1, 2*(m_ball_radius-1), 2*(m_ball_radius-1));
		g.drawOval(m_centerX - m_ball_radius + 1, m_centerY - m_ball_radius + 1, 2*(m_ball_radius-1), 2*(m_ball_radius-1));		
	}
	
	public void reflectMove(Direction dir)
	{
	    switch(dir) {
	      case NORTH:
	    	  setStepY(-getStepY());
	    	  break;
	      case EAST:
	    	  setStepX(-getStepX());
	    	  break;
	      case SOUTH:
	    	  setStepY(-getStepY());
	    	  break;
	      case WEST:
	    	  setStepX(-getStepX());
	    	  break;
	    }
	}
	
	public int moveBall(Graphics g, Vector<Vector<Brick>> brickMatrix, CellType baseMatrix[][], BoardGeometry board)
	{
		if(m_stepX==0 && m_stepY==0)
		{
			return 0;
		}
		int retNrExplodedBricks = 0;
		
		g.setColor(new Color(0,0,102));
		g.fillOval(m_centerX - m_ball_radius + 1, m_centerY - m_ball_radius + 1, 2*(m_ball_radius-1), 2*(m_ball_radius-1));
		g.drawOval(m_centerX - m_ball_radius + 1, m_centerY - m_ball_radius + 1, 2*(m_ball_radius-1), 2*(m_ball_radius-1));		
		
		
		int deltaX = m_stepX;
		int deltaY = m_stepY;	
		
		while((deltaX != 0) && (deltaY != 0))
		{
			int currentCenterCellX = 1 + (m_centerX - board.getBoardMinX())/board.getBoardBaseMatrixCellWidth();
			int currentCenterCellY= 1 + (m_centerY - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth();
			Direction dirX = Direction.EAST;
			if(deltaX < 0)
				dirX = Direction.WEST;
			Direction dirY = Direction.SOUTH;
			if(deltaY < 0)
				dirY = Direction.NORTH;
		    switch(dirX) {
		      case EAST:
		    	  currentCenterCellX = (m_centerX - board.getBoardMinX() + m_ball_radius/2 + board.getBoardBaseMatrixCellWidth())
		    	  						/board.getBoardBaseMatrixCellWidth(); 
		    	  break;
		      case WEST:
		    	  currentCenterCellX = (m_centerX - board.getBoardMinX() - m_ball_radius/2 + board.getBoardBaseMatrixCellWidth())
		    	  						/board.getBoardBaseMatrixCellWidth();
		    	  break;
		      default:
		    	  break;
		    }
		    switch(dirY) {
		      case NORTH:
		    	  currentCenterCellY = (m_centerY - board.getBoardMinY() - m_ball_radius/2 + board.getBoardBaseMatrixCellWidth())
		    	  						/board.getBoardBaseMatrixCellWidth();
		    	  break;
		      case SOUTH:
		    	  currentCenterCellY = (m_centerY - board.getBoardMinY() + m_ball_radius/2 + board.getBoardBaseMatrixCellWidth())
		    	  						/board.getBoardBaseMatrixCellWidth();
		    	  break;
		      default:
		    	  break;
		    }
		    
		    int nextCellX= currentCenterCellX;
			int nextCellY= currentCenterCellY;
		    switch(dirX) {
		      case EAST:
		    	  nextCellX = (m_centerX - board.getBoardMinX() + m_ball_radius + deltaX + board.getBoardBaseMatrixCellWidth())
		    	  				/board.getBoardBaseMatrixCellWidth();////ne referim la margini (daca mingea  
																		//// ajunge sau nu sa atinga ceva in timpul deplasarii)
		    	  break;
		      case WEST:
		    	  nextCellX = (m_centerX - board.getBoardMinX() - m_ball_radius + deltaX + board.getBoardBaseMatrixCellWidth())
		    	  				/board.getBoardBaseMatrixCellWidth();
		    	  break;
		      default:
		    	  break;
		    }
		    switch(dirY) {
		      case NORTH:
		    	  nextCellY = (m_centerY - board.getBoardMinY() - m_ball_radius + deltaY + board.getBoardBaseMatrixCellWidth())
		    	  				/board.getBoardBaseMatrixCellWidth();
		    	  break;
		      case SOUTH:
		    	  nextCellY = (m_centerY - board.getBoardMinY() + m_ball_radius + deltaY + board.getBoardBaseMatrixCellWidth())
		    	  				/board.getBoardBaseMatrixCellWidth();
		    	  break;
		     default:
		    	 break;
		    }
		    
		    boolean ballAtBorder = false;
			if((nextCellX != currentCenterCellX) && (nextCellY != currentCenterCellY)) ////miscare in diagonala
			{
				int distToNextCellX = 0;
				int distToNextCellY = 0;
			    switch(dirX) {
			      case EAST:
			    	  distToNextCellX = nextCellX*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerX - board.getBoardMinX() + m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  					//randul asta: x-ul maxim al mingii 
			    	  if(Math.abs(m_centerX + m_ball_radius - board.getBoardMaxX()) <= 2)
			    		  ballAtBorder = true;
			    	  break;
			      case WEST:
			    	  distToNextCellX = (nextCellX+1)*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerX - board.getBoardMinX() - m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  if(Math.abs(m_centerX - m_ball_radius - board.getBoardMinX()) <= 2)
			    		  ballAtBorder = true;
			    	  break;
			      default: 
			    	  break;
			    }
			    switch(dirY) {
			      case NORTH:
			    	  distToNextCellY = (nextCellY+1)*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerY - board.getBoardMinY() - m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  if(Math.abs(m_centerY - m_ball_radius - board.getBoardMinY()) <= 2)
			    		  ballAtBorder = true;
			    	  break;
			      case SOUTH:
			    	  distToNextCellY = nextCellY*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerY - board.getBoardMinY() + m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  break;
			      default: 
			    	  break;
			    }
			    
			    if(!ballAtBorder)
			    {
			    	if(Math.abs(1.0*distToNextCellX/deltaX) < Math.abs(1.0*distToNextCellY/deltaY))
			    	{
			    		nextCellY = currentCenterCellY;
			    	}
			    	else
			    	{
			    		nextCellX = currentCenterCellX;
			    	}				    	
			    }	    
			}
			if(baseMatrix[nextCellY][nextCellX] == CellType.EMPTY)
			{
				m_centerX = m_centerX + deltaX;
				m_centerY = m_centerY + deltaY;
				deltaX = 0;
				deltaY = 0;
				drawBall(g);
		    }
		    else if(baseMatrix[nextCellY][nextCellX] == CellType.REDWALL)
		    {
		    	///gameOver
		    	retNrExplodedBricks = -1;
				m_centerX = m_centerX + deltaX;
				m_centerY = m_centerY + deltaY;
				deltaX = 0;
				deltaY = 0;
				g.setColor(new Color(0,0,102));
				break;
		    }
		    else if((baseMatrix[nextCellY][nextCellX] == CellType.PADDLE) || (baseMatrix[nextCellY][nextCellX] == CellType.GREENWALL))
		    {
				int distToNextCellX = 0;
				int distToNextCellY = 0;
			    switch(dirX) {
			      case EAST:
			    	  distToNextCellX = nextCellX*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerX - board.getBoardMinX() + m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  break;
			      case WEST:
			    	  distToNextCellX = (nextCellX+1)*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerX - board.getBoardMinX() - m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  break;
			     default:
			    	  break;
			    }
			    switch(dirY) {
			      case NORTH:
			    	  distToNextCellY = (nextCellY+1)*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerY - board.getBoardMinY() - m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  break;
			      case SOUTH:
			    	  distToNextCellY = nextCellY*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerY - board.getBoardMinY() + m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  break;
			      default:
			    	  break;
			    }
			    if(deltaX == 0)
			    {
			    	//mingea se misca pe axa Oy, reflexie pe Oy
					m_centerX = m_centerX + distToNextCellX;
					m_centerY = m_centerY + distToNextCellY;
					deltaX -= distToNextCellX;
					deltaY -= distToNextCellY;
					deltaY = -deltaY;
					reflectMove(Direction.NORTH); 
			    }
			    else if(deltaY == 0)
			    {
			    	//mingea se misca pe axa Ox, reflexie pe Ox
					m_centerX = m_centerX + distToNextCellX;
					m_centerY = m_centerY + distToNextCellY;
					deltaX -= distToNextCellX;
					deltaY -= distToNextCellY;
					deltaX = -deltaX;
					reflectMove(Direction.EAST); 
			    }
			    else
			    {
			    	if(Math.abs(1.0*distToNextCellX/deltaX) < Math.abs(1.0*distToNextCellY/deltaY))///tip de reflexie
			    	{
			    		///reflexie pe x
						m_centerX = m_centerX + distToNextCellX;
						int moveOnY = deltaY*distToNextCellX/deltaX;
						m_centerY = m_centerY + moveOnY;
						deltaX -= distToNextCellX;
						deltaY -= moveOnY;
						deltaX = -deltaX;
						reflectMove(Direction.EAST); // adaptare stepX
			    	}
			    	else
			    	{
			    		//reflexie pe y
						m_centerY = m_centerY + distToNextCellY;
						int moveOnX = deltaX*distToNextCellY/deltaY;
						m_centerX = m_centerX + moveOnX;
						deltaX -= moveOnX;
						deltaY -= distToNextCellY;
						deltaY = -deltaY;
						reflectMove(Direction.NORTH); //adaptare stepY
			    	}			    
			    }
				drawBall(g);
		    	break;		    
		    }
		    else if(baseMatrix[nextCellY][nextCellX] == CellType.BRICK)
		    {
		    	///explozie caramizi 
		    	retNrExplodedBricks += 1;
		    	brickMatrix.elementAt(nextCellY-1).elementAt((nextCellX-1)/2).hideBrick(g);
		    	
		    	baseMatrix[nextCellY][nextCellX] = CellType.EMPTY;
		    	if(nextCellX%2 == 0)
		    		baseMatrix[nextCellY][nextCellX-1] = CellType.EMPTY;
		    	else
		    		baseMatrix[nextCellY][nextCellX+1] = CellType.EMPTY;	    		
		    
		    	///adaptare reflexie 
				int distToNextCellX = 0;
				int distToNextCellY = 0;
			    switch(dirX) {
			      case EAST:
			    	  distToNextCellX = nextCellX*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerX - board.getBoardMinX() + m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  break;
			      case WEST:
			    	  distToNextCellX = (nextCellX+1)*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerX - board.getBoardMinX() - m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  break;
			      default:
			    	  break;
			    }
			    switch(dirY) {
			      case NORTH:
			    	  distToNextCellY = (nextCellY+1)*board.getBoardBaseMatrixCellWidth() - 
			    	  					(m_centerY - board.getBoardMinY() - m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  break;
			      case SOUTH:
			    	  distToNextCellY = nextCellY*board.getBoardBaseMatrixCellWidth() - 
			    	  				(m_centerY - board.getBoardMinY() + m_ball_radius + board.getBoardBaseMatrixCellWidth());
			    	  break;
			     default:
			    	  break;
			    }
			    if(deltaX == 0)
			    {
			    	//mingea se misca pe axa Oy, reflexie pe Oy
					m_centerX = m_centerX + distToNextCellX;
					m_centerY = m_centerY + distToNextCellY;
					deltaX -= distToNextCellX;
					deltaY -= distToNextCellY;
					deltaY = -deltaY;
					reflectMove(Direction.NORTH); 
			    }
			    else if(deltaY == 0)
			    {
			    	//mingea se misca pe axa Ox, reflexie pe Ox
					m_centerX = m_centerX + distToNextCellX;
					m_centerY = m_centerY + distToNextCellY;
					deltaX -= distToNextCellX;
					deltaY -= distToNextCellY;
					deltaX = -deltaX;
					reflectMove(Direction.EAST); 
			    }
			    else
			    {
			    	if(Math.abs(1.0*distToNextCellX/deltaX) < Math.abs(1.0*distToNextCellY/deltaY))///tip de reflexie
			    	{
			    		///reflexie pe x
						m_centerX = m_centerX + distToNextCellX;
						int moveOnY = deltaY*distToNextCellX/deltaX;
						m_centerY = m_centerY + moveOnY;
						deltaX -= distToNextCellX;
						deltaY -= moveOnY;
						deltaX = -deltaX;
						reflectMove(Direction.EAST); // adaptare stepX
			    	}
			    	else
			    	{
			    		//reflexie pe y
						m_centerY = m_centerY + distToNextCellY;
						int moveOnX = deltaX*distToNextCellY/deltaY;
						m_centerX = m_centerX + moveOnX;
						deltaX -= moveOnX;
						deltaY -= distToNextCellY;
						deltaY = -deltaY;
						reflectMove(Direction.NORTH); //adaptare stepY
			    	}			    
			    }
				drawBall(g);
		    	break;		    		    
		    }
		}
		
		return retNrExplodedBricks;
	}
}
