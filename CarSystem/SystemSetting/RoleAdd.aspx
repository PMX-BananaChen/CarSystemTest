<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RoleAdd.aspx.cs" Inherits="CarSystemTest.SystemSetting.RoleAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/jquery-1.8.1.min.js"></script>
    <style type="text/css">
        .tbl {
            border: solid 1px #cbcbcb;
            width: 100%;
            font-family: '微软雅黑';
        }

            .tbl th {
                border: solid 1px #c7c7c7;
                height: 25px;
                line-height: 25px;
                text-align: left;
                font-weight: bolder;
                font-size: 14px;
            }

            .tbl td {
                border: solid 1px #c7c7c7;
                font-size: 18px;
            }

                .tbl td select {
                    border: solid 1px #c7c7c7;
                    width: 300px;
                }
    </style>
    <script type="text/javascript">
        function addAllPrivilege() {
            var $sltValue;
            var $sltText;
            var $option;
           // var $values = "";
            $("#MainContent_sltPrivileges option").each(function () {
               // $values = $values + $(this).val() + ",";
                $sltText = $(this).text();
                $sltValue = $(this).val();
                $option = $("<option>").val($sltValue).text($sltText);
                $("#MainContent_sltedPrivileges").append($option);
            });
            $("#MainContent_sltPrivileges option").remove();
            // $("#MainContent_tbPriviles").val($values);
            setSeletedPrivileges();
        }

        function addPrivilege() {
            var $sltValue;
            var $sltText;
            var $option;
           // var $values = "";
            $("#MainContent_sltPrivileges option:selected").each(function () {
               // $values = $values + $(this).val() + ",";
                $sltText = $(this).text();
                $sltValue = $(this).val();
                $option = $("<option>").val($sltValue).text($sltText);
                $("#MainContent_sltedPrivileges").append($option);
            });
            $("#MainContent_sltPrivileges option:selected").remove();
            // $("#MainContent_tbPriviles").val($values);
            setSeletedPrivileges();
        }

        function clearPrivilege() {
            var $sltValue;
            var $sltText;
            var $option;
            //var $values = "";
            $("#MainContent_sltedPrivileges option:selected").each(function () {
                $sltText = $(this).text();
                $sltValue = $(this).val();
                $option = $("<option>").val($sltValue).text($sltText);
                $("#MainContent_sltPrivileges").append($option);
            });
            $("#MainContent_sltedPrivileges option:selected").remove();
            //$("#MainContent_sltedPrivileges option").each(function () {
            //    $values = $values + $(this).val() + ",";
            //});
            // $("#MainContent_tbPriviles").val($values);
            setSeletedPrivileges();
        }

        function clearAllPrivilege() {            
            var $sltValue;
            var $sltText;
            var $option;
            $("#MainContent_sltedPrivileges option").each(function () {
                $sltText = $(this).text();
                $sltValue = $(this).val();
                $option = $("<option>").val($sltValue).text($sltText);
                $("#MainContent_sltPrivileges").append($option);
            });
            $("#MainContent_sltedPrivileges option").remove();
            setSeletedPrivileges();
        }
        function setSeletedPrivileges() {
            var $values = "";
            $("#MainContent_sltedPrivileges option").each(function () {
                $values = $values + $(this).val() + ",";
            });
             $("#MainContent_tbPriviles").val($values);
        }
    </script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首頁</a></li>
            <li><a href="#">角色</a></li>
        </ul>
    </div>

    <div class="formbody">
        <div class="formtitle"><span>基本信息</span></div>
        <ul class="forminfo">
            <li>
                <label>角色名稱</label><asp:TextBox ID="tbRole" runat="server" CssClass="dfinput"></asp:TextBox><i>标题不能超过10个字符</i></li>
            <li>
                <label>角色描述</label><asp:TextBox ID="tbRoleMark" runat="server" CssClass="dfinput"></asp:TextBox></li>
            <li>
                <div class="vocation">
                    <table class="tbl">
                        <tr>
                            <th>可選的權限
                            </th>
                            <th>&nbsp;
                            </th>
                            <th>選中的權限
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <select id="sltPrivileges" name="sltPrivileges" size="15" runat="server" multiple="true">
                                </select>
                            </td>
                            <td><a href="#" onclick="addAllPrivilege();">
                                <img src="../images/arrow_03.gif" width="16" height="16" border="0" /></a><br />
                                <a href="#" onclick="addPrivilege();">
                                    <img src="../images/arrow_04.gif" width="16" height="16" border="0" /></a><br />
                                <a href="#" onclick="clearPrivilege();">
                                    <img src="../images/arrow_05.gif" width="16" height="16" border="0" /></a><br />
                                <a href="#" onclick="clearAllPrivilege();">
                                    <img src="../images/arrow_06.gif" width="16" height="16" border="0" /></a>
                            </td>
                            <td>
                                <select id="sltedPrivileges" name="sltedPrivileges" runat="server" size="15" multiple="true">
                                </select>
                                <asp:TextBox ID="tbPriviles" runat="server" Style="display: none"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </li>
            <li>
                <label>&nbsp;</label><asp:Button ID="btnSave" runat="server" CssClass="btn" OnClick="btnSave_Click" Text="確認保存" /></li>
        </ul>
    </div>
</asp:Content>
