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

    class DataMinitor
    {
        #region 变量定义
        private Thread m_DataBaseThread_UPLOAD;
        private Thread m_DataBaseThread_ChickenInfo;
        private Thread m_DataBaseThread_ChickenSetting;

        private string m_chConnectString;//连接数据的字符串  确定连接的数据库名称
        private int m_iSleepTime;        //访问数据的间隔时间

        private Random m_RandomNum;
        private int m_iMaxRandom;

        private CommonFunction CommonF;

        #endregion

       
        public DataMinitor()
        {
            CommonF = new CommonFunction();
            

            InitConfiguration();

            m_RandomNum = new Random();
            m_iMaxRandom = 20000;

            try
            {
                m_DataBaseThread_UPLOAD = new Thread(HandleMessage_UPLOAD);                //处理数据上传
                m_DataBaseThread_UPLOAD.IsBackground = true;
                m_DataBaseThread_UPLOAD.Start();

                m_DataBaseThread_ChickenSetting = new Thread(HandleMessage_ChickenSetting);//处理用户对大棚参数的操作
                m_DataBaseThread_ChickenSetting.IsBackground = true;
                m_DataBaseThread_ChickenSetting.Start();

                m_DataBaseThread_ChickenInfo = new Thread(HandleMessage_ChickenInfo);      //处理用户对节点的操作
                m_DataBaseThread_ChickenInfo.IsBackground = true;
                m_DataBaseThread_ChickenInfo.Start();


                CommonF.AddRunningInfo(DateTime.Now.ToString() + " 数据库监视已启动！！");
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::DataMinitor Error: " + ex.Message);
            }

        }


        #region 初始化函数 初始化数据库 和 数据库访问频率
        //初始化字符串   确定连接字符串 和 间隔时间
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
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " InitConfiguration Error: " + em.Message);
            }

        }

        #endregion


        #region main_thread
        //处理上传数据
        private void HandleMessage_UPLOAD()
        {
        
            Thread.Sleep(2000);
            DataTable t_DataTable = new DataTable();

            try
            {
                while (true)
                {
                    Thread.Sleep(m_iSleepTime);

                    t_DataTable = CommonF.GetOneMinuteDataFrom("CodeRecord");
                    /*
                     * Table : CodeRecord
                     * 0: Time                     
                     * 1: Num
                     * 2: Code
                     * 3: Exe
                     * 4: Translation
                     * 5: Others
                     */

                    foreach (DataRow col in t_DataTable.Rows)
                    {
                        if (col[5].ToString() == "UPLOAD" && col[4].ToString() == "False")
                        {
                            //AddRunningInfo(DateTime.Now.ToString() + " : 已将数据[" + col[2].ToString() + "] 处理！");


                            #region 发回应答信息

                            if (col[2].ToString().Substring(0, 2) == "AA")//表明上传为ACK应答
                            {
                                col[3] = "True";
                                col[4] = "True";
                                col[5] = "UPLOAD_ACK";
                                ACK(col[2].ToString());
                            }

                            #endregion

                            else if (col[2].ToString().Substring(0, 2) == "88")//表明是上传数据
                            {
                                if (col[2].ToString().Substring(18, 2) == "11")//表明是上传大棚参数
                                {
                                    UPLOAD_ChickenSetting(col[2].ToString());
                                }
                                else if (col[2].ToString().Substring(18, 2) == "88")//表明是上传节点数据
                                {
                                    UPLOAD_Node(col[2].ToString());
                                }
                               
                            }

                            col[4] = "True";
                        }

                    }
                    //foreach 

                    CommonF.UpdateDatabase("CodeRecord", t_DataTable);
                }
                //while(true)
            }
            //try
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::HandleMessage_UPLOAD Error: " + ex.Message);
            }
        }

        //查看ChickenSetting信息
        private void HandleMessage_ChickenSetting()
        {
            Thread.Sleep(2000);
            DataTable t_DataTable = new DataTable();

            while (true)
            {
                try
                {
                    Thread.Sleep(m_iSleepTime);

                    t_DataTable = CommonF.GetOneMinuteDataFrom("ChickenSetting");
                    /*
                     * Talbe： ChickenSetting
                     * 
                     * 0:  Time date not null,
                     * 1:  Num int not null,
                     * 2:  FarmNum int not null,
                     * 3:  ChickenNum int not null,
                     * 
                     * 4:  NH3    varchar(10),
                     * 5:  CO2    varchar(10),
                     * 6:  Light  varchar(10),
                     * 7:  Temp   varchar(10),
                     * 8:  Humidity varchar(10),
                     * 9:  Leave6 varchar(10),
                     * 10: Leave7 varchar(10),
                     * 11: Leave8 varchar(10),
                     * 
                     * 12: Others varchar(50) not null,
                     */
                    foreach (DataRow t_row in t_DataTable.Rows)
                    {

                        #region 请求查询大棚数据

                        if (t_row[12].ToString() == "INFSET")
                        {
                            string t_commandStr = "";
                            t_commandStr += "11";
                            t_commandStr += "000A";
                            t_commandStr += IntToOx____(m_RandomNum.Next(0, m_iMaxRandom).ToString());

                            t_commandStr += IntToOx____(t_row[2].ToString());
                            t_commandStr += IntToOx____(t_row[3].ToString()).Substring(2, 2);

                            t_commandStr += "FF";
                            t_commandStr += "11";

                            t_row[12] = "INFSET_";

                            CommonF.AddRunningInfo(DateTime.Now.ToString() + " 已将请求： 查询[" + t_row[2].ToString() + "]号养殖场 [" + t_row[3].ToString() + "]号大棚参数 提交");

                            CommonF.InsertIntoCodeRecord(t_commandStr, "INFSET");
                        }

                        #endregion


                        #region 请求设置大棚数据

                        else if (t_row[12].ToString() == "SET")
                        {

                            string t_commandStr = "";
                            t_commandStr += "22";
                            t_commandStr += "002A";
                            t_commandStr += IntToOx____(m_RandomNum.Next(0, m_iMaxRandom).ToString());

                            t_commandStr += IntToOx____(t_row[2].ToString());
                            t_commandStr += IntToOx____(t_row[3].ToString()).Substring(2, 2);

                            t_commandStr += "FF";
                            //大棚参数表编辑
                            ////////////////////////////////////////////////
                            t_commandStr += "11";
                            t_commandStr += IntToOx____(BeforeMidString(t_row[4].ToString()));
                            t_commandStr += IntToOx____(AfterMidString(t_row[4].ToString()));

                            t_commandStr += IntToOx____(BeforeMidString(t_row[5].ToString()));
                            t_commandStr += IntToOx____(AfterMidString(t_row[5].ToString()));

                            t_commandStr += IntToOx____(BeforeMidString(t_row[6].ToString()));
                            t_commandStr += IntToOx____(AfterMidString(t_row[6].ToString()));

                            t_commandStr += IntToOx____(BeforeMidString(t_row[7].ToString()));
                            t_commandStr += IntToOx____(AfterMidString(t_row[7].ToString()));
                            //////////////////////////////////////////////////////////////////Four
                            t_commandStr += IntToOx____(BeforeMidString(t_row[8].ToString()));
                            t_commandStr += IntToOx____(AfterMidString(t_row[8].ToString()));

                            t_commandStr += IntToOx____(BeforeMidString(t_row[9].ToString()));
                            t_commandStr += IntToOx____(AfterMidString(t_row[9].ToString()));

                            t_commandStr += IntToOx____(BeforeMidString(t_row[10].ToString()));
                            t_commandStr += IntToOx____(AfterMidString(t_row[10].ToString()));

                            t_commandStr += IntToOx____(BeforeMidString(t_row[11].ToString()));
                            t_commandStr += IntToOx____(AfterMidString(t_row[11].ToString()));

                            /////////////////////////////////////////////////

                            t_row[12] = "SET_";

                            CommonF.AddRunningInfo(DateTime.Now.ToString() + " 已将请求： 设置[" + t_row[2].ToString() + "]号养殖场 [" + t_row[3].ToString() + "]号大棚参数 提交");

                            CommonF.InsertIntoCodeRecord(t_commandStr, "SET");

                        }

                        #endregion

                    }
                    //foreach

                    CommonF.UpdateDatabase("ChickenSetting", t_DataTable);

                }
                catch (Exception ex)
                {
                    CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::HandleMessage_ChickenSetting Error: " + ex.Message);
                    
                }
              
            }
            
        }

        //查看ChickenInfo信息
        private void HandleMessage_ChickenInfo()
        {
        INFBEGIN:
            Thread.Sleep(2000);
            DataTable t_DataTable = new DataTable();

            try
            {
                while (true)
                {
                    Thread.Sleep(m_iSleepTime);

                    t_DataTable = CommonF.GetOneMinuteDataFrom("ChickenInfo");
                    /*
                     * Table： ChickenInfo
                     * 
                     * 0:  Time date not null,
                     * 1:  Num int not null,
                     * 
                     * 2:  FarmNum int not null,
                     * 3:  ChickenNum int not null,
                     * 4:  NodeNum int not null,
                     * 
                     * 5:  Switch char(1) not null,
                     * 6:  Compare char(1) not null,
                     * 
                     * 7:  Data1 varchar(10),
                     * 8:  Data2 varchar(10),
                     * 9:  Data3 varchar(10),
                     * 10: Data4 varchar(10),
                     * 11: Data5 varchar(10),
                     * 12: Data6 varchar(10),
                     * 13: Data7 varchar(10),
                     * 14: Data8 varchar(10),
                     * 
                     * 15: Others varchar(50) not null 
                     */

                    foreach (DataRow t_row in t_DataTable.Rows)
                    {

                        string t_commandStr = "";

                        #region 请求查询节点数据

                        if (t_row[15].ToString() == "INFNODE")
                        {
                            t_commandStr += "11";
                            t_commandStr += "000A";
                            t_commandStr += IntToOx____(m_RandomNum.Next(0, m_iMaxRandom).ToString());

                            t_commandStr += IntToOx____(t_row[2].ToString());
                            t_commandStr += IntToOx____(t_row[3].ToString()).Substring(2, 2);
                            t_commandStr += IntToOx____(t_row[4].ToString()).Substring(2, 2);

                            t_commandStr += "88";

                            t_row[15] = "INFNODE_";

                            CommonF.AddRunningInfo(DateTime.Now.ToString() + " 已将请求： 查询[" + t_row[2].ToString() + "]号养殖场 [" +
                                                                                            t_row[3].ToString() + "]号大棚 [" +
                                                                                            t_row[4].ToString() + "]号节点 数据 提交");

                            CommonF.InsertIntoCodeRecord(t_commandStr, "INFNODE");

                        }

                        #endregion


                        #region 请求设置节点开关

                        else if (t_row[15].ToString() == "SETNODE")
                        {
                            t_commandStr += "22";
                            t_commandStr += "000B";
                            t_commandStr += IntToOx____(m_RandomNum.Next(0, m_iMaxRandom).ToString());

                            t_commandStr += IntToOx____(t_row[2].ToString());
                            t_commandStr += IntToOx____(t_row[3].ToString()).Substring(2, 2);
                            t_commandStr += IntToOx____(t_row[4].ToString()).Substring(2, 2);

                            t_commandStr += "FF";

                            t_commandStr += CharToString(t_row[5].ToString()[0]);
                            // t_commandStr += CharToString(t_row[6].ToString()[0]);

                            t_row[15] = "SETNODE_";

                            CommonF.AddRunningInfo(DateTime.Now.ToString() + " 已将请求： 设置[" + t_row[2].ToString() + "]号养殖场 [" +
                                                                                            t_row[3].ToString() + "]号大棚 [" +
                                                                                            t_row[4].ToString() + "]号节点 开关 提交");

                            CommonF.InsertIntoCodeRecord(t_commandStr, "SETNODE");
                        }

                        #endregion

                    }
                    //foreach

                    CommonF.UpdateDatabase("ChickenInfo", t_DataTable);
                }
                //whiel (true)
            }
            //try
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::HandleMessage_ChickenInfo Error: " + ex.Message);
                goto INFBEGIN;
            }
        }

        #endregion

        #region 上传数据处理函数

        //上传大鹏参数
        private void UPLOAD_ChickenSetting(string t_argStr)
        {
            try
            {
                DataTable t_DataTable = new DataTable();
                t_DataTable = CommonF.GetOneMinuteDataFrom("ChickenSetting");
                /*
                 * 0: Time date not null,
                 * 1: Num int not null,
                 * 
                 * 2: FarmNum int not null,
                 * 3: ChickenNum int not null,
                 * 
                 * 4: NH3    varchar(10),
                 * 5: CO2    varchar(10),
                 * 6: Light  varchar(10),
                 * 7: Temp   varchar(10),
                 * 8: Humidity varchar(10),
                 * 9: Leave6 varchar(10),
                 * 10:Leave7 varchar(10),
                 * 11:Leave8 varchar(10),
                 * 
                 * 12:Others varchar(50) not null,
                 */

                foreach (DataRow t_row in t_DataTable.Rows)
                {
                    if (t_row[12].ToString() == "INFSET_"
                        && t_row[2].ToString() == Ox____ToInt(t_argStr.Substring(10, 4))
                        && t_row[3].ToString() == Ox____ToInt(t_argStr.Substring(14, 2)))
                    {
                        if (t_argStr.Length < 84)
                        {
                            CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::UPLOAD_ChickenSetting Error: t_argStr is less than 84!");
                            return;
                        }

                        t_row[4] = Ox____ToInt(t_argStr.Substring(20, 4));
                        t_row[4] += "-";
                        t_row[4] += Ox____ToInt(t_argStr.Substring(24, 4));

                        t_row[5] = Ox____ToInt(t_argStr.Substring(28, 4));
                        t_row[5] += "-";
                        t_row[5] += Ox____ToInt(t_argStr.Substring(32, 4));

                        t_row[6] = Ox____ToInt(t_argStr.Substring(36, 4));
                        t_row[6] += "-";
                        t_row[6] += Ox____ToInt(t_argStr.Substring(40, 4));

                        t_row[7] = Ox____ToInt(t_argStr.Substring(44, 4));
                        t_row[7] += "-";
                        t_row[7] += Ox____ToInt(t_argStr.Substring(48, 4));
                        //////////////////////////////////////////////////////////Four

                        t_row[8] = Ox____ToInt(t_argStr.Substring(52, 4));
                        t_row[8] += "-";
                        t_row[8] += Ox____ToInt(t_argStr.Substring(56, 4));

                        t_row[9] = Ox____ToInt(t_argStr.Substring(60, 4));
                        t_row[9] += "-";
                        t_row[9] += Ox____ToInt(t_argStr.Substring(64, 4));

                        t_row[10] = Ox____ToInt(t_argStr.Substring(68, 4));
                        t_row[10] += "-";
                        t_row[10] += Ox____ToInt(t_argStr.Substring(72, 4));

                        t_row[11] = Ox____ToInt(t_argStr.Substring(76, 4));
                        t_row[11] += "-";
                        t_row[11] += Ox____ToInt(t_argStr.Substring(80, 4));
                        ///////////////////////////////////////////////////////Eight

                        t_row[12] = "INFSET_UPLOAD";

                        CommonF.AddRunningInfo(DateTime.Now.ToString() + " 上传成功：[" + t_row[2] + "]号养殖场 [" + t_row[3] + "]号大棚参数数据上传成功");
                    }
                }

                CommonF.UpdateDatabase("ChickenSetting", t_DataTable);
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::UPLOAD_ChickenSetting Error : " + ex.Message);
            }
        }


        //上传节点数据
        private void UPLOAD_Node(string t_argStr)
        {
            try
            {
                DataTable t_DataTable = new DataTable();
                t_DataTable = CommonF.GetOneMinuteDataFrom("ChickenInfo");

                /*
                 * 0:  Time date not null,
                 * 1:  Num int not null,
                 * 
                 * 2:  FarmNum int not null,
                 * 3:  ChickenNum int not null,
                 * 4:  NodeNum int not null,
                 * 
                 * 5:  Switch char(1) not null,
                 * 6:  Compare char(1) not null,
                 * 
                 * 7:  Data1 varchar(10),
                 * 8:  Data2 varchar(10),
                 * 9:  Data3 varchar(10),
                 * 10: Data4 varchar(10),
                 * 11: Data5 varchar(10),
                 * 12: Data6 varchar(10),
                 * 13: Data7 varchar(10),
                 * 14: Data8 varchar(10),
                 * 
                 * 15: Others varchar(50) not null 
                 */

                foreach (DataRow t_row in t_DataTable.Rows)
                {
                    if (t_row[15].ToString() == "INFNODE_"
                        && t_row[2].ToString() == Ox____ToInt(t_argStr.Substring(10, 4))
                        && t_row[3].ToString() == Ox____ToInt(t_argStr.Substring(14, 2))
                        && t_row[4].ToString() == Ox____ToInt(t_argStr.Substring(16, 2))
                        )
                    {
                        if (t_argStr.Length < 58)
                        {
                            CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::UPLOAD_NodeInfo Error: t_argStr is not 58!");
                            return;
                        }
                        t_row[7] = Ox____ToInt(t_argStr.Substring(20, 4));
                        t_row[8] = Ox____ToInt(t_argStr.Substring(24, 4));
                        t_row[9] = Ox____ToInt(t_argStr.Substring(28, 4));
                        t_row[10] = Ox____ToInt(t_argStr.Substring(32, 4));
                        t_row[11] = Ox____ToInt(t_argStr.Substring(36, 4));
                        t_row[12] = Ox____ToInt(t_argStr.Substring(40, 4));
                        t_row[13] = Ox____ToInt(t_argStr.Substring(44, 4));
                        t_row[14] = Ox____ToInt(t_argStr.Substring(48, 4));

                        t_row[5] = t_argStr.Substring(54, 2);
                        t_row[6] = t_argStr.Substring(56, 2);

                        t_row[15] = "INFNODE_UPLOAD";

                        CommonF.AddRunningInfo(DateTime.Now.ToString() + " 上传成功：[" + t_row[2] + "]号养殖场 [" + t_row[3] + "]号大棚 [" + t_row[4] + "]号节点数据上传成功");
                    }

                }

                CommonF.UpdateDatabase("ChickenInfo", t_DataTable);
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::UPLOAD_Node Error : " + ex.Message);
            }
        }

        #endregion


        #region 相关功能函数

        //将字符串 每个字符当成整数字节的4位  并返回整型 002A ==〉 42
        private string Ox____ToInt(string t_argStr)
        {
            int wealth = 1, sum = 0;
            for (int i = t_argStr.Length - 1; i >= 0; i--)
            {
                if (t_argStr[i] >= '0' && t_argStr[i] <= '9')
                {
                    sum += ((t_argStr[i] - '0') * wealth);
                }
                else if (t_argStr[i] >= 'A' && t_argStr[i] <= 'F')
                {
                    sum += ((t_argStr[i] - 'A' + 10) * wealth);
                }
                else
                {
                    CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::Ox____ToInt Error: arguement" + t_argStr + " is not formated! ");
                    return "999";
                }

                wealth *= 16;
            }
            return sum.ToString();
        }

        //从Int 到一个4字节的十六进制数                  42 == 〉002A
        private string IntToOx____(string t_argStr)
        {
            int sum = Int32.Parse(t_argStr);

            string t_result = "";
            int[] temp = new int[4];

            temp[0] = 1;
            temp[1] = 16;
            temp[2] = 16 * 16;
            temp[3] = 16 * 16 * 16;

            int[] num = new int[4];

            num[0] = sum / temp[3];

            if (num[0] > 16)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::IntToOx____ Warning! arguement: " + t_argStr + " is  too large! ");
            }
            num[0] %= 16;             //为了确保可以转换成四个字符
            sum -= temp[3] * num[0];

            num[1] = sum / temp[2];
            sum -= num[1] * temp[2];

            num[2] = sum / temp[1];
            sum -= num[2] * temp[1];

            num[3] = sum;

            char t_ch = '0';
            for (int i = 0; i < 4; i++)
            {

                if (num[i] >= 0 && num[i] <= 9)
                    t_ch = (char)(num[i] + '0');
                else
                    t_ch = (char)('A' + num[i] - 10);
                t_result += t_ch;
            }

            return t_result;
        }

        //将char转换成二个字符的十六进制表示             A ==> 41  0 ==>31
        private string CharToString(char t_argCh)
        {
            string t_result = "";

            char t_ch1 = (char)(t_argCh & (0xF0));
            char t_ch2 = (char)(t_argCh & (0x0F));

            t_ch1 = (char)(t_ch1 >> 4);
            if (t_ch1 <= 9)
                t_ch1 = (char)(t_ch1 + '0');
            else
                t_ch1 = (char)(t_ch1 - 10 + 'A');


            if (t_ch2 <= 9)
                t_ch2 = (char)(t_ch2 + '0');
            else
                t_ch2 = (char)(t_ch2 - 10 + 'A');

            t_result += t_ch1;
            t_result += t_ch2;
            return t_result;
        }
        

        private string BeforeMidString(string t_argStr)
        {
            string temp = "";
            for (int i = 0; i < t_argStr.Length; i++)
            {
                if (t_argStr[i] == '-')
                    break;
                else
                    temp += t_argStr[i];
            }
            if (temp == "")
                return "0";
            else
                return temp;
        }

        private string AfterMidString(string t_argStr)
        {
            string temp = "";
            int flag = 0;
            for (int i = 0; i < t_argStr.Length; i++)
            {
                if (flag == 1)
                {
                    temp += t_argStr[i];
                }
                else if (t_argStr[i] == '-')
                {
                    flag = 1;
                }
            }
            if (temp == "")
                return "0";
            else
                return temp;
        }

        #endregion


        private void ACK(string t_argStr)
        {
            try
            {
                DataTable t_DataTable = CommonF.GetOneMinuteDataFrom("CodeRecord");

                /*
                 * Table : CodeRecord
                 * 0: Time                     
                 * 1: Num
                 * 2: Code
                 * 3: Exe
                 * 4: Translation
                 * 5: Others
                 */
                foreach (DataRow t_row in t_DataTable.Rows)
                {
                    if (t_row[2].ToString().Substring(6, 4) == t_argStr.Substring(6, 4))
                    {
                        t_row[3] = "True"; //收到命令 说明该条命令已经执行


                        //将设置信息返回到相应的数据表中
                        ///////////////////////////////////////////////////

                        #region SET to ChickenSetting
                        if (t_row[5].ToString() == "SET")
                        {
                            DataTable temp = new DataTable();
                            temp = CommonF.GetOneMinuteDataFrom("ChickenSetting");

                            /*
                             * 0: Time date not null,
                             * 1: Num int not null,
                             * 
                             * 2: FarmNum int not null,
                             * 3: ChickenNum int not null,
                             * 
                             * 4 ~ 11  8个数据
                             * 
                             * 12:Others varchar(50) not null,
                             */


                            foreach (DataRow tt_row in temp.Rows)
                            {
                                if (   t_row[2].ToString().Substring(10, 4) == IntToOx____(tt_row[2].ToString())
                                    && t_row[2].ToString().Substring(14, 2) == IntToOx____(tt_row[3].ToString()).Substring(2, 2)
                                    && tt_row[12].ToString() == "SET_")
                                {
                                    tt_row[12] = "SET_OK";

                                    CommonF.AddRunningInfo(DateTime.Now.ToString() + " 已设置成功： 设置[" + 
                                                                      tt_row[2].ToString() + "]号养殖场 [" +
                                                                      tt_row[3].ToString() + "]号大棚 参数");                                                             
                                }
                            }

                            CommonF.UpdateDatabase("ChickenSetting", temp);
                        }

                        #endregion

                        #region SETNODE to ChickenInfo

                        else if (t_row[5].ToString() == "SETNODE")
                        {
                            DataTable temp = new DataTable();
                            temp = CommonF.GetOneMinuteDataFrom("ChickenInfo");


                            /*
                             * Table： ChickenInfo
                             * 
                             * 0:  Time date not null,
                             * 1:  Num int not null,
                             * 
                             * 2:  FarmNum int not null,
                             * 3:  ChickenNum int not null,
                             * 4:  NodeNum int not null,
                             * 
                             * 5:  Switch char(1) not null,
                             * 6:  Compare char(1) not null,
                             * 
                             * 7 ~ 14 8个数据
                             * 
                             * 15: Others varchar(50) not null 
                             */


                            foreach (DataRow tt_row in temp.Rows)
                            {
                                if (   t_row[2].ToString().Substring(10, 4) == IntToOx____(tt_row[2].ToString())
                                    && t_row[2].ToString().Substring(14, 2) == IntToOx____(tt_row[3].ToString()).Substring(2, 2)
                                    && t_row[2].ToString().Substring(16, 2) == IntToOx____(tt_row[4].ToString()).Substring(2, 2)
                                    && tt_row[15].ToString() == "SETNODE_")
                                {
                                    tt_row[15] = "SETNODE_OK";

                                    CommonF.AddRunningInfo(DateTime.Now.ToString() + " 已设置成功： 设置[" + 
                                                                                           tt_row[2].ToString() + "]号养殖场 [" +
                                                                                           tt_row[3].ToString() + "]号大棚 [" +
                                                                                           tt_row[4].ToString() + "]号节点 开关");
                                }
                            }

                            CommonF.UpdateDatabase("ChickenInfo", temp);

                        }
                        /////////////////////////////////////////////////
                        #endregion


                    }
                }
                //foreach

                CommonF.UpdateDatabase("CodeRecord", t_DataTable);
            }
            catch (Exception ex)
            {
                CommonF.AddRunningInfo(DateTime.Now.ToString() + " ACK Error: arguement[" + t_argStr + "] " + ex.Message);
            }
        }

    }
}
