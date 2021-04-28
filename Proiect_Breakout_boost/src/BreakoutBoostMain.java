import javax.swing.BorderFactory;
import javax.swing.JFrame;
import javax.swing.SwingUtilities;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Toolkit;
import java.awt.BorderLayout;
import java.awt.Color;

import javax.swing.JButton;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;

public class BreakoutBoostMain {

	private static void UI() {
		
		JFrame myFrame= new JFrame("Breakout Boost");
		myFrame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		myFrame.setSize(new Dimension(415, 550));
		
		Dimension dim = Toolkit.getDefaultToolkit().getScreenSize();
		myFrame.setLocation(dim.width/2-myFrame.getSize().width/2, dim.height/2-myFrame.getSize().height/2);
		
		PresentationPanel presentation_panel=new PresentationPanel();
		MyPanel main_panel=new MyPanel();
		
		myFrame.add(presentation_panel);
		myFrame.setVisible(true);
		
		presentation_panel.button.addActionListener(new ActionListener(){
			 
			public void actionPerformed(ActionEvent e)
			{
				presentation_panel.button.setVisible(false);
				myFrame.remove(presentation_panel);
				myFrame.add(main_panel);
				main_panel.requestFocusInWindow();
			}
		});		
		
		myFrame.setVisible(true);
		
	}
	
	public static void main(String[] args) {
		SwingUtilities.invokeLater(new Runnable() {
			public void run()
			{
				UI();
			}
				
		});
	}
	
}

	


