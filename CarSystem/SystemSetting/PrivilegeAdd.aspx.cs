using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CarSystemTest.DBUtility;
using CarSystemTest.Common;
using CarSystemTest.Business;

namespace CarSystemTest.SystemSetting
{
    public partial class PrivilegeAdd : BasePage
    {
        private Business.User user = new Business.User();
        private const int genType = 4;
        protected void Page_Load(object sender, EventArgs e)
        {
            Security sc = new Security();
            if (!IsPostBack)
            {
                int privilegeId = 0;
                if (Request["PrivilegeCode"] != null)
                {
                    string privlgCode = sc.Decode64(Request["PrivilegeCode"]);
                    privilegeId = Convert.ToInt32(privlgCode);
                }
                InitData();
                if (privilegeId != 0)
                {
                    BindData(privilegeId);
                }
            }
        }

        private void InitData()
        {
            DataTable dtParentPrivileges = user.GetParentPrivileges();
            ddlPrivilege.DataSource = dtParentPrivileges;
            ddlPrivilege.DataTextField = "PrivilegeName";
            ddlPrivilege.DataValueField = "PrivilegeCode";
            ddlPrivilege.DataBind();
            ddlPrivilege.Items.Insert(0, new ListItem("無父級權限", "0"));
        }

        private void BindData(int privilegeId)
        {
            DataTable dtPrivilege = user.GetPrivilegeById(privilegeId);
            if (dtPrivilege != null && dtPrivilege.Rows.Count > 0)
            {
                DataRow dr = dtPrivilege.Rows[0];
                this.tbPrivilege.Text = dr["PrivilegeName"].ToString();
                this.tbChineseName.Text = dr["ChineseDisplayName"].ToString();
                this.tbEnglishName.Text = dr["EnglishDisplayName"].ToString();
                this.ddlPrivilege.SelectedValue = dr["ParentPrivilegeCode"].ToString();
                this.ddlPriority.SelectedValue = dr["Priority"].ToString();
                this.tbURL.Text = dr["ResourceURL"].ToString();
                this.rbShow.Checked = dr["IsMenu"].ToString() == "1" ? true : false;
                this.rbNotShow.Checked = !this.rbShow.Checked;
                this.rbSee.Checked = dr["IsSuperAdminDisplay"].ToString() == "1" ? true : false;
                this.rbNotSee.Checked = !this.rbSee.Checked;
                this.ddlPrivilege.Enabled = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Security sc = new Security();
            int privilegeCode = 0;
            if (Request["PrivilegeCode"] != null)
            {
                privilegeCode =Convert.ToInt32(sc.Decode64(Request["PrivilegeCode"].ToString()));
            }
            bool isAdd = false;
            if (privilegeCode == 0)
            {
                isAdd = true;
            }
            string message = "修改失敗";
            string[] tblName = new string[] { "sys_privilege" };
            DataSet dsPrivilege = DBSchema.GetTableSchema(tblName);

            if (dsPrivilege != null && dsPrivilege.Tables.Count > 0)
            {
                DataTable dtPrivilege = dsPrivilege.Tables[0];
                DataRow dr = dtPrivilege.NewRow();
                if (privilegeCode == 0)
                {
                    if (this.ddlPrivilege.SelectedValue == "0")
                    {
                        //獲取父級權限ID
                        privilegeCode = Convert.ToInt32(user.GetGenCode(genType));
                    }
                    else
                    {
                        //獲取父級權限對應的下一個子權限ID
                        short privilegeId = Convert.ToInt16(this.ddlPrivilege.SelectedValue);
                        privilegeCode = user.GetNextSonPrivilege(privilegeId);
                    }
                }
                dr["PrivilegeCode"] = privilegeCode;
                dr["PrivilegeName"] = this.tbPrivilege.Text.Trim();
                dr["ChineseDisplayName"] = this.tbChineseName.Text.Trim();
                dr["EnglishDisplayName"] = this.tbEnglishName.Text.Trim();
                dr["PrivilegeTypeCode"] = 1;
                dr["ParentPrivilegeCode"] = this.ddlPrivilege.SelectedValue;
                dr["Priority"] = this.ddlPriority.SelectedValue;
                dr["ResourceURL"] = this.tbURL.Text.Trim();
                dr["IsMenu"] = this.rbShow.Checked == true ? "1" : "0";
                dr["IsSuperAdminDisplay"] = this.rbSee.Checked == true ? "1" : "0";
                dtPrivilege.Rows.Add(dr);
                if (!isAdd)
                {
                    dtPrivilege.Rows[0].AcceptChanges();
                    dtPrivilege.Rows[0].SetModified();
                }
                else {
                    dtPrivilege.Rows[0].AcceptChanges();
                    dtPrivilege.Rows[0].SetAdded();
                }                
                dtPrivilege.TableName = "sys_privilege";
            }            
            bool isSuccess = user.ModifyPrivilege(dsPrivilege, tblName, isAdd);
            if (isSuccess)
            {
                message = "修改成功";
            }
            Util.ShowMessage(message);
        }
    }
}