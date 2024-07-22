using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CarSystemTest.Business;
using CarSystemTest.Common;

namespace CarSystemTest.Car
{
    public partial class BindingCar : BasePage
    {
        private string businessCode = "";
        private string suggestVehicle = "";
        private string userAccount = "";
        private CarBiz cb = new CarBiz();
        protected void Page_Load(object sender, EventArgs e)
        {
            Security sec = new Security();
            businessCode = sec.Decode64(Request["BusinessCode"]);
            suggestVehicle = sec.Decode64(Request["SuggestVehicle"]);
            userAccount = WebUser.CurrentUser.UserAccount;
            if (!IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {
            DataSet ds = cb.GetAppDetails(businessCode);
            string vendorCode = ds.Tables["Vehicle_Master"].Rows[0]["VendorCode"].ToString();
            DataTable dtRemark = ds.Tables["Vehicle_Remark"];
            string message = "";
            if (dtRemark != null && dtRemark.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRemark.Rows)
                {
                    message = message + dr["UserChineseName"].ToString() + " " + dr["Operation"].ToString() + "(" + Convert.ToDateTime(dr["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ")" + "：" + dr["Remark"].ToString() + "<br/>";
                }
            }
            this.lblSuggestion.Text = message;
            Vendor v = new Vendor();
            DataTable dt = v.GetVendorCars(vendorCode);
            this.rptVehicle.DataSource = dt;
            this.rptVehicle.DataBind();
        }

        public bool SetSuggestVehicle(object vehicleNoObj)
        {
            string vehicle = Convert.ToString(vehicleNoObj);
            if (vehicle == suggestVehicle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vehilceNo = "";
            for (int i = 0; i < this.rptVehicle.Items.Count; i++)
            {
                if (((RadioButton)rptVehicle.Items[i].FindControl("rbVehicle")).Checked)
                {
                    vehilceNo = ((HtmlInputText)rptVehicle.Items[i].FindControl("txtVehicleNO")).Value;
                    break;
                }
            }
            string remark = this.remarkTxt.Value.Trim();
            bool isSuccess = cb.BindingCar(userAccount, businessCode, vehilceNo,remark);
            if (isSuccess)
            {
                lblMsg.Text = "保存成功！";
            }
            else
            {
                lblMsg.Text = "保存失敗！";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('.tip').fadeIn(300);</script>");
        }
    }
}