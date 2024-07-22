using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSystemTest.DAL;

namespace CarSystemTest.Common
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (WebUser.CurrentUser == null)
            {
                RedirectLoginPage();
            }
            else
            {
                HasPagePrivilege();
                base.OnInit(e);
            }
        }
        /// <summary>
        /// 判斷是否有進入當前頁面的權限
        /// </summary>
        public void HasPagePrivilege()
        {
            //TempRecord(1);
            bool IsNoPrivilege = false;
            Response.AddHeader("Cache-Control", "no-store");
            Response.AddHeader("Pragrma", "no-cache");

            string CurrentUrl = Request.Url.ToString().ToUpper();
            try
            {
                if (!(CurrentUrl.Contains("LOGIN.ASPX") || CurrentUrl.Contains("MAINPAGE/MAIN.ASPX")))
                {
                    Security security = new Security();
                    UserDao udao = new UserDao();
                    if (!udao.HasPagePrivilege(WebUser.CurrentUser.UserID, Request.Url.LocalPath))
                    {
                        IsNoPrivilege = true;
                    }
                }              
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);             
                RedirectPromptPage();
            }

            //沒權限跳轉頁面
            if (IsNoPrivilege)
                RedirectPromptPage();
        }
        /// <summary>
        /// 跳轉到沒有權限提示頁面
        /// </summary>
        public void RedirectPromptPage()
        {
            string script = this.Request.ApplicationPath + @"/Error.html";
            script = script.Replace(@"//", "/");
            Response.Redirect(script);
        }

        /// <summary>
        /// 跳轉到登錄頁面
        /// </summary>
        private void RedirectLoginPage()
        {
            string script = this.Request.ApplicationPath + @"/Login.aspx";
            script = script.Replace(@"//", "/");
            Response.Redirect(script);
        }
    }
}
