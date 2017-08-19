using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkOver
{
    public partial class Form1 : Form
    {
        int time1 = 0;
        int time2 = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i <= 24; i++)
            {
                IDC_COMBOX_HOUR1.Items.Add(i.ToString());
                IDC_COMBOX_HOUR2.Items.Add(i.ToString());
            }

            for (int i = 0; i <= 60; i++)
            {
                IDC_COMBOX_MINU1.Items.Add(i.ToString());
                IDC_COMBOX_MINU2.Items.Add(i.ToString());
            }

            IDC_COMBOX_HOUR1.SelectedIndex = 11;
            IDC_COMBOX_HOUR2.SelectedIndex = 17;
            IDC_COMBOX_MINU1.SelectedIndex = 30;
            IDC_COMBOX_MINU2.SelectedIndex = 0;

            notifyicon.Text = "提醒";
            notifyicon.Visible = false;
        }

        private void IDC_TIMER_Tick(object sender, EventArgs e)
        {
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            IDC_LABEL_TIME.Text = "当前时间："+ hour.ToString() + "时" + minute.ToString() + "分" + DateTime.Now.Second.ToString() + "秒";
            if (time1 == 0 && IDC_COMBOX_HOUR1.SelectedIndex == hour && IDC_COMBOX_MINU1.SelectedIndex == minute)
            {
                time1 = 1;
                MessageBox.Show("下班了");
                
            }
            if (time2 == 0 && IDC_COMBOX_HOUR2.SelectedIndex == hour && IDC_COMBOX_MINU2.SelectedIndex == minute)
            {
                time2 = 1;
                MessageBox.Show("下班了！！");
                
            }
        }

 

        #region 隐藏任务栏图标、显示托盘图标
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyicon.Visible = true;
                this.Visible = false;
            }
        }
        #endregion

        #region 还原窗体
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Visible = true;
                WindowState = FormWindowState.Normal;
                this.Activate();
                this.ShowInTaskbar = true;
                notifyicon.Visible = false;
            }
        }
        #endregion
    }
}
