<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AppDestinations.aspx.cs" Inherits="CarSystemTest.Car.AppDestinations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery-1.8.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#tblDestination :radio").each(function () {
                $(this).click(function () {
                    $("#tblDestination :radio").each(function () {
                        $(this).removeAttr("checked");
                    });
                    $(this).attr("checked", "checked");
                });
            });

            $(".tiptop a").click(function () {
                $(".tip").fadeOut(200);
                window.location.href = 'AppHistory.aspx';
            });
        });
    </script>
    <style>
        fieldset {
            width: 800px;
            border: 1px solid #ccc;
            height: auto;
            border-radius: 20px;
            font-size: 20px;
            margin-top: 10px;
        }
    </style>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首頁</a></li>
            <li><a href="#">目的地操作</a></li>
        </ul>
    </div>

    <div class="formbody">
        <%--<div class="formtitle"><span>表單信息</span></div>--%>

        <fieldset style="margin-left: 90px;">
            <legend style="font-size: 15px;">目的地信息</legend>
            <asp:Repeater ID="rptDestination" runat="server">
                <HeaderTemplate>
                    <table class="tablelist" id="tblDestination">
                        <tr>
                            <th>選擇</th>
                            <th>申請單號</th>
                            <th>區域</th>
                            <th>詳細地址</th>
                            <th>到達時間</th>
                            <th>離開時間</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbDestination" runat="server" GroupName="Destination" />
                        </td>
                        <td>
                            <%# Eval("VendorShortName") %>
                        </td>
                        <td><%# Eval("VendorCode") %>
                            <input id="txtVendorCode" type="text" style="display: none;" value='<%# Eval("VendorCode") %>' runat="server" />
                        </td>
                        <td>
                            <%# Eval("LinkMan") %>
                        </td>
                        <td>
                            <%# Eval("Tel") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </fieldset>

        <ul class="formcss">
            <li>
                <label>&nbsp;</label>
                <asp:Button ID="btnSave" runat="server" CssClass="btn" OnClientClick="return valid();" OnClick="btnSave_Click" Text="保存" /></li>
        </ul>
    </div>
    <div class="warning"></div>
    <div class="tip">
        <div class="tiptop"><span>提示信息</span><a></a></div>
        <div class="tipinfo">
            <span>
                <img src="../Images/ticon.png" /></span>
            <div class="tipright">
                <p>
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </p>
                <cite></cite>
            </div>
        </div>
    </div>
</asp:Content>
