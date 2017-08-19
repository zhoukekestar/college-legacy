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
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Data.SqlClient;


namespace FarmManagement
{
    public partial class Form1 : Form
    {
        string m_chConnectString;
        int m_winLen;
        int m_winHighBig;
        int m_winHighSmall;

        int m_GroupSizeX;
        int m_GroupSizeY;

        int m_iCurrentIndex;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_winLen = 900;
            m_winHighBig = 600;
            m_winHighSmall = 320;

            m_GroupSizeX = 330;
            m_GroupSizeY = 230;

            m_iCurrentIndex = 0;

            IDC_BUTTON_FRESH_DATA.Visible = false;

            this.Size = new Size(m_winLen, m_winHighSmall);
            
            InitConfiguration();

            FreshData();
        }


        #region 功能函数
        private int GetTime_Now()
        {
            int t_iNum = DateTime.Now.Hour * 10000000 +
                            DateTime.Now.Minute * 100000 +
                            DateTime.Now.Second * 1000 +
                            DateTime.Now.Millisecond;


            return t_iNum;

        }

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

        //更新数据表 入口参数：表名  数据表  
        //将表名为t_argTable的数据更新为t_argDataTable
        private void UpdateDatabase(string t_argTable, DataTable t_argDataTable)
        {
            try
            {
                SqlConnection t_SqlConnection = new SqlConnection(m_chConnectString);
                t_SqlConnection.Open();

                string t_SqlCommand = "select * from ";
                t_SqlCommand += t_argTable;

                SqlDataAdapter t_SqlDataAdapter = new SqlDataAdapter(t_SqlCommand, t_SqlConnection);
                SqlCommandBuilder t_SqlCommandBuilder = new SqlCommandBuilder(t_SqlDataAdapter);

                // SqlDataAdapter t_SqlDA = new SqlDataAdapter("select * from CodeRecord", m_chConnectString);
                t_SqlDataAdapter.Update(t_argDataTable);

                t_SqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("UpdateDatabase error" + ex.Message);
                //AddRunningInfo(DateTime.Now.ToString() + " DataMinitor::UpdateDatabase Error: [" + t_argTable + "] Error! " + ex.Message);
            }
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

        #endregion  

        #region 大鹏操作

        //新建大鹏按钮
        private void IDC_BUTTION_NEW_FARM_Click(object sender, EventArgs e)
        {
            IDC_LABEL_STATUS.Text = "新建大棚数据";

            if (this.Size != new Size(m_winLen, m_winHighBig))
            {
                //IDC_GROUP_OPERATE_NODE.Size = new Size(0, 0);
                IDC_GROUP_OPERATE_NODE.Visible = false;
                IDC_GROUPBOX_ADDINF.Size = new Size(0, 0);
                IDC_GROUPBOX_ADDINF.Visible = false;

                for (int i = m_winHighSmall; i < m_winHighBig; i += 3)
                    this.Size = new Size(m_winLen, i);
                this.Size = new Size(m_winLen, m_winHighBig);

                IDC_GROUPBOX_ADDINF.Visible = true;
                IDC_GROUPBOX_ADDINF.Size = new Size(m_GroupSizeX, m_GroupSizeY);

            }
        }

        //新建大鹏 的提交按钮
        private void IDC_BUTTION_NEW_FARM_OK_Click(object sender, EventArgs e)
        {
            string t_cmdStr;
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

            if (IDC_TEXT_FARM_NAME.Text != "" && IDC_TEXT_FARM_NUM.Text != ""
               && IDC_TEXT_CHICKEN_NAME.Text != "" && IDC_TEXT_CHICKEN_NUM.Text != "")
            {
                //判断原有数据库中是否就有
                //////////////////////////////////////////////////////////////

                DataTable t_DataTable = new DataTable();
                t_DataTable = GetData("FarmMeaning");

                foreach (DataRow t_row in t_DataTable.Rows)
                {
                    if (IDC_TEXT_CHICKEN_NUM.Text == t_row[4].ToString() &&
                        IDC_TEXT_FARM_NUM.Text == t_row[2].ToString())
                    {
                        IDC_LABEL_STATUS.Text = "添加失败，数据库中已有该大棚，需要修改请双击";
                        return;
                    }
                }

                //////////////////////////////////////////////////////////
                t_cmdStr = "insert into FarmMeaning values('";
                t_cmdStr += DateTime.Now.ToShortDateString();
                t_cmdStr += "','";
                t_cmdStr += GetTime_Now().ToString();
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_FARM_NUM.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_FARM_NAME.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_CHICKEN_NUM.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_CHICKEN_NAME.Text;
                t_cmdStr += "','FarmName')";

                ExeSqlCommand(t_cmdStr);

                IDC_TEXT_FARM_NUM.Text = "";
                IDC_TEXT_FARM_NAME.Text = "";
                IDC_TEXT_CHICKEN_NUM.Text = "";
                IDC_TEXT_CHICKEN_NAME.Text = "";



                FreshData();

                IDC_LABEL_STATUS.Text = "添加大棚成功，请在数据库中核实";
            }
            else
            {
                IDC_LABEL_STATUS.Text = "添加大棚失败，数据不合法";
            }
           
        }

        //新建大鹏 取消按钮
        private void IDC_BUTTION_CANCLE_NEW_FARM_Click(object sender, EventArgs e)
        {
            IDC_TEXT_FARM_NUM.Text = "";
            IDC_TEXT_FARM_NAME.Text = "";
            IDC_TEXT_CHICKEN_NUM.Text = "";
            IDC_TEXT_CHICKEN_NAME.Text = "";


            IDC_GROUPBOX_ADDINF.Size = new Size(0, 0);
            IDC_GROUPBOX_ADDINF.Visible = false;
            for (int i = m_winHighBig; i > m_winHighSmall; i -= 3)
                this.Size = new Size(m_winLen, i);
            this.Size = new Size(m_winLen, m_winHighSmall);

            this.Size = new Size(m_winLen, m_winHighSmall);
        }
        
        
       

        private void IDC_DATAVIEW_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            IDC_BUTTON_FRESH_DATA.Visible = true;
        }

        //当输入养殖场编号时 在数据库中查找相应的名字 并填入到养殖场名称中
        private void IDC_TEXT_FARM_NUM_TextChanged(object sender, EventArgs e)
        {
            IDC_LABEL_STATUS.Text = "编辑大棚信息";

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
            
            foreach (DataRow t_row in t_DataTable.Rows)
            {
                if (t_row[2].ToString() == IDC_TEXT_FARM_NUM.Text)
                {
                    IDC_TEXT_FARM_NAME.Text = t_row[3].ToString();
                    
                    return;
                }
            }
            IDC_TEXT_FARM_NAME.Text = "";
        }

        //当鼠标在数据表中选择的时候
        private void IDC_DATAVIEW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_iCurrentIndex = e.RowIndex;
            if (m_iCurrentIndex >= 0)
            {
                IDC_LABEL_STATUS.Text = "你选择了[" + IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[2].Value.ToString() + "]号养殖场 [" +
                                                      IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[4].Value.ToString() + "]号大鹏 ";
            }
            else
                m_iCurrentIndex = 0;
        }

        #endregion

        #region 节点操作

        //下拉框选择节点编号
        private void IDC_COMBOBOX_NODE_SelectedIndexChanged(object sender, EventArgs e)
        {
            NodeChange();
        }    
        
        //当下拉框中的文字改变的时候 需要将其节点刷新
        private void IDC_COMBOBOX_NODE_TextChanged(object sender, EventArgs e)
        {
            NodeChange(IDC_COMBOBOX_NODE.Text);
        }
        
        //节点编号刷新
        private void NodeChange(string str = "")
        {
            try
            {
                DataTable t_DataTable = new DataTable();
                t_DataTable = GetData("NodeInfo");

                //MessageBox.Show( IDC_DATAVIEW.Rows.Count.ToString() + " _ " + m_iCurrentIndex.ToString());
                int t_totalNum = IDC_DATAVIEW.Rows.Count;
                t_totalNum--;

                if (m_iCurrentIndex < t_totalNum) ;
                else m_iCurrentIndex = 0;
                

                string t_Farm = IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[2].Value.ToString();
                string t_Chicken = IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[4].Value.ToString();

                string t_node;
                if (str == "")
                    t_node = (IDC_COMBOBOX_NODE.SelectedIndex + 1).ToString();
                else
                    t_node = str;

                
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

                IDC_LABEL_STATUS.Text = "你选择了 [" + t_Farm + "]养殖场 ["
                                                     + t_Chicken + "]号大鹏 ["
                                                     + t_node + "]号节点";

                IDC_TEXT_DATA1.Text = "";
                IDC_TEXT_DATA2.Text = "";
                IDC_TEXT_DATA3.Text = "";
                IDC_TEXT_DATA4.Text = "";
                IDC_TEXT_DATA5.Text = "";
                IDC_TEXT_DATA6.Text = "";
                IDC_TEXT_DATA7.Text = "";
                IDC_TEXT_DATA8.Text = "";


                IDC_TEXT_S1.Text = "";
                IDC_TEXT_S2.Text = "";
                IDC_TEXT_S3.Text = "";
                IDC_TEXT_S4.Text = "";
                IDC_TEXT_S5.Text = "";
                IDC_TEXT_S6.Text = "";
                IDC_TEXT_S7.Text = "";
                IDC_TEXT_S8.Text = "";


                bool t_flag = false;
                foreach (DataRow t_row in t_DataTable.Rows)
                {
                    if (t_row[2].ToString() == t_Farm &&
                        t_row[3].ToString() == t_Chicken &&
                        t_row[4].ToString() == t_node)
                    {
                        t_flag = true;

                        IDC_TEXT_S1.Text = t_row[5].ToString();
                        IDC_TEXT_S2.Text = t_row[6].ToString();
                        IDC_TEXT_S3.Text = t_row[7].ToString();
                        IDC_TEXT_S4.Text = t_row[8].ToString();
                        IDC_TEXT_S5.Text = t_row[9].ToString();
                        IDC_TEXT_S6.Text = t_row[10].ToString();
                        IDC_TEXT_S7.Text = t_row[11].ToString();
                        IDC_TEXT_S8.Text = t_row[12].ToString();

                        IDC_TEXT_DATA1.Text = t_row[13].ToString();
                        IDC_TEXT_DATA2.Text = t_row[14].ToString();
                        IDC_TEXT_DATA3.Text = t_row[15].ToString();
                        IDC_TEXT_DATA4.Text = t_row[16].ToString();
                        IDC_TEXT_DATA5.Text = t_row[17].ToString();
                        IDC_TEXT_DATA6.Text = t_row[18].ToString();
                        IDC_TEXT_DATA7.Text = t_row[19].ToString();
                        IDC_TEXT_DATA8.Text = t_row[20].ToString();

                        break;
                    }
                }
                if (t_flag == false)
                {
                    IDC_GROUP_NODE_SWITCH.Visible = false;
                    IDC_GROUP_NODE_DATA.Visible = false;
                    IDC_LABEL_STATUS_NODE.Text = "无此节点，请新建";

                    IDC_BUTTION_NODE_SAVE.Visible = false;
                    IDC_BUTTION_NODE_NEW.Visible = true;
                    IDC_BUTTION_NODE_REMOVE.Visible = false;
                    IDC_BUTTION_NODE_SAVE_NEWNODE.Visible = false;
                }
                else
                {
                    IDC_GROUP_NODE_SWITCH.Visible = true;
                    IDC_GROUP_NODE_DATA.Visible = true;
                    IDC_LABEL_STATUS_NODE.Text = "请编辑该节点";

                    IDC_BUTTION_NODE_SAVE.Visible = true;
                    IDC_BUTTION_NODE_NEW.Visible = false;
                    IDC_BUTTION_NODE_REMOVE.Visible = true;
                    IDC_BUTTION_NODE_SAVE_NEWNODE.Visible = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //当点击了操作该大鹏按钮后
        private void IDC_BUTTON_OPERATE_NODE_Click(object sender, EventArgs e)
        {
            IDC_BUTTION_NODE_SAVE.Visible         = false;
            IDC_BUTTION_NODE_NEW.Visible          = false;
            IDC_BUTTION_NODE_SAVE_NEWNODE.Visible = false;
            IDC_GROUP_NODE_DATA.Visible           = false;
            IDC_GROUP_NODE_SWITCH.Visible         = false;
            IDC_BUTTION_NODE_REMOVE.Visible       = false;

            IDC_LABEL_STATUS.Text = "编辑大鹏节点";

            if (this.Size != new Size(m_winLen, m_winHighBig))
            {
               
                IDC_GROUP_OPERATE_NODE.Visible = false;
                IDC_GROUPBOX_ADDINF.Visible    = false;

                //窗口变化动画效果
                ///////////////////////////////////////////////////////
                for (int i = m_winHighSmall; i < m_winHighBig; i += 3)
                    this.Size = new Size(m_winLen, i);
                this.Size = new Size(m_winLen, m_winHighBig);
                ///////////////////////////////////////////////////////

                IDC_GROUP_OPERATE_NODE.Visible = true;
              

            }

            IDC_LABEL_STATUS_NODE.Text = "请选择节点编号";
        }


        //保存当前的节点信息
        private void IDC_BUTTON_NODE_SAVE_Click(object sender, EventArgs e)
        {
            DataTable t_DataTable = new DataTable();
            t_DataTable = GetData("NodeInfo");

            string t_Farm    = IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[2].Value.ToString();
            string t_Chicken = IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[4].Value.ToString();
            string t_node    = (IDC_COMBOBOX_NODE.SelectedIndex + 1).ToString();

            foreach (DataRow t_row in t_DataTable.Rows)
            {
                if (t_row[2].ToString() == t_Farm &&
                    t_row[3].ToString() == t_Chicken &&
                    t_row[4].ToString() == t_node)
                {
                    
                    t_row[5] = IDC_TEXT_S1.Text;
                    t_row[6] = IDC_TEXT_S2.Text;
                    t_row[7] = IDC_TEXT_S3.Text;
                    t_row[8] = IDC_TEXT_S4.Text;
                    t_row[9] = IDC_TEXT_S5.Text;
                    t_row[10] = IDC_TEXT_S6.Text;
                    t_row[11] = IDC_TEXT_S7.Text;
                    t_row[12] = IDC_TEXT_S8.Text;

                    t_row[13] = IDC_TEXT_DATA1.Text;
                    t_row[14] = IDC_TEXT_DATA2.Text;
                    t_row[15] = IDC_TEXT_DATA3.Text;
                    t_row[16] = IDC_TEXT_DATA4.Text;
                    t_row[17] = IDC_TEXT_DATA5.Text;
                    t_row[18] = IDC_TEXT_DATA6.Text;
                    t_row[19] = IDC_TEXT_DATA7.Text;
                    t_row[20] = IDC_TEXT_DATA8.Text;
                    

                    break;
                }
            }
            UpdateDatabase("NodeInfo", t_DataTable);

            IDC_LABEL_STATUS_NODE.Text = "保存节点成功！";

        }
        
        //新建一个节点信息
        private void IDC_BUTTION_NODE_NEW_Click(object sender, EventArgs e)
        {
            IDC_GROUP_NODE_SWITCH.Visible = true;
            IDC_GROUP_NODE_DATA.Visible = true;

            IDC_BUTTION_NODE_NEW.Visible = false;
            IDC_BUTTION_NODE_SAVE_NEWNODE.Visible = true;


            IDC_LABEL_STATUS_NODE.Text = "新建一个节点";

        }

        //建立新节点
        private void NewNode()
        {
            try
            {

                string t_Farm    = IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[2].Value.ToString();
                string t_Chicken = IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[4].Value.ToString();
                string t_node    = (IDC_COMBOBOX_NODE.SelectedIndex + 1).ToString();

                string t_cmdStr = "insert into NodeInfo values('";
                t_cmdStr += DateTime.Now.ToShortDateString();
                t_cmdStr += "','";
                t_cmdStr += GetTime_Now();

                t_cmdStr += "','";
                t_cmdStr += t_Farm;
                t_cmdStr += "','";
                t_cmdStr += t_Chicken;
                t_cmdStr += "','";
                t_cmdStr += t_node;

                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_S1.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_S2.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_S3.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_S4.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_S5.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_S6.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_S7.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_S8.Text;

                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_DATA1.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_DATA2.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_DATA3.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_DATA4.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_DATA5.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_DATA6.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_DATA7.Text;
                t_cmdStr += "','";
                t_cmdStr += IDC_TEXT_DATA8.Text;
                t_cmdStr += "','";

                t_cmdStr += "new node')";

                ExeSqlCommand(t_cmdStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("New Node Error：" + ex.Message);
            }

        }


        //删除节点按钮
        private void IDC_BUTTION_NODE_REMOVE_Click(object sender, EventArgs e)
        {
            string t_Farm    = IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[2].Value.ToString();
            string t_Chicken = IDC_DATAVIEW.Rows[m_iCurrentIndex].Cells[4].Value.ToString();
            string t_node    = (IDC_COMBOBOX_NODE.SelectedIndex + 1).ToString();

            string t_cmdStr = "DELETE NodeInfo WHERE FarmNum='";
            t_cmdStr += t_Farm;
            t_cmdStr += "' AND ChickenNum='";
            t_cmdStr += t_Chicken;
            t_cmdStr += "' AND NodeNum='";
            t_cmdStr += t_node;
            t_cmdStr += "'";

            ExeSqlCommand(t_cmdStr);


            IDC_LABEL_STATUS_NODE.Text = "已成功删除该节点";

            IDC_GROUP_NODE_SWITCH.Visible = false;
            IDC_GROUP_NODE_DATA.Visible   = false;

            IDC_BUTTION_NODE_SAVE.Visible         = false;
            IDC_BUTTION_NODE_NEW.Visible          = true;
            IDC_BUTTION_NODE_REMOVE.Visible       = false;
            IDC_BUTTION_NODE_SAVE_NEWNODE.Visible = false;

            IDC_TEXT_DATA1.Text = "";
            IDC_TEXT_DATA2.Text = "";
            IDC_TEXT_DATA3.Text = "";
            IDC_TEXT_DATA4.Text = "";
            IDC_TEXT_DATA5.Text = "";
            IDC_TEXT_DATA6.Text = "";
            IDC_TEXT_DATA7.Text = "";
            IDC_TEXT_DATA8.Text = "";


            IDC_TEXT_S1.Text = "";
            IDC_TEXT_S2.Text = "";
            IDC_TEXT_S3.Text = "";
            IDC_TEXT_S4.Text = "";
            IDC_TEXT_S5.Text = "";
            IDC_TEXT_S6.Text = "";
            IDC_TEXT_S7.Text = "";
            IDC_TEXT_S8.Text = "";
        }

        //取消节点按钮
        private void IDC_BUTTION_NODE_CANCLE_Click(object sender, EventArgs e)
        {
            IDC_GROUP_OPERATE_NODE.Visible = false;
            IDC_GROUPBOX_ADDINF.Visible    = false;

            //取消的窗口动画效果
            /////////////////////////////////////////////////////////////////
            for (int i = m_winHighBig; i > m_winHighSmall; i -= 3)
                this.Size = new Size(m_winLen, i);
            this.Size = new Size(m_winLen, m_winHighSmall);
            //////////////////////////////////////////////////////////////////

            this.Size = new Size(m_winLen, m_winHighSmall);
        }

        //保存节点按钮
        private void IDC_BUTTION_NODE_SAVE_NEWNODE_Click(object sender, EventArgs e)
        {
            NewNode();
            IDC_BUTTION_NODE_SAVE.Visible         = true;
            IDC_BUTTION_NODE_NEW.Visible          = false;
            IDC_BUTTION_NODE_REMOVE.Visible       = true;
            IDC_BUTTION_NODE_SAVE_NEWNODE.Visible = false;

            IDC_LABEL_STATUS_NODE.Text = "保存新节点成功！";
        }

        #endregion

        //按下更新数据库按钮
        private void IDC_BUTTON_FRESH_DATA_Click_1(object sender, EventArgs e)
        {
            IDC_LABEL_STATUS.Text = "更新数据完毕！";
            IDC_BUTTON_FRESH_DATA.Visible = false;

            //将表格中的数据更新至服务器
            ///////////////////////////////////////////
            DataTable dt = new DataTable();
            dt = IDC_DATAVIEW.DataSource as DataTable;
            UpdateDatabase("FarmMeaning", dt);
            /////////////////////////////////////////

            FreshData(); 
        }

        //更新数据表
        private void FreshData()
        {
            //将服务器上的数据更新到本地
            //////////////////////////////////////////////
            DataTable t_DataTable = new DataTable();
            t_DataTable = GetData("FarmMeaning");
            IDC_DATAVIEW.DataSource = t_DataTable;   
            //////////////////////////////////////////////
            
        }


    }
}
