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
    public partial class AppCancel : System.Web.UI.Page
    {

        private CarBiz cb = new CarBiz();
        private const int flowStatus = 6; //表示申請單已結束行程
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
                this.txtStartDay.Value = f_startDay;
                this.txtEndDay.Value = f_endDay;
                this.ddlFlowStatus.SelectedValue = f_flowStatusCode;
                int page = f_page;
                GetData(f_page);
            }
        }

        private void InitData()
        {
            DataTable dt = cb.GetAllFlowStatus();
            this.ddlFlowStatus.DataSource = dt;
            this.ddlFlowStatus.DataTextField = "FlowStatusName";
            this.ddlFlowStatus.DataValueField = "FlowStatusCode";
            this.ddlFlowStatus.DataBind();
            this.ddlFlowStatus.Items.Insert(0, new ListItem("", ""));
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
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
            string url = "CalFee.aspx?BusinessCode=" + sec.Encode64(businessCode.ToString());
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
    }
}
