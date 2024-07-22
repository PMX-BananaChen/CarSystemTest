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
    public partial class AppHistory : BasePage
    {
        private CarBiz cb = new CarBiz();
        private const int flowStatus = 6; //表示申請單已結束行程
        //private const int userType = 2;  //表示用戶為總務
        private const int roleId = 12;//表示用戶角色為總務     

        string f_startDay = "";
        string f_endDay = "";
        string f_flowStatusCode = "";
        int f_page = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                if (Session["StartDay"] == null)
                {
                    f_startDay = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                }
                else
                {
                    f_startDay = Convert.ToString(Session["StartDay"]);
                }

                if (Session["EndDay"] == null)
                {
                    f_endDay = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    f_endDay = Convert.ToString(Session["EndDay"]);
                }
                if (Session["FilterFlowStatus"] == null)
                {
                    f_flowStatusCode = "";
                }
                else
                {
                    f_flowStatusCode = Convert.ToString(Session["FilterFlowStatus"]);
                }
                if (Session["Page"] == null)
                {
                    f_page = 1;
                }
                else
                {
                    f_page = Convert.ToInt32(Session["Page"]);
                }

                //if (Request["StartDay"] == null)
                //{
                //    f_startDay = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                //}
                //else
                //{
                //    f_startDay = Convert.ToString(Request["StartDay"]);
                //}
                //if (Request["EndDay"] == null)
                //{
                //    f_endDay = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                //else
                //{
                //    f_endDay = Convert.ToString(Request["EndDay"]);
                //}
                //if (Request["FlowStatus"] == null)
                //{
                //    f_flowStatusCode = "";
                //}
                //else
                //{
                //    f_flowStatusCode = Convert.ToString(Request["FlowStatus"]);
                //}
                this.txtStartDay.Value = f_startDay;
                this.txtEndDay.Value = f_endDay;
                this.ddlFlowStatus.SelectedValue = f_flowStatusCode;
                int page = f_page;
                GetData(f_page);
            }
        }

        private void InitData()
        {
            //this.txtStartDay.Value = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            //this.txtEndDay.Value = DateTime.Now.ToString("yyyy-MM-dd");

            DataTable dt = cb.GetAllFlowStatus();
            this.ddlFlowStatus.DataSource = dt;
            this.ddlFlowStatus.DataTextField = "FlowStatusName";
            this.ddlFlowStatus.DataValueField = "FlowStatusCode";
            this.ddlFlowStatus.DataBind();
            this.ddlFlowStatus.Items.Insert(0, new ListItem("", ""));

            //
            User u = new User();
            DataTable dtRoles = u.GetRolesByUserId(WebUser.CurrentUser.UserID);
            if (dtRoles.Select("RolePrivilegeCode=11").Length > 0)
            {
                this.btnBatchApprove.Visible = true;
            }
            else {
                this.btnBatchApprove.Visible = false;
            }
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            //string startDay = this.txtStartDay.Value;
            //string endDay = this.txtEndDay.Value;
            //string flowStatusCode = this.ddlFlowStatus.SelectedValue;         
            //SeachData(startDay, endDay, flowStatusCode, 10, 1);
            int currentPage = 1;
            GetData(currentPage);
        }

        private void SeachData(string startDay, string endDay, string flowStatus, int pageSize, int currentPage)
        {
            string userAccount = WebUser.CurrentUser.UserAccount;
            //string userAccount = @"pcn\ang.yu";
            DataSet dsAppHistory = cb.GetAppHistory(userAccount, startDay, endDay, flowStatus, pageSize, currentPage);
            if (dsAppHistory != null)
            {
                this.rptAppHistory.DataSource = dsAppHistory.Tables[0];
                this.rptAppHistory.DataBind();

                DataTable dtRecord = dsAppHistory.Tables[1];
                DataRow dr = dtRecord.Rows[0];
                this.lblTotal.Text = dr["TtlRecord"].ToString();
                this.lblCurrPage.Text = dr["CurrentPage"].ToString();
                this.lblTttPage.Text = dr["TtlPage"].ToString();
            }
        }

        /// <summary>
        /// 首頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbFirst_Click(object sender, EventArgs e)
        {
            //string startDay = this.txtStartDay.Value;
            //string endDay = this.txtEndDay.Value;
            //string flowStatus = this.ddlFlowStatus.SelectedValue;
            //SeachData(startDay, endDay, flowStatus, 10, 1);
            int currentPage = 1;
            GetData(currentPage);
        }

        /// <summary>
        /// 上一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            //string startDay = this.txtStartDay.Value;
            //string endDay = this.txtEndDay.Value;
            //string flowStatus = this.ddlFlowStatus.SelectedValue;
            //int currentPage = Convert.ToInt32(this.lblCurrPage.Text);
            //SeachData(startDay, endDay, flowStatus, 10, (currentPage - 1));
            int currentPage = Convert.ToInt32(this.lblCurrPage.Text);
            GetData((currentPage - 1));
        }

        /// <summary>
        /// 下一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbNext_Click(object sender, EventArgs e)
        {
            //string startDay = this.txtStartDay.Value;
            //string endDay = this.txtEndDay.Value;
            //string flowStatus = this.ddlFlowStatus.SelectedValue;
            //int currentPage = Convert.ToInt32(this.lblCurrPage.Text);            
            //SeachData(startDay, endDay, flowStatus, 10, (currentPage + 1));
            int currentPage = Convert.ToInt32(this.lblCurrPage.Text);
            GetData((currentPage + 1));
        }

        /// <summary>
        /// 末頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLast_Click(object sender, EventArgs e)
        {
            //string startDay = this.txtStartDay.Value;
            //string endDay = this.txtEndDay.Value;
            //string flowStatus = this.ddlFlowStatus.SelectedValue;
            //int lastPage = 0;
            //int leftRecord = Convert.ToInt32(lblTotal.Text) % 10;
            //if (leftRecord != 0)
            //{
            //    lastPage = Convert.ToInt32(lblTotal.Text) / 10 + 1;
            //}
            //else
            //{
            //    lastPage = Convert.ToInt32(lblTotal.Text) / 10;
            //}
            //SeachData(startDay, endDay, flowStatus, 10, lastPage);
            int lastPage = 0;
            int leftRecord = Convert.ToInt32(lblTotal.Text) % 10;
            if (leftRecord != 0)
            {
                lastPage = Convert.ToInt32(lblTotal.Text) / 10 + 1;
            }
            else
            {
                lastPage = Convert.ToInt32(lblTotal.Text) / 10;
            }
            GetData(lastPage);
        }

        private void GetData(int page)
        {
            string startDay = this.txtStartDay.Value;
            string endDay = this.txtEndDay.Value;
            string flowStatus = this.ddlFlowStatus.SelectedValue;
            int pageSize = 10;
            f_startDay = startDay;
            f_endDay = endDay;
            f_flowStatusCode = flowStatus;
            f_page = page;
            SeachData(startDay, endDay, flowStatus, pageSize, page);
        }

        /// <summary>
        /// 費用功能對總務，且結案狀態下才顯示
        /// </summary>
        /// <param name="flowStatusCode"></param>
        /// <returns></returns>
        public bool CheckUser(object flowStatusCode)
        {
            User u = new User();
            bool hasPrivilege = false;
            DataTable dt = u.GetRolesByUserId(WebUser.CurrentUser.UserID);
            if (Convert.ToInt32(flowStatusCode) == flowStatus && dt.Select("RolePrivilegeCode='" + roleId.ToString() + "'").Length > 0)
            {
                hasPrivilege = true;
            }
            return hasPrivilege;
        }

        /// <summary>
        /// 跳轉到計費頁面
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public string GetCalFeeURL(object businessCode)
        {
            Security sec = new Security();
            string url = "../Car/CalFee.aspx?BusinessCode=" + sec.Encode64(businessCode.ToString());
            return url;
        }

        /// <summary>
        /// 獲取詳細信息
        /// </summary>
        /// <param name="businessCode"></param>
        /// <param name="flowStatusCode"></param>
        /// <returns></returns>
        public string GetDetailURL(object businessCode, object flowStatusCode)
        {
            //助理跳轉到另一頁面
            User u = new User();
            string userAccount = WebUser.CurrentUser.UserAccount;
            bool isPendingUser = cb.IsPendingUser(businessCode.ToString(), userAccount);
            Security sec = new Security();
            businessCode = sec.Encode64(businessCode.ToString());
            //string isEdit = sec.Encode64("0");           
            string currFlowStatus = sec.Encode64(flowStatusCode.ToString()); //申請單的流程狀態
            //int currentUserType = WebUser.CurrentUser.UserTypeCode;
            DataTable dt = u.GetRolesByUserId(WebUser.CurrentUser.UserID);
            string url = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                if (isPendingUser && dt.Select("RolePrivilegeCode=10").Length > 0 && flowStatusCode.ToString() == "8")         //助理角色且是處理人且當前的流程狀態是“主管已駁回” 其他情況跳到詳細頁面
                {
                    url = "Application.aspx?BusinessCode=" + businessCode;
                }
                else
                {
                    Session["StartDay"] = f_startDay;
                    Session["EndDay"] = f_endDay;
                    Session["FilterFlowStatus"] = f_flowStatusCode;
                    Session["Page"] = f_page;
                    url = "AppDetail.aspx?BusinessCode=" + businessCode + "&FlowStatus=" + currFlowStatus;
                    //url = "AppDetail.aspx?BusinessCode=" + businessCode + "&FlowStatus=" + currFlowStatus + "&StartDay=" + f_startDay + "&EndDay=" + f_endDay + "&FilterFlowStatus=" + f_flowStatusCode;
                    //string url = "AppDetail.aspx?BusinessCode=" + businessCode + "&IsEdit=" + isEdit + "&FlowStatus=" + flowStatus;
                }
            }
            return url;
        }

        /// <summary>
        /// 獲取跳轉的gpsURL
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public string GetGpsUrl(object businessCode)
        {
            Security sec = new Security();
            string url = "~/GPS/CarTracking.aspx?BusinessCode=" + sec.Encode64(businessCode.ToString());
            //string url = cb.GetGpsURL(businessCode.ToString());
            return url;
        }

        public bool IsFinished(object finishTime)
        {
            if (finishTime == DBNull.Value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 批量核准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchApprove_Click(object sender, EventArgs e)
        {
            bool result = true;
            string businessCode = "";
            string userAccount = WebUser.CurrentUser.UserAccount;            
            foreach (RepeaterItem item in rptAppHistory.Items)
            {
                CheckBox ckbox = (CheckBox)item.FindControl("cbApprove");
                if (ckbox.Checked )
                {
                    businessCode = ckbox.ToolTip;
                    bool success=cb.Audit(businessCode, userAccount, "1", "");
                    if (!success) {
                        result = success;
                    }
                }
            }
            int currentPage = Convert.ToInt32(this.lblCurrPage.Text);
            GetData(currentPage);                     
        }

        public bool CheckPrivilege(object businessId,object flowStatusCode)
        {
            User u = new User();
            string userAccount = WebUser.CurrentUser.UserAccount;
            bool isPendingUser = false;
            string businessCode = businessId.ToString();
            isPendingUser = cb.IsPendingUser(businessCode, userAccount);
            DataTable dtRoles = u.GetRolesByUserId(WebUser.CurrentUser.UserID);
            bool hasPrivilege = false;
            if (isPendingUser && dtRoles.Select("RolePrivilegeCode=11").Length > 0 && Convert.ToInt32(flowStatusCode) == 1 )
            {
                hasPrivilege = true;
            }
            return hasPrivilege;
        }
    }
}