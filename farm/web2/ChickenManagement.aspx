<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChickenManagement.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="css/style.css" type="text/css" />
    <title></title>
</head>

<body>
    

    <br />
    <br />
    <div id="page">
        <div id="header">
            <a href="Index.aspx" id="logo">
                <img src="images/ChickenLogo.jpg" alt="LOGO"></a>
            
            <ul id="navigation">
                <li>
                    <a href="Index.aspx" title="Home"></a>
                </li>
                <li >
                    <a href="ChickenInfo.aspx">大棚信息</a>
                </li>
                <li class="selected">
                    <a href="ChickenManagement.aspx">大棚管理</a>
                </li>
                <li>
					<a href="NodeInfo.aspx">节点信息</a>
				</li>
                 <li >
					<a href="SetNode.aspx">设置节点参数</a>
				</li>
                <li>
					<a href="SetChicken.aspx">设置大棚参数</a>
				</li>
                
            </ul>
        </div>

        <br />
        <br />
        <br />
        <hr />
        <br />
        <br />
        <div>
            <form id="form1" runat="server" >
            <div>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderWidth="1px" CellPadding="4" DataKeyNames="ID" DataSourceID="SqlDataSourceChickenManagement" OnRowCreated="GridView1_RowCreated" BorderStyle="None">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True"   CancelText="取消" DeleteText="删除" EditText="编辑" HeaderText="操作命令" InsertText="插入" NewText="新建" SelectText="选择" UpdateText="更新" />
                        <asp:BoundField DataField="Time" HeaderText="时间" SortExpression="Time" />
                        <asp:BoundField DataField="Num" HeaderText="编号" SortExpression="Num" />
                        <asp:BoundField DataField="FarmNum" HeaderText="养殖场编号" SortExpression="FarmNum" />
                        <asp:BoundField DataField="FarmName" HeaderText="养殖场名称" SortExpression="FarmName" />
                        <asp:BoundField DataField="ChickenNum" HeaderText="大棚编号" SortExpression="ChickenNum" />
                        <asp:BoundField DataField="ChickenName" HeaderText="大棚名称" SortExpression="ChickenName" />
                        <asp:BoundField DataField="Others" HeaderText="其它" SortExpression="Others" />
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                    </Columns>
                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center"/>
                    <HeaderStyle BackColor="#000000" Font-Bold="True" HorizontalAlign="Center" ForeColor="#FFFFCC" />
                    <PagerStyle BackColor="#FFFFFF" ForeColor="#330099" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#330099" />
                    <SelectedRowStyle BackColor="#FFCC66" ForeColor="#663399" Font-Bold="True" />
                    <SortedAscendingCellStyle BackColor="#FEFCEB" />
                    <SortedAscendingHeaderStyle BackColor="#AF0101" />
                    <SortedDescendingCellStyle BackColor="#F6F0C0" />
                    <SortedDescendingHeaderStyle BackColor="#7E0000" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSourceChickenManagement" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:FarmManagementConnectionString %>" DeleteCommand="DELETE FROM [FarmMeaning] WHERE [ID] = @original_ID AND [Time] = @original_Time AND [Num] = @original_Num AND [FarmNum] = @original_FarmNum AND [FarmName] = @original_FarmName AND [ChickenNum] = @original_ChickenNum AND [ChickenName] = @original_ChickenName AND [Others] = @original_Others" InsertCommand="INSERT INTO [FarmMeaning] ([Time], [Num], [FarmNum], [FarmName], [ChickenNum], [ChickenName], [Others]) VALUES (@Time, @Num, @FarmNum, @FarmName, @ChickenNum, @ChickenName, @Others)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [FarmMeaning] ORDER BY [FarmNum], [ChickenNum]" UpdateCommand="UPDATE [FarmMeaning] SET [Time] = @Time, [Num] = @Num, [FarmNum] = @FarmNum, [FarmName] = @FarmName, [ChickenNum] = @ChickenNum, [ChickenName] = @ChickenName, [Others] = @Others WHERE [ID] = @original_ID AND [Time] = @original_Time AND [Num] = @original_Num AND [FarmNum] = @original_FarmNum AND [FarmName] = @original_FarmName AND [ChickenNum] = @original_ChickenNum AND [ChickenName] = @original_ChickenName AND [Others] = @original_Others">
                    <DeleteParameters>
                        <asp:Parameter Name="original_ID" Type="Int32" />
                        <asp:Parameter DbType="Date" Name="original_Time" />
                        <asp:Parameter Name="original_Num" Type="Int32" />
                        <asp:Parameter Name="original_FarmNum" Type="Int32" />
                        <asp:Parameter Name="original_FarmName" Type="String" />
                        <asp:Parameter Name="original_ChickenNum" Type="Int32" />
                        <asp:Parameter Name="original_ChickenName" Type="String" />
                        <asp:Parameter Name="original_Others" Type="String" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter DbType="Date" Name="Time" />
                        <asp:Parameter Name="Num" Type="Int32" />
                        <asp:Parameter Name="FarmNum" Type="Int32" />
                        <asp:Parameter Name="FarmName" Type="String" />
                        <asp:Parameter Name="ChickenNum" Type="Int32" />
                        <asp:Parameter Name="ChickenName" Type="String" />
                        <asp:Parameter Name="Others" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter DbType="Date" Name="Time" />
                        <asp:Parameter Name="Num" Type="Int32" />
                        <asp:Parameter Name="FarmNum" Type="Int32" />
                        <asp:Parameter Name="FarmName" Type="String" />
                        <asp:Parameter Name="ChickenNum" Type="Int32" />
                        <asp:Parameter Name="ChickenName" Type="String" />
                        <asp:Parameter Name="Others" Type="String" />
                        <asp:Parameter Name="original_ID" Type="Int32" />
                        <asp:Parameter DbType="Date" Name="original_Time" />
                        <asp:Parameter Name="original_Num" Type="Int32" />
                        <asp:Parameter Name="original_FarmNum" Type="Int32" />
                        <asp:Parameter Name="original_FarmName" Type="String" />
                        <asp:Parameter Name="original_ChickenNum" Type="Int32" />
                        <asp:Parameter Name="original_ChickenName" Type="String" />
                        <asp:Parameter Name="original_Others" Type="String" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </div>
            </form>

            <br />
            <br />
            <hr />
            <br />
            <br />
            <div id="footer">
            <p>
                &copy; Copyright &copy; 2013.TZC All rights reserved.             
            </p>
            </div>

        </div>

    </div>
</body>
</html>
