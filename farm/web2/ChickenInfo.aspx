<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChickenInfo.aspx.cs" Inherits="Chicken" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                <li class="selected">
                    <a href="ChickenInfo.aspx">大棚信息</a>
                </li>
                <li>
                    <a href="ChickenManagement.aspx?zhoukek">大棚管理</a>
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

       
        <!-- 大棚信息显示代码 -->
        <br />
        <br />
        <br />
        <hr />
        <br />
        <br />

        <form  runat="server">
            <div>
                <asp:Table ID="Table1" runat="server" Font-Bold="True" Font-Size="Large">

                </asp:Table>
            </div>
        </form>

         <!-- 显示页面页脚的内容 -->
        <hr />
        <br />
        <br />
        <div id="footer">
            <p>
                &copy; Copyright &copy; 2013.TZC All rights reserved.             
            </p>
        </div> 
   </div>
   

</body>
</html>
