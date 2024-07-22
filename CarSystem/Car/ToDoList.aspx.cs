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
    public partial class ToDoList : BasePage
    {

        private CarBiz cb = new CarBiz();
        private const int flowStatus = 6; //表示申請單已結束行程
        //private const int userType = 2;  //表示用戶為總務
        private const int roleId = 12;//表示用戶角色為總務     


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData(1);
            }
        }

        private void SeachData(int pageSize, int currentPage)
        {
            string userAccount = WebUser.CurrentUser.UserAccount;
            //string userAccount = @"pcn\ang.yu";
            DataSet ds = cb.GetToDoList(userAccount, pageSize, currentPage);

            if (ds != null && ds.Tables.Count > 0)
            {
                this.rptAppHistory.DataSource = ds.Tables[0];
                this.rptAppHistory.DataBind();

                DataTable dtRecord = ds.Tables[1];
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
            int pageSize = 10;
            SeachData(pageSize, page);
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
            string currFlowStatus = sec.Encode64(flowStatusCode.ToString()); //申請單的流程狀態
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
                    url = "AppDetail.aspx?BusinessCode=" + businessCode + "&FlowStatus=" + currFlowStatus;
                }
            }
            return url;
        }

    }
}