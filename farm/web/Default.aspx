<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="css/css.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="head1">
        <div class="head1_left">浙江省植物进化生态学与保护重点实验室</div>
        <div class="head1_right"><%= cDate()%></div>
    </div>
    <div class="head2"><%=cHello()%></div>

<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="padding-top: 1px;">
    <div class="grid_box">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div align="center" class="grid_btn_top">
        <div style="float:left; margin-left: 10px;">
            标题：<asp:TextBox ID="txttitle" Width="300px" runat="server"></asp:TextBox>
        </div>
        <div style="float:left; margin-left: 4px;">
            <asp:ImageButton ID="btnQuery" runat="server" ImageUrl="images/btn_query1.gif"  />
        </div>
    </div>
    <div align="center">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
            CssClass="grid_style" PageSize="20" AllowPaging="False" ShowFooter="False"
            onrowcreated="GridView1_RowCreated" >
        <Columns>
            <asp:TemplateField HeaderText="序号" InsertVisible="False">
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" Width="40px" />
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# this.GridView1.PageIndex * this.GridView1.PageSize + this.GridView1.Rows.Count + 1%>'/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="文章标题">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# "../../article.aspx?id="+Eval("id") %>'
                        Text='<%# Eval("title") %>' Target="_blank"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:BoundField DataField="pro_date" HeaderText="发布日期" DataFormatString="{0:yyyy-MM-dd}" ReadOnly="true">
                <ItemStyle HorizontalAlign="Center" Width="80px"  />
            </asp:BoundField>             
            <asp:BoundField DataField="display_order" HeaderText="显示序号">
                <ItemStyle HorizontalAlign="Right" Width="60px" />
            </asp:BoundField>
            <asp:BoundField DataField="is_verify_name" HeaderText="审核" ReadOnly="true">
                <ItemStyle HorizontalAlign="Center" Width="60px" />
            </asp:BoundField>
            <asp:CommandField ShowEditButton="True" EditText="修改">
                <ItemStyle HorizontalAlign="Left" Width="60px" />
            </asp:CommandField>
            <asp:ButtonField CommandName="UpdateText" Text="编辑内容">
                <ItemStyle HorizontalAlign="Center" Width="60px" />
            </asp:ButtonField>
            <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div onclick=&quot;JavaScript:return confirm('确定删除吗？')&quot;&gt;删除&lt;/div&gt; ">
                <ItemStyle HorizontalAlign="Left" Width="50px" />
            </asp:CommandField>
        </Columns>
        <AlternatingRowStyle CssClass="gridalteritem" />
        <HeaderStyle CssClass="gridhead" />
        <PagerStyle HorizontalAlign="Center" />
        <FooterStyle BackColor="WhiteSmoke" />
        <PagerSettings Visible="False" />
        </asp:GridView>
        <div class="grid_btn_bottom"></div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>    
    </div>
    </div>
    </form>
</body>
</html>
