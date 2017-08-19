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


public partial class NodeInfo : System.Web.UI.Page
{
    
    string m_chConnectString;

    string m_chFarmNum;
    string m_chChickenNum;

    protected void Page_Load(object sender, EventArgs e)
    {
        MyInit();
        LoadNodeInfoTable();   
       
          
    }

    private void MyInit()
    {
        string url_str = Request.Url.ToString();
        
        m_chChickenNum = "0";
        m_chFarmNum = "0";
    

        m_chConnectString = "Data Source=ZKK-COMPUTER;Initial Catalog=FarmManagement;Persist Security Info=True;User ID=sa;Password=sa123";

        int t_iFalg = 0;
        for (int i = 0; i < url_str.Length; i++)
        {
            if (url_str[i] == '?')
            {
                t_iFalg++;
            }
            else if (t_iFalg == 1)
            {
                m_chFarmNum += url_str[i];
            }
            else if (t_iFalg == 2)
            {
                m_chChickenNum += url_str[i];
            }

        }
        if (string.IsNullOrEmpty(m_chFarmNum))
            m_chFarmNum = "0";
        if (string.IsNullOrEmpty(m_chChickenNum))
            m_chChickenNum = "0";

        m_chFarmNum = (Int32.Parse(m_chFarmNum)).ToString();
        m_chChickenNum = (Int32.Parse(m_chChickenNum)).ToString();

        Label_ShowNode.ForeColor = Color.FromArgb(255, 255, 255);
        if ((m_chFarmNum == "" || m_chFarmNum == "0") && (m_chChickenNum == "" || m_chChickenNum == "0"))
            Label_ShowNode.Text = "您还没有选择任何大棚";
        else
            Label_ShowNode.Text = "您选择了[" + m_chFarmNum + "]养殖场 [" + m_chChickenNum + "]号大棚\n";

    }


    #region 功能函数
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
            t_CommandString += " order by [NodeNum]";

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
        DataSet t_DataSet = new DataSet();
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
            Response.Write(ex.Message);
            //MessageBox.Show(DateTime.Now.ToString() + " CommonFunction::AddRunningInfo Error: " + ex.Message);
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
    #endregion
   

    private void LoadNodeInfoTable()
    {
        DataTable t_DataTable = new DataTable();
        t_DataTable = GetAllDataFrom("NodeInfo");

        /*
       * 0: Time date not null,
       * 1: Num int not null,
       * 2: FarmNum int not null,
       * 3: ChickenNum int not null,
       * 4: NodeNum int not null,
       * 
       * */
        {
            string[] t_headStr = { "节点号", "开关1", "开关2", "开关3", "开关4", "开关5", "开关6", "开关7", "开关8","设置",
                                     "传感器1", "传感器2","传感器3","传感器4","传感器5","传感器6","传感器7","传感器8"};
            TableRow t_row = new TableRow();
            for (int i = 0; i < 10; i++)
            {
                TableCell t_cell = new TableCell();
                t_cell.Text = t_headStr[i];
                t_row.Cells.Add(t_cell);
            }
            Table_ShowNode.Rows.Add(t_row);
            
        }

        bool have_data = false;
        foreach (DataRow d_row in t_DataTable.Rows)
        {

            if (d_row[2].ToString() == m_chFarmNum &&
                d_row[3].ToString() == m_chChickenNum)
            {
                have_data = true;
                TableRow t_row = new TableRow();
                for (int i = 4; i < 13; i++)         //设置显示的信息
                {
                    TableCell t_cell = new TableCell();
                    if (string.IsNullOrEmpty(d_row[i].ToString()))
                        t_cell.Text = "/";
                    else
                        t_cell.Text = d_row[i].ToString();
                    t_row.Cells.Add(t_cell);
                }

                //添加按钮
                ////////////////////////////////////////////
                Button bt = new Button();
                bt.CommandName = d_row[2].ToString() + "?" + d_row[3].ToString() + "?" + d_row[4].ToString() + "?";
                bt.Text = "设置该节点";

                bt.Click += new EventHandler(Button_Click);

                TableCell tcell = new TableCell();
                tcell.Controls.Add(bt);
                t_row.Cells.Add(tcell);

                ///////////////////////////////////////////
                Table_ShowNode.Rows.Add(t_row);
            }

        }

        if (have_data == false)
        {

            Table_ShowNode.Rows.Clear();

            TableRow t_row = new TableRow();   
            TableCell t_cell1 = new TableCell();
            t_cell1.Text = "没有该大棚的相关节点数据，请核实";
            t_row.Cells.Add(t_cell1);

            Table_ShowNode.Rows.Add(t_row);
        }

        Table_ShowNode.BorderWidth = 2;
        Table_ShowNode.BorderColor = Color.FromArgb(0, 255, 0);
        Table_ShowNode.GridLines = GridLines.Both;
        Table_ShowNode.ForeColor = Color.FromArgb(255, 255, 255);
        
    }

    private void Button_Click(object sender, EventArgs e)
    {
        string t_str =  ((Button)sender).CommandName;
        string url = "SetNode.aspx?" + t_str;

        Response.Redirect(url, true);
        
    }

}