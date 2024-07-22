<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="CarSystemTest.MainPage.Main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" />
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首頁</a></li>
            <li><a href="#">工作臺</a></li>
        </ul>
    </div>
    <div class="mainbox">
        <div class="mainleft">
            <div class="infoleft">
                <div class="listtitle"><a href="../Car/ToDoList.aspx" class="more1">更多</a>待办事项</div>
                <ul class="newlist">
                    <asp:Repeater ID="rptToDoItems" runat="server">
                        <ItemTemplate>
                            <li>
                                <asp:HyperLink Target="_self" ID="hlUrl" NavigateUrl='<%# EncodeUrlStr(Eval("BusinessCode"),Eval("FlowStatusCode"))%>' runat="server" ToolTip='<%# Eval("BusinessCode") %>'><%# Eval("BusinessCode")%></asp:HyperLink>
                                <b>
                                    <label style="font: bold 12px arial,verdana;">申請人：</label><%# Eval("ApplicationUser") %></b><b><label style="font: bold 12px arial,verdana;">出行時間：</label><%# String.Format("{0:yyyy-MM-dd HH:mm}", Eval("RequireStartTime")) %></b><b><label style="font: bold 12px arial,verdana;">行程：</label><%# Eval("RoadLine") %></b><br /><b style="margin-left: 90px;"><label style="font: bold 12px arial,verdana;">乘客：</label><%# Eval("PasengerName") %></b><b><label style="font: bold 12px arial,verdana;">成本中心：</label><%# Eval("CostCenter") %></b></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
        <div class="mainright">
            <div class="dflist">
                <div class="listtitle"><a href="#" class="more1">更多</a>公司公告</div>
                <ul class="newlist">
                    <asp:Repeater ID="rptNotice" runat="server">
                        <ItemTemplate>
                            <li>
                                <%--<asp:HyperLink Target="_blank" ID="hlUrl" NavigateUrl='<%# EncodeUrlStr(Eval("BusinessCode"),Eval("FlowStatusCode"))%>' runat="server" ToolTip='<%# Eval("BusinessCode") %>'><%# Eval("BusinessCode")%></asp:HyperLink>
                                <b><%# Eval("RequireStartTime") %></b>--%>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
