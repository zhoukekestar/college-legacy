<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NodeInfo.aspx.cs" Inherits="NodeInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="css/style.css" type="text/css"/>
    <!-- java script------------------------------------------------------------------>
        <script  type="text/javascript">
            function jumpto() {
                var Farm = document.getElementById("Text_FarmNum").value;
                var Chicken = document.getElementById("Text_ChickenNum").value;
                var url = "NodeInfo.aspx?" + Farm + "?" + Chicken + "?";
                location.href = url;
               
            }
        </script>
    <!-- java script------------------------------------------------------------------>
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
                <li class="selected">
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
        <div id ="footer2">                 
            养殖场编号：
            <input id="Text_FarmNum" type="text" size="5"/> 
            大棚编号：
            <input id="Text_ChickenNum" type="text" size="5"/>
            <input id="ButtonSubmit" type="button" value="转到该大鹏" onclick="jumpto()"/><br />
        </div>
        
        <br />
        <br />

        <!--  begin content ----------------------------- -->
        <form id="Form" runat="server">
            <div>
                <asp:Label ID="Label_ShowNode" runat="server" Text="显示"></asp:Label>
                
                <asp:Table ID="Table_ShowNode" runat="server"></asp:Table>
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


    </body>
</html>
