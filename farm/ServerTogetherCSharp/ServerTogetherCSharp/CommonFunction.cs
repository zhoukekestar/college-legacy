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
using System.Net;
using System.Net.Sockets;
using System.Threading;


using System.Data.SqlClient;

namespace ServerTogetherCSharp
{
    class CommonFunction
    {
        
        public string m_chConnectString;

        public CommonFunction()
        {
            InitConfiguration();
        }


        //初始化配置文件
        public void InitConfiguration()
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
                }
                Reader.Close();

            }
            catch (Exception em)
            {
                AddRunningInfo(DateTime.Now.ToString() + " CommonFunction::InitConfiguration Error: " + em.Message);
            }

        }

        /// 将运行信息加入显示列表
        public void AddRunningInfo(string message)
        {
            try
            {
                string time = ".\\";
                time += DateTime.Now.ToShortDateString();
                time += ".txt";

                FileStream FS = new FileStream(time, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter Writer = new StreamWriter(FS);

                Writer.WriteLine(message);
                Writer.Flush();


                Writer.Close();
                FS.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(DateTime.Now.ToString() + " CommonFunction::AddRunningInfo Error: " + ex.Message);
            }

        }   
   

        //得到前一分钟的时间 以整型返回 如112351231 表示11点23分51秒231毫秒
        public int GetTime_BeforeOneMinute()
        {
            int t_iNum = DateTime.Now.Hour * 10000000 +
                         (DateTime.Now.Minute - 1) * 100000 +
                         DateTime.Now.Second * 1000 +
                         DateTime.Now.Millisecond;


            return t_iNum;

        }

        //得到当前的时间 以整型返回 如112351231 表示11点23分51秒231毫秒
        public int GetTime_Now()
        {
            int t_iNum = DateTime.Now.Hour * 10000000 +
                         DateTime.Now.Minute * 100000 +
                         DateTime.Now.Second * 1000 +
                         DateTime.Now.Millisecond;


            return t_iNum;

        }

        //得到所有的数据 入口参数：数据表名 返回参数：数据表
        public DataTable GetAllDataFrom(string t_argTable)
        {
            DataSet t_DataSet = new DataSet();
            try
            {
                SqlConnection t_ConnectString = new SqlConnection(m_chConnectString);
                string t_CommandString;

                t_CommandString = "select * from ";
                t_CommandString += t_argTable;
               
                //得到数据集合
                ///////////////////////////////
                t_ConnectString.Open();
                SqlDataAdapter SqlDap = new SqlDataAdapter(t_CommandString, t_ConnectString);
                t_DataSet.Clear();
                SqlDap.Fill(t_DataSet);
                t_ConnectString.Close();
                ////////////////////////////////

            }
            catch (Exception ex)
            {
                AddRunningInfo(DateTime.Now.ToString() + " CommonFunction::GetOneMinuteData Error: [" + t_argTable + "] Error! " + ex.Message);
            }
            return t_DataSet.Tables[0];
        }

         //得到一份钟前的表格数据 默认为CodeRecord
        public DataTable GetOneMinuteDataFrom(string t_argTable = "CodeRecord")
        {
            DataSet t_DataSet = new DataSet() ;
            try
            {
                SqlConnection t_ConnectString = new SqlConnection(m_chConnectString);
                string t_CommandString;


                //编辑数据命令  获取当前时间前一分钟的所有数据
                /////////////////////////////////////////
                t_CommandString = "select * from ";
                t_CommandString += t_argTable;
                t_CommandString += " where ";
                t_CommandString += " Time='";
                t_CommandString += DateTime.Now.ToShortDateString();
                t_CommandString += "' AND Num>'";
                t_CommandString += GetTime_BeforeOneMinute().ToString();
                t_CommandString += "'";
                ///////////////////////////////////////


                //得到数据集合
                ////////////////////////////////
                t_ConnectString.Open();
                SqlDataAdapter SqlDap = new SqlDataAdapter(t_CommandString, t_ConnectString);
                t_DataSet.Clear();
                SqlDap.Fill(t_DataSet);
                t_ConnectString.Close();
                ////////////////////////////////
            }
            catch (Exception ex)
            {
                AddRunningInfo(DateTime.Now.ToString() + " CommonFunction::GetOneMinuteData Error: " + ex.Message);
            }

            return t_DataSet.Tables[0];
        }

        //更新数据表 入口参数：表名  数据表  
        //将表名为t_argTable的数据更新为t_argDataTable
        public void UpdateDatabase(string t_argTable, DataTable t_argDataTable)
        {
            try
            {
                SqlConnection t_SqlConnection = new SqlConnection(m_chConnectString);
                              
                t_SqlConnection.Open();
                /////////////////////////////////////////////////////////////////////////////////////////////////
                string t_SqlCommand = "select * from ";
                t_SqlCommand += t_argTable;

                SqlDataAdapter t_SqlDataAdapter = new SqlDataAdapter(t_SqlCommand, t_SqlConnection);
                SqlCommandBuilder t_SqlCommandBuilder = new SqlCommandBuilder(t_SqlDataAdapter);
                // SqlDataAdapter t_SqlDA = new SqlDataAdapter("select * from CodeRecord", m_chConnectString);
                t_SqlDataAdapter.Update(t_argDataTable);
                /////////////////////////////////////////////////////////////////////////////////////////////////
                t_SqlConnection.Close();
            }
            catch (Exception ex)
            {
                AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::UpdateDatabase Error: [" + t_argTable + "] Error! " + ex.Message);
            }
        }

        //插入到数据库 前三个为编号 t_argStr为标识符 t_argSettingStr为设置参数 如果是查询命令 设置参数缺省
        public void InsertIntoSql(int t_FarmNum, int t_ChickenNum, int t_NodeNum, string t_argStr, string t_argSettingStr = "")
        {
            try
            {
                string t_cmdStr = "";
                if (t_argStr == "INFSET")
                {
                    t_cmdStr = "insert into ChickenSetting values('";
                    t_cmdStr += DateTime.Now.ToShortDateString();
                    t_cmdStr += "','";
                    t_cmdStr += GetTime_Now();
                    t_cmdStr += "','";
                    t_cmdStr += t_FarmNum.ToString();
                    t_cmdStr += "','";
                    t_cmdStr += t_ChickenNum.ToString();
                    t_cmdStr += "','d1','d2','d3','d4','d5','d6','d7','d8','INFSET')";

                    ExeSqlCommand(t_cmdStr);
                }
                else if (t_argStr == "INFNODE")
                {
                    t_cmdStr = "insert into ChickenInfo values('";
                    t_cmdStr += DateTime.Now.ToShortDateString();
                    t_cmdStr += "','";
                    t_cmdStr += GetTime_Now();
                    t_cmdStr += "','";
                    t_cmdStr += t_FarmNum.ToString();
                    t_cmdStr += "','";
                    t_cmdStr += t_ChickenNum.ToString();
                    t_cmdStr += "','";
                    t_cmdStr += t_NodeNum.ToString();
                    t_cmdStr += "','s1','s2','d1','d2','d3','d4','d5','d6','d7','d8','INFNODE')";

                    ExeSqlCommand(t_cmdStr);
                }

                else if (t_argStr == "SET")
                {

                    t_cmdStr = "insert into ChickenSetting values('";
                    t_cmdStr += DateTime.Now.ToShortDateString();
                    t_cmdStr += "','";
                    t_cmdStr += GetTime_Now();
                    t_cmdStr += "','";
                    t_cmdStr += t_FarmNum.ToString();
                    t_cmdStr += "','";
                    t_cmdStr += t_ChickenNum.ToString();
                    t_cmdStr += "',";
                    t_cmdStr += t_argSettingStr;//   '0','0','0','0','0','0','0','0'  [['1-2','89-45','12-13','4-4','5-5','6-6','7-7','8-8']]
                    t_cmdStr += ",'SET')";

                    ExeSqlCommand(t_cmdStr);


                    //insert into ChickenSetting values('2013-07-30','240000000','1','2','1-2','2-3','3-3','4-4','25-25','111-111','0-0','0-0','SET')

                }
                else if (t_argStr == "SETNODE")
                {

                    t_cmdStr = "insert into ChickenInfo values('";
                    t_cmdStr += DateTime.Now.ToShortDateString();
                    t_cmdStr += "','";
                    t_cmdStr += GetTime_Now();
                    t_cmdStr += "','";
                    t_cmdStr += t_FarmNum.ToString();
                    t_cmdStr += "','";
                    t_cmdStr += t_ChickenNum.ToString();
                    t_cmdStr += "','";
                    t_cmdStr += t_NodeNum.ToString();
                    t_cmdStr += "','";
                    t_cmdStr += t_argSettingStr;  //AF  or FF something
                    t_cmdStr += "','0','d1','d2','d3','d4','d5','d6','d7','d8','SETNODE')";

                    ExeSqlCommand(t_cmdStr);


                    // insert into ChickenInfo values('2013-07-30','240000000','1','2','3','F2','0','0','0','0','0','0','0','0','0','SETNODE')

                }
            }
            catch (Exception ex)
            {
                AddRunningInfo(DateTime.Now.ToString() + " CommonFunction::InsertIntoSql Error: " + ex.Message);
            }
        }

        //将需要执行的命令放入CodeRecord中
        public void InsertIntoCodeRecord(string t_argCommand, string t_argOthers)
        {
            try
            {
                SqlConnection t_Connection = new SqlConnection(m_chConnectString);
                t_Connection.Open();

                ////编辑数据库命令
                ///////////////////////////////////////////////////////////

                string t_strCmd = "insert into CodeRecord values('";

                t_strCmd += DateTime.Now.ToShortDateString().ToString();
                t_strCmd += "', '";
                t_strCmd += GetTime_Now().ToString();
                t_strCmd += "', '";
                t_strCmd += t_argCommand;
                t_strCmd += "', 'false', 'true', '";
                t_strCmd += t_argOthers;
                t_strCmd += "')";
                ///////////////////////////////////////////////////////////


                SqlCommand comm = new SqlCommand(t_strCmd, t_Connection);
                comm.ExecuteNonQuery();

                t_Connection.Close();
            }
            catch (Exception ex)
            {
                AddRunningInfo(DateTime.Now.ToString() + " CommonFunction::InsertIntoCodeRecord Error: [" + t_argCommand + "] [" + t_argOthers + "] " + ex.Message);
            }

        }

        //执行数据库命令
        public void ExeSqlCommand(string t_argCommand)
        {
            try
            {
                SqlConnection t_Connection = new SqlConnection(m_chConnectString);
                
                //通过连接执行
                ///////////////////////
                t_Connection.Open();
                SqlCommand comm = new SqlCommand(t_argCommand, t_Connection);
                comm.ExecuteNonQuery();
                t_Connection.Close();
                //////////////////////

            }
            catch (Exception ex)
            {
                AddRunningInfo(DateTime.Now.ToString() + " CommonFunction::ExeSqlCommand Error: " + ex.Message);
            }

        }

        //睡眠时间 单位为分钟
        public void SleepMinute(int time)
        {
            while (time > 0)
            {
                time--;
                Thread.Sleep(60000);
            }
        }
       
    }

     
}
