using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using CarSystemTest.Business;
using CarSystemTest.Common;
using System.Web.Services;

namespace CarSystemTest
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckAuthenticationMode();
            }
        }


        [WebMethod]
        public static string UserLogin(string userAccount, string password)
        {
            User user = new User();
            string message = "";
            int userID = 0;
            userAccount = HttpUtility.UrlDecode(userAccount);
            password = HttpUtility.UrlDecode(password);
            userAccount = @"pcn\Ang.Yu";// Lucy.Lei ptw\George.Huang pcn\Ang.Yu
            password = "1";
            try
            {
                string userHostAddress = HttpContext.Current.Request.UserHostAddress;
                string userHostName = HttpContext.Current.Request.UserHostName;
                user.UserLogin(userAccount, password, userHostAddress, userHostName, "", Convert.ToInt32(WebConfigurationManager.AppSettings["LockUserByLoginErrorCount"]), ref userID, ref message);
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
            finally
            {
                if (string.IsNullOrEmpty(message))
                {
                    user.SetUserInfo(userID);
                }
            }
            return message;
        }

        private void CheckAuthenticationMode()
        {
            string authMode = WebConfigurationManager.AppSettings["AuthenticationMode"].ToString();
            User user = new User();
            if (authMode == "Windows")
            {
                string domainUser = @"pcn\Ang.Yu";
                //string domainUser = HttpContext.Current.Request.ServerVariables["LOGON_USER"];

                if (WebUser.CurrentUser == null)
                {
                    int userID = user.UserADLogin(domainUser);
                    if (userID == 0)
                    {
                        //this.divContent.InnerHtml = "<div><label color='red'>該帳號" + domainUser + "不存在，請聯繫管理員!<abel></div>";
                        //lblMsg.Text = "該帳號" + domainUser + "不存在，請聯繫管理員!";
                        Response.Redirect("NoPrivilege.html");
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.tip').fadeIn(300);</script>");
                        return;
                    }
                    else
                    {
                        user.SetUserInfo(userID);
                    }
                }
                Response.Redirect(@"MainPage/Main.aspx");
            }
        }
    }
}