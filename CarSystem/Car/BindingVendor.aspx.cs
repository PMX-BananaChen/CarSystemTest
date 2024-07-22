using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CarSystemTest.Common;
using CarSystemTest.Business;

namespace CarSystemTest.Car
{
    public partial class BindingVendor : BasePage
    {
        private string businessCode = "";
        private string userAccount = "";
        private string suggestVendor = "";
        private string suggestVehicle = "";
        private CarBiz cb = new CarBiz();
        protected void Page_Load(object sender, EventArgs e)
        {
            Security sec = new Security();
            businessCode = sec.Decode64(Request["BusinessCode"].ToString());
            userAccount = WebUser.CurrentUser.UserAccount;
            DataSet ds = cb.GetAppDetails(businessCode);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables["Vehicle_Master"].Rows[0];
                suggestVendor = dr["SuggestVendorCode"] == DBNull.Value ? "" : dr["SuggestVendorCode"].ToString();
                suggestVehicle = dr["SuggestVehicleNO"].ToString();
                string vendor = dr["VendorCode"] == DBNull.Value ? "" : dr["VendorCode"].ToString();
                if (!string.IsNullOrEmpty(vendor))
                {
                    this.lblVendor.Text = "1"; //表示已經設置廠商
                }
                else
                {
                    this.lblVendor.Text = "0";//表示未設置過廠商
                }
                DataTable dtRemark = ds.Tables["Vehicle_Remark"];
                string message = "";
                if (dtRemark != null && dtRemark.Rows.Count > 0)
                {
                    foreach (DataRow drow in dtRemark.Rows)
                    {
                        message = message + drow["UserChineseName"].ToString() + " " + drow["Operation"].ToString() + "(" + Convert.ToDateTime(drow["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ")" + "：" + drow["Remark"].ToString() + "<br/>";
                    }
                }
                this.lblSuggestion.Text = message;
            }
            if (!IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {
            //初始化廠商信息
            Vendor v = new Vendor();
            DataTable dt = v.GetAvailabeVendors(userAccount);
            this.rptVendor.DataSource = dt;
            this.rptVendor.DataBind();

            //初始化拼車單信息
            DataTable dtCarpoolApps = cb.GetCarpoolApps(businessCode);
            this.rptCarpoolApp.DataSource = dtCarpoolApps;
            this.rptCarpoolApp.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string businessA = businessCode;
            string businessB = "";
            string businessC = "";
            string vendorCode = "";
            string vehicleNO = "";
            string remark = "";
            if (this.rbCarpool.Checked)
            {
                for (int j = 0; j < this.rptCarpoolApp.Items.Count; j++)
                {
                    if (((CheckBox)rptCarpoolApp.Items[j].FindControl("cbCarpoolApp")).Checked)
                    {
                        if (businessB == "")
                        {
                            businessB = ((HtmlInputText)rptCarpoolApp.Items[j].FindControl("txtBusinessCode")).Value;
                        }
                        else
                        {
                            businessC = ((HtmlInputText)rptCarpoolApp.Items[j].FindControl("txtBusinessCode")).Value;
                        }
                        if (businessC != "")
                        {
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < this.rptVendor.Items.Count; i++)
            {
                if (((RadioButton)rptVendor.Items[i].FindControl("rbVendor")).Checked)
                {
                    vendorCode = ((HtmlInputText)rptVendor.Items[i].FindControl("txtVendorCode")).Value;
                    break;
                }
            }

            for (int m = 0; m < this.rptVehicle.Items.Count; m++)
            {
                if (((RadioButton)rptVehicle.Items[m].FindControl("rbVehicle")).Checked)
                {
                    vehicleNO = ((HtmlInputText)rptVehicle.Items[m].FindControl("txtVehicleNO")).Value;
                    break;
                }
            }
            remark = this.remarkTxt.Value.Trim();
            bool isSuccess = cb.BindingVendor(userAccount, businessA, businessB, businessC, vendorCode, vehicleNO, remark);
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

        /// <summary>
        /// 綁定建議廠商
        /// </summary>
        /// <param name="vendorCode"></param>
        /// <returns></returns>
        public bool SetSuggestVendor(object vendorCodeObj)
        {
            Vendor v = new Vendor();
            string vendorCode = Convert.ToString(vendorCodeObj);
            if (vendorCode == suggestVendor)
            {
                DataTable dt = v.GetVendorCars(vendorCode);
                this.rptVehicle.DataSource = dt;
                this.rptVehicle.DataBind();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SetSuggestVehicle(object vehicleNoObj)
        {
            //string vehicle = Convert.ToString(vehicleNoObj);
            //if (vehicle == suggestVehicle)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            //暫不綁定車輛
            return false;
        }

        protected void rbVendor_CheckedChanged(object sender, EventArgs e)
        {
            Vendor v = new Vendor();
            int count = rptVendor.Items.Count;
            string vendorCode = "";
            for (int i = 0; i < count; i++)
            {
                RadioButton rdb = rptVendor.Items[i].FindControl("rbVendor") as RadioButton;
                if (rdb.Checked)
                {
                    vendorCode = ((HtmlInputText)rptVendor.Items[i].FindControl("txtVendorCode")).Value;
                    DataTable dt = v.GetVendorCars(vendorCode);
                    this.rptVehicle.DataSource = dt;
                    this.rptVehicle.DataBind();
                    break;
                }
            }
        }

    }
}