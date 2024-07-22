using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarSystemTest.Common;
using System.Data;

namespace CarSystemTest.SystemSetting
{
    public partial class User : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                GetData(1);
            }
        }

        private void InitData() {
            Business.User user = new Business.User();
            DataSet ds=user.GetUserTypeGradeEmp();
            if (ds != null && ds.Tables.Count > 0) {
                DataTable dt = ds.Tables[0];
                this.ddlType.DataSource = dt;
                this.ddlType.DataTextField = "UserTypeName";
                this.ddlType.DataValueField = "UserTypeCode";
                this.ddlType.DataBind();
                this.ddlType.Items.Insert(0, new ListItem("", ""));
            }
        }

        private void SearchData(string englishName,string isEnabled,string area,string userType,int pageSize, int currentPage)
        {
            CarSystemTest.Business.User user = new CarSystemTest.Business.User();
            DataSet ds = user.getUserInfo(englishName,isEnabled,area,userType,pageSize, currentPage);
            if (ds != null)
            {
                this.rptUser.DataSource = ds.Tables[0];
                this.rptUser.DataBind();

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
            string englishName = this.txtEnglishName.Text.Trim();
            string isEnabled = this.ddlEnabled.SelectedValue;
            string area = this.ddlArea.SelectedValue;
            string userType = this.ddlType.SelectedValue;
            int pageSize = 10;
            SearchData(englishName,isEnabled,area,userType,pageSize, page);
        }

        public string GetDetailURL(object userID)
        {
            Security sc = new Security();
            string userId = sc.Encode64(userID.ToString());
            return "UserAdd.aspx?UserID=" + userId;
        }

        public string GetRoleBindURL(object userID) {
            Security sc = new Security();
            string userId = sc.Encode64(userID.ToString());
            return "UserRoleBind.aspx?UserID=" + userId;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserAdd.aspx");
        }

        protected void rptUser_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string userId = e.CommandArgument.ToString();
                Business.User user = new Business.User();
                bool result = user.DeleteUser(Convert.ToInt32(userId));
                string msg = "刪除失敗";
                if (result)
                {
                    msg = "刪除成功";
                }
                Util.ShowMessage(msg);
            }
            int currentPage = Convert.ToInt32(this.lblCurrPage.Text);
            GetData((currentPage));
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            GetData(1);
        }
    }
}