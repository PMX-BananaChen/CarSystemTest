using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CarSystemTest.Business;
using CarSystemTest.Common;

namespace CarSystemTest.GPS
{
    public partial class CurrentPosition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitData();
                UserView();
            }
        }

        private void InitData()
        {
            string userAccount = WebUser.CurrentUser.UserAccount;
            Vendor v = new Vendor();
            //初始化廠商
            DataTable dtVendors = v.GetAvailabeVendors(userAccount);
            ddlVendor.DataSource = dtVendors;
            ddlVendor.DataTextField = "VendorAndLinkMan";
            ddlVendor.DataValueField = "VendorCode";
            ddlVendor.DataBind();

            //初始化廠商下對應的車輛
            BindingVehicles(ddlVendor.SelectedValue);
        }

        private void BindingVehicles(string vendorCode)
        {
            Vendor v = new Vendor();
            DataTable dtVehicles = v.GetVendorCars(vendorCode);
            ddlVehicle.DataSource = dtVehicles;
            ddlVehicle.DataTextField = "VehicleName";
            ddlVehicle.DataValueField = "VehicleNO";
            ddlVehicle.DataBind();
        }

        private void UserView()
        {
            User u = new User();
            Vendor v = new Vendor();
            DataTable dtRoles = u.GetRolesByUserId(WebUser.CurrentUser.UserID);
            string userAccount = WebUser.CurrentUser.UserAccount;

            //10:助理;11:主管;12:總務;13:廠商;14:司機;15:調度人;
            if (dtRoles.Select("RolePrivilegeCode=14").Length > 0)
            {
                DataTable dtVehicle = v.GetCarByUserAccount(userAccount);
                if (dtVehicle != null)
                {
                    DataRow dr = dtVehicle.Rows[0];
                    this.ddlVendor.SelectedValue = dr["VendorCode"].ToString();
                    this.ddlVehicle.SelectedValue = dr["VehicleNo"].ToString();
                }
                this.ddlVendor.Enabled = false;
                this.ddlVehicle.Enabled = false;
            }
            if (dtRoles.Select("RolePrivilegeCode=13").Length > 0)
            {
                DataTable dtVendor = v.GetVendorInfo(userAccount);
                this.ddlVendor.Enabled = true;
                if (dtVendor != null)
                {
                    DataRow dr = dtVendor.Rows[0];
                    ddlVendor.SelectedValue = dr["VendorCode"].ToString();
                }
                this.ddlVendor.Enabled = false;
            }
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindingVehicles(ddlVendor.SelectedValue);
        }
    }
}