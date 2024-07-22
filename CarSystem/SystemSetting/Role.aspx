<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Role.aspx.cs" Inherits="CarSystemTest.SystemSetting.Role" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首页</a></li>
            <li><a href="#">角色管理</a></li>
        </ul>
    </div>
    <div class="formbody">
        <div class="formtitle"><span>角色</span></div>
        <ul class="seachform">
            <li>
                <asp:Button ID="btnAdd" runat="server" CssClass="scbtn" Text="新增" OnClick="btnAdd_Click" />
            </li>
        </ul>
        <div style="width: 100%">
            <asp:Repeater ID="rptRole" runat="server" OnItemCommand="rptRole_ItemCommand" >
                <HeaderTemplate>
                    <table class="tablelist" id="tblAppHistory">
                        <tr>
                            <th>角色代碼
                            </th>
                            <th>角色名稱
                            </th>
                            <th>備註
                            </th>
                            <th>操作
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 20%;">
                            <asp:Label ID="lblPrivilegeName" runat="server" Text='<%#Eval("RoleCode") %>'></asp:Label></td>
                        <td style="width: 20%;">
                            <asp:Label ID="lblEnglishDisplayName" runat="server" Text='<%#Eval("RoleName") %>'></asp:Label></td>
                        <td style="width: 30%;">
                            <asp:Label ID="lblResourceURL" runat="server" Text='<%#Eval("Remark")%>'></asp:Label></td>
                        <td style="width: 30%;">
                            <asp:ImageButton ID="ImgDetail"
                                ImageAlign="absmiddle" ToolTip="詳情" BorderStyle="None" ImageUrl="../Images/f03.png" PostBackUrl='<%# GetDetailURL(Eval("RoleCode")) %>'
                                Style="cursor: pointer;" runat="server" AlternateText="Detail" />詳情&nbsp;&nbsp;&nbsp;
                             <asp:ImageButton ID="ImgDelete"
                                ImageAlign="absmiddle" ToolTip="刪除" BorderStyle="None" ImageUrl="../Images/t03.png" CommandArgument='<%#Eval("RoleCode")%>'
                                Style="cursor: pointer;" runat="server" AlternateText="delete" CommandName="Delete" OnClientClick='return confirm("確定刪除該角色");' />刪除&nbsp;&nbsp;&nbsp;
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
