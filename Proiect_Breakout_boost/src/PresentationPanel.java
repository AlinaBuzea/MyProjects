import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import javax.swing.BorderFactory;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;

public class PresentationPanel extends JPanel {

	public JButton button=new JButton("START");
	private JLabel imageLabel=new JLabel();
	
	PresentationPanel()
	{
		setPreferredSize(new Dimension(415, 550));
		setBorder(BorderFactory.createLineBorder(Color.black,Font.BOLD ));
		setBackground(new Color(0, 0, 102));
		
		imageLabel.setIcon(new ImageIcon("src\\PresentationBreakoutBoost.jpg"));
        
		setLayout(new GridBagLayout());

        GridBagConstraints gbc = new GridBagConstraints();
        gbc.anchor = GridBagConstraints.PAGE_START;
        gbc.fill = GridBagConstraints.HORIZONTAL;
        gbc.gridx=0;
        gbc.gridy=0;
        
        add(imageLabel,gbc);
        
        gbc.gridy=1;
       
        JPanel buttonPanel=new JPanel();
		button.setSize(70, 30);
		button.setBackground(new Color(51,153,255));
		button.setVisible(true);
		buttonPanel.add(button);
		buttonPanel.setBackground(new Color(0, 0, 102));
		
		add(buttonPanel, gbc);
	}
}

