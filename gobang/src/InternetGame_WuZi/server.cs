using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

using System.IO;
using System.Threading;
using System.Collections;

namespace InternetGame_WuZi
{
    public partial class fivechess : Form
    {
        #region name
        
        private const int None = 2;
        private const int White = 0;
        private const int Black = 1;
        private int[,] MyMapBack = new int[15, 15];

        static int ChessLength = 15;
        private int length = 30;

        private int nextPlayer = White;

        private Thread Flash;
        enum NUM{A=10,B,C,D,E,F};
        #endregion

       

        Stack mysta = new Stack();
        int WhichOne = -1;

        public fivechess()
        {
            InitializeComponent();
            
            
        }
#region Draw
        private void DrawBackGround()
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            Pen p = new Pen(Color.Red, 1);

            Point a = new Point(length, length);
            Point b = new Point(length * ChessLength, length);

            g.DrawLine(p, a, b);
            for (int i = 1; i <= ChessLength; i++)
            {
                a.Y = length * i;
                b.Y = a.Y;
                g.DrawLine(p, a, b);
            }

            a.X = length;
            a.Y = length;
            b.X = length;
            b.Y = length * ChessLength;
            for (int i = 1; i <= ChessLength; i++)
            {
                a.X = length * i;
                b.X = a.X;
                g.DrawLine(p, a, b);
            }

            a.X = length * 3;
            a.Y = length * 3;
            SolidBrush b1 = new SolidBrush(Color.Black);
            Rectangle re = new Rectangle(a.X - 5, a.Y - 5, 10, 10);
            g.FillEllipse(b1, re);
            //左上

            a.X = length * 13;
            a.Y = length * 13;
            re = new Rectangle(a.X - 5, a.Y - 5, 10, 10);
            g.FillEllipse(b1, re);
            //右下


            a.X = length * 8;
            a.Y = length * 8;
            re = new Rectangle(a.X - 5, a.Y - 5, 10, 10);
            g.FillEllipse(b1, re);
            //中心


            a.X = length * 3;
            a.Y = length * 13;
            re = new Rectangle(a.X - 5, a.Y - 5, 10, 10);
            g.FillEllipse(b1, re);
            //左下


            a.X = length * 13;
            a.Y = length * 3;
            re = new Rectangle(a.X - 5, a.Y - 5, 10, 10);
            g.FillEllipse(b1, re);
            //右上

            
        }

        private void DrawMap()
        {
            Point p = ChangeIt(this.PointToClient(Control.MousePosition));

            Graphics g = this.pictureBox1.CreateGraphics();
            
            
            for (int i = 0; i < ChessLength; i++)
            {
                for (int j = 0; j < ChessLength; j++)
                {
                    if (MyMapBack[i, j] == White)
                    {
                        SolidBrush b1 = new SolidBrush(Color.Yellow);
                        Rectangle re = new Rectangle(i * length + length / 2, j * length + length / 2, length, length);
                        g.FillEllipse(b1, re);
                    }
                    if (MyMapBack[i, j] == Black)
                    {
                        SolidBrush b1 = new SolidBrush(Color.Black);
                        Rectangle re = new Rectangle(i * length + length / 2, j * length + length / 2, length, length);
                        g.FillEllipse(b1, re);
                    }

                }
            }
        }

        private void InitMap()
        {
            for (int i = 0; i < ChessLength; i++)
            {
                for (int j = 0; j < ChessLength; j++)
                    MyMapBack[i, j] = None;
            }
        }

        
#endregion
        private void fivechess_Load(object sender, EventArgs e)
        {

            label5.Visible = false;
            label4.Text = "本地IP:"+getIPAddress();

            InitMap();

            Flash = new Thread(new ThreadStart(this.MyFlase));
            Flash.Start();
        
            ListenTcpThread = new Thread(StartListen);
            ListenTcpThread.Start();

        }


#region control
        private Point ChangeIt(Point temp)
        {
            int a = 0, b = 0;
            for (int i = 0; i < ChessLength; i++)
            {
                if (temp.X > length / 2 + length * i && temp.X < length * 3 / 2 + length * i)
                {
                    a = i + 1;
                    break;
                }
            }

            for (int i = 0; i < ChessLength; i++)
            {
                if (temp.Y > length / 2 + length * i && temp.Y < length * 3 / 2 + length * i)
                {
                    b = i + 1;
                    break;
                }
            }

            return new Point(a * length, b * length);
        }

        private void DrawFourPoint(Point p)
        {
            //int big = 10;
            int tt = 4;
             Graphics g = this.pictureBox1.CreateGraphics();
            SolidBrush b1 = new SolidBrush(Color.Red);
            Rectangle re = new Rectangle(p.X  - 14, p.Y  - 14, tt, tt);
            g.FillEllipse(b1, re);

            re = new Rectangle(p.X - 14, p.Y + 10, tt,tt);
            g.FillEllipse(b1, re);

            re = new Rectangle(p.X + 12, p.Y - 14, tt, tt);
            g.FillEllipse(b1, re);

            re = new Rectangle(p.X + 11, p.Y + 11, tt, tt);
            g.FillEllipse(b1, re);


        }

        private void ReDrawFourPoint(Point p)
        {
            //int big = 10;
            int tt = 4;
            Graphics g = this.pictureBox1.CreateGraphics();
            SolidBrush b1 = new SolidBrush(Color.White);
            Rectangle re = new Rectangle(p.X - 14, p.Y - 14, tt, tt);
            g.FillEllipse(b1, re);

            re = new Rectangle(p.X - 14, p.Y + 10, tt, tt);
            g.FillEllipse(b1, re);

            re = new Rectangle(p.X + 12, p.Y - 14, tt, tt);
            g.FillEllipse(b1, re);

            re = new Rectangle(p.X + 11, p.Y + 11, tt, tt);
            g.FillEllipse(b1, re);


        }


        Point pp;
        int flag = 0;
        int bflag = 0;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (bflag < 5)
            {
                DrawBackGround();
                bflag ++;
            }

            Point p = ChangeIt(this.PointToClient(Control.MousePosition));

            if (p.X + p.Y == 0)
            {
                ;
            }
            else
            {
                if (p != pp)
                {
                    ReDrawFourPoint(pp);
                    pp = p;
                    flag = 0;
                }
                else
                {
                    if (flag == 0)
                    {
                        DrawFourPoint(pp);
                        flag = 1;
                    }
                }
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 5)
            {
                MessageBox.Show("请设置你的ip");
                return;
            }

            if (button2.Visible == true)
            {
                MessageBox.Show("请抢先手");
                return;
            }

            if ((pp.X / length - 1) >= 0 && (pp.Y / length - 1) >= 0 
                 && MyMapBack[pp.X / length - 1, pp.Y / length - 1] == None)
            {

           
                if (mysta.Count % 2== WhichOne)
                {
                    MyMapBack[pp.X / length - 1, pp.Y / length - 1] = WhichOne;
                    mysta.Push(new Point(pp.X / length - 1, pp.Y / length - 1));

                    String temp = "";
                    temp += "###";
                    temp += (char)(pp.X / length - 1 + '0');
                    temp += (char)(pp.Y / length - 1 + '0');
                    temp += (char)(WhichOne + '0');
                    temp += "###";

                    SendSys(temp);

                    CheckGame(pp.X / length - 1, pp.Y / length - 1);
                }
                else
                {
                    MessageBox.Show("请等待对手下棋");
                }

            }
            
            DrawMap();
        }

        private void MyFlase()
        {
            while (true)
            {
                DrawMap();
                Thread.Sleep(100);
            }
        }
#endregion





//########################################################################
//######################################################################
        //network
       
        private Thread ListenTcpThread;
        private TcpListener tcpListener;

        private static string getIPAddress()
        {
            System.Net.IPAddress addr;

            addr = new System.Net.IPAddress(Dns.GetHostByName(
                Dns.GetHostName()).AddressList[0].Address);
            return addr.ToString();
        }


        char CH(int n)
        {
            if (n >= 0 && n < 9)
                return (char)(n+'1');

            return (char)('A' + n - 9);
        }


        private void StartListen()
        {
            tcpListener = new TcpListener(888);
        
            tcpListener.Start();

            richTextBox1.Text += "服务已启动....\n等待连接...\n";
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                NetworkStream ns = tcpClient.GetStream();
                byte[] bytes=new byte[1024];
                int byteRead = ns.Read(bytes, 0, bytes.Length);
                string Msg =  Encoding.Default.GetString(bytes, 0, byteRead);


                int temp = Correct(Msg, 1);
                if (temp==0)// usr'a messge
                    richTextBox1.Text += "远程：" + Msg + "\n";
                else
                {
                    if (temp == 1)//do something after return 1
                    {
                        MyMapBack[Msg[3] - '0', Msg[4] - '0'] = Msg[5] - '0';

                        richTextBox1.Text += "位置：";
                        richTextBox1.Text += CH(Msg[3] - '0');
                        richTextBox1.Text += CH(Msg[4] - '0');
                        richTextBox1.Text += "\n";


                        mysta.Push(new Point(Msg[3] - '0', Msg[4] - '0'));


                        CheckGame(Msg[3] - '0', Msg[4] - '0');
                    }
                    //do nothing after return 2
                    
                }
            }
        }

        private int Correct(String temp, int Fromyuan = 0)
        {
            if (temp.Length == 9)
            {
                for (int i = 0; i < 3; i++)
                    if (temp[i] != '#')
                        return 0;

                for (int i = 6; i < 9; i++)
                    if (temp[i] != '#')
                        return 0;
                #region sys_message
                if (Fromyuan == 1)
                {
                    if (temp[3] == 'h' && temp[4] == 'h')
                    {
                        WhichOne = 1;
                        button2.Visible = false;

                        textBox1.Visible = false;
                        label5.Text = textBox1.Text;
                        label5.Visible = true;

                        return 2;//sys_message   shouldn't do something
                    }

                    if (temp[5] == None)
                    {
                        Point pp;
                        pp = (Point)mysta.Pop();
                        MyMapBack[pp.X, pp.Y] = None;


                        richTextBox1.Text += "悔棋！\n";


                        FillWhite();
                        DrawBackGround();
                        return 2;//sys_message shouldn't do something
                    }
                }
                #endregion
                return 1;//sys_message  should add something
            }
            return 0;//usr's message
            
        }


        private void fivechess_FormClosed(object sender, FormClosedEventArgs e)
        {
             try
            {
                if (this.tcpListener!=null)
                    this.tcpListener.Stop();
                if (ListenTcpThread!=null)
                {
                    if (this.ListenTcpThread.ThreadState==ThreadState.Running)
                    {
                        this.ListenTcpThread.Abort();
                    }
                }
            }
            catch (Exception Err)
            {                
                MessageBox.Show(Err.Message ,"信息提示",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
            }

            Flash.Abort();
        }

 
        private void SendSys(String Msg = "")
        {
            if (textBox1.Text.Length < 5)
            {
                MessageBox.Show("请设置你的目的ip");
                return;// is correct ip?
            }
            try
            {
                if (Msg.Length == 0)
                    return;//is a message?

                TcpClient tcpClient = new TcpClient(this.textBox1.Text, 888);

                NetworkStream ns = tcpClient.GetStream();
                StreamWriter sw = new StreamWriter(ns);

                sw.Write(Msg);
                sw.Flush();

                sw.Close();
                tcpClient.Close();

                
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, "信息提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 5)
            {
                MessageBox.Show("请设置你的目的ip");
                return;// is correct ip?
            }
            try
            {
                String Msg = "";
                Msg += this.richTextBox2.Text;
 

                if (Msg.Length == 0)
                    return;//is a message?

                TcpClient tcpClient = new TcpClient(this.textBox1.Text, 888);

                NetworkStream ns = tcpClient.GetStream();
                StreamWriter sw = new StreamWriter(ns);


                //sw.Write(temp);
                sw.Write(Msg);
                sw.Flush();

                sw.Close();
                tcpClient.Close();

         
                this.richTextBox1.AppendText("本地：" + this.richTextBox2.Text + "\n");
                this.richTextBox2.Clear();
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, "信息提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 5)
                return;
            WhichOne = 0;
            SendSys("###hh1###");
            button2.Visible = false;
            textBox1.Visible = false;


            label5.Text = textBox1.Text;
            label5.Visible = true;

        }


        private int CheckGame(int x, int y)
        {
            //heng
            int sum = 1, flag = 0;
            for (int i = x + 1; i < 15; i++)
            {
                if (MyMapBack[i, y] == MyMapBack[x, y])
                    sum++;
                else
                    break;
            }
            for (int i = x - 1; i >= 0; i--)
            {
                if (MyMapBack[i, y] == MyMapBack[x, y])
                    sum++;
                else
                    break;
            }
            if (sum >= 5)
                flag = 1;

            //shu
            sum = 1;
            for (int i = y + 1; i < 15; i++)
            {
                if (MyMapBack[x, i] == MyMapBack[x, y])
                    sum++;
                else
                    break;
            }
            for (int i = y - 1; i >= 0; i--)
            {
                if (MyMapBack[x, i] == MyMapBack[x, y])
                    sum++;
                else
                    break;
            }
            if (sum >= 5)
                flag = 1;

            //xie
            sum = 1;
            for (int i = 1; x + i < 15 && y + i < 15; i++)
            {
                if (MyMapBack[x + i, y + i] == MyMapBack[x, y])
                    sum++;
                else
                    break;
            }

            for (int i = 1; x - i >= 0 && y - i >= 0; i++)
            {
                if (MyMapBack[x - i, y - i] == MyMapBack[x, y])
                    sum++;
                else
                    break;
            }

            if (sum >= 5)
                flag = 1;


            //xie
            sum = 1;
            for (int i = 1; x - i >=0  && y + i < 15; i++)
            {
                if (MyMapBack[x - i, y + i] == MyMapBack[x, y])
                    sum++;
                else
                    break;
            }

            for (int i = 1; x + i < 15 && y - i >= 0; i++)
            {
                if (MyMapBack[x + i, y - i] == MyMapBack[x, y])
                    sum++;
                else
                    break;
            }

            if (sum >= 5)
                flag = 1;

//#########################

           
            if (flag == 1)
            {
                if (WhichOne == MyMapBack[x, y])
                    MessageBox.Show("你赢了！");
                else
                    MessageBox.Show("你输了！");
            }
            return 0;
        }

        private void button3_Click(object sender, EventArgs e)//restart
        {
            FillWhite();

            InitMap();
            DrawBackGround();

            WhichOne = -1;

            button2.Visible = true;
            richTextBox1.Clear();

            mysta.Clear();

        }

        private void FillWhite()
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            SolidBrush b1 = new SolidBrush(Color.White);

            g.FillRectangle(b1, 0, 0, 470, 470);
        }

        private void HuiQi()
        {
            Point pp;
            if (mysta.Count != 0)
            {
                pp = (Point)mysta.Pop();

                MyMapBack[pp.X, pp.Y] = None;
                //mysta.Push(new Point(pp.X / length - 1, pp.Y / length - 1));

                String temp = "";
                temp += "###";
                temp += (char)(pp.X);
                temp += (char)(pp.Y);
                temp += (char)(None);
                temp += "###";

                SendSys(temp);
            }

            FillWhite();
    
            DrawBackGround();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HuiQi();
        }

    }
}