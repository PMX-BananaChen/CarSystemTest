<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AuditLine.aspx.cs" Inherits="CarSystemTest.Car.AuditLine" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/select.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <script src="../Scripts/select-ui.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
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
    </script>
    <div class="formbody">
        <div class="reporttitle">
            <span>簽核線明細</span>
            <div style="float: right;">
                <asp:Button ID="btnExcel" runat="server" Width="70px" Height="25px" BackColor="#fff" Text="導出EXCEL" OnClick="btnExcel_Click" />
                <%--<input style="width: 70px; height: 25px; color: #fff; background: url(../images/btnbg.png);" type="button" runat="server" value="導出EXCEL" onclick="EXCEL" />--%>
            </div>
        </div>
        <div style="margin-left: 50px; margin-bottom: 8px;">
            <table style="width: 1000px; border-spacing: 10px; padding: 10px;">
                <tr>
                    <td>申請人英文名</td>
                    <td>
                        <input id="txtAssistent" runat="server" class="dfinput" type="text" style="width: 100px;" />
                    </td>
                    <td>主管英文名</td>
                    <td>
                        <input id="txtLeader" runat="server" class="dfinput" type="text" style="width: 100px;" />
                    </td>
                     <td>派車人</td>
                    <td>
                        <asp:DropDownList ID="ddlDispatcher" runat="server" CssClass="select3">
                        </asp:DropDownList>
                    </td>
                    <td>狀態</td>
                    <td>
                        <asp:DropDownList ID="ddlIsEnabled" runat="server" CssClass="select3">
                            <asp:ListItem Text=" " Value=""></asp:ListItem>
                            <asp:ListItem Text="啟用" Value="1"></asp:ListItem>
                            <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>                   
                    <td><asp:Button ID="btnSeach" runat="server" CssClass="scbtn" Text="查詢" OnClick="btnSeach_Click" />
                    </td>
                </tr>              
            </table>
        </div>
        <div style="margin-left: 20px;">
            <asp:Repeater ID="rptAuditData" runat="server">
                <HeaderTemplate>
                    <table id="tblReportData" class="tablelist">
                        <thead>
                            <tr>
                                <th>申請人
                                </th>
                                <th>申請人英文名
                                </th>
                                <th>第一階主管
                                </th>
                                <th>第一階主管英文名
                                </th>
                                <th>第二階主管
                                </th>
                                <th>第二階主管英文名
                                </th>
                                <th>派車人
                                </th>
                                <th>派車人英文名
                                </th>
                                <th>狀態
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 10%;">
                            <asp:Label ID="lblAssistentName" runat="server" Text='<%#Eval("AssistentName") %>'></asp:Label></td>
                        <td style="width: 15%;">
                            <asp:Label ID="lblAssistenEnglishName" runat="server" Text='<%#Eval("AssistenEnglishName") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblFirstLeaderName" runat="server" Text='<%#Eval("FirstLeaderName") %>'></asp:Label></td>
                        <td style="width: 15%; text-align: left;">
                            <asp:Label ID="lblFirstLeaderEnglishName" runat="server" Text='<%#Eval("FirstLeaderEnglishName") %>'></asp:Label></td>
                        <td style="width: 10%">
                            <asp:Label ID="lblSecondLeaderName" runat="server" Text='<%#Eval("SecondLeaderName") %>'></asp:Label></td>
                        <td style="width: 15%;">                  
                            <asp:Label ID="lblSecondLeaderEnglishName" runat="server" Text='<%#Eval("SecondLeaderEnglishName") %>'></asp:Label></td>
                        <td style="width: 10%; text-align: left;">
                            <asp:Label ID="lblDispatcherName" runat="server" Text='<%#Eval("DispatcherName") %>'></asp:Label></td>
                        <td style="width: 10%">
                            <asp:Label ID="lblDispatcherEnglishName" runat="server" Text='<%#Eval("DispatcherEnglishName") %>'></asp:Label></td>
                        <td style="width: 5%;">
                            <asp:Label ID="lblIsEnabled" runat="server" Text='<%#Eval("IsEnabled") %>'></asp:Label></td>        
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
