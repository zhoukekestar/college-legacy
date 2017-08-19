using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Threading;

namespace ServerTogetherCSharp
{
    public partial class Main : Form
    {

        Connect m_Conncet;
        DataMinitor m_DataMinitor;
        FreshFarm m_FreshFarm;
        CommonFunction CommonF;

        Thread Fresh;
        string m_endString;
        string m_Today;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            CommonF = new CommonFunction();

            //初始化一些信息
            /////////////////////////////////////////////////////////////////////
            CommonF.AddRunningInfo(".");
            CommonF.AddRunningInfo("..");
            CommonF.AddRunningInfo("...");
            CommonF.AddRunningInfo(DateTime.Now.ToString() + " #########################################主程序已经启动！！");
            
            IDC_LABLE_GEBIN_TIME.Text = "启动时间：" + DateTime.Now.ToString();
            m_Today = DateTime.Now.ToShortDateString();

            m_endString = "";
            ////////////////////////////////////////////////////////////////////
            


            //三个模块开启
            ////////////////////////////////////
            m_Conncet = new Connect();         //网络连接模块
            m_DataMinitor = new DataMinitor();//数据库监视模块
            m_FreshFarm = new FreshFarm();    //刷新所有养殖场模块
            ///////////////////////////////////




            //主程序的显示刷新功能模块
            ////////////////////////////////////
            Fresh = new Thread(FreshLishBox);
            Fresh.IsBackground = true;
            Fresh.Start();
            ////////////////////////////////////
           
        }

        //刷新主程序的显示功能
        private void FreshLishBox()
        {
            
            while (true)
            {
                Thread.Sleep(1000);
                
                try
                {
                    string time = ".\\";
                    time += DateTime.Now.ToShortDateString();
                    time += ".txt";

                    FileStream FS = new FileStream(time, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    StreamReader SR = new StreamReader(FS);

                    //IDC_MAIN_LISTBOX.Items.Clear();

                    if (m_endString == "")
                    {
                        string sLine = "";
                        while (sLine != null)
                        {
                            sLine = SR.ReadLine();
                            if (sLine != "" && sLine != null && sLine != "语句已终止。")
                            {
                                IDC_MAIN_LISTBOX.Items.Insert(0, sLine);
                                m_endString = sLine;
                            }
                        }
                    }
                    else
                    {
                        bool flag = false;
                        string sLine = "";
                        while (sLine != null)
                        {
                            sLine = SR.ReadLine();
                            if (flag == true && sLine != "" && sLine != null && sLine != "语句已终止。")
                            {
                                IDC_MAIN_LISTBOX.Items.Insert(0, sLine);
                                m_endString = sLine;
                            }
                            else if (sLine == m_endString)
                            {
                                flag = true;
                            }

                        }
                    }
 

                    SR.Close();
                    FS.Close();
                }
                catch (Exception ex)
                {      
                    MessageBox.Show(DateTime.Now.ToString() +  " FreshLishBox Error: " + ex.Message);
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            IDC_LABLE_LOGDATE.Text = DateTime.Now.ToShortDateString() + " 的运行日志：";
            IDC_LABLE_NOW_TIME.Text = "当前时间：" + DateTime.Now.ToString();

            if (m_Today != DateTime.Now.ToShortDateString())
            {
                m_Today = DateTime.Now.ToShortDateString();
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " 新的运行日志！！");
                m_endString = "";
            }
        }

        //主程序关闭操作
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommonF.AddRunningInfo(DateTime.Now.ToString() + " #########################################主程序已经关闭！！");
        }

    }
}
