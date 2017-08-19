using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


public partial class SetNode : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Button_SubmitSwitchSetting.Visible = false;
            Label_m_ConnectString.Visible = false;
            Label_m_FarmNum.Visible = false;
            Label_m_ChickenNum.Visible = false;
            Label_m_NodeNum.Visible = false;

            MyInit();
            Label_ShowNodeNum.ForeColor = Color.FromArgb(255, 255, 0);

            if (Label_m_FarmNum.Text == "0" && 
                Label_m_ChickenNum.Text == "0" && 
                Label_m_NodeNum.Text == "0")
                    Label_ShowNodeNum.Text = "您目前还没有选择任何节点";
            else
                Label_ShowNodeNum.Text = "您正在设置 [" + Label_m_FarmNum.Text + "]号养殖场 [" 
                                                     + Label_m_ChickenNum.Text + "]号大棚 [" 
                                                        + Label_m_NodeNum.Text + "]号节点";
            
            ShowList();
        }
    }

    private void MyInit()
    {
        string url_str = Request.Url.ToString();
        Label_m_FarmNum.Text = "0";
        Label_m_ChickenNum.Text = "0";
        Label_m_NodeNum.Text = "0";

        int t_iFalg = 0;
        for (int i = 0; i < url_str.Length; i++)
        {
            if (url_str[i] == '?')
            {
                t_iFalg++;
            }
            else if (t_iFalg == 1)
            {
                Label_m_FarmNum.Text += url_str[i];
            }
            else if (t_iFalg == 2)
            {
                Label_m_ChickenNum.Text += url_str[i];
            }
            else if (t_iFalg == 3)
            {
                Label_m_NodeNum.Text += url_str[i];
            }

        }

        Label_m_FarmNum.Text    = (Int32.Parse(Label_m_FarmNum.Text)).ToString();
        Label_m_ChickenNum.Text = (Int32.Parse(Label_m_ChickenNum.Text)).ToString();
        Label_m_NodeNum.Text    = (Int32.Parse(Label_m_NodeNum.Text)).ToString();

        Label_m_ConnectString.Text = "Data Source=ZKK-COMPUTER;Initial Catalog=FarmManagement;Persist Security Info=True;User ID=sa;Password=sa123";
      
    }

    private void ShowList()
    {
        if (Label_m_FarmNum.Text == "0" && Label_m_ChickenNum.Text == "0" && Label_m_NodeNum.Text == "0")
        {
             Label_ShowNodeNum.Text = "您目前还没有选择任何节点";
            return;
        }

        #region 获取相应节点的当前开光数据

        DataTable d_DataTable = new DataTable();
        d_DataTable = GetDataBy("select * from ChickenInfo where Others='INFNODE_UPLOAD' order by [Time] desc, [Num] desc");

        /*
         * Time date not null,      0
         * Num int not null,        1
         * 
         * FarmNum int not null,    2
         * ChickenNum int not null, 3
         * NodeNum int not null,    4
         * 
         * Switch char(3),          5
         * Compare char(3),         6
         * 
         */

        string Switch_Result = "";
        foreach (DataRow d_row in d_DataTable.Rows)
        {
            if (d_row[2].ToString() == Label_m_FarmNum.Text &&
                d_row[3].ToString() == Label_m_ChickenNum.Text &&
                d_row[4].ToString() == Label_m_NodeNum.Text)
            {
                Switch_Result += d_row[5].ToString();
                break;
            }
        }
        if (Switch_Result == "")
        {
            Switch_Result = "00";
        }
        #endregion

        #region 获取相应节点对应的设备
        /////////////////////////////////////////////////
        string[] Switch_Items = new string[8];
        d_DataTable = GetDataBy("select * from NodeInfo order by [Time], [Num]");
        /*
         * Time date not null,      0
            Num int not null,       1
         * 
            FarmNum int not null,   2
            ChickenNum int not null,3
            NodeNum int not null,   4
         * 
            Switch1 varchar(50),    5
            Switch2 varchar(50),    6
            Switch3 varchar(50),    7
            Switch4 varchar(50),    8
            Switch5 varchar(50),    9
            Switch6 varchar(50),    10
            Switch7 varchar(50),    11
            Switch8 varchar(50),    12
          
         * */
        bool b_HaveNode = false;
        foreach (DataRow d_row in d_DataTable.Rows)
        {
            if (d_row[2].ToString() == Label_m_FarmNum.Text &&
                d_row[3].ToString() == Label_m_ChickenNum.Text &&
                d_row[4].ToString() == Label_m_NodeNum.Text)
            {
                b_HaveNode = true;
                Switch_Items[0] = d_row[5].ToString();
                Switch_Items[1] = d_row[6].ToString();
                Switch_Items[2] = d_row[7].ToString();
                Switch_Items[3] = d_row[8].ToString();
                Switch_Items[4] = d_row[9].ToString();
                Switch_Items[5] = d_row[10].ToString();
                Switch_Items[6] = d_row[11].ToString();
                Switch_Items[7] = d_row[12].ToString();

                break;
            }
        }
        ////////////////////////////////////////////////////////
        #endregion

        

        if (b_HaveNode == false)
        {
            Label_ShowNodeNum.Text = "未找到[" + Label_m_FarmNum.Text + "]号养殖场 [" 
                                            + Label_m_ChickenNum.Text + "]号大棚 [" 
                                               + Label_m_NodeNum.Text + "]号节点的相关数据，请核实";
            return;
        }
        


        #region 初始化CheckBoxList
        ///////////////////////////////////////////////////////////////////
        CheckBoxList_Switch.ForeColor = Color.FromArgb(255, 255, 255);
        for (int i = 0; i < 8; i++)
        {
            if (Switch_Items[i] == "")
                CheckBoxList_Switch.Items.Add("空");
            else
                CheckBoxList_Switch.Items.Add(Switch_Items[i]);
        }
        CheckBoxList_Switch.Items[0].Selected = false;
        CheckBoxList_Switch.Items[1].Selected = false;
        CheckBoxList_Switch.Items[2].Selected = false;
        CheckBoxList_Switch.Items[3].Selected = false;

        CheckBoxList_Switch.Items[4].Selected = false;
        CheckBoxList_Switch.Items[5].Selected = false;
        CheckBoxList_Switch.Items[6].Selected = false;
        CheckBoxList_Switch.Items[7].Selected = false;
        /////////////////////////////////////////////////////////////////////////
        #endregion


        #region 根据数据库的内容生成相应的对号
        int i_temp = 0;
        if (Switch_Result[0] >= '0' && Switch_Result[0] <= '9') i_temp = Switch_Result[0] - '0';
        else                                                    i_temp = Switch_Result[0] - 'A' + 10;

        if (i_temp >= 8)
        {
            CheckBoxList_Switch.Items[0].Selected = true;
            i_temp -= 8;
        }
        if (i_temp >= 4)
        {
            CheckBoxList_Switch.Items[1].Selected = true;
            i_temp -= 4;
        }
        if (i_temp >= 2)
        {
            CheckBoxList_Switch.Items[2].Selected = true;
            i_temp -= 2;
        }
        if (i_temp >= 1)
        {
            CheckBoxList_Switch.Items[3].Selected = true;
           
        }

        i_temp = 0;
        if (Switch_Result[1] >= '0' && Switch_Result[1] <= '9') i_temp = Switch_Result[1] - '0';
        else i_temp = Switch_Result[1] - 'A' + 10;

        if (i_temp >= 8)
        {
            CheckBoxList_Switch.Items[4].Selected = true;
            i_temp -= 8;
        }
        if (i_temp >= 4)
        {
            CheckBoxList_Switch.Items[5].Selected = true;
            i_temp -= 4;
        }
        if (i_temp >= 2)
        {
            CheckBoxList_Switch.Items[6].Selected = true;
            i_temp -= 2;
        }
        if (i_temp >= 1)
        {
            CheckBoxList_Switch.Items[7].Selected = true;
        }
        #endregion

        for (int i = 0; i < 8; i++)
        {
            if (CheckBoxList_Switch.Items[i].Selected == true)
            {
                CheckBoxList_Switch.Items[i].Text += "..........[开启]...原来的状态";
            }
            else
            {
                CheckBoxList_Switch.Items[i].Text += "..........[关闭]...原来的状态";
            }
        }
        Button_SubmitSwitchSetting.Visible = true;
        

    }


    #region 功能函数
    //得到所有的数据 入口参数：数据表名 返回参数：数据表
    public DataTable GetDataBy(string CommandString)
    {
        DataSet t_DataSet = new DataSet();   
        SqlConnection t_ConnectString = new SqlConnection(Label_m_ConnectString.Text);
            
        //得到数据集合
        ///////////////////////////////
        t_ConnectString.Open();
        SqlDataAdapter SqlDap = new SqlDataAdapter(CommandString, t_ConnectString);
        t_DataSet.Clear();
        SqlDap.Fill(t_DataSet);
        t_ConnectString.Close();
        ////////////////////////////////

        return t_DataSet.Tables[0];
    }
 
    #endregion


    protected void Button_SubmitSwitchSetting_Click(object sender, EventArgs e)
    {
        string t_ResultStr = "";

        #region 将开关值转换成十六进制
        int t_iwealth = 1;
        int t_isum = 0;
        for (int i = 3; i >= 0; i--)
        {
            if (CheckBoxList_Switch.Items[i].Selected == true)
            {
                t_isum = t_isum + t_iwealth;
            }
            t_iwealth *= 2;
        }

        if (t_isum <= 9) t_ResultStr += (char)(t_isum + '0');
        else t_ResultStr += (char)(t_isum - 10 + 'A');

        t_iwealth = 1;
        t_isum = 0;
        for (int i = 7; i >= 4; i--)
        {
            if (CheckBoxList_Switch.Items[i].Selected == true)
            {
                t_isum = t_isum + t_iwealth;
            }
            t_iwealth *= 2;
        }
        if (t_isum <= 9) t_ResultStr += (char)(t_isum + '0');
        else t_ResultStr += (char)(t_isum - 10 + 'A');
        #endregion

        //Response.Write(t_ResultStr + " " + Label_m_FarmNum.Text + Label_m_ChickenNum.Text + " " + Label_m_NodeNum.Text);
        InsertIntoSql(t_ResultStr);
        Response.Write("<script>alert('操作成功')</script>");
        
    }

    private void InsertIntoSql(string t_argSettingStr)
    {
        string t_cmdStr = "";
        t_cmdStr = "insert into ChickenInfo values('";
        t_cmdStr += DateTime.Now.ToShortDateString();
        t_cmdStr += "','";
        t_cmdStr += GetTime_Now();
        t_cmdStr += "','";
        t_cmdStr += Label_m_FarmNum.Text;
        t_cmdStr += "','";
        t_cmdStr += Label_m_ChickenNum.Text;
        t_cmdStr += "','";
        t_cmdStr += Label_m_NodeNum.Text;
        t_cmdStr += "','";
        t_cmdStr += t_argSettingStr;  //AF  or FF something
        t_cmdStr += "','0','d1','d2','d3','d4','d5','d6','d7','d8','SETNODE')";

        ExeSqlCommand(t_cmdStr);

    }

    //执行数据库命令
    public void ExeSqlCommand(string t_argCommand)
    { 
        SqlConnection t_Connection = new SqlConnection(Label_m_ConnectString.Text);

        //通过连接执行
        ///////////////////////
        t_Connection.Open();
        SqlCommand comm = new SqlCommand(t_argCommand, t_Connection);
        comm.ExecuteNonQuery();
        t_Connection.Close();
        //////////////////////

        

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

   
}