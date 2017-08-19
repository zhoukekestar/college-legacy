using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Data.SqlClient;

namespace ServerTogetherCSharp
{
    class FreshFarm
    {
        string m_chConnectString;
        Thread m_FreshThread;
        int m_iFreshFarmTime;
        CommonFunction CommonF;
        
        public FreshFarm()
        {
            m_iFreshFarmTime = 5; //默认刷新所有养殖场为5分钟
            InitConfiguration();  //初始化

            CommonF = new CommonFunction();
            
            //刷新进程启动
            ////////////////////////////////////
            m_FreshThread = new Thread(Start);
            m_FreshThread.IsBackground = true;
            m_FreshThread.Start();
            ///////////////////////////////////
        }

        void Start()
        {
            try
            {
                while (true)
                {
                    ///查询所有节点的数据
                    ////////////////////////////////////////////////////////////////////
                    DataTable t_DataTable = new DataTable();
                    t_DataTable = CommonF.GetAllDataFrom("NodeInfo");
                    /*
                     * 0: Time date not null,
                     * 1: Num int not null,
                     * 2: FarmNum int not null,
                     * 3: ChickenNum int not null,
                     * 4: NodeNum int not null,
                     * 
                     * */
                    foreach (DataRow t_row in t_DataTable.Rows)
                    {
                        CommonF.InsertIntoSql(Int32.Parse(t_row[2].ToString()),
                                              Int32.Parse(t_row[3].ToString()),
                                              Int32.Parse(t_row[4].ToString()),
                                             "INFNODE");
                        Thread.Sleep(5);

                    }
                    ///////////////////////////////////////////////////////////////////////




                    //查询所有的大棚的参数
                    ////////////////////////////////////////////////////////////////////
                    DataTable t_DataTable2 = new DataTable();
                    t_DataTable2 = CommonF.GetAllDataFrom("FarmMeaning");
                    /*
                     * 0: Time date not null,
                     * 1: Num int not null,
                     * 2: FarmNum int,
                     * 3: FarmName varchar(50),
                     * 4: ChickenNum int,
                     * 5: ChickenName varchar(50),
                     * 6: Others varchar(50),
                     * */
                    foreach (DataRow t_row in t_DataTable2.Rows)
                    {
                        CommonF.InsertIntoSql(Int32.Parse(t_row[2].ToString()),
                                              Int32.Parse(t_row[4].ToString()),
                                              0,
                                              "INFSET");
                         Thread.Sleep(5);
                    }
                    /////////////////////////////////////////////////////////////////


                    CommonF.AddRunningInfo(DateTime.Now.ToString() + " 刷新了所有养殖场的信息！");
                    CommonF.SleepMinute(m_iFreshFarmTime);
                }
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " FreshFarm::Start Error: " + ex.Message);
            }
            
        } 

        //初始化配置文件
        private void InitConfiguration()
        {
            try
            {
                StreamReader Reader = new StreamReader(".\\Configuration.txt");
                string sLine = "";

                while (sLine != null)
                {
                    sLine = Reader.ReadLine();

                    if (sLine == "SQL Connect String:")
                    {
                        sLine = Reader.ReadLine();
                        m_chConnectString = sLine;
                    }

                    else if (sLine == "Farm Fresh Time:")
                    {
                        sLine = Reader.ReadLine();
                        m_iFreshFarmTime = Int32.Parse(sLine);
                    }

                }
                Reader.Close();

            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " FreshFarm::InitConfiguration Error: " + ex.Message);
            }

        }

    }
}
