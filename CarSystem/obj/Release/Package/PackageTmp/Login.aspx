<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CarSystemTest.Login" %>

<%--<!DOCTYPE html>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Styles/Login.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-1.8.1.min.js"></script>
    <title>Primax 車輛調度管理系統登錄</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnLogin").click(function () {
                //var $user = $("#account").val().trim();
                //var $pwd = $("#pwd").val().trim();
                var name = encodeURIComponent($("#account").val());
                var pwd = encodeURIComponent($("#pwd").val());
                if (name == "") {
                    alert("帳號不能為空，請重新輸入！");
                    $("#account").focus();
                    return;
                }
                if (pwd == "") {
                    alert("密碼不能為空，請重新輸入！");
                    $("#pwd").focus();
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "Login.aspx/UserLogin",
                    data: "{userAccount:'" + name + "',password:'" + pwd + "'}",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.d == null || data.d == "") {
                            window.location.href = "MainPage/Main.aspx";
                        }
                        else {
                            alert(data.d);
                        }
                    }
                });
            });

            $("#btnReset").click(function () {
                $("#account").val("").focus();
                $("#pwd").val("");
            });

            $(".tiptop a").click(function () {
                $(".tip").fadeOut(200);
            });
        });


    </script>
</head>
<body>
    <div class="content" id="divContent" runat="server">
        <div class="panel">
            <div class="group">
                <label>帳號</label>
                <input id="account" placeholder="請輸入帳號" />
            </div>
            <div class="group">
                <label>密碼</label>
                <input id="pwd" placeholder="請輸入密碼" type="password" />
            </div>
            <div class="login">
                <button id="btnLogin">登錄</button>
            </div>
            <div class="reset">
                <button id="btnReset">重置</button>
            </div>
        </div>
        <div class="show">
            <label>Copyright © 2016 PRIMAX ELECTRONICS LTD., All Rights Reserved</label>            
        </div>
        <div class="tip">
            <div class="tiptop"><span>提示信息</span><a></a></div>
            <div class="tipinfo">
                <span>
                    <img src="../Images/t03.png" /></span>
                <div class="tipright">
                    <p>
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </p>
                    <cite></cite>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
