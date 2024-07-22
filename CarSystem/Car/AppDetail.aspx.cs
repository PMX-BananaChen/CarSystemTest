using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CarSystemTest.Business;
using CarSystemTest.Common;

namespace CarSystemTest.Car
{
    public partial class AppDetail : BasePage
    {
        private string auditResult;  // "1":批准 "0":駁回 
        private string businessCode = "";
        private string userAccount = "";//當前用戶
        //private string isEdit = "0";//"0"表示只能查看,"1"表示可以操作
        private string flowStatus = "";//1是”待主管核准“;2是”待总务调度“;3是”待廠商派車“;4是”待出行“;5是”在途中“;6是”已结案“;7是”流程取消“;8是”主管已驳回“
        private CarBiz cb = new CarBiz();

        private float baseFeeTtl = 0f;
        private float detourFeeTtl = 0f;
        private float tollFeeTtl = 0f;
        private float waitingFeeTtl = 0f;
        private float subtotalFeeTtl = 0f;
        protected void Page_Load(object sender, EventArgs e)
        {
            Security sec = new Security();
            businessCode = sec.Decode64(Request["BusinessCode"].ToString());
            //businessCode = "20160521003";
            //isEdit = sec.Decode64(Request["IsEdit"].ToString());
            flowStatus = sec.Decode64(Request["FlowStatus"].ToString());

            userAccount = WebUser.CurrentUser.UserAccount;
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(businessCode.Trim()))
                {
                    Response.Redirect(@"../Error.html");
                }
                else
                {
                    BindData(businessCode);
                    BindFeeData(businessCode);
                    ShowButton();
                }
            }
        }

        private void BindFeeData(string businessCode)
        {
            DataTable dtAppFee = cb.GetAppFee(businessCode);
            if (dtAppFee != null && dtAppFee.Rows.Count > 0)
            {
                string remark = "";
                for (int i = 0; i < dtAppFee.Rows.Count; i++)
                {
                    DataRow dr = dtAppFee.Rows[i];
                    if (remark != Convert.ToString(dr["ZwRemark"]))
                    {
                        remark = remark + Convert.ToString(dr["ZwRemark"]);
                    }
                }
                lblZwRemark.Text = remark;
                this.rptAppFee.DataSource = dtAppFee;
                this.rptAppFee.DataBind();
            }
        }

        private void BindData(string businessCode)
        {
            CarBiz cb = new CarBiz();
            DataSet dsApp = cb.GetAppDetails(businessCode);
            if (dsApp != null && dsApp.Tables.Count > 0)
            {
                DataTable dtMaster = dsApp.Tables["Vehicle_Master"];
                DataTable dtDestination = dsApp.Tables["Vehicle_Destination"];
                DataTable dtPassenger = dsApp.Tables["Vehicle_Passenger"];
                DataTable dtRemark = dsApp.Tables["Vehicle_Remark"];

                if (dtMaster != null && dtMaster.Rows.Count > 0)
                {
                    DataRow dr = dtMaster.Rows[0];
                    if (dr["VehicleNO"] == DBNull.Value)
                    {
                        this.lblVendorTxt.Text = "建議廠商：";
                        this.lblVehicleTxt.Text = "建議車輛：";
                        this.lblVendor.Text = dr["SuggestVendorName"].ToString();
                        this.lblVehicle.Text = dr["SuggestVehicleNO"].ToString() + " " + dr["DriverUserName"].ToString() + " " + dr["DriverUserMobile"].ToString();
                    }
                    else
                    {
                        this.lblVendorTxt.Text = "實際廠商：";
                        this.lblVehicleTxt.Text = "出行車輛：";
                        this.lblVendor.Text = dr["ActualVendorName"].ToString();
                        this.lblVehicle.Text = dr["VehicleNO"].ToString() + " " + dr["DriverUserName"].ToString() + " " + dr["DriverUserMobile"].ToString();
                    }

                    lblBusienssCode.Text = dr["BusinessCode"].ToString();
                    lblBusinessType.Text = dr["BusinessType"].ToString();
                    //lblRoute.Text = dr["IsOneWay"].ToString() == "1" ? "單程" : "往返";
                    //lblVehicle.Text = dr["SuggestVehicleNO"].ToString();
                    lblEstimatedTime.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dr["RequireStartTime"]);
                    //lblDeparture.Text = dr["DepartureName"].ToString();
                    lblDepartureDetail.Text = dr["DepartureName"].ToString() + dr["DepartureDetailAddress"].ToString();
                    lblAttribute.Text = dr["PassengerAttributeCodeName"].ToString();
                    lblPay.Text = dr["IsPassengerPay"].ToString() == "0" ? "否" : "是";
                    lblPurpose.Text = dr["VehiclePurposeName"].ToString();
                    lblCostCenter.Text = dr["CostCenter"].ToString();
                    this.lblArriveTime.Text = dr["ArriveDepartureRegionTime"] == DBNull.Value ? "" : Convert.ToDateTime(dr["ArriveDepartureRegionTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    this.lblSetOutTime.Text = dr["DepartureTime"] == DBNull.Value ? "" : Convert.ToDateTime(dr["DepartureTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                }
                string message = "";
                if (dtRemark != null && dtRemark.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRemark.Rows)
                    {
                        message = message + dr["UserChineseName"].ToString() + " " + dr["Operation"].ToString() + "(" + Convert.ToDateTime(dr["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ")" + "：" + dr["Remark"].ToString() + "<br/>";
                    }
                }
                this.lblSuggestion.Text = message;

                rptDestination.DataSource = dtDestination;
                rptDestination.DataBind();
                rptPassenger.DataSource = dtPassenger;
                rptPassenger.DataBind();
            }
        }

        private void ShowButton()
        {
            //用戶類型：1是部門助理;2是總務;3是司機;4是司機老闆;5是主管;6是調度人
            //用戶角色：10是助理;11是主管;12是總務;13是廠商;14是司機;15是調度人
            //流程狀態：1是”待主管核准“;2是”待总务调度“;3是”待廠商派車“;4是”待出行“;5是”在途中“;
            //6是”結束行程“;7是”流程取消“;8是”主管已驳回“;9是“BU調度”;10是“結案”
            User u = new User();
            bool isPendingUser = false;
            isPendingUser = cb.IsPendingUser(businessCode, userAccount);
            DataTable dtRoles = u.GetRolesByUserId(WebUser.CurrentUser.UserID);
            //if (dtRoles.Select("RolePrivilegeCode=13").Length > 0 || dtRoles.Select("RolePrivilegeCode=14").Length > 0)
            //{
            //    //意見不能讓司機或司機老闆看到
            //    this.fldSugggestion.Visible = false;
            //}
            if (flowStatus == "10")
            {
                this.lblZwRemarkTitle.Visible = true;
                this.lblZwRemark.Visible = true;
                this.rptAppFee.Visible = true;
            }

            if (isPendingUser)
            {
                //int userType = WebUser.CurrentUser.UserTypeCode;               
                this.remarkTxt.Visible = true;
                //主管
                if (dtRoles.Select("RolePrivilegeCode=11").Length > 0 && flowStatus == "1")
                {
                    this.liAgree.Visible = true;
                    this.liDisagree.Visible = true;
                    this.btnAgree.Visible = true;
                    this.btnDisagree.Visible = true;

                    //填寫意見
                    //this.remarkTxt.Visible = true;
                }

                //總務
                if (dtRoles.Select("RolePrivilegeCode=12").Length > 0)
                {
                    if (flowStatus == "2" || flowStatus == "3" || flowStatus == "4")
                    {
                        this.liDispatch.Visible = true;
                        this.liCancel.Visible = true;
                        this.btnDispatch.Visible = true;
                        this.btnCancel.Visible = true; 
                    }
                    if (flowStatus == "6")
                    {
                        this.liClose.Visible = true;
                        this.btnClose.Visible = true;
                        this.btnClose0.Visible = true;
                      
                    }
                }

                //調度人
                if (dtRoles.Select("RolePrivilegeCode=15").Length > 0 && (flowStatus == "9" || flowStatus == "3" || flowStatus == "4"))
                {
                    this.liDispatch.Visible = true;
                    this.liCancel.Visible = true;
                    this.btnDispatch.Visible = true;
                    this.btnCancel.Visible = true;
                }

                //司機老闆
                if (dtRoles.Select("RolePrivilegeCode=13").Length > 0 && flowStatus == "3")
                {                    
                    this.btnSettingCar.Visible = true;
                }

                //司機
                if (dtRoles.Select("RolePrivilegeCode=14").Length > 0)
                {
                    CarBiz cb = new CarBiz();
                    DataSet dsApp = cb.GetAppDetails(businessCode);
                    if (flowStatus == "4")
                    {
                        DataTable dtMaster = dsApp.Tables["Vehicle_Master"];
                        DataRow dr = dtMaster.Rows[0];
                        if (dr["ArriveDepartureRegionTime"] == DBNull.Value)
                        {
                            this.btnArriveDeparture.Visible = true;
                        }
                        else
                        {
                            this.btnSetOut.Visible = true;
                        }
                    }
                    if (flowStatus == "5")
                    {
                        DataTable dtDestination = dsApp.Tables["Vehicle_Destination"];
                        DataRow[] drs = dtDestination.Select("ArrivalTime is null or DepartureTime is null");
                        if (drs.Length > 0)
                        {
                            this.btnDesOp.Visible = true;
                        }
                        else
                        {
                            this.btnFinish.Visible = true;  
                        }
                    }
                    if (flowStatus == "6")
                    {
                        DateTime dtime = cb.GetUploadTollFeeLastTime(businessCode);
                        if (DateTime.Now < dtime)
                        {
                            this.btnUploadFee.Visible = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 主管同意派車申請
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgree_Click(object sender, EventArgs e)
        {
            auditResult = "1";
            string remark = this.remarkTxt.Value.Trim();
            //string userAccount = WebUser.CurrentUser.UserAccount;
            bool isSuccess = cb.Audit(businessCode, userAccount, auditResult, remark);           
            if (isSuccess)
            {
                lblMsg.Text = "審核成功！";
            }
            else
            {
                lblMsg.Text = "審核失敗！";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('#divTip').fadeIn(300);</script>");
        }

        /// <summary>
        /// 主管不同意，駁回派車申請
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDisagree_Click(object sender, EventArgs e)
        {
            auditResult = "0";
            string remark = this.remarkTxt.Value.Trim();
            //string userAccount = WebUser.CurrentUser.UserAccount;
            bool isSuccess = cb.Audit(businessCode, userAccount, auditResult, remark);
            if (isSuccess)
            {
                lblMsg.Text = "駁回成功！";
            }
            else
            {
                lblMsg.Text = "駁回失敗！";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('#divTip').fadeIn(300);</script>");
        }

        /// <summary>
        /// 總務調度，設置廠商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDispatch_Click(object sender, EventArgs e)
        {
            Security sec = new Security();
            Response.Redirect("BindingVendor.aspx?BusinessCode=" + sec.Encode64(businessCode));
        }

        /// <summary>
        /// 總務強制取消申請單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //string userAccount = WebUser.CurrentUser.UserAccount;
            string remark = this.remarkTxt.Value.Trim();
            bool isSuccess = cb.CancelForm(businessCode, userAccount, remark);
            if (isSuccess)
            {
                lblMsg.Text = "強制取消申請單成功！";
            }
            else
            {
                lblMsg.Text = "強制取消申請單失敗！";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('#divTip').fadeIn(300);</script>");
        }

        /// <summary>
        /// 廠商設置車輛
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSettingCar_Click(object sender, EventArgs e)
        {
            //這裡需要優化 可以直接傳vendorcode
            Security sec = new Security();
            string suggestVehicle = this.lblVehicle.Text;
            Response.Redirect("BindingCar.aspx?BusinessCode=" + sec.Encode64(businessCode) + "&SuggestVehicle=" + sec.Encode64(suggestVehicle));
        }

        /// <summary>
        /// 司機到達出發地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnArriveDeparture_Click(object sender, EventArgs e)
        {
            //"1":到達出發地時間
            string businessType = this.lblBusinessType.Text;
            if (businessType == "正常單")
            {
                bool isSuccess = cb.ArriveDepartureplace(userAccount, businessCode, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (isSuccess)
                {
                    lblMsg.Text = "操作成功！";
                }
                else
                {
                    lblMsg.Text = "操作失敗！";
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('#divTip').fadeIn(300);</script>");
            }
            else
            {
                this.lblType.Text = "1";
                this.lblTime.Text = "到達出發地時間";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('#divTime').fadeIn(300);</script>");
            }
        }

        /// <summary>
        /// 司機從出發地出發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSetOut_Click(object sender, EventArgs e)
        {
            //"2":從出發地出發時間
            string businessType = this.lblBusinessType.Text;
            if (businessType == "正常單")
            {
                bool isSuccess = cb.SetOut(userAccount, businessCode, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (isSuccess)
                {
                    lblMsg.Text = "操作成功！";
                }
                else
                {
                    lblMsg.Text = "操作失敗！";
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('#divTip').fadeIn(300);</script>");
            }
            else
            {
                this.lblType.Text = "2";
                this.lblTime.Text = "從出發地出發時間";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('#divTime').fadeIn(300);</script>");
            }
        }

        /// <summary>
        /// 目的地操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDesOp_Click(object sender, EventArgs e)
        {
            Security sec = new Security();
            Response.Redirect("AppDestinations.aspx?BusinessCode=" + sec.Encode64(businessCode));
        }

        /// <summary>
        /// 結束行程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFinish_Click(object sender, EventArgs e)
        {
            string remark = this.remarkTxt.Value.Trim();
            bool isSuccess = cb.FinishDispatchment(userAccount, businessCode, remark);
            if (isSuccess)
            {
                lblMsg.Text = "操作成功！";
            }
            else
            {
                lblMsg.Text = "操作失敗！";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('#divTip').fadeIn(300);</script>");
        }

        protected void btnUploadFee_Click(object sender, EventArgs e)
        {
            Security sec = new Security();
            Response.Redirect("UploadTollFee.aspx?BusinessCode=" + sec.Encode64(businessCode));
        }

        protected void btnSaveTime_Click(object sender, EventArgs e)
        {
            string type = this.lblType.Text;
            string txtTime = this.txtTime.Value;
            bool isSuccess = false;
            //type "1":到達出發地時間 "2":從出發地出發時間
            if (type == "1")
            {
                isSuccess = cb.ArriveDepartureplace(userAccount, businessCode, txtTime);
            }
            if (type == "2")
            {
                isSuccess = cb.SetOut(userAccount, businessCode, txtTime);
            }
            if (isSuccess)
            {
                lblMsg.Text = "保存成功!";
            }
            else
            {
                lblMsg.Text = "保存失敗!";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('#divTip').fadeIn(300);</script>");
        }

        /// <summary>
        /// 結案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            string remark = this.remarkTxt.Value.Trim();
            bool isSuccess = cb.CloseForm(userAccount, businessCode, remark);
            if (isSuccess)
            {
                lblMsg.Text = "操作成功！";
            }
            else
            {
                lblMsg.Text = "操作失敗！";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('#divTip').fadeIn(300);</script>");
        }

        protected void AppFee_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1)
            {
                float baseFee = float.Parse(((Label)e.Item.FindControl("lblBaseFee")).Text.Trim());
                float detourFee = float.Parse(((Label)e.Item.FindControl("lblDetourFee")).Text.Trim());
                float tollFee = float.Parse(((Label)e.Item.FindControl("lblTollFee")).Text.Trim());
                float waitingFee = float.Parse(((Label)e.Item.FindControl("lblWaitingFee")).Text.Trim());
                float subtotalFee = float.Parse(((Label)e.Item.FindControl("lblSubtotalFee")).Text.Trim());
                baseFeeTtl += baseFee;
                detourFeeTtl += detourFee;
                tollFeeTtl += tollFee;
                waitingFeeTtl += waitingFee;
                subtotalFeeTtl += subtotalFee;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                ((Label)e.Item.FindControl("lblBaseFeeTtl")).Text = Convert.ToDecimal(baseFeeTtl).ToString();
                ((Label)e.Item.FindControl("lblDetourFeeTtl")).Text = detourFeeTtl.ToString();
                ((Label)e.Item.FindControl("lblTollFeeTtl")).Text = tollFeeTtl.ToString();
                ((Label)e.Item.FindControl("lblWaitingFeeTtl")).Text = waitingFeeTtl.ToString();
                ((Label)e.Item.FindControl("lblSubtotalFeeTtl")).Text = subtotalFeeTtl.ToString();
            }
        }

        protected void btnClose0_Click(object sender, EventArgs e)
        {
            Security sec = new Security();
            string url = "../Car/CalFee.aspx?BusinessCode=" + sec.Encode64(businessCode.ToString());
            Response.Redirect(url);

        }

        //protected bool IsPendingUser()
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('該單已經處理。')</script>");
        //    return false;
        //}
    }
}