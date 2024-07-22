using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using CarSystemTest.Business;
using CarSystemTest.Common;

namespace CarSystemTest.SystemSetting
{
    public partial class RoleAdd : BasePage
    {
        private Business.User user = new Business.User();
        private const int genType = 3;        
        protected void Page_Load(object sender, EventArgs e)
        {
            Security sc = new Security();
            if (!IsPostBack)
            {
                int roleId = 0;
                if (Request["RoleCode"] != null) {
                    string roleCode=sc.Decode64(Request["RoleCode"].ToString());
                    roleId = Convert.ToInt32(roleCode);
                }                
                InitData(roleId);
            }
        }
        private void InitData(int roleId)
        {
            DataSet ds = user.GetRolePrivileges(roleId);
            if (ds != null && ds.Tables.Count > 0)
            {
                //可供選擇的權限綁定
                //this.sltPrivileges.Items.Clear();
                this.sltPrivileges.DataSource = ds.Tables[2];
                this.sltPrivileges.DataTextField = "PrivilegeName";
                this.sltPrivileges.DataValueField = "PrivilegeCode";
                this.sltPrivileges.DataBind();
                //選中的權限綁定
                //this.sltedPrivileges.Items.Clear();
                this.sltedPrivileges.DataSource = ds.Tables[1];
                this.sltedPrivileges.DataTextField = "PrivilegeName";
                this.sltedPrivileges.DataValueField = "PrivilegeCode";
                this.sltedPrivileges.DataBind();
                if (roleId != 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    this.tbRole.Text = dr["RoleName"].ToString();
                    this.tbRoleMark.Text = dr["Remark"].ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Business.User user = new Business.User();
            Security sc = new Security();
            int roleId = 0;
            if (Request["RoleCode"] != null) {
                string roleID = sc.Decode64(Request["RoleCode"].ToString());
                roleId = Convert.ToInt32(roleID);
            }
            if (roleId == 0)
            {
                roleId = user.GetGenCode(genType);
            }
            int roleCode = roleId;
            string roleName = this.tbRole.Text.Trim();
            string remark = this.tbRoleMark.Text.Trim();
            string privileges = this.tbPriviles.Text;
            //StringBuilder sb = new StringBuilder();
            //foreach (ListItem li in this.sltPrivileges.Items)
            //{
            //    sb.Append(li.Value + ",");
            //}
            //string privileges = sb.ToString();
            bool isSuccess = user.ModifyRolePrivilege(roleCode, roleName, remark, privileges);
            string msg = "修改失敗";
            if (isSuccess)
            {
                msg = "修改成功";
            }
            Util.ShowMessage(msg);  
        }
    }
}