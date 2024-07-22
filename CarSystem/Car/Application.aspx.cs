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
using System.Collections;

namespace CarSystemTest.Car
{
    public partial class Application : BasePage
    {
        private CarBiz cb = new CarBiz();
        private string businessCode;
        private DataSet dsAppSchema = null;
        private string userAccount;
        protected void Page_Load(object sender, EventArgs e)
        {
            userAccount = WebUser.CurrentUser.UserAccount;
            Security sec = new Security();
            object obj = Request["BusinessCode"];
            if (obj == null)
            {
                businessCode = "0";
            }
            else
            {
                businessCode = sec.Decode64(obj.ToString());
            }
            dsAppSchema = cb.GetAppSchema();
            lblBusinessCode.Text = businessCode=="0"?"":businessCode;
            if (!IsPostBack)
            {
                InitData();
                if (businessCode != "0")
                {
                    BindData(businessCode);
                    btnCancel.Visible = true;
                }
            }
        }

        private void InitData()
        {
            //初始化建議車輛
            CarInfo ci = new CarInfo();
            DataTable dtVehicle = ci.GetSuggestCar(userAccount);
            if (dtVehicle != null)
            {
                ddlVehicle.DataSource = dtVehicle;
                ddlVehicle.DataTextField = "VehicleName";
                ddlVehicle.DataValueField = "VehicleNO";
                ddlVehicle.DataBind();

                ddlVehicle.Items.Insert(0, new ListItem(" ", " "));
            }

            //初始化出發地
            Region r = new Region();
            DataTable dtRegion = r.GetRegions(userAccount);
            if (dtRegion != null)
            {
                ddlDeparture.DataSource = dtRegion;
                ddlDeparture.DataTextField = "RegionDetailName";
                ddlDeparture.DataValueField = "RegionCode";
                ddlDeparture.DataBind();
            }

            //初始化乘客屬性
            CarBiz cb = new CarBiz();
            DataTable dtAttribute = cb.GetPassengerAttribute();
            if (dtAttribute != null)
            {
                ddlAttribute.DataSource = dtAttribute;
                ddlAttribute.DataTextField = "PassengerAttributeCodeName";
                ddlAttribute.DataValueField = "PassengerAttributeCode";
                ddlAttribute.DataBind();
            }

            //初始化派車用途
            DataTable dtPurpose = ci.GetCarPurpose();
            if (dtPurpose != null)
            {
                ddlPurpose.DataSource = dtPurpose;
                ddlPurpose.DataTextField = "VehiclePurposeName";
                ddlPurpose.DataValueField = "VehiclePurposeCode";
                ddlPurpose.DataBind();
            }

            //初始化成本中心
            Corporation c = new Corporation();
            DataTable dtCostCenter = c.GetAllCostCenter(userAccount);
            if (dtCostCenter != null)
            {
                ddlCostCenter.DataSource = dtCostCenter;
                ddlCostCenter.DataTextField = "CostCenter";
                ddlCostCenter.DataValueField = "CostCenter";
                ddlCostCenter.DataBind();
            }

            //string userAccount = WebUser.CurrentUser.UserAccount;
            DataTable dtDefaultCostCenter = c.GetDefaultCostCenter(userAccount);
            if (dtDefaultCostCenter != null && dtDefaultCostCenter.Rows.Count > 0)
            {
                string costCenter = dtDefaultCostCenter.Rows[0]["CostCenter"].ToString();
                ddlCostCenter.SelectedValue = costCenter;
            }
        }

        private void BindData(string businessCode)
        {
            DataSet dsApp = cb.GetAppDetails(businessCode);
            DataTable dtPassenger = new DataTable();
            DataTable dtDestination = new DataTable();
            DataTable dtMaster = new DataTable();
            DataTable dtRemark = new DataTable();
            if (dsApp != null && dsApp.Tables.Count > 0)
            {
                dtMaster = dsApp.Tables["Vehicle_Master"];
                dtPassenger = dsApp.Tables["Vehicle_Passenger"];
                dtDestination = dsApp.Tables["Vehicle_Destination"];
                dtRemark = dsApp.Tables["Vehicle_Remark"];

                if (dtMaster != null && dtMaster.Rows.Count > 0)
                {
                    DataRow dr = dtMaster.Rows[0];
                    rbSecond.Checked = dr["BusinessTypeCode"].ToString() == "2" ? true : false;
                    // rbDouble.Checked = dr["IsOneWay"].ToString() == "0" ? true : false;
                    ddlVehicle.SelectedValue = dr["SuggestVehicleNo"].ToString();
                    txtTime.Value = Convert.ToDateTime(dr["RequireStartTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    ddlDeparture.SelectedValue = dr["DepartureRegionCode"].ToString();
                    tbDepartureDetail.Text = dr["departureDetailAddress"].ToString();
                    ddlAttribute.SelectedValue = dr["PassengerAttributeCode"].ToString();
                    rbNoPay.Checked = dr["IsPassengerPay"].ToString() == "0" ? true : false;
                    ddlPurpose.SelectedValue = dr["VehiclePurposeCode"].ToString();
                    ddlCostCenter.SelectedValue = dr["CostCenter"].ToString();
                }

                string message = "";
                if (dtRemark != null && dtRemark.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRemark.Rows)
                    {
                        message = message + dr["UserChineseName"].ToString() + ":" + dr["Remark"].ToString() + "<br/>";
                    }
                }
                this.lblSuggestion.Text = message;

                rptPassenger.DataSource = dtPassenger;
                rptPassenger.DataBind();

                rptDestination.DataSource = dtDestination;
                rptDestination.DataBind();
            }



        }

        protected void rptPassenger_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptPassenger_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataTable dtPassenger = dsAppSchema.Tables["Vehicle_Passenger"].Clone();
                GetPassengerInfoData(ref dtPassenger);

                dtPassenger.Rows.RemoveAt(e.Item.ItemIndex);
                rptPassenger.DataSource = dtPassenger;
                rptPassenger.DataBind();
            }
        }

        private void GetPassengerInfoData(ref DataTable dt)
        {
            DataRow dr;
            for (int i = 0; i < rptPassenger.Items.Count; i++)
            {
                dr = dt.NewRow();
                if (((Label)rptPassenger.Items[i].FindControl("lblName")).Text.Trim() != "")
                {
                    dr["PassengerName"] = ((Label)rptPassenger.Items[i].FindControl("lblName")).Text.Trim();
                }
                else
                {
                    dr["PassengerName"] = DBNull.Value;
                }
                if (((Label)rptPassenger.Items[i].FindControl("lblEmpNO")).Text.Trim() != "")
                {
                    dr["EmpNO"] = ((Label)rptPassenger.Items[i].FindControl("lblEmpNO")).Text.Trim();
                }
                else
                {
                    dr["EmpNO"] = DBNull.Value;
                }
                if (((Label)rptPassenger.Items[i].FindControl("lblMobile")).Text.Trim() != "")
                {
                    dr["Mobile"] = ((Label)rptPassenger.Items[i].FindControl("lblMobile")).Text.Trim();
                }
                else
                {
                    dr["Mobile"] = DBNull.Value;
                }
                if (((Label)rptPassenger.Items[i].FindControl("lblRemark")).Text.Trim() != "")
                {
                    dr["Remark"] = ((Label)rptPassenger.Items[i].FindControl("lblRemark")).Text.Trim();
                }
                else
                {
                    dr["Remark"] = DBNull.Value;
                }
                dt.Rows.Add(dr);
            }
        }

        protected void NewPassenger_Click(object sender, EventArgs e)
        {
            DataTable dtPassenger = dsAppSchema.Tables["Vehicle_Passenger"].Clone();
            GetPassengerInfoData(ref dtPassenger);

            DataRow dr = dtPassenger.NewRow();
            dr["PassengerName"] = tbName.Text.Trim();
            dr["EmpNO"] = tbEmpNO.Text.Trim();
            dr["Mobile"] = tbMobile.Text.Trim();
            dr["Remark"] = tbRemark.Text.Trim();
            dtPassenger.Rows.Add(dr);

            rptPassenger.DataSource = dtPassenger;
            rptPassenger.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtAppMaster = dsAppSchema.Tables["Vehicle_Master"];
            DataTable dtDestination = dsAppSchema.Tables["Vehicle_Destination"];
            DataTable dtPassenger = dsAppSchema.Tables["Vehicle_Passenger"];

            DataRow dr = dtAppMaster.NewRow();
            //string userAccount = WebUser.CurrentUser.UserAccount;
            //long businessCode = cb.GetBizCode();
            if (businessCode == "0")
            {
                businessCode = cb.GetBizCode().ToString();
            }
            dr["BusinessCode"] = businessCode;
            dr["ApplicationUserAccount"] = userAccount;
            //dr["RequireStartTime"] = tbTime.Text.Trim();
            dr["RequireStartTime"] = txtTime.Value.Trim();
            dr["DepartureRegionCode"] = ddlDeparture.SelectedValue;
            dr["departureDetailAddress"] = tbDepartureDetail.Text.Trim();
            //dr["IsOneWay"] = rbDouble.Checked ? "0" : "1";    //"0"：表示往返；"1":表示單程
            dr["IsOneWay"] = "1";
            dr["BusinessTypeCode"] = rbNormal.Checked ? "1" : "2"; //"1":表示正常單；"2":表示補單
            dr["VehiclePurposeCode"] = ddlPurpose.SelectedValue;
            dr["IsPassengerPay"] = rbNoPay.Checked ? "0" : "1";
            dr["PassengerAttributeCode"] = ddlAttribute.SelectedValue;
            dr["SuggestVehicleNo"] = ddlVehicle.SelectedValue;
            dr["CostCenter"] = ddlCostCenter.SelectedValue;
            dtAppMaster.Rows.Add(dr);

            string remark = remarkTxt.Value.Trim();


            GetDestinationInfoData(ref dtDestination);
            GetPassengerInfoData(ref dtPassenger);

            //目的地中，同一區域的詳細地址合併到一起
            DataTable dt = dtDestination.Clone();
            dt.PrimaryKey = new DataColumn[] { dt.Columns["DestinationRegionCode"] };

            foreach (DataRow drow in dtDestination.Rows)
            {
                DataRow dtRow = dt.Rows.Find(new object[] { drow["DestinationRegionCode"].ToString() });
                if (dtRow == null)
                {
                    dt.Rows.Add(drow.ItemArray);
                }
                else
                {
                    dtRow["DetailAddress"] = dtRow["DetailAddress"].ToString() + "," + drow["DetailAddress"].ToString();
                    dtRow["Remark"] = dtRow["Remark"].ToString() + " " + drow["Remark"].ToString();
                }
            }
            dtDestination.Rows.Clear();
            foreach (DataRow dtr in dt.Rows)
            {
                dtDestination.Rows.Add(dtr.ItemArray);
            }


            bool isSuccess = cb.SubmitForm(dsAppSchema, remark);
            if (isSuccess)
            {
                //增加发消息到手机端的功能
                lblMsg.Text = "提交申請單成功！";
            }
            else
            {
                lblMsg.Text = "提交申請單失敗！";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('.tip').fadeIn(300);</script>");
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.tip').fadeIn(300);</script>");
        }

        protected void rptDestination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataTable dtDestination = dsAppSchema.Tables["Vehicle_Destination"].Clone();
                GetDestinationInfoData(ref dtDestination);

                dtDestination.Rows.RemoveAt(e.Item.ItemIndex);
                rptDestination.DataSource = dtDestination;
                rptDestination.DataBind();
            }
        }

        protected void rptDestination_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string userAccount = WebUser.CurrentUser.UserAccount;
            if (e.Item.ItemIndex != -1)
            {
                DropDownList ddlCity = (DropDownList)e.Item.FindControl("ddlCity");
                string destinationCode = ((HtmlInputHidden)e.Item.FindControl("txtCity")).Value;
                Region r = new Region();
                DataTable dtRegion = r.GetRegions(userAccount);
                if (dtRegion != null)
                {
                    ddlCity.DataSource = dtRegion;
                    ddlCity.DataTextField = "RegionDetailName";
                    ddlCity.DataValueField = "RegionCode";
                    ddlCity.DataBind();
                    ddlCity.SelectedValue = destinationCode;
                }
            }
        }

        protected void NewDestination_Click(object sender, EventArgs e)
        {

            DataTable dtDestination = dsAppSchema.Tables["Vehicle_Destination"].Clone();
            GetDestinationInfoData(ref dtDestination);

            DataRow dr = dtDestination.NewRow();
            dr["DestinationRegionCode"] = DBNull.Value;
            dr["DetailAddress"] = "";
            dtDestination.Rows.Add(dr);

            rptDestination.DataSource = dtDestination;
            rptDestination.DataBind();
        }

        private void GetDestinationInfoData(ref DataTable dt)
        {
            DataRow dr;
            for (int i = 0; i < rptDestination.Items.Count; i++)
            {
                dr = dt.NewRow();
                if (((DropDownList)rptDestination.Items[i].FindControl("ddlCity")).SelectedValue.Trim() != "")
                {
                    dr["DestinationRegionCode"] = ((DropDownList)rptDestination.Items[i].FindControl("ddlCity")).SelectedValue.Trim();
                }
                else
                {
                    dr["DestinationRegionCode"] = DBNull.Value;
                }
                if (((HtmlInputText)rptDestination.Items[i].FindControl("txtDestination")).Value.Trim() != "")
                {
                    dr["DetailAddress"] = ((HtmlInputText)rptDestination.Items[i].FindControl("txtDestination")).Value.Trim();
                }
                else
                {
                    dr["DetailAddress"] = DBNull.Value;
                }
                if (((HtmlInputText)rptDestination.Items[i].FindControl("txtRemark")).Value.Trim() != "")
                {
                    dr["Remark"] = ((HtmlInputText)rptDestination.Items[i].FindControl("txtRemark")).Value.Trim();
                }
                else
                {
                    dr["Remark"] = DBNull.Value;
                }
                dt.Rows.Add(dr);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //string businessCode = Request["BusinessCode"].ToString();
            //string userAccount = WebUser.CurrentUser.UserAccount;
            string remark = this.remarkTxt.Value;
            bool isSuccess = cb.CancelForm(businessCode, userAccount,remark);
            if (isSuccess)
            {
                lblMsg.Text = "取消申請單成功！";
            }
            else
            {
                lblMsg.Text = "取消申請單失敗！";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.warning').fadeIn(300);$('.tip').fadeIn(300);</script>");
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.tip').fadeIn(300);</script>");
        }
    }
}