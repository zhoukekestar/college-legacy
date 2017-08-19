<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetNode.aspx.cs" Inherits="SetNode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="css/style.css" type="text/css"/>
    <title></title>
</head>
<body>
    

    <br />
    <br />
	<div id="page">
		<div id="header">
			<a href="Index.aspx" id="logo"><img src="images/ChickenLogo.jpg" alt="LOGO"></a>
			
            <ul id="navigation">
				<li>
					<a href="Index.aspx" title="Home"></a>
				</li>
				<li>
					<a href="ChickenInfo.aspx">大棚信息</a>
				</li>
				<li>
					<a href="ChickenManagement.aspx">大棚管理</a>
				</li>
                <li>
					<a href="NodeInfo.aspx">节点信息</a>
				</li>
                <li class="selected">
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

        <!--  begin content ----------------------------- -->
        <form id="form1" runat="server">
            <div>
                <asp:Label ID="Label_m_ConnectString" runat="server"></asp:Label>
                <asp:Label ID="Label_m_FarmNum" runat="server"></asp:Label>
                <asp:Label ID="Label_m_ChickenNum" runat="server"></asp:Label>
                <asp:Label ID="Label_m_NodeNum" runat="server"></asp:Label>
            </div>
            <div>
                <asp:Label ID="Label_ShowNodeNum" runat="server" Text="显示节点号"></asp:Label>
                <asp:CheckBoxList ID="CheckBoxList_Switch" runat="server" ></asp:CheckBoxList>
                <asp:Button ID="Button_SubmitSwitchSetting" runat="server" Text="执行当前节点设置" OnClick="Button_SubmitSwitchSetting_Click" />
            </div>
        </form>
        <!-- end content ----------------------------------->

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
    </form>

    </body>
</html>