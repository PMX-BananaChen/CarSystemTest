using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarSystemTest.Common;
using System.Data;
using CarSystemTest.Business;

namespace CarSystemTest.SystemSetting
{
    public partial class Privilege : BasePage
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
            DataSet ds= user.GetPrivilege(pageSize, currentPage);
            if (ds != null) {
                this.rptPrivilege.DataSource = ds.Tables[0];
                this.rptPrivilege.DataBind();

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

        public string GetDetailURL(object privilegeCode)
        {
            Security sc = new Security();
            string pvlegeCode = sc.Encode64(privilegeCode.ToString());
            return "PrivilegeAdd.aspx?PrivilegeCode="+pvlegeCode;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("PrivilegeAdd.aspx");
        }
    }
}