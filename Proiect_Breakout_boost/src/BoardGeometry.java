
public class BoardGeometry {
	private int m_borderBoxMinX;
	private int m_borderBoxMaxX;
	private int m_borderBoxMinY;
	private int m_borderBoxMaxY;
	private int m_baseMatrixCellWidth;
	
	public BoardGeometry()
	{
		m_borderBoxMinX = 20;
		m_borderBoxMaxX = 380;
		m_borderBoxMinY = 40;
		m_borderBoxMaxY = 460;
		m_baseMatrixCellWidth = 20;
	}
	public BoardGeometry(int borderBoxMinX, int borderBoxMaxX, int borderBoxMinY, int borderBoxMaxY, int baseMatrixCellWidth)
	{
		m_borderBoxMinX = borderBoxMinX;
		m_borderBoxMaxX = borderBoxMaxX;
		m_borderBoxMinY = borderBoxMinY;
		m_borderBoxMaxY = borderBoxMaxY;
		m_baseMatrixCellWidth = baseMatrixCellWidth;
	}
	public int getBoardMinX()
	{
		return m_borderBoxMinX;
	}
	public void setBoardMinX(int borderBoxMinX)
	{
		m_borderBoxMinX=borderBoxMinX;
	}
	public int getBoardMinY()
	{
		return m_borderBoxMinY;
	}
	public void setBoardMinY(int borderBoxMinY)
	{
		m_borderBoxMinY=borderBoxMinY;
	}
	public int getBoardMaxX()
	{
		return m_borderBoxMaxX;
	}
	public void setBoardMaxX(int borderBoxMaxX)
	{
		m_borderBoxMaxX=borderBoxMaxX;
	}
	public int getBoardMaxY()
	{
		return m_borderBoxMaxY;
	}
	public void setBoardMaxY(int borderBoxMaxY)
	{
		m_borderBoxMaxY=borderBoxMaxY;
	}
	public int getBoardBaseMatrixCellWidth()
	{
		return m_baseMatrixCellWidth;
	}
	public void setBoardBaseMatrixCellWidth(int baseMatrixCellWidth)
	{
		m_baseMatrixCellWidth=baseMatrixCellWidth;
	}
}
