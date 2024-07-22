<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BindingVendor.aspx.cs" Inherits="CarSystemTest.Car.BindingVendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery-1.8.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#MainContent_rbCarpool").change(function () {
                if ($("#MainContent_rbCarpool").is(":checked")) {
                    $("#fldCarpool").css("display", "");
                }
            });
            $("#MainContent_rbNotCarpool").change(function () {
                if (!$("#MainContent_rbCarpool").is(":checked")) {
                    $("#fldCarpool").css("display", "none");
                }
            });

            $("#tblVendor :radio").each(function () {
                $(this).click(function () {
                    $("#tblVendor :radio").each(function () {
                        $(this).removeAttr("checked");
                    });
                    $(this).attr("checked", "checked");
                });
            });

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
            if ($("#MainContent_lblVendor").text() == "1") {
                return confirm("已經設置過廠商，確定繼續設置？");
            }
            if ($("#MainContent_rbCarpool").is(":checked")) {
                if (!$("#tblApp :checkbox").is(":checked")) {
                    alert("請選擇拼車的申請單!");
                    return false;
                }
                else {
                    if ($("#tblApp input[type='checkbox']:checked").length > 2) {
                        alert("拼車總共不能超過三單!");
                        return false;
                    }
                }
            }
            if (!$("#tblVendor :radio").is(":checked")) {
                alert("請選擇廠商!");
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
        <%--<div class="formtitle"><span>表單信息</span></div>--%>
        <ul class="formcss">
            <li>
                <label>是否拼車:</label>
                <asp:RadioButton ID="rbCarpool" runat="server" GroupName="Carpool" />是
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbNotCarpool" runat="server" GroupName="Carpool" Checked="true" />否
                <asp:Label ID="lblVendor" runat="server" style="display:none"></asp:Label>
                <i></i></li>
        </ul>

        <fieldset style="margin-left: 90px; display: none" id="fldCarpool">
            <legend style="font-size: 15px;">拼車信息(未顯示本單,最多再選擇兩單)</legend>
            <asp:Repeater ID="rptCarpoolApp" runat="server">
                <HeaderTemplate>
                    <table class="tablelist" id="tblApp">
                        <tr>
                            <th>選擇</th>
                            <th>申請單號</th>
                            <th>申請人</th>
                            <th>車牌號</th>
                            <th>出行時間</th>
                            <th>目的地1</th>
                            <th>目的地2</th>
                            <th>目的地3</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbCarpoolApp" runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton runat="server"></asp:LinkButton><%# Eval("BusinessCode") %>
                            <input id="txtBusinessCode" type="text" style="display: none" runat="server" value='<%# Eval("BusinessCode") %>' />
                        </td>
                        <td>
                            <%# Eval("ApplicationUserName") %>
                        </td>
                        <td>
                            <%# Eval("SuggestVehicleNO") %>
                        </td>
                        <td>
                            <%# string.Format("{0:yyyy-MM-dd HH:mm:ss}",Eval("RequireStartTime")) %>
                        </td>
                        <td>
                            <%# Eval("AddressOne") %>
                        </td>
                        <td>
                            <%# Eval("AddressTwo") %>
                        </td>
                        <td>
                            <%# Eval("AddressThree") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </fieldset>

        <fieldset style="margin-left: 90px;">
            <legend style="font-size: 15px;">廠商信息</legend>
            <asp:Repeater ID="rptVendor" runat="server">
                <HeaderTemplate>
                    <table class="tablelist" id="tblVendor">
                        <tr>
                            <th>選擇</th>
                            <th>廠商簡稱</th>
                            <th>廠商代碼</th>
                            <th>負責人</th>
                            <th>聯繫方式</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbVendor" runat="server" GroupName="Vendor" Checked='<%# SetSuggestVendor(Eval("VendorCode")) %>' OnCheckedChanged="rbVendor_CheckedChanged" AutoPostBack="true" />
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
        <fieldset style="margin-left: 90px;">
            <legend style="font-size: 15px;">車輛信息</legend>
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
                        <td>
                            <%# Eval("UserChineseName") %>                            
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

        <%--  <div class="tipbtn">
        <input name="" type="button"  class="sure" value="确定" />&nbsp;
        <input name="" type="button"  class="cancel" value="取消" />
        </div>--%>
    </div>
</asp:Content>
