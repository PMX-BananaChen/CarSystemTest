<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CurrentPosition.aspx.cs" Inherits="CarSystemTest.GPS.CurrentPosition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/select.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <script src="../Scripts/select-ui.min.js"></script>
    <script>
        $(document).ready(function (e) {
            $(".select1").uedSelect({
                width: 250
            });
            $(".select2").uedSelect({
                width: 80
            });
            $(".select3").uedSelect({
                width: 150
            });
        });

        function changeURL() {
            var $ddlVehicleVal = $("#MainContent_ddlVehicle option:selected").val();
            $ddlVehicleVal = $ddlVehicleVal.substr(1, 6);
            var $url = "http://218.244.129.243:89/Interface/findPosition.action?carNum=" + $ddlVehicleVal + "&u=gbk";
            $("#ifrm").attr("src", $url);
        }
    </script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首页</a></li>
            <li><a href="#">GPS</a></li>
        </ul>
    </div>
    <div class="formbody">
        <div class="formtitle"><span>車輛當前位置</span></div>
        <ul class="seachform">
            <li>
                <label>所屬廠商</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlVendor" runat="server" CssClass="select3" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <label>所屬車輛</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlVehicle" runat="server" CssClass="select1">
                    </asp:DropDownList>
                </div>

            </li>
            <%--  <li>
                <label>流程狀態</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlFlowStatus" runat="server" CssClass="select3">
                    </asp:DropDownList>
                </div>
            </li>--%>
            <li>
                <label>&nbsp;</label>
                <input id="btnSearch" class="scbtn" type="button" value="查詢" onclick="changeURL();" />
                <%--<asp:Button ID="btnSeach" runat="server" CssClass="scbtn" Text="查詢" OnClick="btnSeach_Click" />--%>
            </li>
        </ul>
        <div style="margin-left: 30px; margin-right: 30px; height: 450px;">
            <iframe id="ifrm" height="100%" width="100%" frameborder="0"></iframe>
        </div>
    </div>
</asp:Content>
