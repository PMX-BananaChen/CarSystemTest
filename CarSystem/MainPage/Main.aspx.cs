using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CarSystemTest.Common;
using CarSystemTest.Business;

namespace CarSystemTest.MainPage
{
    public partial class Main : BasePage
    {
        private CarBiz cb = new CarBiz();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {
            string userAccount = WebUser.CurrentUser.UserAccount;
            //string userAccount = "pcn\annie.tan";
            DataSet ds = cb.GetToDoList(userAccount, 7, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                rptToDoItems.DataSource = ds.Tables[0];
                rptToDoItems.DataBind();
            }

            //
        }


        protected string EncodeUrlStr(object businessCode, object flowStatusCode)
        {
            //助理跳轉到另一頁面
            User u = new User();
            string userAccount = WebUser.CurrentUser.UserAccount;
            bool isPendingUser = cb.IsPendingUser(businessCode.ToString(), userAccount);
            Security sec = new Security();
            businessCode = sec.Encode64(businessCode.ToString());
            //string isEdit = sec.Encode64("0");           
            string currFlowStatus = sec.Encode64(flowStatusCode.ToString());
            //int currentUserType = WebUser.CurrentUser.UserTypeCode;
            DataTable dt = u.GetRolesByUserId(WebUser.CurrentUser.UserID);
            string url = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                if (isPendingUser && dt.Select("RolePrivilegeCode=10").Length > 0 && flowStatusCode.ToString() == "8")         //助理角色且是處理人且當前的流程狀態是“主管已駁回” 其他情況跳到詳細頁面
                {
                    url = "~/Car/Application.aspx?BusinessCode=" + businessCode;
                }
                else
                {
                    url = "~/Car/AppDetail.aspx?BusinessCode=" + businessCode + "&FlowStatus=" + currFlowStatus;
                    //string url = "AppDetail.aspx?BusinessCode=" + businessCode + "&IsEdit=" + isEdit + "&FlowStatus=" + flowStatus;
                }
            }
            return url;

            //用戶類型：1是部門助理;2是總務;3是司機;4是司機老闆;5是主管
            //Security sec = new Security();
            //int userType = WebUser.CurrentUser.UserTypeCode;
            //businessCode = sec.Encode64(businessCode.ToString());
            ////string isEdit = sec.Encode64("1");
            //string flowStatus = sec.Encode64(flowStatusCode.ToString());
            //string url = "";
            //if (userType == 1)
            //{
            //    url = "~/Car/Application.aspx?BusinessCode=" + businessCode;
            //}
            //else
            //{
            //    url = "~/Car/AppDetail.aspx?BusinessCode=" + businessCode + "&FlowStatus=" + flowStatus;
            //    //url = "~/Car/AppDetail.aspx?BusinessCode=" + businessCode + "&IsEdit=" + isEdit + "&FlowStatus=" + flowStatus;
            //}
            //return url;
        }
    }
}