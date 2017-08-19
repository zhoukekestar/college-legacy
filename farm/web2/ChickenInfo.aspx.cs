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


public partial class Chicken : System.Web.UI.Page
{
    
    string m_chConnectString;

    protected void Page_Load(object sender, EventArgs e)
    {

        m_chConnectString = "";
        m_chConnectString = "Data Source=ZKK-COMPUTER;Initial Catalog=FarmManagement;Persist Security Info=True;User ID=sa;Password=sa123";

        DataTable t_DataTable = new DataTable();

        t_DataTable = GetAllDataFrom("FarmMeaning");
       

        string[] t_headString = new string[5];
        t_headString[0] = "养殖场编号";
        t_headString[1] = "养殖场名称";
        t_headString[2] = "大棚编号";
        t_headString[3] = "大棚名称";
        t_headString[4] = "操作";
        ////
        ////////////////////////////////////////////////////
        TableRow at_row = new TableRow();
        for (int i = 0; i < 5; i++)
        {
            TableCell cell = new TableCell();
            cell.Text = t_headString[i];

            at_row.Cells.Add(cell);
        }
        Table1.Rows.Add(at_row);
        ////////////////////////////////////////////////////
        foreach (DataRow row in t_DataTable.Rows)
        {
            TableRow t_row = new TableRow();
            
            for (int i = 2; i < 6; i++)
            {
                TableCell cell = new TableCell();
                cell.Text = row[i].ToString();

                t_row.Cells.Add(cell);
            }

            //添加按钮
            ////////////////////////////////////////////
            Button bt = new Button();
            bt.CommandName = row[2].ToString() + "?" + row[4].ToString() + "?";
            bt.Text = "查看节点信息";
            
            bt.Click += new EventHandler(Button_Click);

            TableCell tcell = new TableCell();
            tcell.Controls.Add(bt);
            t_row.Cells.Add(tcell);
            ///////////////////////////////////////////

            
            Table1.Rows.Add(t_row);
        }

        Table1.BorderWidth = 2;
        Table1.BorderColor = Color.FromArgb(0, 255, 0);
        Table1.GridLines = GridLines.Both;
        Table1.ForeColor = Color.FromArgb(255, 255, 255);
        
      
    }

    void Button_Click(object sender, EventArgs e)
    {
        m_chConnectString = ((Button)sender).CommandName;
        string temp = "NodeInfo.aspx?" + m_chConnectString;
        Response.Redirect(temp, false); 
        
    }

   

    #region 功能函数
    //得到所有的数据 入口参数：数据表名 返回参数：数据表
    public DataTable GetAllDataFrom(string t_argTable)
    {
        DataSet t_DataSet = new DataSet();

            SqlConnection t_ConnectString = new SqlConnection(m_chConnectString);
            string t_CommandString;

            t_CommandString = "select * from ";
            t_CommandString += t_argTable;
            t_CommandString += " order by [FarmNum], [ChickenNum]";
            /*select * from FarmMeaning order by [FarmNum] , [ChickenNum]*/

            //得到数据集合
            ///////////////////////////////
            t_ConnectString.Open();
            SqlDataAdapter SqlDap = new SqlDataAdapter(t_CommandString, t_ConnectString);
            t_DataSet.Clear();
            SqlDap.Fill(t_DataSet);
            t_ConnectString.Close();
            ////////////////////////////////


       
        return t_DataSet.Tables[0];
    }

    //得到一份钟前的表格数据 默认为CodeRecord
    public DataTable GetOneMinuteDataFrom(string t_argTable = "CodeRecord")
    {
        DataSet t_DataSet = new DataSet();
  
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
 
       

        return t_DataSet.Tables[0];
    }

    //更新数据表 入口参数：表名  数据表  
    //将表名为t_argTable的数据更新为t_argDataTable
    public void UpdateDatabase(string t_argTable, DataTable t_argDataTable)
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
}