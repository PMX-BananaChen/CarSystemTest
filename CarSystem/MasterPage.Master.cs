using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using CarSystemTest.Common;
using CarSystemTest.Business;

namespace CarSystemTest
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckSession();
                SetUserMenu();
            }
        }

        private void CheckSession()
        {
            if (WebUser.CurrentUser == null)
            {
                string url = this.Request.ApplicationPath + @"/Login.aspx";
                url = url.Replace(@"//", "/");
                Response.Redirect(url);
            }
            else
            {
                this.lblUser.Text = WebUser.CurrentUser.UserChineseName;
            }
        }

        /// <summary>
        /// 設置系統菜單
        /// </summary>
        private void SetUserMenu()
        {
            User user = new User();
            DataTable dtMenus = user.GetUserMenus(WebUser.CurrentUser.UserID);
            StringBuilder sb = new StringBuilder();
            int menuId = 0;
            string menuName = "";
            string privilegeURL = "";
            int imgId = 0;
            sb.Append("<div class=\"lefttop\"><span></span>系統菜單</div>");
            sb.Append("<dl class=\"leftmenu\">");
            if (dtMenus != null)
            {
                for (int i = 0; i < dtMenus.Rows.Count; i++)
                {
                    DataRow dr = dtMenus.Rows[i];
                    menuId = Convert.ToInt32(dr["PrivilegeCode"]);
                    menuName = Convert.ToString(dr["ChineseDisplayName"]);
                    bool bl = false;
                    //sb.Append("<dd>");
                    if (menuId % 100 == 0)
                    {
                        imgId++;
                        sb.Append("<dd><div class=\"title\"><span><img src =\"../images/leftico0" + imgId.ToString() + ".png\"/></span>" + menuName + "</div>");
                    }
                    else
                    {
                        bl = true;
                        privilegeURL = Convert.ToString(dr["ResourceURL"]);
                        if (imgId == 1)
                        {
                            sb.Append("<ul class=\"menuson\"><li class=\"active\"><cite></cite><a href=\"../" + privilegeURL + "\">" + menuName + "</a><i></i></li></ul>");
                        }
                        else
                        {
                            sb.Append("<ul class=\"menuson\"><li><cite></cite><a href=\"../" + privilegeURL + "\">" + menuName + "</a><i></i></li></ul>");
                        }
                        imgId++;
                    }
                    if (bl)
                    {
                        sb.Append("</dd>");
                    }
                }
            }
            sb.Append("</dl>");
            this.divMenu.InnerHtml = sb.ToString();
        }
    }
}