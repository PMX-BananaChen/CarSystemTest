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
    public partial class Role : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int pageSize = 10;
                SearchData(pageSize, 1);
            }
        }

        private void SearchData(int pageSize, int currentPage)
        {
            CarSystemTest.Business.User user = new CarSystemTest.Business.User();
            DataSet ds = user.GetRoles(pageSize, currentPage);
            if (ds != null)
            {
                this.rptRole.DataSource = ds.Tables[0];
                this.rptRole.DataBind();

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
            SearchData(pageSize, page);
        }

        public string GetDetailURL(object roleId)
        {
            Security sc = new Security();
            string roleCode = sc.Encode64(roleId.ToString());
            return "RoleAdd.aspx?RoleCode=" + roleCode;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("RoleAdd.aspx");
        }

        //public string DeleteRole(object roleId) {
        //    string roleCode = roleId.ToString();
        //    string msg = "刪除失敗";
        //    Business.User user = new Business.User();
        //    bool result = user.DeleteRole(Convert.ToInt32(roleCode));
        //    if (result) {
        //        msg = "刪除成功";
        //    }
        //    Util.ShowMessage(msg);
        //    return "";
        //}

        protected void rptRole_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName== "Delete") {
                string roleCode = e.CommandArgument.ToString();
                Business.User user = new Business.User();
                bool result = user.DeleteRole(Convert.ToInt32(roleCode));
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
    }
}