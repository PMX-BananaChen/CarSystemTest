using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CarSystemTest.Common;
using CarSystemTest.Business;
using CarSystemTest.Model;

namespace CarSystemTest.SystemSetting
{
    public partial class UserAdd : BasePage
    {
        //genType：2 表示獲取用戶id
        private const int genType = 2;

        private Business.User user = new Business.User();
        private Vendor vd = new Vendor();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = 0;
                Security sc = new Security();
                if (Request["UserID"] != null)
                {
                    userId = Convert.ToInt32(sc.Decode64(Request["UserID"].ToString()));
                }
                InitData();
                if (userId != 0)
                {
                    BindData(userId);
                }
            }
        }

        private void InitData()
        {
            DataSet ds = user.GetUserTypeGradeEmp();
            if (ds != null && ds.Tables.Count > 0)
            {
                //綁定用戶類型
                this.ddlUserType.DataSource = ds.Tables[0];
                this.ddlUserType.DataTextField = "UserTypeName";
                this.ddlUserType.DataValueField = "UserTypeCode";
                this.ddlUserType.DataBind();
                //綁定用戶級別
                this.ddlUserGrade.DataSource = ds.Tables[1];
                this.ddlUserGrade.DataTextField = "UserGradeName";
                this.ddlUserGrade.DataValueField = "UserGradeCode";
                this.ddlUserGrade.DataBind();
                //綁定用戶所屬部門或廠商
                this.ddlEmpVendor.DataSource = ds.Tables[2];
                this.ddlEmpVendor.DataTextField = "DepartmentName";
                this.ddlEmpVendor.DataValueField = "DepartmentCode";
                this.ddlEmpVendor.DataBind();
            }
            this.txtEffectiveTime.Value = "2016-01-01";
            this.txtExpireTime.Value = "2026-12-31";
        }

        private void BindData(int userId)
        {
            DataTable dtUser = user.GetUserInfoById(userId);
            Security sc = new Security();
            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                DataRow dr = dtUser.Rows[0];
                tbAccount.Text = dr["UserAccount"].ToString();                
                tbAccount.Enabled = false;
                tbChineseName.Text = dr["UserChineseName"].ToString();
                tbEnglishName.Text = dr["UserEnglishName"].ToString();
                ddlState.SelectedValue = dr["IsEnabled"].ToString();
                txtEffectiveTime.Value =String.Format("{0:yyyy-MM-dd}", dr["EffectiveTime"]);
                txtExpireTime.Value = String.Format("{0:yyyy-MM-dd}", dr["ExpireTime"]);
                tbMail.Text = dr["Email"].ToString();
                tbTel.Text = dr["Mobile"].ToString();
                tbExt.Text = dr["Tel"].ToString();
                if (dr["Sex"].ToString() == "1")
                {
                    rbMale.Checked = true;
                }
                else
                {
                    rbFemale.Checked = true;
                }
                string userType = dr["UserTypeCode"].ToString();
                ddlUserType.SelectedValue = userType;
                //用戶類型 3:司機  4:司機老闆 屬於外部人員
                if (userType == "3" || userType == "4")
                {
                    ddlEmpVendor.SelectedValue = dr["VendorCode"].ToString();
                }
                else
                {
                    ddlEmpVendor.SelectedValue = dr["DepartmentCode"].ToString();
                }
                ddlUserGrade.SelectedValue = dr["UserGradeCode"].ToString();
                tbRemark.Text = dr["Remark"].ToString();
                tbEmpNo.Text = dr["EmpNo"].ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserEntity ue = new UserEntity();
            Security sc = new Security();
            int userId = 0;
            if (Request["UserID"] != null)
            {
                userId = Convert.ToInt32(sc.Decode64(Request["UserID"].ToString()));
            }
            if (userId == 0)
            {
                userId = user.GetGenCode(genType);
            }
            ue.UserID = userId;
            ue.UserAccount = tbAccount.Text.Trim();
            ue.UserChineseName = tbChineseName.Text.Trim();
            ue.UserEnglishName = tbEnglishName.Text.Trim();
            ue.IsEnabled = Convert.ToChar(ddlState.SelectedValue);
            ue.EffectiveTime = Convert.ToDateTime(txtEffectiveTime.Value);
            ue.ExpireTime = Convert.ToDateTime(txtExpireTime.Value);
            ue.Email = tbMail.Text.Trim();
            ue.Mobile = tbTel.Text.Trim();
            ue.Tel = tbExt.Text.Trim();
            ue.Password = Security.EncryptPassword(tbPwd.Text.Trim(), "MD5");
            ue.Sex = rbMale.Checked ? '1' : '0';
            ue.UserTypeCode = Convert.ToInt32(ddlUserType.SelectedValue);
            ue.UserGradeCode = Convert.ToInt32(ddlUserGrade.SelectedValue);
            ue.Remark = tbRemark.Text.Trim();
            string empNo = tbEmpNo.Text.Trim();
            string depOrVendor = ddlEmpVendor.SelectedValue;
            bool isSuccess = user.ModifyUserInfo(ue, empNo, depOrVendor);
            string msg = "操作失敗！";
            if (isSuccess)
            {
                msg = "操作成功";
            }
            Util.ShowMessage(msg);
        }

        protected void UserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userTypeCode = Convert.ToInt32(this.ddlUserType.SelectedValue);
            //userTypeCode 3:司機 4：司機老闆 非公司員工  綁定所屬廠商；其他綁定所屬部門
            if (userTypeCode == 3 || userTypeCode == 4)
            {
                this.ddlEmpVendor.Items.Clear();
                this.ddlEmpVendor.DataSource = vd.GetVendors();
                this.ddlEmpVendor.DataTextField = "VendorName";
                this.ddlEmpVendor.DataValueField = "VendorCode";
                this.ddlEmpVendor.DataBind();
            }
            else
            {
                this.ddlEmpVendor.Items.Clear();
                this.ddlEmpVendor.DataSource = user.GetAllDepts();
                this.ddlEmpVendor.DataTextField = "DepartmentName";
                this.ddlEmpVendor.DataValueField = "DepartmentCode";
                this.ddlEmpVendor.DataBind();
            }
        }
    }
}