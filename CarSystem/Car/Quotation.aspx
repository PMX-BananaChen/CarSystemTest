<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Quotation.aspx.cs" Inherits="CarSystemTest.Car.Quotation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">  

    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/select.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/My97DatePicker/WdatePicker.js"></script>
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <script src="../Scripts/select-ui.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //FixTable("tblQuotationData", 1, 1000, 200);
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
            <span>車輛里程報價</span>
            <div style="float: right;">
                <asp:Button ID="btnExcel" runat="server" Width="70px" Height="25px" BackColor="#fff" Text="導出EXCEL" OnClick="btnExcel_Click" />
                <%--<input style="width: 70px; height: 25px; color: #fff; background: url(../images/btnbg.png);" type="button" runat="server" value="導出EXCEL" onclick="EXCEL" />--%>
            </div>
        </div>
        <div style="margin-left: 50px; margin-bottom: 8px;">
            <table style="width: 800px; border-spacing: 10px; padding: 10px;">
                <tr>       
                    <td>車輛類型</td>
                    <td>
                        <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="select3">       
                            <asp:ListItem Text="" Value=""></asp:ListItem>  
                           <asp:ListItem Text="小車" Value="1"></asp:ListItem>       
                           <asp:ListItem Text="商務車" Value="2"></asp:ListItem>                       
                        </asp:DropDownList>
                    </td>
                    <td>出發地</td>
                    <td>
                        <asp:DropDownList ID="ddlDeparture" runat="server" CssClass="select3">  
                           <asp:ListItem Text="" Value=""></asp:ListItem>  
                           <asp:ListItem Text="石碣" Value="10001"></asp:ListItem>       
                           <asp:ListItem Text="永川區工廠" Value="50001"></asp:ListItem>                               
                            <asp:ListItem Value="30001">昆山市玉山镇</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>目的地</td>
                    <td>
                        <asp:DropDownList ID="ddlDestination" runat="server" CssClass="select3">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnSeach" runat="server" CssClass="scbtn" Text="查詢" OnClick="btnSeach_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-left: 20px;">
            <asp:Repeater ID="rptQuotationData" runat="server">
                <HeaderTemplate>
                    <table id="tblQuotationData" class="tablelist">
                        <thead>
                            <tr>
                                <th>車輛類型
                                </th>
                                <th>出發地
                                </th>
                                <th>目的地
                                </th>
                                <th>基本費用
                                </th>
                                <th>高速費用
                                </th>                
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 20%;">
                            <asp:Label ID="lblVehicleTypeName" runat="server" Text='<%#Eval("VehicleTypeName") %>'></asp:Label></td>
                        <td style="width: 20%;">
                            <asp:Label ID="lblFromRegionName" runat="server" Text='<%#Eval("FromRegionName") %>'></asp:Label></td>
                        <td style="width: 30%;">
                            <asp:Label ID="lblToRegionName" runat="server" Text='<%#Eval("ToRegionName") %>'></asp:Label></td>
                        <td style="width: 15%; text-align: left;">
                            <asp:Label ID="lblFee" runat="server" Text='<%#Eval("Fee") %>'></asp:Label></td>
                        <td style="width: 15%">
                            <asp:Label ID="lblTollFee" runat="server" Text='<%#Eval("TollFee") %>'></asp:Label></td>          
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
