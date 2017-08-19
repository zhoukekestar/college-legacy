<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="css/style.css" type="text/css"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>

    <br />
    <br />
	<div id="page">
		<div id="header">
			<a href="Index.aspx" id="logo"><img src="images/ChickenLogo.jpg" alt="LOGO"></a>
			
            <ul id="navigation">
				<li class="selected">
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

        <!--  begin content ----------------------------- -->
        <div>

            <table style="width: 100%;" id="HomeTable">
                <tr>
                    <td>hello
                        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
                    </td>
                    <td>2</td>
                    <td>3</td>
                </tr>
                <tr>
                    <td>helo2</td>
                    <td>2</td>
                    <td>23</td>
                </tr>
                <tr>
                    <td>lal</td>
                    <td>lla2</td>
                    <td>lala3</td>
                </tr>
            </table>
        </div>
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
