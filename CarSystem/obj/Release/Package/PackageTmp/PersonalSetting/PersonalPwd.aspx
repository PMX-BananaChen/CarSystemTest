<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PersonalPwd.aspx.cs" Inherits="CarSystemTest.PersonalSetting.PersonalPwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <script>
        function check() {
            var $pwd = $("#MainContent_tbPwd").val().trim();
            var $confirmPwd = $("#MainContent_tbConfirmPwd").val().trim();
            if ($pwd == "") {
                alert("密碼欄位不能為空！");
                $("#MainContent_tbPwd").focus();
                return false;
            }
            if ($confirmPwd == "") {
                alert("確認密碼欄位不能為空！");
                $("#MainContent_tbConfirmPwd").focus();
                return false;
            }
            if ($pwd != $confirmPwd) {
                alert("密碼欄位與確認密碼的數據不一致！");
                $("#MainContent_tbConfirmPwd").focus();
                return false;
            }
        }
    </script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首頁</a></li>
            <li><a href="#">密碼修改</a></li>
        </ul>
    </div>
    <div class="formbody">
        <div class="formtitle"><span>帳戶信息</span></div>
        <ul class="forminfo">
            <li>
                <label>用戶帳戶</label><asp:Label ID="lblAccount" runat="server"></asp:Label></li>
            <li>
                <label>密碼</label><asp:TextBox ID="tbPwd" runat="server" CssClass="dfinput" TextMode="Password" MaxLength="10"></asp:TextBox><i>密碼長度最長為10位</i></li>
            <li>
                <label>確認密碼</label><asp:TextBox ID="tbConfirmPwd" runat="server" CssClass="dfinput" TextMode="Password" MaxLength="10"></asp:TextBox><i>請與密碼欄位的數據保持一致</i></li>
            <li></li>
            <li>
                <label>&nbsp;</label><asp:Button ID="btnSave" runat="server" CssClass="btn" OnClick="btnSave_Click" Text="確認保存" OnClientClick="return check();" /></li>
        </ul>
    </div>
</asp:Content>
