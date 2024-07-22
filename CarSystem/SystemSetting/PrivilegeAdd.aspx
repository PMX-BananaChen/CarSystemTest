<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrivilegeAdd.aspx.cs" Inherits="CarSystemTest.SystemSetting.PrivilegeAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Styles/select.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <script src="../Scripts/select-ui.min.js"></script>
    <script>
        $(document).ready(function (e) {
            $(".select1").uedSelect({
                width: 345
            });
            $(".select2").uedSelect({
                width: 167
            });
            $(".select3").uedSelect({
                width: 100
            });
        });
    </script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首頁</a></li>
            <li><a href="#">權限</a></li>
        </ul>
    </div>

    <div class="formbody">
        <div class="formtitle"><span>基本信息</span></div>
        <ul class="forminfo">
            <li>
                <label>權限名稱</label><asp:TextBox ID="tbPrivilege" runat="server" CssClass="dfinput"></asp:TextBox><i>标题不能超过30个字符</i></li>
            <li>
                <label>中文名稱</label><asp:TextBox ID="tbChineseName" runat="server" CssClass="dfinput"></asp:TextBox></li>
            <li>
                <label>英文名稱</label><asp:TextBox ID="tbEnglishName" runat="server" CssClass="dfinput"></asp:TextBox></li>
            <li>
                <label>界面URL</label><asp:TextBox ID="tbURL" runat="server" CssClass="dfinput"></asp:TextBox></li>
            <li>
                <label>父級權限</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlPrivilege" runat="server" CssClass="select2">
                    </asp:DropDownList>
                </div>
                <i>該權限為第一級,請選擇"無父級權限"</i>
            </li>
            <li>
                <label>顯示優先級</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlPriority" runat="server" CssClass="select2">
                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <i>數字大的優先級高</i></li>
            <li>
                <label>菜單是否可見</label><cite><asp:RadioButton ID="rbShow" GroupName="ShowMenu" runat="server" Checked="true" />是&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbNotShow" GroupName="ShowMenu" runat="server" />否</cite></li>
            <li>
                <label>管理員是否可見</label><cite><asp:RadioButton ID="rbSee" GroupName="AdminSee" runat="server" Checked="true" />是&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbNotSee" GroupName="AdminSee" runat="server" />否</cite></li>
            <li>
                <label>&nbsp;</label><asp:Button ID="btnSave" runat="server" CssClass="btn" OnClick="btnSave_Click" Text="確認保存" /></li>
        </ul>
    </div>
</asp:Content>
