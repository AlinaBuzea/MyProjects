import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import javax.swing.BorderFactory;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.Timer;

import java.util.Vector;
import java.awt.event.KeyListener;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

enum CellType{
	GREENWALL,
	REDWALL,
	PADDLE,
	BRICK,
	EMPTY
}

enum Direction{
	NORTH,
	SOUTH,
	EAST,
	WEST
}
public class MyPanel extends JPanel implements KeyListener{

	private Vector<Vector<Brick>> brickMatrix=new Vector<Vector<Brick>>();
	private Ball ball=new Ball();
	private Paddle paddle=new Paddle();
	private BoardGeometry board=new BoardGeometry();
	public Timer mytimer;
	private boolean timerStarted=false;
	private boolean timerEnded=false;
	private int nrBricksLeftInPlay = -1;
	private int score;
	private int level;
	
	private JTextField scoreField= new JTextField();
	private JLabel imageLabel=new JLabel();
	
	private JLabel scoreLabel=new JLabel("Score: 0");
	private JLabel levelLabel=new JLabel("Level: 1");
	private CellType baseMatrix[][] = new CellType[(board.getBoardMaxY() - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth() + 2]
													[(board.getBoardMaxX() - board.getBoardMinX())/board.getBoardBaseMatrixCellWidth() + 2];
	
	private Direction paddleLastDir=Direction.WEST;
	
	public String getScore()
	{
		return scoreLabel.toString();
	}
	
	public String getLevel()
	{
		return levelLabel.toString();
	}
	
	public MyPanel()
	{
		setPreferredSize(new Dimension(415, 550));
		setBorder(BorderFactory.createLineBorder(Color.black,Font.BOLD ));
		setBackground(new Color(0, 0, 102));
		initializeMatrix();
		setFocusable(true);
		addKeyListener(this);		
		
		ActionListener taskPerformer = new ActionListener()
		{
	      public void actionPerformed(ActionEvent evt)
	      {
	    	  int nrExplodedBricks = ball.moveBall(getGraphics(), brickMatrix, baseMatrix, board);
	    	  if(nrExplodedBricks != 0)
	    	  {
		    	  updateScoreAndLevel(nrExplodedBricks);
	    	  }
	      }
	    };
	    
	    mytimer = new Timer ((int)ball.getStepTime(), taskPerformer);
	    
	    initializeMyPanel();
		
	}
	
	
	@Override
	public void keyPressed(KeyEvent event)
	{
		if(event.getKeyCode() == KeyEvent.VK_LEFT)
		{
			if(!timerStarted && paddle.moveLeftWithBall(board, ball))
			{
				ball.setStepX(0);
				paddleLastDir=Direction.WEST;
				int paddleCellsLength = paddle.getPaddleLength() / board.getBoardBaseMatrixCellWidth();
				baseMatrix[(board.getBoardMaxY() - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth()]
						   [paddle.getCoordX()/board.getBoardBaseMatrixCellWidth()] = CellType.PADDLE;
				baseMatrix[(board.getBoardMaxY() - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth()]
							[paddle.getCoordX()/board.getBoardBaseMatrixCellWidth() + paddleCellsLength] = CellType.EMPTY;
			}		
			else if(paddle.moveLeft(board))
			{
				int paddleCellsLength = paddle.getPaddleLength() / board.getBoardBaseMatrixCellWidth();
				baseMatrix[(board.getBoardMaxY() - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth()]
							[paddle.getCoordX()/board.getBoardBaseMatrixCellWidth()] = CellType.PADDLE;
				baseMatrix[(board.getBoardMaxY() - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth()]
							[paddle.getCoordX()/board.getBoardBaseMatrixCellWidth() + paddleCellsLength] = CellType.EMPTY;
			}
			repaint();
		}
		if(event.getKeyCode() == KeyEvent.VK_RIGHT)
		{
			if(!timerStarted && paddle.moveRightWithBall(board, ball))
			{
				ball.setStepX(0);
				paddleLastDir=Direction.EAST;
				int paddleCellsLength = paddle.getPaddleLength() / board.getBoardBaseMatrixCellWidth();
				baseMatrix[(board.getBoardMaxY() - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth()]
							[paddle.getCoordX()/board.getBoardBaseMatrixCellWidth() - 1] = CellType.EMPTY;
				baseMatrix[(board.getBoardMaxY() - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth()]
							[paddle.getCoordX()/board.getBoardBaseMatrixCellWidth() + paddleCellsLength - 1] = CellType.PADDLE;
			}
			else if(paddle.moveRight(board))
			{
				int paddleCellsLength = paddle.getPaddleLength() / board.getBoardBaseMatrixCellWidth();
				baseMatrix[(board.getBoardMaxY() - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth()]
							[paddle.getCoordX()/board.getBoardBaseMatrixCellWidth() - 1] = CellType.EMPTY;
				baseMatrix[(board.getBoardMaxY() - board.getBoardMinY())/board.getBoardBaseMatrixCellWidth()]
							[paddle.getCoordX()/board.getBoardBaseMatrixCellWidth() + paddleCellsLength - 1] = CellType.PADDLE;
			}
			repaint();
		}
		if(event.getKeyCode()==KeyEvent.VK_SPACE)
		{
			if(!timerStarted)
			{
				if(paddleLastDir==Direction.EAST)
				{
					ball.setStepX(ball.getBallRadius());
				}
				else
				{
					ball.setStepX(-ball.getBallRadius());
				}
				ball.setStepY(-ball.getBallRadius());
				mytimer.start();
				timerStarted=true;
			}
			repaint();
		}
	}
    
	@Override
	public void keyReleased(KeyEvent event){
    	
		repaint();
	}
    
	@Override
	public void keyTyped(KeyEvent event) {

    }
	
	private void initializeMyPanel()
	{
		score = 0;
	    level = 1;
	    
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.anchor = GridBagConstraints.SOUTHWEST;
        gbc.fill = GridBagConstraints.HORIZONTAL;
        levelLabel.setSize(100, 30);
        levelLabel.setForeground(Color.WHITE);
        levelLabel.setVisible(true);
        levelLabel.setAlignmentX(105);
        levelLabel.setAlignmentY(500);
		add(levelLabel, gbc);
		gbc.anchor = GridBagConstraints.SOUTHEAST;
		scoreLabel.setSize(100, 30);
		scoreLabel.setForeground(Color.WHITE);
		scoreLabel.setVisible(true);
		scoreLabel.setAlignmentX(260);
		scoreLabel.setAlignmentY(500);
		add(scoreLabel, gbc);
		
		updateScoreAndLevel(0);
	}
	
	private void initializeMatrix()
	{
		Brick initial_brick=new Brick();
		
		int space=0;
		int nrRows = (board.getBoardMaxY() - board.getBoardMinY())/initial_brick.getBrickWidth();
		int nrCols = (board.getBoardMaxX() - board.getBoardMinX())/(initial_brick.getBrickLength() + space);
		int nrRowsFilled = nrRows/2 - 1;
		
		for(int index_rows=0;index_rows<nrRows+2;index_rows++)
		{
			CellType line_baseMatrix[] = new CellType[2*nrCols+2];
			for(int index_columns=0;index_columns<2*nrCols+2;index_columns++)
			{
				if(index_rows == 0)
					line_baseMatrix[index_columns] = CellType.GREENWALL;
				else if(index_rows == (nrRows+1))
					line_baseMatrix[index_columns] = CellType.REDWALL;
				else if(index_rows <= nrRowsFilled)
				{
					if((index_columns==0) || (index_columns==2*nrCols+1))
						line_baseMatrix[index_columns] = CellType.GREENWALL;
					else
						line_baseMatrix[index_columns] = CellType.BRICK;						
				}
				else
				{
					if((index_columns==0) || (index_columns==2*nrCols+1))
						line_baseMatrix[index_columns] = CellType.GREENWALL;
					else
						line_baseMatrix[index_columns] = CellType.EMPTY;						
				}
			}
			baseMatrix[index_rows] = line_baseMatrix;
		}

		nrBricksLeftInPlay = 0;
		for(int index_rows=0;index_rows<nrRowsFilled;index_rows++)
		{
			Vector<Brick> line=new Vector<Brick>();
			for(int index_columns=0;index_columns<nrCols;index_columns++)
			{
				Brick br=new Brick(initial_brick.getCoordX()+index_columns*(initial_brick.getBrickLength()+space),
						initial_brick.getCoordY()+index_rows*(initial_brick.getBrickWidth()+space),
						initial_brick.getBrickWidth(),initial_brick.getBrickLength(), true);
				nrBricksLeftInPlay += 1;
				line.add(br);
			}
			brickMatrix.add(line);
		}
	}
	
	private boolean increaseLevel(int score, int newScore)
	{
		if(((score<=10) && (newScore>10))
				|| ((score<=23) && (newScore>23))
				|| ((score<=39) && (newScore>39))
				|| ((score<=59) && (newScore>59))
				|| ((score<=83) && (newScore>83))
				|| ((score<=114) && (newScore>114))
				|| ((score<=152) && (newScore>152))
				|| ((score<=200) && (newScore>200))
				|| ((score<=260) && (newScore>260))
				|| ((score<=335) && (newScore>335))
				|| ((score<=428) && (newScore>428))
				|| ((score<=544) && (newScore>544))
				|| ((score<=690) && (newScore>690))
				|| ((score<=872) && (newScore>872))
				|| ((score<=1100) && (newScore>1100))
				|| ((score<=1384) && (newScore>1384))
				|| ((score<=1750) && (newScore>1740)))
			return true;
		else
			return false;
	}
	
	private void EndGame()
	{
		imageLabel.setIcon(new ImageIcon("src//PresentationBreakoutBoost.jpg"));
        
		setLayout(new GridBagLayout());

        GridBagConstraints gbc = new GridBagConstraints();
        gbc.anchor = GridBagConstraints.PAGE_START;
        gbc.fill = GridBagConstraints.HORIZONTAL;
        
		gbc.gridx=0;
        gbc.gridy=0;
        
        scoreField.setText(scoreLabel.getText());
        scoreField.setFont(new Font("Georgia", Font.BOLD, 50));
        scoreField.setForeground(Color.WHITE);
        scoreField.setOpaque(false);
        scoreField.setHorizontalAlignment(JTextField.CENTER);
        scoreField.setVisible(true);
        
        add(scoreField,gbc);
        
		gbc.gridy = 1;
		add(imageLabel,gbc);
        
	}
	
	private void updateScoreAndLevel(int nrExplodedBricks)
	{
		if(nrExplodedBricks == -1)
		{
			ball.stopMovement();
			scoreLabel.setVisible(false);
			levelLabel.setVisible(false);
			timerEnded=true;
			repaint();
			
			EndGame();
			repaint();
			
			return;
		}
		
		nrBricksLeftInPlay -= nrExplodedBricks;
		int newScore = score + nrExplodedBricks*level;
		
		if(increaseLevel(score, newScore))
		{
			++level;
			ball.setStepTime(ball.getStepTime()/1.15);
			mytimer.setDelay((int)ball.getStepTime());
		}
		
		score = newScore;
		
		scoreLabel.setText("Score: " + score);
		levelLabel.setText("Level: " + level);
		
		if(nrBricksLeftInPlay == 0)
		{
			ball.stopMovement();
			timerEnded=true;
			repaint();
			
			EndGame();
			repaint();
		}
	}
	
	protected void paintComponent(Graphics g) 
	{
		if(timerEnded==true)
		{
			g.setColor(new Color(0,0,102));
			super.paintComponent(g);
			return;
		}
		
		super.paintComponent(g);
		ball.drawBall(g);
		paddle.drawPaddle(g);
		
		for(int index_rows=0;index_rows<brickMatrix.size();index_rows++)
		{
			for(int index_columns=0;index_columns<brickMatrix.elementAt(index_rows).size();index_columns++)
			{
				if(brickMatrix.elementAt(index_rows).elementAt(index_columns).getVisible()==true)
				{
					brickMatrix.elementAt(index_rows).elementAt(index_columns).drawBrick(g);
				}
			}
		}
		
		g.setColor(new Color(255,0,0));
		g.fillRect(board.getBoardMinX(), board.getBoardMaxY(), board.getBoardMaxX() - board.getBoardMinX(), 5);
		g.setColor(new Color(0,255,0));
		g.fillRect(board.getBoardMinX(), board.getBoardMinY()-5, board.getBoardMaxX() - board.getBoardMinX(), 5);
		g.fillRect(board.getBoardMinX()-5, board.getBoardMinY()-5, 5, board.getBoardMaxY() - board.getBoardMinY()+10);
		g.fillRect(board.getBoardMaxX(), board.getBoardMinY()-5, 5, board.getBoardMaxY() - board.getBoardMinY()+10);		

		g.setColor(new Color(0,0,102));
	}
}
