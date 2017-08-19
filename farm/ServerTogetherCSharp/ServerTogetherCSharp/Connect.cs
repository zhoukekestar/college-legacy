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
    class Connect
    {
        static int MAXNUM = 1000;

        private Thread m_ServerThread;
        private Thread m_DataBaseThread;
        private Socket m_ServerSocket;

        private string m_chServerIP;
        private string m_chConnectString;
        private string[] m_IntToString = new string[MAXNUM];

        private int m_iServerPort;
        private int m_iSleepTime;


        private CommonFunction CommonF;
        private List<Socket> m_SocketList = new List<Socket>();
        private DataSet m_DataSet = new DataSet();


        public Connect()
        {
            try
            {
                CommonF = new CommonFunction();
                InitConfiguration();
                Start();
            }
            catch (Exception em)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::Connect Error: " + em.Message);
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
                    if (sLine == "Server Port:")
                    {
                        sLine = Reader.ReadLine();
                        m_iServerPort = Int32.Parse(sLine);
                    }

                    else if (sLine == "Local Address IP:")
                    {
                        sLine = Reader.ReadLine();
                        m_chServerIP = sLine;
                    }

                    else if (sLine == "SQL Connect String:")
                    {
                        sLine = Reader.ReadLine();
                        m_chConnectString = sLine;
                    }

                    else if (sLine == "SQL Fresh Time:")
                    {
                        sLine = Reader.ReadLine();
                        m_iSleepTime = Int32.Parse(sLine);
                    }
                }
                Reader.Close();

            }
            catch (Exception em)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::InitConfiguration Error: " + em.Message);
            }

        }


        /// 开始服务

        private void Start()
        {
            try
            {
                //建立相应的套接字
                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(m_chServerIP), m_iServerPort);
                m_ServerSocket.Bind(localEndPoint);
                m_ServerSocket.Listen(100);
                ///////////////////////////////////////////////////////////////////////////////////////////////////////

                //开始监听网络
                m_ServerThread = new Thread(new ThreadStart(ReceiveAccept));
                m_ServerThread.IsBackground = true;
                m_ServerThread.Start();

                //开始监视数据库的变化
                m_DataBaseThread = new Thread(new ThreadStart(SQLMonitorStart));
                m_DataBaseThread.IsBackground = true;
                m_DataBaseThread.Start();

                CommonF.AddRunningInfo(DateTime.Now.ToString() + " 网站监控已启动！！");

            }
            catch (SocketException se)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::Start Socket Error: " + se.Message);             
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::Start Error: " + ex.Message);
            }
        }

        private void ReceiveAccept()
        {
            try
            {
                while (true)
                {
                    Socket client;

                    client = m_ServerSocket.Accept();

                    //加入socket链表中
                    ///////////////////////////////////////////////
                    m_SocketList.Add(client);
                    ////////////////////////////////////////////////

                    CommonF.AddRunningInfo(DateTime.Now.ToString() + " 客户端  [" + client.RemoteEndPoint.ToString() + "] 已连接");

                    Thread thrd = new Thread(ReceiveMessages);
                    thrd.IsBackground = true;
                    thrd.Start((object)client);

                }
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::ReceiveAccept Error: " + ex.Message);
            }
        }

        //向指定socket发送字符串信息
        private void SendMessage(Socket client, string t_str)
        {
            try
            {
                if (!string.IsNullOrEmpty(t_str))
                {

                    int sendBufferSize = Encoding.ASCII.GetByteCount(t_str);
                    byte[] sendBuffer  = new byte[sendBufferSize];
                    sendBuffer = Encoding.ASCII.GetBytes(t_str);

                    client.Send(sendBuffer);
                }
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::SendMessage Error: " + ex.Message);
            }
        }


        //收到信息  该类的主要处理函数
        //private void ReceiveMessages(Socket client)
        private void ReceiveMessages(object temp)
        {
            Socket client = (Socket)temp;
            while (true)
            {
                try
                {
                    byte[] receiveBuffer = new byte[1024];


                    //处理socket的断开
                    ///////////////////////////////////////////////////////////////
                    if (client.Poll(-1, SelectMode.SelectRead))
                    {
                        int nRead = client.Receive(receiveBuffer);
                        if (nRead == 0)
                        {
                            CommonF.AddRunningInfo(DateTime.Now.ToString() + " 客户端  [" + client.RemoteEndPoint.ToString() + "] 已断开连接");
                           
                            for (int i = 0; i < MAXNUM; i++)
                            {
                                if (m_IntToString[i] == ((IPEndPoint)(client.RemoteEndPoint)).ToString())
                                    m_IntToString[i] = "";
                            } 
                            m_SocketList.Remove(client);
                            break;
                        }
                    }
                    ////////////////////////////////////////////////////////////////

             
                    //处理接收到的数据 将收到的数据转换成有长度的string类型
                    //////////////////////////////////////////////////////////////////////////
                    string t_strTemp = Encoding.ASCII.GetString(receiveBuffer);
                    string ResultString = "";
                    for (int i = 0; i < 1024; i++)
                    {
                        if (t_strTemp[i] != (char)(0x00))
                        {
                            ResultString += t_strTemp[i];
                        }
                        else
                            break;
                    }
                    //数据中的第二三个字节表示字符串长度
                    //////////////////////////////////////////////////////////////////////////


                    if (!string.IsNullOrEmpty(ResultString))
                    {
                        CommonF.AddRunningInfo(DateTime.Now.ToString() + " 收到来自[" + client.RemoteEndPoint.ToString() + "] 的数据：{" + ResultString + "}完成");

                        //如果不是应答信息，则需将应答信息发给控制节点
                        if (ResultString.Substring(0,2) != "AA")
                        {
                            //应答信息
                            //////////////////////////
                            string ACK = "";
                            ACK += "AA0005";
                            
                            ACK += ResultString.Substring(6,4);
                            SendMessage(client, ACK);
                            //////////////////////////////////////////
                        }

                        //建立ip和养殖场的映射
                        //////////////////////
                        if (ResultString.Substring(0,2) == "FF")
                        {
                            MapStirngAndInt(ResultString.Substring(10,4), client);
                        }
                        /////////////////////
                      

                        //任何数据都插入到数据库中
                        InsertIntoSql(ResultString);

                    }

                }
                catch (Exception ex)
                {
                    CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::ReceiveMessages Error: " + ex.Message);
                }
            }

        }


        //将上传的数据放入到数据库中
        private void InsertIntoSql(string t_argStr)
        {            
            try
            {
                ////编辑数据库命令
                ///////////////////////////////////////////////////////////
                string t_strCmd = "insert into CodeRecord values('";

                t_strCmd += DateTime.Now.ToShortDateString();
                t_strCmd += "', '";
                t_strCmd += CommonF.GetTime_Now().ToString();
                t_strCmd += "', '";
                t_strCmd += t_argStr;
                t_strCmd += "', 'True', 'False', 'UPLOAD')";
                ///////////////////////////////////////////////////////////

                CommonF.ExeSqlCommand(t_strCmd);
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::InsertIntoSql Error: " + ex.Message);
            }

        }

        //映射ip地址 和 socket
        private void MapStirngAndInt(string t_strNum, Socket client)
        {
            try
            {
                int num = Int32.Parse(StringToInt(t_strNum));
                if (num < MAXNUM && num > 0)
                {
                    for (int i = 0; i < MAXNUM; i++)
                    {
                        if (m_IntToString[i] == ((IPEndPoint)client.RemoteEndPoint).ToString())//将之前的该客户端信息清空
                            m_IntToString[i] = "";
                    }
                    m_IntToString[num] = ((IPEndPoint)client.RemoteEndPoint).ToString();//重新映射该养殖场的客户端地址

                    CommonF.AddRunningInfo(DateTime.Now.ToString() + " 系统已将 " + ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + " 的养殖场编号设置为 " + num.ToString());
                }
                else
                {
                    CommonF.AddRunningInfo(DateTime.Now.ToString() + " 系统无法设置 " + ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + " 的养殖场编号（注意养殖场编号小于" + MAXNUM.ToString() + "), 而你将养殖场设置为" + t_strNum + " = "+ num.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connext::MapStringAndInt Error: " + ex.Message);
            }
        }


       

        //窗口关闭后的动作 将服务器的Socket关闭
        ~Connect()
        {
            try
            {
                m_ServerSocket.Close();

                foreach (Socket it in m_SocketList)
                {
                    it.Close();
                }
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::~Connect Error: " + ex.Message);
            }
        }

        //解析命令的目标地址，并发送信息
        private void SendMessageToNode(string t_argStr)
        {
            try
            {
                int temp = Int32.Parse(StringToInt(t_argStr.Substring(10, 4)));

                if (temp > 0 && temp < MAXNUM) //要保证数据在合理范围
                {
                    foreach (Socket it in m_SocketList)
                    {
                        if (((IPEndPoint)it.RemoteEndPoint).ToString() == m_IntToString[temp])
                        {
                            SendMessage(it, t_argStr);
                            return;
                        }
                        //将命令源码发至相应的客户端
                    }
                }
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " 字符串  [" + t_argStr + "] 发送失败, 养殖场编号为[" + temp.ToString() + "] 不存在");
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::SendMessageToNode Error: " + ex.Message);
            }
        }

       
        //监视数据库的数据变化
        private void SQLMonitorStart()
        {
            try
            {
                DataTable t_DataTable = new DataTable();
                Thread.Sleep(1000);

                while (true)
                {
                    Thread.Sleep(m_iSleepTime);
                    t_DataTable = CommonF.GetOneMinuteDataFrom();

                    foreach (DataRow col in t_DataTable.Rows)
                    {
                        /*
                         * 0 time
                         * 1 num
                         * 
                         * 2 code
                         * 3 exe
                         * 4 trans
                         * 5 others
                         * 
                         * */
                        if (   col[3].ToString() == "False"  //Exe
                            && col[5].ToString() != "UPLOAD" //Others
                            )
                        {
                            SendMessageToNode(col[2].ToString());//code
                        }
                    }
                    //foreach
                }
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::SQLMonitorStart Error: " + ex.Message);
            }

        }
        
        //将字符串 每个字符当成整数字节的4位  并返回整型
        private string StringToInt(string t_argStr)
        {
            int wealth = 1, sum = 0;

            try
            {

                for (int i = t_argStr.Length - 1; i >= 0; i--)
                {
                    if (t_argStr[i] >= '0' && t_argStr[i] <= '9')
                        sum += ((t_argStr[i] - '0') * wealth);
                    else
                        sum += ((t_argStr[i] - 'A' + 10) * wealth);

                    wealth *= 16;
                }
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " Connect::StringToInt Error: " + ex.Message);
            }

            return sum.ToString();
        }
    }
}
