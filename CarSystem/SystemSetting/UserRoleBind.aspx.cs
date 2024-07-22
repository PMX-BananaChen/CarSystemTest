using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CarSystemTest.Business;
using CarSystemTest.Common;

namespace CarSystemTest.SystemSetting
{
    public partial class UserRoleBind : BasePage
    {
        private Business.User user = new Business.User();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = 0;
                Security sc = new Security();
                if (Request["UserID"] != null)
                {
                    userId = Convert.ToInt32(sc.Decode64(Request["UserId"]));
                }
                InitData(userId);
            }
        }

        private void InitData(int userId)
        {
            DataSet ds = user.GetUserRolePrivilege(userId);
            if (ds != null && ds.Tables.Count > 0)
            {
                //綁定未選擇的角色
                this.sltRoles.DataSource = ds.Tables[1];
                this.sltRoles.DataTextField = "RoleName";
                this.sltRoles.DataValueField = "RoleCode";
                this.sltRoles.DataBind();
                //綁定已選擇的角色
                this.sltedRoles.DataSource = ds.Tables[2];
                this.sltedRoles.DataTextField = "RoleName";
                this.sltedRoles.DataValueField = "RoleCode";
                this.sltedRoles.DataBind();
                //綁定未選擇的權限
                this.sltPrivileges.DataSource = ds.Tables[3];
                this.sltPrivileges.DataTextField = "PrivilegeName";
                this.sltPrivileges.DataValueField = "PrivilegeCode";
                this.sltPrivileges.DataBind();
                //綁定已選擇的權限
                this.sltedPrivileges.DataSource = ds.Tables[4];
                this.sltedPrivileges.DataTextField = "PrivilegeName";
                this.sltedPrivileges.DataValueField = "PrivilegeCode";
                this.sltedPrivileges.DataBind();
            }
            if (userId != 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.Rows[0];
                this.tbAccount.Text = dr["UserAccount"].ToString();
                this.tbChineseName.Text = dr["UserChineseName"].ToString();
                this.tbEnglishName.Text = dr["UserEnglishName"].ToString();
            }
            this.tbAccount.Enabled = false;
            this.tbChineseName.Enabled = false;
            this.tbEnglishName.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int userId = 0;
            Security sc = new Security();
            if (Request["UserID"] != null)
            {
                userId = Convert.ToInt32(sc.Decode64(Request["UserId"]));
            }
            string roles = this.tbRoles.Text.ToString();
            string privileges = this.tbPriviles.Text.ToString();
            bool isSuccess = user.ModifyUserRolePrivilege(userId, roles, privileges);
            string msg = "修改失敗！";
            if (isSuccess)
            {
                msg = "修改成功！";
            }
            Util.ShowMessage(msg);
        }
    }
}