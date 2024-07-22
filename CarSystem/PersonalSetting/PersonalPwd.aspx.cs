using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarSystemTest.Common;
using CarSystemTest.Business;

namespace CarSystemTest.PersonalSetting
{
    public partial class PersonalPwd : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lblAccount.Text = WebUser.CurrentUser.UserAccount;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int userId = WebUser.CurrentUser.UserID;
            //int userId = 10000;
            string pwd = Security.EncryptPassword(this.tbPwd.Text.Trim(), "MD5");
            User user = new User();
            bool isSuccess = user.UpdatePwd(userId, pwd);
            string msg = "密碼修改失敗！";
            if (isSuccess)
            {
                msg = "密碼修改成功！";
            }
            Util.ShowMessage(msg);
        }
    }
}