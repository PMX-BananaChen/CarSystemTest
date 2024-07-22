<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CarTracking.aspx.cs" Inherits="CarSystemTest.Gps.CarTracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首页</a></li>
            <li><a href="#">GPS</a></li>
        </ul>
    </div>
    <div class="formbody">
        <div class="formtitle">
            <span>車輛行駛軌跡</span>
            <div style="float: right;">
                <a href="~/AppHistory.aspx">
                    <input style="width: 50px; height: 25px; color: #fff; background: url(../images/btnbg.png);" type="button" value="返回" /></a>
            </div>
        </div>
        <div style="margin-left: 30px; margin-right: 30px; height: 500px;">
            <iframe id="frm" height="100%" width="100%" frameborder="0" runat="server"></iframe>
        </div>
    </div>
</asp:Content>
