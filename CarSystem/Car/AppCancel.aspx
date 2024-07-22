<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AppCancel.aspx.cs" Inherits="CarSystemTest.Car.AppCancel" %>
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
    <script type="text/javascript">
        $(document).ready(function (e) {
            $(".select1").uedSelect({
                width: 345
            });
            $(".select2").uedSelect({
                width: 80
            });
            $(".select3").uedSelect({
                width: 150
            });
        });

        function isFinished(finishTimeObj) {
            //alert("123");
            if (typeof (finishTimeObj) == undefined) {
                alert("該車輛行程未完成，不能查看歷史軌跡!");
                return false;
            }
            return true;
        }
    </script>
    <div class="formbody">
        <div class="formtitle"><span>歷史記錄</span></div>
        <ul class="seachform">
            <li>
                <label>開始時間</label>
                <input id="txtStartDay" runat="server" class="dfinput" type="text" onfocus="WdatePicker({ skin: 'twoer', dateFmt: 'yyyy-MM-dd', })" style="width: 150px;" />
                <%--<input id="txtStartDay" type="date" class="dfinput" runat="server" style="width: 150px;" />--%>

            </li>
            <li>
                <label>結束時間</label>
                <input id="txtEndDay" runat="server" class="dfinput" type="text" onfocus="WdatePicker({ skin: 'twoer', dateFmt: 'yyyy-MM-dd', })" style="width: 150px;" />
                <%--<input id="txtEndDay" type="date" class="dfinput" runat="server" style="width: 150px;" />--%>
            </li>
            <li>
                <label>流程狀態</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlFlowStatus" runat="server" CssClass="select3">
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <label>&nbsp;</label>
                <asp:Button ID="btnSeach" runat="server" CssClass="scbtn" Text="查詢" OnClick="btnSeach_Click" />
            </li>
        </ul>
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
                            <asp:Label ID="lblFlowStatus" runat="server" Text='<%#Eval("FlowStatusName") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblNextPending" runat="server" Text='<%#Eval("PendingUser") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblCostCenter" runat="server" Text='<%#Eval("CostCenter") %>'></asp:Label></td>
                        <td style="width: 20%;">
                            <asp:ImageButton ID="ImgDetail"
                                ImageAlign="absmiddle" ToolTip="詳情" BorderStyle="None" ImageUrl="../Images/f03.png" PostBackUrl='<%# GetDetailURL(Eval("BusinessCode"),Eval("FlowStatusCode")) %>'
                                Style="cursor: pointer;" runat="server" AlternateText="Detail" />詳情&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="ImgFee"
                                ImageAlign="absmiddle" ToolTip="費用" BorderStyle="None" ImageUrl="../Images/t02.png" Visible='<%# CheckUser(Eval("FlowStatusCode")) %>' PostBackUrl='<%# GetCalFeeURL(Eval("BusinessCode")) %>'
                                Style="cursor: pointer;" runat="server" AlternateText="Fee" /><label style="display: inline;" runat="server" visible='<%# CheckUser(Eval("FlowStatusCode")) %>'>費用</label>
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
