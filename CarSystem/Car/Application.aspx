<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Application.aspx.cs" Inherits="CarSystemTest.Car.Application" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/select.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.1.min.js"></script>

    <%--<script type="text/javascript" src="../Scripts/jquery.idTabs.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/select-ui.min.js"></script>

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
    <script type="text/javascript">
        $(document).ready(function (e) {
            $(".select1").uedSelect({
                width: 345
            });
            $(".select2").uedSelect({
                width: 180
            });
            $(".select3").uedSelect({
                width: 150
            });
            //$(".tablelist tbody tr:odd").addClass("odd");
            $(".tiptop a").click(function () {
                $(".tip").fadeOut(200);
                window.location.href = window.location.href;
            });

            $("#MainContent_rbNormal").change(function () {
                $("#MainContent_txtTime").val("");
            });
            $("#MainContent_rbSecond").change(function () {
                $("#MainContent_txtTime").val("");
            });
        });

        function confirmDel() {
            return confirm("確定刪除該條數據？");
        }

        function valid() {
            //出行時間不能為空
            var $requireTime = $("#MainContent_txtTime").val();
            if ($requireTime == "") {
                alert("出行時間不能為空！");
                return false;
            }
            //出發地詳細地址不能為空
            var $departureDetail = $("#MainContent_tbDepartureDetail").val().trim();
            if ($departureDetail == "") {
                alert("出發地詳細地址不能為空！");
                return false;
            }

            //乘客不能為空
            var $passenger = $("#tblPassenger");
            if ($passenger.find("tr").length <= 1) {
                alert("乘客不能為空！");
                return false;
            }
            var flag = true;
            $passenger.find("tr:gt(0)").each(function () {
                $passengerName = $(this).find("td:first").children(":first").text();
                if ($passengerName == "") {
                    alert("乘客姓名不能為空！");
                    flag = false;
                }
            });
            if (!flag) {
                return flag;
            }

            //目的地不能為空
            var $destination = $("#tblDestination");
            if ($destination.find("tr").length <= 1) {
                alert("目的地不能為空！");
                return false;
            }
            $destination.find("tr:gt(0)").each(function () {
                $desDetail = $(this).find("td:eq(1)").children(":eq(0)").val();
                if ($desDetail == "") {
                    alert("目的地詳細地址不能為空！");
                    flag = false;
                }
            });
            if (!false) {
                return flag;
            }
            return true;
        }

        function checkPassenger() {
            var $passengerName = $("#MainContent_tbName").val().trim();
            if ($passengerName == "") {
                alert("乘客姓名不能為空！");
                return false;
            }
            var $phoneNum = $("#MainContent_tbMobile").val().trim();
            if ($phoneNum == "") {
                alert("手機號碼不能為空！");
                return false;
            }
            else {
                var reg = /^1[3|4|5|7|8|9][0-9]{9}$/;
                if (!reg.test($phoneNum))
                {
                    alert("手機號碼輸入不正確！");
                    return false;
                }
            }
            var flag = true;
            $("#tblPassenger tr:gt(0)").each(function () {
                if ($(this).find("td:first").children(":first").text() == $passengerName) {
                    alert("該乘客已添加！");
                    flag = false;
                }
            });
            return flag;
        }

        function setRequireTime() {
            if ($("#MainContent_rbNormal").is(":checked")) {
                WdatePicker({ skin: 'twoer', dateFmt: 'yyyy-MM-dd HH:mm:00', minDate: '%y-%M-%d %H:%m' });
            }
            else {
                WdatePicker({ skin: 'twoer', dateFmt: 'yyyy-MM-dd HH:mm:00', maxDate: '%y-%M-%d %H:%m' });
            }
        }
    </script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首頁</a></li>
            <li><a href="#">助理申請</a></li>
        </ul>
    </div>

    <div class="formbody">
        <div class="formtitle">
            <span>表單信息</span><div style="float: right;">
                <a href="AppHistory.aspx">
                    <input style="width: 50px; height: 25px; color: #fff; background: url(../images/btnbg.png);" type="button" value="返回" /></a>
            </div>
        </div>
        <ul class="formcss">
            <li>
                <label>申請單單號：</label><asp:Label ID="lblBusinessCode" runat="server" Width="50px"></asp:Label><i></i></li>
            <li>
                <label>申請單類型：</label><asp:RadioButton ID="rbNormal" runat="server" GroupName="AppType" Checked="true" />正常單
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbSecond" runat="server" GroupName="AppType" />補單<i></i></li>
            <%-- <li>
                <label>單程/往返：</label><asp:RadioButton ID="rbSingle" runat="server" GroupName="OneWay" Checked="true" />單程&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="rbDouble" runat="server" GroupName="OneWay" />往返</li>--%>
            <li>
                <label>建議車輛：</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlVehicle" CssClass="select2" runat="server">
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <label>出行時間：</label>
                <input id="txtTime" runat="server" class="dfinput" type="text" onfocus="setRequireTime();" />
                <%--<input type="datetime-local" id="txtTime" class="dfinput" runat="server" />--%>
                <%--<asp:TextBox ID="tbTime" CssClass="dfinput" runat="server"></asp:TextBox>--%>
            </li>
            <li>
                <label>出發地：</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlDeparture" CssClass="select3" runat="server">
                    </asp:DropDownList>
                </div>
                <i></i></li>
            <li>
                <label>出發地詳細地址：</label>
                <asp:TextBox ID="tbDepartureDetail" CssClass="dfinput" runat="server"></asp:TextBox>
                <i></i></li>
            <li>
                <label>乘客人屬性：</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlAttribute" CssClass="select3" runat="server"></asp:DropDownList>
                </div>
                <i></i></li>
            <li>
                <label>是否乘客自付：</label>
                <asp:RadioButton ID="rbPay" runat="server" GroupName="PayMoney" />是&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rbNoPay" runat="server" GroupName="PayMoney" Checked="true" />否
                <i></i></li>
            <li>
                <label>派車用途：</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlPurpose" CssClass="select3" runat="server">
                    </asp:DropDownList>
                </div>
                <i></i></li>
            <%--            <li>
                <label>備註：</label>
                <asp:TextBox ID="tbRemark" CssClass="dfinput" runat="server"></asp:TextBox><i></i></li>--%>
            <li>
                <label>成本中心：</label>
                <div class="vocation">
                    <asp:DropDownList ID="ddlCostCenter" CssClass="select3" runat="server">
                    </asp:DropDownList>
                </div>
                <i></i></li>
        </ul>

        <fieldset style="margin-left: 90px;">
            <legend style="font-size: 15px;">乘客信息</legend>
            <table style="margin-left: 30px; width: 90%;">
                <tr>
                    <td>
                        <label>姓名：</label><asp:TextBox ID="tbName" runat="server" CssClass="dfinput" Width="80px"></asp:TextBox></td>
                    <td>
                        <label>工號：</label><asp:TextBox ID="tbEmpNO" runat="server" CssClass="dfinput" Width="80px"></asp:TextBox></td>
                    <td>
                        <label>手機號：</label><asp:TextBox ID="tbMobile" runat="server" CssClass="dfinput" Width="100px"></asp:TextBox></td>
                    <td>
                        <label>備註：</label><asp:TextBox ID="tbRemark" runat="server" CssClass="dfinput" Width="120px"></asp:TextBox></td>
                    <td>
                        <%--<asp:Button ID="btnNewPassenger" runat="server" OnClick="NewPassenger_Click" Text="新增乘客" />--%>
                        <asp:ImageButton ID="imgAddPassenger" OnClientClick="return checkPassenger();" OnClick="NewPassenger_Click"
                            ImageAlign="absmiddle" BorderStyle="None" ImageUrl="../Images/t01.png"
                            Style="cursor: pointer;" runat="server" />新增乘客
                    </td>
                </tr>
                <%--<tr>
                    <td>
                        <label>部門：</label>
                        <div class="vocation">
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="select2"></asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <label>姓名：</label>
                        <div class="vocation">
                            <asp:DropDownList ID="ddlName" runat="server" CssClass="select2"></asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <label>工號：</label>
                        <div class="vocation">
                            <asp:DropDownList ID="ddlEmpNO" runat="server" CssClass="select2"></asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="新增乘客" /></td>
                    <td></td>
                </tr>--%>
            </table>
            <asp:Repeater ID="rptPassenger" runat="server" OnItemCommand="rptPassenger_ItemCommand" OnItemDataBound="rptPassenger_ItemDataBound">
                <HeaderTemplate>
                    <table class="tablelist" id="tblPassenger">
                        <tr>
                            <th>姓名
                            </th>
                            <th>工號
                            </th>
                            <th>手機號
                            </th>
                            <th>備註
                            </th>
                            <th>操作
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 15%;">
                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("PassengerName") %>'></asp:Label></td>
                        <td style="width: 15%;">
                            <asp:Label ID="lblEmpNO" runat="server" Text='<%#Eval("EmpNO") %>'></asp:Label></td>
                        <td style="width: 30%;">
                            <asp:Label ID="lblMobile" runat="server" Text='<%#Eval("Mobile") %>'></asp:Label></td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>'></asp:Label></td>
                        <td style="width: 15%;">
                            <%--<span class="btnSpan">--%>
                            <asp:ImageButton ID="ImgDelete" OnClientClick="return confirmDel();" CommandName="Delete"
                                ImageAlign="absmiddle" ToolTip="Delete" BorderStyle="None" ImageUrl="../Images/close1.png"
                                Style="cursor: pointer;" runat="server" />刪除
                            <%--</span>--%>

                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </fieldset>

        <fieldset style="margin-left: 90px;">
            <legend style="font-size: 15px;">目的地信息</legend>
            <table style="margin-left: 30px; width: 90%;">
                <tr>
                    <td style="width: 88%">&nbsp</td>
                    <td style="width: 12%">
                        <%--<asp:Button ID="btnNewDestination" runat="server" OnClick="NewDestination_Click" Text="新增目的地" />--%>
                        <asp:ImageButton ID="imgAddDestination" OnClick="NewDestination_Click"
                            ImageAlign="absmiddle" BorderStyle="None" ImageUrl="../Images/t01.png"
                            Style="cursor: pointer;" runat="server" />新增目的地
                    </td>
                </tr>
            </table>
            <asp:Repeater ID="rptDestination" runat="server" OnItemCommand="rptDestination_ItemCommand" OnItemDataBound="rptDestination_ItemDataBound">
                <HeaderTemplate>
                    <table class="tablelist" id="tblDestination">
                        <tr>
                            <th>城市
                            </th>
                            <th>詳細地址
                            </th>
                            <th>備註
                            </th>
                            <th>操作
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 25%;">
                            <asp:DropDownList ID="ddlCity" CssClass="select3" runat="server">
                            </asp:DropDownList>
                            <input id="txtCity" type="hidden" runat="server" value='<%#Eval("DestinationRegionCode") %>' />
                        </td>
                        <td style="width: 40%;">
                            <input id="txtDestination" type="text" runat="server" class="dfinput" style="width: 260px;" value='<%#Eval("DetailAddress") %>' maxlength="200" /></td>
                        <td style="width: 20%;">
                            <input id="txtRemark" type="text" runat="server" class="dfinput" style="width: 150px;" value='<%#Eval("Remark") %>' maxlength="100" /></td>
                        <td style="width: 15%;">
                            <asp:ImageButton ID="ImgDel" OnClientClick="return confirmDel();" CommandName="Delete"
                                ImageAlign="absmiddle" ToolTip="Delete" BorderStyle="None" ImageUrl="../Images/close1.png"
                                Style="cursor: pointer;" runat="server" />刪除</td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </fieldset>

        <fieldset style="margin-left: 90px;">
            <legend style="font-size: 15px;">附註</legend>
            <asp:Label ID="lblSuggestion" runat="server" CssClass="suggestion"></asp:Label>
            <textarea class="textinput" id="remarkTxt" runat="server" style="margin-left: 30px; width: 90%; margin-top: 8px; margin-bottom: 8px;"></textarea>
        </fieldset>
        <ul class="formcss">
            <li>
                <label>&nbsp;</label>
                <asp:Button ID="btnSave" runat="server" CssClass="btn" OnClientClick="return valid();" OnClick="btnSave_Click" Text="提交申請" /></li>
            <li>
                <asp:Button ID="btnCancel" runat="server" CssClass="btn" OnClick="btnCancel_Click" Text="取消訂單" Visible="false" /></li>
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
