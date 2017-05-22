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

namespace InternetGame_WuZi
{
    public partial class 网络五子棋 : Form
    {
        public 网络五子棋()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.Text = getIPAddress();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请填写昵称");
            }


            if (radioButton1.Checked == true)
            {
                statusStrip1.Text = "正在创建服务器";
                fivechess five = new fivechess();
                five.Show();
            }
            else
            {
                statusStrip1.Text = "正在连接服务器";
                

            }
        }

  
        #region other
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = getIPAddress();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "0.0.0.0";
        }

        private static string getIPAddress()
        {
            System.Net.IPAddress addr;
          
            addr = new System.Net.IPAddress(Dns.GetHostByName(
                Dns.GetHostName()).AddressList[0].Address);
            return addr.ToString();
        }
        #endregion
    }
}
