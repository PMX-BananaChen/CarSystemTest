<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UserRoleBind.aspx.cs" Inherits="CarSystemTest.SystemSetting.UserRoleBind" %>

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
        //function typeChange() {
        //    var $type = $("#sltType option:selected").val();
        //    if ($type == "0") {
        //        $("#divRole").css("display", "none");
        //        $("#divPrivilege").css("display", "");
        //    }
        //    else {
        //        $("#divPrivilege").css("display", "none");
        //        $("#divRole").css("display", "");
        //    }
        //}
        $(document).ready(function () {
            setSeletedRoles();
            setSeletedPrivileges
        });

        function addAllRole() {
            var $sltValue;
            var $sltText;
            var $option;
            // var $values = "";
            $("#MainContent_sltRoles option").each(function () {
                //   $values = $values + $(this).val() + ",";
                $sltText = $(this).text();
                $sltValue = $(this).val();
                $option = $("<option>").val($sltValue).text($sltText);
                $("#MainContent_sltedRoles").append($option);
            });
            $("#MainContent_sltRoles option").remove();
            //  $("#MainContent_lblPriviles").val($values);
            setSeletedRoles();
        }

        function addRole() {
            var $sltValue;
            var $sltText;
            var $option;
            // var $values = "";
            $("#MainContent_sltRoles option:selected").each(function () {
                //$values = $values + $(this).val() + ",";
                $sltText = $(this).text();
                $sltValue = $(this).val();
                $option = $("<option>").val($sltValue).text($sltText);
                $("#MainContent_sltedRoles").append($option);
            });
            $("#MainContent_sltRoles option:selected").remove();
            // $("#MainContent_tbRoles").val($values);
            setSeletedRoles();
        }

        function clearRole() {
            var $sltValue;
            var $sltText;
            var $option;
            // var $values = "";
            $("#MainContent_sltedRoles option:selected").each(function () {
                $sltText = $(this).text();
                $sltValue = $(this).val();
                $option = $("<option>").val($sltValue).text($sltText);
                $("#MainContent_sltRoles").append($option);
            });
            $("#MainContent_sltedRoles option:selected").remove();
            //$("#MainContent_sltedRoles option").each(function () {
            //    $values = $values + $(this).val() + ",";
            //});
            //$("#MainContent_tbRoles").val($values);
            setSeletedRoles();
        }

        function clearAllRole() {
            var $sltValue;
            var $sltText;
            var $option;
            $("#MainContent_sltedRoles option").each(function () {
                $sltText = $(this).text();
                $sltValue = $(this).val();
                $option = $("<option>").val($sltValue).text($sltText);
                $("#MainContent_sltRoles").append($option);
            });
            $("#MainContent_sltedRoles option").remove();
            setSeletedRoles();
        }

        function setSeletedRoles() {
            var $values = "";
            $("#MainContent_sltedRoles option").each(function () {
                $values = $values + $(this).val() + ",";
            });
            $("#MainContent_tbRoles").val($values);
        }

        function addAllPrivilege() {
            var $sltValue;
            var $sltText;
            var $option;
            // var $values = "";
            $("#MainContent_sltPrivileges option").each(function () {
                //$values = $values + $(this).val() + ",";
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
            //var $values = "";
            $("#MainContent_sltPrivileges option:selected").each(function () {
                // $values = $values + $(this).val() + ",";
                $sltText = $(this).text();
                $sltValue = $(this).val();
                $option = $("<option>").val($sltValue).text($sltText);
                $("#MainContent_sltedPrivileges").append($option);
            });
            $("#MainContent_sltPrivileges option:selected").remove();
            //$("#MainContent_tbPriviles").val($values);
            setSeletedPrivileges();
        }

        function clearPrivilege() {
            var $sltValue;
            var $sltText;
            var $option;
            // var $values = "";
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
            //$("#MainContent_tbPriviles").val($values);
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
            <li><a href="#">用戶</a></li>
        </ul>
    </div>

    <div class="formbody">
        <div class="formtitle"><span>基本信息</span></div>
        <ul class="forminfo">
            <li>
                <label>用戶帳號</label><asp:TextBox ID="tbAccount" runat="server" CssClass="dfinput"></asp:TextBox><i>标题不能超过10个字符</i></li>
            <li>
                <label>中文名稱</label><asp:TextBox ID="tbChineseName" runat="server" CssClass="dfinput"></asp:TextBox></li>
            <li>
                <label>英文名稱</label><asp:TextBox ID="tbEnglishName" runat="server" CssClass="dfinput"></asp:TextBox></li>
            <li>
                <label>所屬角色</label>
                <%--    <select id="sltType" onchange="typeChange();">
                    <option value="1" selected="selected">角色</option>
                    <option value="0">權限</option>
                </select>--%>
            </li>
            <li>
                <div class="vocation" id="divRole">
                    <table class="tbl">
                        <tr>
                            <th>可選的角色
                            </th>
                            <th>&nbsp;
                            </th>
                            <th>選中的角色
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <select id="sltRoles" name="sltRoles" size="15" runat="server" multiple="true">
                                </select>
                            </td>
                            <td><a href="#" onclick="addAllRole();">
                                <img src="../images/arrow_03.gif" width="16" height="16" border="0" /></a><br />
                                <a href="#" onclick="addRole();">
                                    <img src="../images/arrow_04.gif" width="16" height="16" border="0" /></a><br />
                                <a href="#" onclick="clearRole();">
                                    <img src="../images/arrow_05.gif" width="16" height="16" border="0" /></a><br />
                                <a href="#" onclick="clearAllRole();">
                                    <img src="../images/arrow_06.gif" width="16" height="16" border="0" /></a>
                            </td>
                            <td>
                                <select id="sltedRoles" name="sltedRoles" size="15" runat="server" multiple="true">
                                </select>
                                <asp:TextBox ID="tbRoles" runat="server" Style="display: none"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </li>
            <li>
                <label>擁有權限</label></li>
            <li>
                <div class="vocation" id="divPrivilege">
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
