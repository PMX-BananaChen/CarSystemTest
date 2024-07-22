<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ToDoList.aspx.cs" Inherits="CarSystemTest.Car.ToDoList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/select.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/My97DatePicker/WdatePicker.js"></script>
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <script src="../Scripts/select-ui.min.js"></script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首页</a></li>
            <li><a href="#">表单</a></li>
        </ul>
    </div>
    </script>
    <div class="formbody">
        <div class="formtitle"><span>待辦事項</span></div>

        <div style="width: 100%">
            <asp:Repeater ID="rptAppHistory" runat="server">
                <HeaderTemplate>
                    <table class="tablelist" id="tblAppHistory">
                        <tr>
                            <th>申請單號
                            </th>
                            <th>申請人
                            </th>
                            <th>出行時間
                            </th>
                            <th>乘車人員
                            </th>
                            <th>行程
                            </th>
                            <th>流程狀態
                            </th>
                            <th>下一處理人
                            </th>
                            <th>成本中心
                            </th>
                            <th>操作
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 10%;">
                            <asp:Label ID="lblBusinessCode" runat="server" Text='<%#Eval("BusinessCode") %>'></asp:Label></td>
                        <td style="width: 8%;">
                            <asp:Label ID="lblBusinessType" runat="server" Text='<%#Eval("ApplicationUser") %>'></asp:Label></td>
                        <td style="width: 12%;">
                            <asp:Label ID="lblRequireTime" runat="server" Text='<%#String.Format("{0:yyyy-MM-dd HH:mm}", Eval("RequireStartTime"))%>'></asp:Label></td>
                        <td style="width: 20%;">
                            <asp:Label ID="lblPassenger" runat="server" Text='<%#Eval("PasengerName") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblRoadLine" runat="server" Text='<%#Eval("RoadLine") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblFlowStatus" runat="server" Text='<%#Eval("FlowStatusName") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblNextPending" runat="server" Text='<%#Eval("PendingUser") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblCostCenter" runat="server" Text='<%#Eval("CostCenter") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:ImageButton ID="ImgDetail"
                                ImageAlign="absmiddle" ToolTip="詳情" BorderStyle="None" ImageUrl="../Images/f03.png" PostBackUrl='<%# GetDetailURL(Eval("BusinessCode"),Eval("FlowStatusCode")) %>'
                                Style="cursor: pointer;" runat="server" AlternateText="Detail" />詳情
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="pagin">
            <div class="message">
                共<i class="blue">
                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                </i>條記錄，當前顯示第&nbsp;<i class="blue"><asp:Label ID="lblCurrPage" runat="server"></asp:Label>&nbsp;</i>頁，共&nbsp;<i class="blue"><asp:Label ID="lblTttPage" runat="server"></asp:Label>&nbsp;</i>頁
            </div>
            <ul class="paginList">
                <li class="paginItem">
                    <asp:LinkButton ID="lbFirst" runat="server" OnClick="lbFirst_Click">首頁</asp:LinkButton></li>
                <li class="paginItem">
                    <asp:LinkButton ID="lbPrevious" runat="server" OnClick="lbPrevious_Click">上一頁</asp:LinkButton></li>
                <li class="paginItem">
                    <asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_Click">下一頁</asp:LinkButton></li>
                <li class="paginItem">
                    <asp:LinkButton ID="lbLast" runat="server" OnClick="lbLast_Click">末頁</asp:LinkButton></li>
            </ul>
        </div>
    </div>
</asp:Content>
