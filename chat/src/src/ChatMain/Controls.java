package ChatMain;


import java.awt.Color;
import java.awt.Font;
import java.awt.TextArea;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;

import java.sql.Date;
import java.text.SimpleDateFormat;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;

public class Controls extends JFrame{
	
	private static final long serialVersionUID = -775407016492810063L;
	

	static TextArea  	textArea 	= new TextArea("",1,1,TextArea.SCROLLBARS_VERTICAL_ONLY);
	static TextArea 	textArea2 	= new TextArea("",1,1,TextArea.SCROLLBARS_VERTICAL_ONLY);
	static JButton 		btnSend 	= new JButton();
	static JButton		btnSet		= new JButton();
	
	
	@SuppressWarnings("deprecation")
	static void InitPanel(BjPanel panel)
	{
		//新建面板 并 设置面板
		////////////////////////////////////
		//JPanel panel = new JPanel();
		
		//Panel panel = new Panel();
		
		panel.setLayout(null);
		panel.setLocation(0, 0);
		panel.setSize(600,600);
		
		
		//显示IP地址
		//////////////////////////////////////////////////////////
		JLabel noteJLabel = new JLabel("本地IP：" + Config.Local);
		noteJLabel.setSize(400, 20);
		noteJLabel.setFont(new Font("宋体", 1, 21));
		noteJLabel.setLocation(10, 10);
		panel.add(noteJLabel);
		
		
		//显示通讯主窗口
		////////////////////////////////////////////
		textArea.setSize(500, 400);
		textArea.setFont(new Font("宋体", 3, 16));
		
		
		textArea.setForeground(new Color(0,0,0));
	
		textArea.setLocation(10, 50);
		textArea.enable(false);
		textArea.setForeground(new Color(52,50,151));
		textArea.setText("程序已启动...");
		panel.add(textArea);
		
		
		//显示发送信息窗口
		//////////////////////////////////////////////////
		textArea2.setLocation(10, 460);
	
		textArea2.setFont(new Font("宋体", 3, 18));
		textArea2.setBackground(new Color(243, 247, 242));
		textArea2.setSize(500, 90);
		panel.add(textArea2);
		
		//发送按钮
		////////////////////////////////////////
		btnSend.setText("发送");
		btnSend.setFont(new Font("宋体", 1, 12));
		btnSend.setSize(60,90);
		btnSend.setLocation(530, 460);		
		panel.add(btnSend);
		
		
		//设置按钮
		//////////////////////////////////
		btnSet.setText("设置");
		btnSet.setSize(60,400);
		btnSet.setFont(new Font("宋体", 1, 12));
		btnSet.setLocation(530, 50);	
		panel.add(btnSet);

	}


	static class MyWinowsListener implements WindowListener{
		
		@Override
		public void windowClosing(WindowEvent e) {
			Config.isRun = false;
			try {
				Thread.sleep(Config.sleepTime * 2);
			} catch (InterruptedException e1) {
				e1.printStackTrace();
			}
			System.exit(0);
		}

		@Override
		public void windowOpened(WindowEvent e) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void windowClosed(WindowEvent e) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void windowIconified(WindowEvent e) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void windowDeiconified(WindowEvent e) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void windowActivated(WindowEvent e) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void windowDeactivated(WindowEvent e) {
			// TODO Auto-generated method stub
			
		}

	}
	
	static class MyActionListener_Send implements ActionListener{

		@Override
		public void actionPerformed(ActionEvent e) {
			
			Date now = new Date(System.currentTimeMillis());
			SimpleDateFormat df = new SimpleDateFormat("MM-dd HH:mm:ss");//设置日期格式
			
			
			String temp = Controls.textArea2.getText().trim();
			if (temp.length() == 0) return;
			Controls.textArea.setText(Controls.textArea.getText() + "\n" + df.format(now) + " 自己:\n"  +  temp + "\n");
			Controls.textArea.setCaretPosition(textArea.getText().length());
			
			MySocket.SendMes(temp);
			
			Controls.textArea2.setText("");
		}
		
	}
	
	static class MyActionListener_Set implements ActionListener{

		@Override
		public void actionPerformed(ActionEvent e) {
			
			SetConfig setConfig = new SetConfig();
			setConfig.Show();
		}
	}


}
