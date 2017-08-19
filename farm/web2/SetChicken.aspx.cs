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


public partial class SetChicken : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
       // if (!IsPostBack)
        {
            //Button_SubmitChickenSetting.Visible = false;
            Label_m_ConnectString.Visible = false;
            Label_m_FarmNum.Visible = false;
            Label_m_ChickenNum.Visible = false;
            Table_SetChicken.ForeColor = Color.FromArgb(255, 255, 255);
          

            MyInit();
            Label_ShowChickenNum.ForeColor = Color.FromArgb(255, 255, 0);

            if (Label_m_FarmNum.Text == "0" &&
                Label_m_ChickenNum.Text == "0" )
                Label_ShowChickenNum.Text = "您目前还没有选择任何节点";
            else
                Label_ShowChickenNum.Text = "您正在设置 [" + Label_m_FarmNum.Text + "]号养殖场 ["
                                                        + Label_m_ChickenNum.Text + "]号大棚 参数";


            ShowChickenSetting();
              
        }
    }

    private void MyInit()
    {
        string url_str = Request.Url.ToString();
        Label_m_FarmNum.Text = "0";
        Label_m_ChickenNum.Text = "0";
        
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
        }

        Label_m_FarmNum.Text = (Int32.Parse(Label_m_FarmNum.Text)).ToString();
        Label_m_ChickenNum.Text = (Int32.Parse(Label_m_ChickenNum.Text)).ToString();

        Label_m_ConnectString.Text = "Data Source=ZKK-COMPUTER;Initial Catalog=FarmManagement;Persist Security Info=True;User ID=sa;Password=sa123";

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

  
    private void InsertIntoSql(string t_argSettingStr)
    {
        string t_cmdStr = "";
        t_cmdStr = "insert into ChickenSetting values('";
        t_cmdStr += DateTime.Now.ToShortDateString();
        t_cmdStr += "','";
        t_cmdStr += GetTime_Now();
        t_cmdStr += "','";
        t_cmdStr += Label_m_FarmNum.Text;
        t_cmdStr += "','";
        t_cmdStr += Label_m_ChickenNum.Text;
        t_cmdStr += "',";
        t_cmdStr += t_argSettingStr;
        t_cmdStr += ",'SET')";

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
    #endregion

    protected void Button_SubmitChickenSetting_Click(object sender, EventArgs e)
    {
        string t_SettingStr = "'";
        
        for (int i = 0; i < 7; i++)
        {
            string t_temp = "";
            t_temp = ((TextBox)(Table_SetChicken.Rows[i].Cells[1].Controls[0])).Text + "-" + ((TextBox)(Table_SetChicken.Rows[i].Cells[3].Controls[0])).Text;

            t_SettingStr += t_temp + "','";
        }
        string tt_temp =
            ((TextBox)(Table_SetChicken.Rows[7].Cells[1].Controls[0])).Text + "-" +
            ((TextBox)(Table_SetChicken.Rows[7].Cells[3].Controls[0])).Text;

        t_SettingStr += tt_temp + "'";

        InsertIntoSql(t_SettingStr);

        Response.Write("<script>alert('操作成功')</script>");
    }

    private void ShowChickenSetting()
    {
        DrawTable();
        DataTable d_DataTable = new DataTable();
        d_DataTable = GetDataBy("select * from ChickenSetting where Others='INFSET_UPLOAD' order by [Time] desc, [Num] desc");

        
        /*
         * 2 farm
         * 3 chicken
         * 4 nh3
         * 5 co2
         * 6 light
         * 7 temp
         * 8 hum
         * */

        bool b_haveData = false;
        foreach (DataRow d_row in d_DataTable.Rows)
        {
            if (d_row[2].ToString() == Label_m_FarmNum.Text &&
                d_row[3].ToString() == Label_m_ChickenNum.Text)
            {
                b_haveData = true;
                for (int i = 0; i < 8; i++)
                {
                    ((TextBox)(Table_SetChicken.Rows[i].Cells[1].Controls[0])).Text = BeforeMidString(d_row[4 + i].ToString());
                    ((TextBox)(Table_SetChicken.Rows[i].Cells[3].Controls[0])).Text = AfterMidString(d_row[4 + i].ToString());
                }
            }
        }

        if (b_haveData == false)
        {
            Label_ShowChickenNum.Text += "  暂时无该大棚的数据";
        }


    }

    private void DrawTable()
    {
        string[] t_headStr = { "氨气", "二氧化碳", "光照", "温度", "湿度", "保留", "保留", "保留" };
        
        for (int i = 0; i < 8; i++)
        {
            TableRow t_row = new TableRow();
            for (int j = 0; j < 4; j++)
            {
                TableCell t_cell = new TableCell();
                if (j == 0)
                {
                    t_cell.Text = t_headStr[i];
                    t_row.Cells.Add(t_cell);
                }
                else if (j == 1)
                {
                    TextBox t_box = new TextBox();
                    t_cell.Controls.Add(t_box);
                    t_row.Cells.Add(t_cell);
                }
                else if (j == 2)
                {
                    t_cell.Text = "到";
                    t_row.Cells.Add(t_cell);
                }
                else if (j == 3)
                {
                    TextBox t_box = new TextBox();
                    t_cell.Controls.Add(t_box);
                    t_row.Cells.Add(t_cell);
                }
            }
            Table_SetChicken.Rows.Add(t_row);
        }
    }

    #region function

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

}