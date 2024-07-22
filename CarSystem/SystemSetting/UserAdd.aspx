<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs" Inherits="CarSystemTest.SystemSetting.UserAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" />
    <link href="../Styles/select.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <script src="../Scripts/select-ui.min.js"></script>
    <script src="../Scripts/DateUtil.js" type="text/javascript"></script>
    <script src="../Scripts/My97DatePicker/WdatePicker.js"></script>
    <script src="../Scripts/Popcalendar.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function (e) {
            $(".select1").uedSelect({
                width: 345
            });
            $(".select2").uedSelect({
                width: 180
            });
            $(".select3").uedSelect({
                width: 200
            });
        })
    </script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首頁</a></li>
            <li><a href="#">用戶</a></li>
        </ul>
    </div>

    <div class="formbody">
        <div class="formtitle"><span>基本信息</span></div>
        <table class="tablelist" style="border: 1px solid #ccc">
            <tr>
                <td align="center">
                    <label>用戶帳號</label>
                </td>
                <td>
                    <asp:TextBox ID="tbAccount" runat="server" CssClass="dfinput"></asp:TextBox>
                </td>
                <td align="center">
                    <label>帳號密碼</label>
                </td>
                <td>
                    <asp:TextBox ID="tbPwd" runat="server" TextMode="Password" CssClass="dfinput"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <label>用戶名稱</label>
                </td>
                <td>
                    <asp:TextBox ID="tbChineseName" runat="server" CssClass="dfinput"></asp:TextBox>
                </td>
                <td align="center">
                    <label>英文名稱</label>
                </td>
                <td>
                    <asp:TextBox ID="tbEnglishName" runat="server" CssClass="dfinput"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <label>手機號</label>
                </td>
                <td>
                    <asp:TextBox ID="tbTel" runat="server" CssClass="dfinput"></asp:TextBox>
                </td>
                <td align="center">
                    <label>電話</label>
                </td>
                <td>
                    <asp:TextBox ID="tbExt" runat="server" CssClass="dfinput"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <label>郵箱</label>
                </td>
                <td>
                    <asp:TextBox ID="tbMail" runat="server" CssClass="dfinput"></asp:TextBox>
                </td>
                <td align="center">
                    <label>性別</label>
                </td>
                <td>
                    <asp:RadioButton ID="rbMale" runat="server" GroupName="sex" Checked="true" />男
                    <asp:RadioButton ID="rbFemale" runat="server" GroupName="sex" />女
                </td>
            </tr>
            <tr>
                <td align="center">
                    <label>用戶類型</label>
                </td>
                <td>
                    <div class="vocation" style="margin-left: 12px;">
                        <asp:DropDownList ID="ddlUserType" runat="server" OnSelectedIndexChanged="UserType_SelectedIndexChanged" AutoPostBack="true" CssClass="select3"></asp:DropDownList>
                    </div>
                </td>
                <td align="center">
                    <label>用戶級別</label>
                </td>
                <td>
                    <div class="vocation" style="margin-left: 12px;">
                        <asp:DropDownList ID="ddlUserGrade" runat="server" CssClass="select3"></asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <label>員工工號</label>
                </td>
                <td>
                    <asp:TextBox ID="tbEmpNo" runat="server" CssClass="dfinput"></asp:TextBox>
                </td>
                <td align="center">
                    <label>所屬部門或廠商</label>
                </td>
                <td>
                    <div class="vocation" style="margin-left: 12px;">
                        <asp:DropDownList ID="ddlEmpVendor" runat="server" CssClass="select3"></asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <label>生效時間</label>
                </td>
                <td>
                    <input id="txtEffectiveTime" runat="server" class="dfinput" type="text" onfocus="WdatePicker({ skin: 'twoer', dateFmt: 'yyyy-MM-dd', })" style="width: 200px;" />

                </td>
                <td align="center">
                    <label>失效時間</label>
                </td>
                <td>
                    <input id="txtExpireTime" runat="server" class="dfinput" type="text" onfocus="WdatePicker({ skin: 'twoer', dateFmt: 'yyyy-MM-dd', })" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <label>帳號狀態</label>
                </td>
                <td>
                    <div class="vocation" style="margin-left: 12px;">
                        <asp:DropDownList ID="ddlState" runat="server" CssClass="select3">
                            <asp:ListItem Text="啟用" Value="1" Selected="True">
                            </asp:ListItem>
                            <asp:ListItem Text="禁用" Value="0">
                            </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td align="center">
                    <label>備註</label>
                </td>
                <td>
                    <asp:TextBox ID="tbRemark" runat="server" MaxLength="255" CssClass="dfinput"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label></label>
                </td>
                <td></td>
                <td>
                    <asp:Button ID="btnSave" runat="server" CssClass="btn" OnClick="btnSave_Click" Text="確認保存" />
                </td>
                <td></td>
            </tr>
        </table>
    </div>
</asp:Content>
