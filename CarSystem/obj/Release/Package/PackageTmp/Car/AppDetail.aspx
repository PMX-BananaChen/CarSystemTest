<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AppDetail.aspx.cs" Inherits="CarSystemTest.Car.AppDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/select.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tiptop a").click(function () {
                $(".tip").fadeOut(200);
                window.location.href = window.location.href;
                //window.location.reload();
            });
        });

        function cancel() {
            $("#divTime").fadeOut(200);
            $(".warning").fadeOut(200);
        }

        function checkSuggestion() {
            var $remark = $("#MainContent_remarkTxt").val().trim();
            if ($remark == "") {
                alert("請填寫駁回原因！");
                $("#MainContent_remarkTxt").focus();
                return false;
            }
            return true;
        }
    </script>
    <style type="text/css">
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
            <li><a href="#">助理申請</a></li>
        </ul>
    </div>

    <div class="formbody">
        <div class="formtitle">
            <span>表單信息</span>
            <div style="float: right;">
                <a href="AppHistory.aspx">
                    <input style="width: 50px; height: 25px; color: #fff; background: url(../images/btnbg.png);" type="button" value="返回" /></a>
            </div>
        </div>
        <ul class="formcss">
            <li>
                <label>申請單單號：</label><asp:Label ID="lblBusienssCode" runat="server"></asp:Label></li>
            <li>
                <label>申請單類型：</label><asp:Label ID="lblBusinessType" runat="server"></asp:Label></li>
            <%--          <li>
                <label>單程/往返：</label><asp:Label ID="lblRoute" runat="server"></asp:Label></li>--%>
            <li>
                <label>出發地詳細地址：</label>
                <asp:Label ID="lblDepartureDetail" runat="server"></asp:Label></li>

            <li>
                <label>出行時間：</label>
                <asp:Label ID="lblEstimatedTime" runat="server"></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblVendorTxt" runat="server" CssClass="txtShow"></asp:Label>
                <asp:Label ID="lblVendor" runat="server" CssClass="txtShow"></asp:Label>
            </li>
            <li>
                <asp:Label ID="lblVehicleTxt" runat="server" CssClass="txtShow"></asp:Label>
                <asp:Label ID="lblVehicle" runat="server" CssClass="txtShow"></asp:Label>
            </li>
            <%--<li>
                <label>出發地：</label>
                <asp:Label ID="lblDeparture" runat="server"></asp:Label></li>--%>
            <li>
                <label>乘客人屬性：</label>
                <asp:Label ID="lblAttribute" runat="server"></asp:Label></li>
            <li>
                <label>是否乘客自付：</label>
                <asp:Label ID="lblPay" runat="server"></asp:Label></li>
            <li>
                <label>派車用途：</label>
                <asp:Label ID="lblPurpose" runat="server"></asp:Label></li>
            <li>
                <label>成本中心：</label>
                <asp:Label ID="lblCostCenter" runat="server"></asp:Label></li>
            <li>
                <label>到達出發地時間：</label>
                <asp:Label ID="lblArriveTime" runat="server"></asp:Label></li>
            <li>
                <label>實際出發時間：</label>
                <asp:Label ID="lblSetOutTime" runat="server"></asp:Label></li>
        </ul>
        <fieldset style="margin-left: 90px;">
            <legend style="font-size: 15px;">乘客信息</legend>
            <asp:Repeater ID="rptPassenger" runat="server">
                <HeaderTemplate>
                    <table class="tablelist">
                        <tr>
                            <th>姓名
                            </th>
                            <th>工號
                            </th>
                            <th>手機號
                            </th>
                            <th>備註
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
                        <td style="width: 40%;">
                            <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>'></asp:Label></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </fieldset>

        <fieldset style="margin-left: 90px;">
            <legend style="font-size: 15px;">目的地信息</legend>
            <asp:Repeater ID="rptDestination" runat="server">
                <HeaderTemplate>
                    <table class="tablelist">
                        <tr>
                            <th>城市
                            </th>
                            <th>詳細地址
                            </th>
                            <th>到達目的地時間
                            </th>
                            <th>從目的地離開時間
                            </th>
                            <th>備註
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 15%;">
                            <asp:Label ID="lblCity" runat="server" Text='<%#Eval("DestinationName") %>'></asp:Label></td>
                        <td style="width: 35%;">
                            <asp:Label ID="lblDestination" runat="server" Text='<%#Eval("DetailAddress")%>'></asp:Label></td>
                        <td style="width: 20%;">
                            <asp:Label ID="lblArrialTime" runat="server" Text='<%#string.Format("{0:yyyy-MM-dd HH:mm:ss}", Eval("ArrivalTime")) %>'></asp:Label></td>
                        <td style="width: 20%;">
                            <asp:Label ID="lblDepartureTime" runat="server" Text='<%#string.Format("{0:yyyy-MM-dd HH:mm:ss}", Eval("DepartureTime")) %>'></asp:Label></td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
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
            <textarea class="textinput" id="remarkTxt" runat="server" style="margin-left: 30px; width: 90%; margin-top: 8px; margin-bottom: 8px;" visible="false"></textarea>
        </fieldset>

        <ul class="formcss">
            <li id="liAgree" runat="server" visible="false">
                <label>&nbsp;</label>
                <asp:Button ID="btnAgree" runat="server" CssClass="btn" OnClick="btnAgree_Click" Text="同意" Visible="false" /></li>
            <li id="liDisagree" runat="server" visible="false">
                <asp:Button ID="btnDisagree" runat="server" CssClass="btn" OnClick="btnDisagree_Click" OnClientClick="return checkSuggestion();" Text="駁回" Visible="false" /></li>
            <li id="liDispatch" runat="server" visible="false">
                <asp:Button ID="btnDispatch" runat="server"  CssClass="btn" OnClick="btnDispatch_Click" Text="設置廠商" Visible="false" /></li>
            <li id="liCancel" runat="server" visible="false">
                <asp:Button ID="btnCancel" runat="server"  CssClass="btn" OnClick="btnCancel_Click" Text="強制取消" Visible="false" /></li>
            <li id="liSettingCar" runat="server" visible="false">
                <asp:Button ID="btnSettingCar" runat="server"  CssClass="btn" OnClick="btnSettingCar_Click" Text="安排車輛" Visible="false" /></li>
            <li id="liArriveDeparture" runat="server" visible="false">
                <asp:Button ID="btnArriveDeparture" runat="server"  CssClass="btn"  OnClick="btnArriveDeparture_Click" Text="到達出發地" Visible="false" /></li>
            <li id="liSetOut" runat="server" visible="false">
                <asp:Button ID="btnSetOut" runat="server"  CssClass="btn" OnClick="btnSetOut_Click" Text="出發" Visible="false" /></li>
            <li id="liDesOp" runat="server" visible="false">>
                <asp:Button ID="btnDesOp" runat="server"  CssClass="btn" OnClick="btnDesOp_Click" Text="目的地操作" Visible="false" /></li>
            <li id="liFinish" runat="server" visible="false">
                <asp:Button ID="btnFinish" runat="server"  CssClass="btn" OnClick="btnFinish_Click" Text="結束行程" Visible="false" /></li>
            <li id="liUploadFee" runat="server" visible="false">
                <asp:Button ID="btnUploadFee" runat="server"  CssClass="btn"  OnClick="btnUploadFee_Click" Text="上傳高速費" Visible="false" /></li>
            <li id="liClose" runat="server" visible="false">
                <asp:Button ID="btnClose" runat="server"  CssClass="btn" OnClick="btnClose_Click" Text="結案" Visible="false" />&nbsp;<asp:Button ID="btnClose0" runat="server"  CssClass="btn" OnClick="btnClose0_Click" Text="费用" Visible="false" /></li>
        </ul>

        <div style="width: 800px; margin-left: 60px;">
            <asp:Repeater ID="rptAppFee" runat="server" OnItemDataBound="AppFee_ItemDataBound" Visible="false">
                <HeaderTemplate>
                    <table class="tablelist" id="tblAppFee">
                        <tr>
                            <th>申請單號
                            </th>
                            <th>基本運費
                            </th>
                            <th>繞路費
                            </th>
                            <th>高速費
                            </th>
                            <th>候時費
                            </th>
                            <th>總計
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 15%;">
                            <asp:Label ID="lblBusinessCode" runat="server" Text='<%#Eval("BusinessCode") %>'></asp:Label>
                        </td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblBaseFee" runat="server" Text='<%# Eval("BaseFee") %>'></asp:Label>

                        </td>
                        <td style="width: 30%;">
                            <asp:Label ID="lblDetourFee" runat="server" Text='<%# Eval("DetourFee") %>'></asp:Label>
                        </td>
                        <td style="width: 15%;">
                            <asp:Label ID="lblTollFee" runat="server" Text='<%# Eval("TollFee") %>'></asp:Label>
                        </td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblWaitingFee" runat="server" Text='<%# Eval("WaitingFee") %>'></asp:Label>
                        </td>
                        <td style="width: 20%;">
                            <asp:Label ID="lblSubtotalFee" runat="server" Text='<%# Eval("SubtotalFee") %>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td>總計：
                        </td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblBaseFeeTtl" runat="server"></asp:Label>
                        </td>
                        <td style="width: 30%;">
                            <asp:Label ID="lblDetourFeeTtl" runat="server"></asp:Label>
                        </td>
                        <td style="width: 15%;">
                            <asp:Label ID="lblTollFeeTtl" runat="server"></asp:Label>
                        </td>
                        <td style="width: 10%;">
                            <asp:Label ID="lblWaitingFeeTtl" runat="server"></asp:Label>
                        </td>
                        <td style="width: 20%;">
                            <asp:Label ID="lblSubtotalFeeTtl" runat="server"></asp:Label>
                        </td>
                    </tr>
                    </table>                
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div id="divZwRemark" style="margin-left: 90px;">
            <asp:Label ID="lblZwRemarkTitle" runat="server" Visible="false" >備註</asp:Label><asp:Label ID="lblZwRemark" runat="server" Width="500px" Visible="false"></asp:Label>
        </div>

        <%--  <ul class="formcss">
            <li>
                <label>&nbsp;</label>
                <asp:Button ID="btnDispatch" runat="server" CssClass="btn" OnClick="btnDispatch_Click" Text="設置廠商" Visible="false" /></li>
          
            <li>
                <asp:Button ID="btnCancel" runat="server" CssClass="btn" OnClick="btnCancel_Click" Text="強制取消" Visible="false" /></li>
        </ul>--%>
        <%-- <ul class="formcss">
            <li>
                <label>&nbsp;</label>
                <asp:Button ID="btnSettingCar" runat="server" CssClass="btn" OnClick="btnSettingCar_Click" Text="安排車輛" Visible="false" /></li>
        </ul>--%>
        <%--  <ul class="formcss">
            <li>
                <label>&nbsp;</label>
                <asp:Button ID="btnArriveDeparture" runat="server" CssClass="btn" OnClick="btnArriveDeparture_Click" Text="到達出發地" Visible="false" /></li>
            <li>
                <asp:Button ID="btnSetOut" runat="server" CssClass="btn" OnClick="btnSetOut_Click" Text="出發" Visible="false" /></li>
        </ul>--%>
        <%--       <ul class="formcss">
            <li>
                <label>&nbsp;</label>
                <asp:Button ID="btnDesOp" runat="server" CssClass="btn" OnClick="btnDesOp_Click" Text="目的地操作" Visible="false" /></li>
            <li>
                <asp:Button ID="btnFinish" runat="server" CssClass="btn" OnClick="btnFinish_Click" Text="結束行程" Visible="false" /></li>
        </ul>--%>
        <%--  <ul class="formcss">
            <li>
                <label>&nbsp;</label>
                <asp:Button ID="btnUploadFee" runat="server" CssClass="btn" OnClick="btnUploadFee_Click" Text="上傳高速費" Visible="false" /></li>
        </ul>--%>
    </div>
    <div class="warning">
    </div>
    <div class="tip" id="divTip">
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
    <div class="tip" id="divTime">
        <div class="tiptop"><span>提示信息</span><a onclick="cancel();"></a></div>
        <div class="tipinfo">

            <span>
                <asp:Label ID="lblTime" runat="server"></asp:Label>
            </span>
            <div class="tipright">
                <p>
                    <input id="txtTime" runat="server" class="dfinput" type="text" onfocus="WdatePicker({ skin: 'twoer', dateFmt: 'yyyy-MM-dd HH:mm:00',maxDate: '%y-%M-%d %H:%m' })" />
                </p>
                <cite></cite>
            </div>
        </div>
        <div class="tipbtn">
            <asp:Button ID="btnSaveTime" runat="server" CssClass="sure" Text="確定" OnClick="btnSaveTime_Click" />
            <%--<input id="btnSaveTime" type="button" class="sure" value="确定" runat="server" onclick="btnSaveTime_Click" />--%>&nbsp;
        <input type="button" class="cancel" value="取消" onclick="cancel();" />
            <asp:Label ID="lblType" runat="server" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>
