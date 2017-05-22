package ChatMain;

import java.awt.Container;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.Toolkit;
import java.io.IOException;
import java.sql.Date;
import java.text.SimpleDateFormat;

import javax.swing.*;

public class ChatMain extends JFrame {
	
	private static final long serialVersionUID = 8764532995177762817L;
	Config C_8764532995177762817L = new Config();

	static ChatMain 	mainJF 		= new ChatMain();	
	
	public static final int WTH = 600;
	public static final int HHT = 600;
	
	public static void main(String[] args){
			
		InitJFrame();
		
		AddListener();
		
		Config.receiveThread.start();
		
		
		BjPanel pl = new BjPanel();	
		pl.setLayout(null);
		Controls.InitPanel(pl);
		Container contentPane = mainJF.getContentPane();
		contentPane.add(pl);
		pl.setOpaque(true);
		
		
		mainJF.setVisible(true);
	}
	
	static void InitJFrame()
	{
		mainJF.setResizable(false);
		mainJF.setTitle("JAVA简易聊天工具");
		mainJF.setSize(600,600);
		
		mainJF.setLocationRelativeTo(null);	
		//mainJF.setLayout(null);
	}
	
	
	static void AddListener()
	{
		
		mainJF.addWindowListener(new Controls.MyWinowsListener());	
		Controls.btnSend.addActionListener(new Controls.MyActionListener_Send());
		Controls.btnSet.addActionListener(new Controls.MyActionListener_Set());
	}
	
	
	
	
	static public class ReceiveMsg extends Thread{

		@Override
		public void run() {
		
			while (Config.isRun)
			{
				String str = MySocket.GetMsg();
				
				Date now = new Date(System.currentTimeMillis());
				SimpleDateFormat df = new SimpleDateFormat("MM-dd HH:mm:ss");//设置日期格式
				
				str = str.trim();
				if (str != "") 
				{
					Controls.textArea.setText(Controls.textArea.getText() + "\n" + df.format(now) + " 对方:\n"  +  str);
					Controls.textArea.setCaretPosition(Controls.textArea.getText().length());
				}
				else {
					try {
						Thread.sleep(Config.sleepTime * 5);
					} 
					catch (InterruptedException e) {
						e.printStackTrace();
					}
				}
				try {
					Thread.sleep(Config.sleepTime);
				} 
				catch (InterruptedException e) {
					e.printStackTrace();
				}
			}
			try {
				Config.sSocket.close();		
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
		
	}
}


@SuppressWarnings("serial")
class BjPanel extends JPanel {

	Image im;

	public BjPanel() {
		im = Toolkit.getDefaultToolkit().getImage("Back.jpg");// 需要注意的是如果用相对路径载入图片,则图片文件必须放在类文件所在文件夹或项目的根文件夹中,否则必须用绝对路径。
	}

	public void paintComponent(Graphics g) {
		super.paintComponent(g);
		int imWidth = im.getWidth(this);
		int imHeight = im.getHeight(this); // 定义图片的宽度、高度
		int FWidth = getWidth();
		int FHeight = getHeight();// 定义窗口的宽度、高度
		int x = (FWidth - imWidth) / 2;
		int y = (FHeight - imHeight) / 2;// 计算图片的坐标,使图片显示在窗口正中间
		g.drawImage(im, x, y, null);// 绘制图片
	}
}
	
