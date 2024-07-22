<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="CarSystemTest.SystemSetting.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/select.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/My97DatePicker/WdatePicker.js"></script>
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <script src="../Scripts/select-ui.min.js"></script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首页</a></li>
            <li><a href="#">用戶管理</a></li>
        </ul>
    </div>
    <script type="text/javascript">
        $(document).ready(function (e) {
            $(".select1").uedSelect({
                width: 345
            });
            $(".select2").uedSelect({
                width: 80
            });
            $(".select3").uedSelect({
                width: 150
            });
        });
    </script>
    <div class="formbody">
        <div class="formtitle"><span>用戶</span></div>
        <ul class="seachform">
            <li>
                <label>英文名稱</label>
                <asp:TextBox ID="txtEnglishName" runat="server" Width="80" CssClass="dfinput"></asp:TextBox>
            </li>
            <li>
                <label>賬號狀態</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlEnabled" runat="server" CssClass="select2">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="啟用" Value="1"></asp:ListItem>
                        <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <label>區域</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="select2">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="DG" Value="DG"></asp:ListItem>
                        <asp:ListItem Text="CQ" Value="CQ"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <label>用戶類型</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="select2">
                    </asp:DropDownList>
                </div>
            </li>
            <li>                
                <asp:Button ID="btnSeach" runat="server" CssClass="scbtn" Text="查詢" OnClick="btnSeach_Click" />
                <label>&nbsp;</label>
                <asp:Button ID="btnAdd" runat="server" CssClass="scbtn" Text="新增" OnClick="btnAdd_Click" />
            </li>
        </ul>
        <div style="width: 100%">
            <asp:Repeater ID="rptUser" runat="server" OnItemCommand="rptUser_ItemCommand">
                <HeaderTemplate>
                    <table class="tablelist" id="tblAppHistory">
                        <tr>
                            <th>用戶賬號
                            </th>
                            <th>姓名
                            </th>
                            <th>英文名
                            </th>
                            <th>用戶類型
                            </th>
                            <th>賬號狀態
                            </th>
                            <th>區域
                            </th>
                            <th>操作
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 15%;">
                            <asp:Label ID="lblUserAccount" runat="server" Text='<%#Eval("UserAccount") %>'></asp:Label></td>
                        <td style="width: 15%;">
                            <asp:Label ID="lblUserChineseName" runat="server" Text='<%#Eval("UserChineseName") %>'></asp:Label></td>
                        <td style="width: 15%;">
                            <asp:Label ID="lblUserEnglishName" runat="server" Text='<%#Eval("UserEnglishName") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblUserType" runat="server" Text='<%#Eval("UserType") %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblIsEnabled" runat="server" Text='<%#Eval("IsEnabled") %>'></asp:Label></td>
                        <td style="width: 5%;">
                            <asp:Label ID="lblArea" runat="server" Text='<%#Eval("Area") %>'></asp:Label></td>
                        <td style="width: 30%;">
                            <asp:ImageButton ID="ImgDetail"
                                ImageAlign="absmiddle" ToolTip="詳情" BorderStyle="None" ImageUrl="../Images/f03.png" PostBackUrl='<%# GetDetailURL(Eval("UserID")) %>'
                                Style="cursor: pointer;" runat="server" AlternateText="Detail" />詳情&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="ImageButton1"
                                 ImageAlign="absmiddle" ToolTip="設置角色" BorderStyle="None" ImageUrl="../Images/t05.png" PostBackUrl='<%# GetRoleBindURL(Eval("UserID")) %>'
                                 Style="cursor: pointer;" runat="server" AlternateText="BindRole" />設置角色&nbsp;&nbsp;&nbsp;
                             <asp:ImageButton ID="ImgDelete"
                                 ImageAlign="absmiddle" ToolTip="刪除" BorderStyle="None" ImageUrl="../Images/t03.png" CommandArgument='<%#Eval("UserID")%>'
                                 Style="cursor: pointer;" runat="server" AlternateText="delete" CommandName="Delete" OnClientClick='return confirm("確定刪除該用戶");' />刪除&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
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
