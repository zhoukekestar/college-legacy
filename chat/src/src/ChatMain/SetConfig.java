package ChatMain;


import java.awt.FlowLayout;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JTextArea;
import javax.swing.JTextPane;

public class SetConfig extends JFrame {

	private static final long serialVersionUID = -819581390694314352L;
	
	String cur_IP;
	JTextPane jTextPane = new JTextPane();
	
	public SetConfig()
	{
		cur_IP = Config.Addr;
		
		this.setVisible(true);
		this.setLocationRelativeTo(null);
		this.setSize(200, 100);
		this.setLayout(null);
		this.setResizable(false);
		addControl();
		
		
	}
	
	private void addControl()
	{
		JLabel jLabel = new JLabel("对方IP:");
		jLabel.setSize(100, 25);
		jLabel.setLocation(5, 5);
		this.add(jLabel);
		
		
		jTextPane.setSize(100, 25);
		jTextPane.setLocation(5, 35);
		jTextPane.setText(cur_IP);
		
		this.add(jTextPane);
		
		JButton jButton = new JButton("保存");
		jButton.setSize(60,25);
		jButton.setLocation(120, 35);
		jButton.addActionListener(new MyActionListener());
		this.add(jButton);
	}
	
	public void Show()
	{
		this.setVisible(true);
	}
	
	public class MyActionListener implements ActionListener{

		@Override
		public void actionPerformed(ActionEvent e) {
			
			String str = jTextPane.getText().trim();
			Config.ChangeIP(str);
			Config.Addr = str;
			
			ShowDialog();
			dispose();
		}
	}
	
	public void ShowDialog()
	{
		JDialog jDialog = new JDialog();
		jDialog.setLayout(new FlowLayout());
		//jDialog.setTitle("提示");
		jDialog.setSize(150,80);
		jDialog.setLocationRelativeTo(null);
		
		JTextArea jTextArea = new JTextArea();
		jTextArea.setFont(new Font("宋体", 1, 21));
		jTextArea.setText("保存成功");
		jDialog.add(jTextArea);
		
		jDialog.setVisible(true);		
	}
}
