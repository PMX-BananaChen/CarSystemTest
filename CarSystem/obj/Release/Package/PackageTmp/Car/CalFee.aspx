<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CalFee.aspx.cs" Inherits="CarSystemTest.Car.CalFee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.8.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/common.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tiptop a").click(function () {
                $(".tip").fadeOut(200);
            });

            $("#MainContent_rptAppFee_txtDetourFeeTtl").blur(function () {
                var $detourFeeTtl = $(this).val();
                if (!isValidIntOrDoubleDigitDecimal($detourFeeTtl)) {
                    alert("請輸入有效數據!");
                    $(this).focus();
                    return;
                }
                //計算detourFee 繞路費
                var $rowCnt = $("#tblAppFee tr").length;
                var $baseFeeTtl = $("#tblAppFee tr:last td:eq(1)").find("input").val();
                var $subTtl = 0;
                $("#tblAppFee tr:gt(0):lt(" + ($rowCnt - 3) + ")").each(function () {
                    var $baseFee = $(this).find("td:eq(1) input").val();
                    if (Number($baseFeeTtl) == 0) {
                        $(this).find("td:eq(2) input").val("0");
                    }
                    else {
                        var $detourFee = getDecimalRound(Number($baseFee) / Number($baseFeeTtl) * Number($detourFeeTtl), 2);
                        $(this).find("td:eq(2) input").val($detourFee);
                        $subTtl = getDecimalRound(Number($subTtl) + Number($detourFee), 2);
                    }
                });
                var $leftFee = getDecimalRound(Number($detourFeeTtl) - Number($subTtl), 2);
                $("#tblAppFee tr:eq(" + ($rowCnt - 2) + ") td:eq(2) input").val($leftFee);

                //計算總total
                $("#tblAppFee tr:gt(0)").each(function () {
                    //var $baseFee = $(this).find("td:eq(1) input").val();
                    //var $detourFee = $(this).find("td:eq(2) input").val();
                    //var $tollFee = $(this).find("td:eq(3) input").val();
                    //var $waitingFee = $(this).find("td:eq(4) input").val();
                    var $subTotal = 0;
                    $(this).find("td:gt(0):lt(4)").each(function () {
                        $subTotal = getDecimalRound(Number($subTotal) + Number($(this).find("input").val()), 2);
                    });
                    $(this).find("td:eq(5) span").text($subTotal);
                });
            });
        });
    </script>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">首頁</a></li>
            <li><a href="#">費用計算</a></li>
        </ul>
    </div>

    <div class="formbody">
        <div class="formtitle">
            <span>表單信息</span><div style="float: right;">
                <a href="AppHistory.aspx">
                    <input style="width: 50px; height: 25px; color: #fff; background: url(../images/btnbg.png);" type="button" value="返回" /></a>
            </div>
        </div>
        <ul class="seachform">
            <li>
                <label>&nbsp;</label>
                <asp:Button ID="btnSave" runat="server" CssClass="scbtn" Text="保存" OnClick="btnSave_Click" />
            </li>
        </ul>
        <div style="width: 100%">
            <asp:Repeater ID="rptAppFee" runat="server" OnItemDataBound="AppFee_ItemDataBound">
                <HeaderTemplate>
                    <table class="tablelist" id="tblAppFee">
                        <tr>
                            <th>申請單號
                            </th>
                            <th>基本運費
                            </th>
                            <th>里程數
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
                            <%--<input id="txtBusinessCode" type="text" runat="server" value='<%# Eval("BusinessCode") %>' class="dfinput" disabled="disabled" />--%>
                            <asp:Label ID="lblBusinessCode" runat="server" Text='<%#Eval("BusinessCode") %>'></asp:Label>
                        </td>
                        <td style="width: 10%;">
                            <input id="txtBaseFee" type="text" runat="server" value='<%# Eval("BaseFee") %>' class="dfinput" />
                            <%--<asp:Label ID="lblBaseFee" runat="server" Text='<%#Eval("BaseFee") %>'></asp:Label>--%>
                        </td>
                        <td style="width: 10%;">
                            <input id="txtMileageFee" type="text" runat="server" value='<%# Eval("MileageFee") %>' class="dfinput" />
                            <%--<asp:Label ID="lblBaseFee" runat="server" Text='<%#Eval("BaseFee") %>'></asp:Label>--%>
                        </td>
                        <td style="width: 30%;">
                            <input id="txtDetourFee" type="text" runat="server" value='<%# Eval("DetourFee") %>' class="dfinput" />
                            <%--<asp:Label ID="lblDetourFee" runat="server" Text='<%#Eval("DetourFee") %>'></asp:Label>--%>
                        </td>
                        <td style="width: 15%;">
                            <input id="txtTollFee" type="text" runat="server" value='<%# Eval("TollFee") %>' class="dfinput"  />
                            <%--<asp:Label ID="lblTollFee" runat="server" Text='<%#Eval("TollFee") %>'></asp:Label>--%>
                        </td>
                        <td style="width: 10%;">
                            <input id="txtWaitingFee" type="text" runat="server" value='<%# Eval("WaitingFee") %>' class="dfinput" />
                            <%--<asp:Label ID="lblWaitingFee" runat="server" Text='<%#Eval("WaitingFee") %>'></asp:Label>--%>
                        </td>
                        <td style="width: 20%;">
                            <asp:Label ID="lblSubtotalFee" runat="server" Text='<%#Eval("SubtotalFee") %>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td>總計：
                        </td>
                        <td style="width: 10%;">
                            <input id="txtBaseFeeTtl" type="text" runat="server" class="dfinput" />
                            <%--<asp:Label ID="lblBaseFeeTtl" runat="server"></asp:Label>--%>
                        </td>
                        <td style="width: 10%;">
                            <input id="txtMileageFeeTtl" type="text" runat="server" class="dfinput" />
                            <%--<asp:Label ID="lblBaseFeeTtl" runat="server"></asp:Label>--%>
                        </td>
                        <td style="width: 30%;">
                            <input id="txtDetourFeeTtl" type="text" runat="server" class="dfinput" />
                            <%--<asp:TextBox ID="tbDetourFeeTtl" runat="server" CssClass="dfinput"></asp:TextBox>--%>
                        </td>
                        <td style="width: 15%;">
                            <input id="txtTollFeeTtl" type="text" runat="server" class="dfinput" disabled="disabled" />
                            <%--<asp:Label ID="lblTollFeeTtl" runat="server"></asp:Label>--%>
                        </td>
                        <td style="width: 10%;">
                            <input id="txtWaitingFeeTtl" type="text" runat="server" class="dfinput" disabled="disabled" />
                            <%--<asp:Label ID="lblWaitingFeeTtl" runat="server"></asp:Label>--%>
                        </td>
                        <td style="width: 20%;">
                            <asp:Label ID="lblSubtotalFeeTtl" runat="server"></asp:Label>
                        </td>
                    </tr>
                    </table>                
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div id="divZwRemark" style="margin-left: 30px;">
            <label>備註：</label><asp:TextBox ID="tbZwRemark" runat="server" CssClass="dfinput" Width="500px"></asp:TextBox>
        </div>
    </div>
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
