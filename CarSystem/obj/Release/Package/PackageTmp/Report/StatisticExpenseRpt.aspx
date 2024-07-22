<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="StatisticExpenseRpt.aspx.cs" Inherits="CarSystem.Report.StatisticExpenseRpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/select.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/My97DatePicker/WdatePicker.js"></script>
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <script src="../Scripts/select-ui.min.js"></script>
    <script src="../Scripts/fixTable.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            FixTable("tblReportData", 1, 1000, 400);
            $(".select1").uedSelect({
                width: 345
            });
            $(".select2").uedSelect({
                width: 100
            });
            $(".select3").uedSelect({
                width: 150
            });
        });

        function checkDate() {
            var $starDay = $("#MainContent_txtStartDay").val();
            var $endDay = $("#MainContent_txtEndDay").val();
            if ($starDay == "" || $endDay == "") {
                alert("開始日期或結束日期不能為空!");
                return false;
            }
            var $dt1 = new Date(Date.parse($starDay));
            var $dt2 = new Date(Date.parse($endDay));
            var $diff = parseInt(($dt2.getTime() - $dt1.getTime()) / (1000 * 60 * 60 * 24));
            if ($diff > 90) {
              //  alert("時間間隔不能超過3個月!");
                return false;
            }
            return true;
        }
    </script>
    <div class="formbody">
        <div class="reporttitle">
            <span>月結費用明細</span>
            <div style="float: right;">
                <asp:Button ID="btnExcel" runat="server" Width="70px" Height="25px" BackColor="#fff" Text="導出EXCEL" OnClick="btnExcel_Click" />
                <%--<input style="width: 70px; height: 25px; color: #fff; background: url(../images/btnbg.png);" type="button" runat="server" value="導出EXCEL" onclick="EXCEL" />--%>
            </div>
        </div>
        <div style="margin-left: 50px; margin-bottom: 8px;">
            <table style="width: 1000px; border-spacing: 10px; padding: 10px;">
                <tr>
                    <td>出行開始時間</td>
                    <td>
                        <input id="txtStartDay" runat="server" class="dfinput" type="text" onfocus="WdatePicker({ skin: 'twoer', dateFmt: 'yyyy-MM-dd', })" style="width: 100px;" />
                    </td>
                    <td>出行結束時間</td>
                    <td>
                        <input id="txtEndDay" runat="server" class="dfinput" type="text" onfocus="WdatePicker({ skin: 'twoer', dateFmt: 'yyyy-MM-dd', })" style="width: 100px;" />
                    </td>
                    <td>成本中心</td>
                    <td>
                        <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="select3">
                        </asp:DropDownList>
                    </td>
                    <td>目的地</td>
                    <td>
                        <asp:DropDownList ID="ddlDestination" runat="server" CssClass="select3">
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>廠商</td>
                    <td>
                        <asp:DropDownList ID="ddlVendor" runat="server" CssClass="select2" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>司機</td>
                    <td>
                        <asp:DropDownList ID="ddlDriver" runat="server" CssClass="select2">
                        </asp:DropDownList>
                    </td>
                    <td>出行目的</td>
                    <td>
                        <asp:DropDownList ID="ddlPurpose" runat="server" CssClass="select3">
                        </asp:DropDownList>
                    </td>
                    <td>申請單類型</td>
                    <td>
                        <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="select3">
                            <asp:ListItem Text=" " Value=""></asp:ListItem>
                            <asp:ListItem Text="正常單" Value="1"></asp:ListItem>
                            <asp:ListItem Text="補單" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnSeach" runat="server" CssClass="scbtn" Text="查詢" OnClientClick="return checkDate();" OnClick="btnSeach_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-left: 50px;">
            <asp:Repeater ID="rptReportData" runat="server">
                <HeaderTemplate>
                    <table id="tblReportData" class="reportlist">
                        <thead>
                            <tr>
                                <th>申請單號
                                </th>
                                <th>申請單類型
                                </th>
                                <th>申請人
                                </th>
                                <th>用車人
                                </th>
                                <th>成本中心
                                </th>
                                <th>出行時間
                                </th>
                                <th>出發地
                                </th>
                                <th>出發地详细地址
                                </th>
                                <th>目的地
                                </th>
                                 <th>目的地詳細地址
                                </th>
                                <th>廠商
                                </th>
                                <th>司機
                                </th>
                                <th>車牌號
                                </th>
                                <th>派車人
                                </th>
                                <th>附註(申請人)
                                </th>
                                <th>基本費用
                                </th>
                                <th>里程費
                                </th>
                                <th>高速費
                                </th>
                                <th>候時費
                                </th>
                                <th>繞路費
                                </th>
                                <th>合計
                                </th>
                                <th>備註
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 3%;">
                            <asp:Label ID="lblBusinessCode" runat="server" Text='<%#Eval("BusinessCode") %>'></asp:Label></td>
                        <td style="width: 3%;">
                            <asp:Label ID="lblBusinessType" runat="server" Text='<%#Eval("BusinessTypeName") %>'></asp:Label></td>
                        <td style="width: 2%;">
                            <asp:Label ID="lblApplicationUser" runat="server" Text='<%#Eval("ApplicationUser") %>'></asp:Label></td>
                        <td style="width: 12%; text-align: left;">
                            <asp:Label ID="lblPassenger" runat="server" Text='<%#Eval("PassengerName") %>'></asp:Label></td>
                        <td style="width: 3%">
                            <asp:Label ID="lblCostCenter" runat="server" Text='<%#Eval("CostCenter") %>'></asp:Label></td>
                        <td style="width: 5%;">
                            <asp:Label ID="lblRequireTime" runat="server" Text='<%#String.Format("{0:yyyy-MM-dd HH:mm}", Eval("RequireStartTime"))%>'></asp:Label></td>
                        <td style="width: 7%; text-align: left;">
                            <asp:Label ID="lblDepartureRegion" runat="server" Text='<%#Eval("DepartureRegion") %>'></asp:Label></td>
                         <td style="width: 7%; text-align: left;">
                            <asp:Label ID="Label3" runat="server" Text='<%#Eval("DepartureDetailAddress") %>'></asp:Label></td>
                        <td style="width: 15%; text-align: left;">
                            <asp:Label ID="lblDestinationRegion" runat="server" Text='<%#Eval("DestinationRegion") %>'></asp:Label></td>
                        <td style="width: 15%; text-align: left;">
                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("DetailAddress") %>'></asp:Label></td>
                        <td style="width: 2%">
                            <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("VendorShortName") %>'></asp:Label></td>
                        <td style="width: 2%;">
                            <asp:Label ID="lblDriverUser" runat="server" Text='<%#Eval("DriverUser") %>'></asp:Label></td>
                        <td style="width: 3%;">
                            <asp:Label ID="lblVehicleNO" runat="server" Text='<%#Eval("VehicleNO") %>'></asp:Label></td>
                        <td style="width: 3%;">
                            <asp:Label ID="lblPurpose" runat="server" Text='<%#Eval("VehiclePurposeName") %>'></asp:Label></td>
                        <td style="width: 20%; text-align: left;">
                            <asp:Label ID="lblAppUserRemark" runat="server" Text='<%#Eval("AppUserRemark") %>'></asp:Label></td>
                        <td style="width: 3%;">
                            <asp:Label ID="lblBaseFee" runat="server" Text='<%#Eval("BaseFee") %>'></asp:Label></td>
                        <td style="width: 3%;">
                         <asp:Label ID="Label1" runat="server" Text='<%#Eval("MileageFee") %>'></asp:Label></td>
                        <td style="width: 3%;">
                            <asp:Label ID="lblTollFee" runat="server" Text='<%#Eval("TollFee") %>'></asp:Label></td>
                        <td style="width: 3%;">
                            <asp:Label ID="lblWaitingFee" runat="server" Text='<%#Eval("WaitingFee") %>'></asp:Label></td>
                        <td style="width: 3%;">
                            <asp:Label ID="lblDetourFee" runat="server" Text='<%#Eval("DetourFee") %>'></asp:Label></td>
                        <td style="width: 3%;">
                            <asp:Label ID="lblSubtotalFee" runat="server" Text='<%#Eval("SubtotalFee")%>'></asp:Label></td>
                        <td style="width: 12%; text-align: left;">
                            <asp:Label ID="lblZwRemark" runat="server" Text='<%# Eval("ZwRemark")%>'></asp:Label></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
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
