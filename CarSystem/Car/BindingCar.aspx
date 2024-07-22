<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BindingCar.aspx.cs" Inherits="CarSystemTest.Car.BindingCar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery-1.8.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#tblVehicle :radio").each(function () {
                $(this).click(function () {
                    $("#tblVehicle :radio").each(function () {
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

        function valid() {
            if (!$("#tblVehicle :radio").is(":checked")) {
                alert("請選擇車輛!");
                return false;
            }
        }
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

        .suggestion {
            margin-left: 30px;
            margin-top: 3px;
            margin-bottom: 3px;
            font-size: 13px;
            font-family: "微软雅黑";
        }
    </style>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首頁</a></li>
            <li><a href="#">設置廠商</a></li>
        </ul>
    </div>

    <div class="formbody">
        <div class="formtitle"><span>表單信息</span></div>

        <fieldset style="margin-left: 90px;">
            <legend style="font-size: 15px;">車輛信息(非必選選項)</legend>
            <asp:Repeater ID="rptVehicle" runat="server">
                <HeaderTemplate>
                    <table class="tablelist" id="tblVehicle">
                        <tr>
                            <th>選擇</th>
                            <th>車牌號</th>
                            <th>司機</th>
                            <th>聯繫方式</th>
                            <th>車輛類型</th>
                            <th>容納人數</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbVehicle" runat="server" GroupName="Vendor" Checked='<%# SetSuggestVehicle(Eval("VehicleNO")) %>' />
                        </td>
                        <td>
                            <%# Eval("VehicleNO") %>
                            <input id="txtVehicleNO" type="text" style="display: none;" value='<%# Eval("VehicleNO") %>' runat="server" />
                        </td>
                        <td><%# Eval("UserChineseName") %>
                            
                        </td>
                        <td>
                            <%# Eval("Tel") %>
                        </td>
                        <td>
                            <%# Eval("VehicleTypeName") %>
                        </td>
                        <td>
                            <%# Eval("PermitNumber") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </fieldset>
        <fieldset style="margin-left: 90px;" runat="server" id="fldSugggestion">
            <legend style="font-size: 15px;">附註</legend>
            <asp:Label ID="lblSuggestion" runat="server" CssClass="suggestion"></asp:Label>
            <textarea class="textinput" id="remarkTxt" runat="server" style="margin-left: 30px; width: 90%; margin-top: 8px; margin-bottom: 8px;"></textarea>
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
