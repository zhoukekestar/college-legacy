using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Data.SqlClient;

namespace FarmManagement_Setting
{
    public partial class Form1 : Form
    {
        string m_chConnectString;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitConfiguration();

            IDC_GROUP_CHOICE_CHICKEN_OR_NODE.Visible = false;
            IDC_GROUP_NODE_DATA.Visible = false;
            IDC_GROUP_NODE_SWITCH.Visible = false;
        }
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
                }
                Reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("init error" + ex.Message);
                //AddRunningInfo(DateTime.Now.ToString() + " FreshFarm::InitConfiguration Error: " + ex.Message);
            }

        }

        #region 选择按钮 设置大棚 或 设置节点

        private void IDC_BUTTION_SET_CHICKEN_SET_Click(object sender, EventArgs e)
        {
            IDC_LABEL_NOTE_FARM.Visible = true;
            IDC_LABEL_NOTE_FARM.Visible = true;
            IDC_LABEL_NOTE_NODE.Visible = false;//设置左边文字的显示

            IDC_TEXT_FARM_NUM.Visible = true;
            IDC_TEXT_CHICKEN_NUM.Visible = true;
            IDC_TEXT_NODE_NUM.Visible = false;//设置输入文本框的显示

            IDC_TEXT_FARM_NUM.Text = "";
            IDC_TEXT_CHICKEN_NUM.Text = "";
            IDC_TEXT_NODE_NUM.Text = "";//初始化文本框

            IDC_GROUP_NODE_SWITCH.Visible = false;
            IDC_GROUP_NODE_DATA.Visible = false;


            IDC_GROUP_CHOICE_CHICKEN_OR_NODE.Visible = true;
            ///////////////////////////////////////////////////////////////////
            //以上显示 选择信息

            

        }

        private void IDC_BUTTION_SET_SWITCH_SET_Click(object sender, EventArgs e)
        {
            IDC_LABEL_NOTE_FARM.Visible = true;
            IDC_LABEL_NOTE_FARM.Visible = true;
            IDC_LABEL_NOTE_NODE.Visible = true;

            IDC_TEXT_FARM_NUM.Visible = true;
            IDC_TEXT_CHICKEN_NUM.Visible = true;
            IDC_TEXT_NODE_NUM.Visible = true;

            IDC_TEXT_FARM_NUM.Text = "";
            IDC_TEXT_CHICKEN_NUM.Text = "";
            IDC_TEXT_NODE_NUM.Text = "";

            IDC_GROUP_NODE_SWITCH.Visible = false;
            IDC_GROUP_NODE_DATA.Visible = false;

            IDC_GROUP_CHOICE_CHICKEN_OR_NODE.Visible = true;


            
        }

        #endregion

        private void IDC_BUTTION_CHOICE_CHICKEN_OR_NODE_OK_Click(object sender, EventArgs e)
        {

            string t_FarmNum = IDC_TEXT_FARM_NUM.Text;
            string t_ChickenNum = IDC_TEXT_CHICKEN_NUM.Text;
            string t_NodeNum = IDC_TEXT_NODE_NUM.Text;

            if (IDC_TEXT_NODE_NUM.Visible == false)
            {
                
                IDC_GROUP_NODE_SWITCH.Visible = false;


                //显示大棚信息
                ////////////////////////////////////////////////////////////
                if (t_FarmNum != "" &&
                    t_ChickenNum != "")
                {
                    IDC_LABEL_DATA1.Text = "氨气";
                    IDC_LABEL_DATA2.Text = "二氧化碳";
                    IDC_LABEL_DATA3.Text = "光照";
                    IDC_LABEL_DATA4.Text = "温度";
                    IDC_LABEL_DATA5.Text = "湿度";
                    IDC_LABEL_DATA6.Text = "";
                    IDC_LABEL_DATA7.Text = "";
                    IDC_LABEL_DATA8.Text = "";

                    DataTable t_DataTable = new DataTable();

                    t_DataTable = GetData("FarmMeaning");

                    foreach (DataRow t_row in t_DataTable.Rows)
                    {
                        if (t_row[2].ToString() == t_FarmNum &&
                            t_row[4].ToString() == t_ChickenNum)
                        {
                            IDC_GROUP_NODE_DATA.Visible = true;
                            IDC_LABEL_STATUS.Text = "请设置大棚参数";
                            return;
                        }
                    }

                    IDC_LABEL_STATUS.Text = "无此大鹏，无法设置";
                    IDC_GROUP_NODE_DATA.Visible = false;

                }
            }

            #region 设置节点

            else
            {
                IDC_GROUP_NODE_DATA.Visible = false;
                //IDC_GROUP_NODE_SWITCH.Visible = true;

                if (t_FarmNum != "" &&
                    t_ChickenNum != "" &&
                    t_NodeNum != "")
                {
                    //显示节点信息
                    //////////////////////////////////////////////////////////
                    DataTable t_DataTable = new DataTable();

                    t_DataTable = GetData("NodeInfo");
                   /*
                    * 
                    * 0: Time date not null,
                    * 1: Num int not null,
                    * 
                    * 2: FarmNum int not null,
                    * 3: ChickenNum int not null,
                    * 4: NodeNum int not null,
                    * 
                    * 5: Switch1 varchar(50),
                    * 6: Switch2 varchar(50),
                    * 7: Switch3 varchar(50),
                    * 8: Switch4 varchar(50),
                    * 9: Switch5 varchar(50),
                    * 10: Switch6 varchar(50),
                    * 11: Switch7 varchar(50),
                    * 12: Switch8 varchar(50),
                    * 
                    * 13: Data1 varchar(50),
                    * 14: Data2 varchar(50),
                    * 15: Data3 varchar(50),
                    * 16: Data4 varchar(50),
                    * 17: Data5 varchar(50),
                    * 18: Data6 varchar(50),
                    * 19: Data7 varchar(50),
                    * 20: Data8 varchar(50),
                    * 
                    * 21: Others varchar(50),
                    * 
                    * */

                    IDC_LABEL_SWITCH1.Text = "";
                    IDC_LABEL_SWITCH2.Text = "";
                    IDC_LABEL_SWITCH3.Text = "";
                    IDC_LABEL_SWITCH4.Text = "";
                    IDC_LABEL_SWITCH5.Text = "";
                    IDC_LABEL_SWITCH6.Text = "";
                    IDC_LABEL_SWITCH7.Text = "";
                    IDC_LABEL_SWITCH8.Text = "";
                    IDC_GROUP_NODE_SWITCH.Visible = false;

                    foreach (DataRow t_row in t_DataTable.Rows)
                    {
                        if (t_row[2].ToString() == t_FarmNum &&
                            t_row[3].ToString() == t_ChickenNum &&
                            t_row[4].ToString() == t_NodeNum)
                        {
                            IDC_GROUP_NODE_SWITCH.Visible = true;
                            IDC_LABEL_SWITCH1.Text = t_row[5].ToString();
                            IDC_LABEL_SWITCH2.Text = t_row[6].ToString();
                            IDC_LABEL_SWITCH3.Text = t_row[7].ToString();
                            IDC_LABEL_SWITCH4.Text = t_row[8].ToString();
                            IDC_LABEL_SWITCH5.Text = t_row[9].ToString();
                            IDC_LABEL_SWITCH6.Text = t_row[10].ToString();
                            IDC_LABEL_SWITCH7.Text = t_row[11].ToString();
                            IDC_LABEL_SWITCH8.Text = t_row[12].ToString();
                            IDC_LABEL_STATUS.Text = "请设置开关";

                            IDC_COMBOBOX_S1.SelectedIndex = 0;
                            IDC_COMBOBOX_S2.SelectedIndex = 0;
                            IDC_COMBOBOX_S3.SelectedIndex = 0;
                            IDC_COMBOBOX_S4.SelectedIndex = 0;
                            IDC_COMBOBOX_S5.SelectedIndex = 0;
                            IDC_COMBOBOX_S6.SelectedIndex = 0;
                            IDC_COMBOBOX_S7.SelectedIndex = 0;
                            IDC_COMBOBOX_S8.SelectedIndex = 0;
                            return;
                        }
                    }
                    IDC_LABEL_STATUS.Text = "无此节点，无法设置";

                }
                
            }
            #endregion
        }


        #region 功能函数

        //得到所有的数据 入口参数：数据表名 返回参数：数据表
        private DataTable GetData(string t_argTable)
        {
            DataSet t_DataSet = new DataSet();
            try
            {
                SqlConnection t_ConnectString = new SqlConnection(m_chConnectString);
                string t_CommandString;

                //获取当前日期 以及 前一分钟的时间，用于命令标识
                ///////////////////////////////////////////////////////////////////////
                //string t_DateStr = DateTime.Now.ToShortDateString();
                // int t_num = DateTime.Now.Hour * 10000000 + (DateTime.Now.Minute - 1) * 100000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                /////////////////////////////////////////////////////////////////////////////

                //编辑数据命令  获取当前时间前一分钟的所有数据
                /////////////////////////////////////////
                t_CommandString = "select * from ";
                t_CommandString += t_argTable;
                /*t_CommandString += " where ";
                t_CommandString += " Time='";
                t_CommandString += t_DateStr;
                t_CommandString += "' AND Num>'";
                t_CommandString += t_num.ToString();
                t_CommandString += "'";*/
                ///////////////////////////////////////


                //用连接字符串和命令字符串，将数据存入m_DataSet
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
                MessageBox.Show("GetOneMinuteData Error" + ex.Message);
                //AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::GetOneMinuteData Error: [" + t_argTable + "] Error! " + ex.Message);
            }
            return t_DataSet.Tables[0];
        }

        private void ExeSqlCommand(string t_argCommand)
        {

            try
            {
                SqlConnection t_Connection = new SqlConnection(m_chConnectString);
                t_Connection.Open();

                SqlCommand comm = new SqlCommand(t_argCommand, t_Connection);
                comm.ExecuteNonQuery();

                t_Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("exesql command error" + ex.Message);
                //AddRunningInfo(DateTime.Now.ToString() + " FreshFarm::ExeSqlCommand Error: " + ex.Message);
            }

        }

        private int GetTime_Now()
        {
            int t_iNum = DateTime.Now.Hour * 10000000 +
                            DateTime.Now.Minute * 100000 +
                            DateTime.Now.Second * 1000 +
                            DateTime.Now.Millisecond;


            return t_iNum;

        }

        #endregion

        private void IDC_BUTTION_SUBMIT_CHICKENSET_Click(object sender, EventArgs e)
        {
            string t_FarmNum = IDC_TEXT_FARM_NUM.Text;
            string t_ChickenNum = IDC_TEXT_CHICKEN_NUM.Text;
            string t_NodeNum = IDC_TEXT_NODE_NUM.Text;

            string t_argStr = "'";
            t_argStr += IDC_TEXT_DATA1_DOWN.Text;
            t_argStr += "-";
            t_argStr += IDC_TEXT_DATA1_UP.Text;
            t_argStr += "','";

            t_argStr += IDC_TEXT_DATA2_DOWN.Text;
            t_argStr += "-";
            t_argStr += IDC_TEXT_DATA2_UP.Text;
            t_argStr += "','";

            t_argStr += IDC_TEXT_DATA3_DOWN.Text;
            t_argStr += "-";
            t_argStr += IDC_TEXT_DATA3_UP.Text;
            t_argStr += "','";

            t_argStr += IDC_TEXT_DATA4_DOWN.Text;
            t_argStr += "-";
            t_argStr += IDC_TEXT_DATA4_UP.Text;
            t_argStr += "','";
            /////////////////////////////////////////four
            t_argStr += IDC_TEXT_DATA5_DOWN.Text;
            t_argStr += "-";
            t_argStr += IDC_TEXT_DATA5_UP.Text;
            t_argStr += "','";

            t_argStr += IDC_TEXT_DATA6_DOWN.Text;
            t_argStr += "-";
            t_argStr += IDC_TEXT_DATA6_UP.Text;
            t_argStr += "','";

            t_argStr += IDC_TEXT_DATA7_DOWN.Text;
            t_argStr += "-";
            t_argStr += IDC_TEXT_DATA7_UP.Text;
            t_argStr += "','";

            t_argStr += IDC_TEXT_DATA8_DOWN.Text;
            t_argStr += "-";
            t_argStr += IDC_TEXT_DATA8_UP.Text;
            t_argStr += "'";

            InsertIntoSql(Int32.Parse(t_FarmNum), Int32.Parse(t_ChickenNum), 0, "SET", t_argStr);

            IDC_LABEL_STATUS.Text = "设置大棚参数成功！";
            IDC_GROUP_NODE_DATA.Visible = false;

        }

        private void IDC_BUTTION_SUBMIT_SWITCHSET_Click(object sender, EventArgs e)
        {
            string t_FarmNum = IDC_TEXT_FARM_NUM.Text;
            string t_ChickenNum = IDC_TEXT_CHICKEN_NUM.Text;
            string t_NodeNum = IDC_TEXT_NODE_NUM.Text;

            string t_argStr = "";

            int t_sa = 0;
            
            if (IDC_COMBOBOX_S1.SelectedIndex == 1) t_sa += 8;
            if (IDC_COMBOBOX_S2.SelectedIndex == 1) t_sa += 4;
            if (IDC_COMBOBOX_S3.SelectedIndex == 1) t_sa += 2;
            if (IDC_COMBOBOX_S4.SelectedIndex == 1) t_sa += 1;
            

            if (t_sa < 10)
                t_argStr += (char)(t_sa + '0');
            else
                t_argStr += (char)(t_sa - 10 + 'A');

            int t_sb = 0;
            if (IDC_COMBOBOX_S5.SelectedIndex == 1) t_sb += 8;
            if (IDC_COMBOBOX_S6.SelectedIndex == 1) t_sb += 4;
            if (IDC_COMBOBOX_S7.SelectedIndex == 1) t_sb += 2;
            if (IDC_COMBOBOX_S8.SelectedIndex == 1) t_sb += 1;


            if (t_sb < 10)
                t_argStr += (char)(t_sb + '0');
            else
                t_argStr += (char)(t_sb - 10 + 'A');

           
            InsertIntoSql(Int32.Parse(t_FarmNum), Int32.Parse(t_ChickenNum), Int32.Parse(t_NodeNum), "SETNODE", t_argStr);

            IDC_LABEL_STATUS.Text = "设置节点开关成功";
            IDC_GROUP_NODE_SWITCH.Visible = false;
        }

        private void InsertIntoSql(int t_FarmNum, int t_ChickenNum, int t_NodeNum, string t_argStr, string t_argSettingStr = "")
        {

            string t_cmdStr = "";
           
            if (t_argStr == "SET")
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


        //输入养殖场号 和 大鹏号
        ///////////////////////////////////////////////////////

        private void IDC_TEXT_FARM_NUM_TextChanged(object sender, EventArgs e)
        {
            DataTable t_DataTable = new DataTable();
            t_DataTable = GetData("FarmMeaning");
            /*
             *
             *0:  Time date not null,
             *1:  Num int not null,
             *2:  FarmNum int,
             *3： FarmName varchar(50),
             *4： ChickenNum int,
             *5： ChickenName varchar(50),
             *6： Others varchar(50),
             * 
             */
            IDC_LABEL_FARM_NAME.Text = "未知";
            foreach (DataRow t_row in t_DataTable.Rows)
            {
                if (t_row[2].ToString() == IDC_TEXT_FARM_NUM.Text)
                {
                    IDC_LABEL_FARM_NAME.Text = t_row[3].ToString() + " 养殖场";
                    return;
                }
            }
        }

        private void IDC_TEXT_CHICKEN_NUM_TextChanged(object sender, EventArgs e)
        {
            DataTable t_DataTable = new DataTable();
            t_DataTable = GetData("FarmMeaning");


            IDC_LABEL_CHICKEN_NAME.Text = "未知";
            foreach (DataRow t_row in t_DataTable.Rows)
            {
                if (t_row[2].ToString() == IDC_TEXT_FARM_NUM.Text &&
                    t_row[4].ToString() == IDC_TEXT_CHICKEN_NUM.Text)
                {
                    IDC_LABEL_CHICKEN_NAME.Text = t_row[5].ToString() + " 大棚";
                    break;
                }
            }

            if (IDC_LABEL_CHICKEN_NAME.Text != "未知" && IDC_TEXT_NODE_NUM.Visible == true)
            {
                DataTable dt = new DataTable();
                dt = GetData("NodeInfo");

                string t_nodeStr = "";
                int t_sum = 0;
                foreach (DataRow t_row in dt.Rows)
                {
                    if (t_row[2].ToString() == IDC_TEXT_FARM_NUM.Text &&
                        t_row[3].ToString() == IDC_TEXT_CHICKEN_NUM.Text)
                    {
                        if (t_nodeStr != "")
                            t_nodeStr += ","; 
                        t_nodeStr += t_row[4].ToString();
                        t_sum++;
                        
                    }
                }
                if (t_nodeStr == "")
                    IDC_LABEL_STATUS.Text = "该大鹏无节点可供操作";
                else
                    IDC_LABEL_STATUS.Text = "该大鹏共有" + t_sum.ToString() + "个节点：" + t_nodeStr + " 可供操作";
            }
        }

        //////////////////////////////////////////////////////////
    }
}
