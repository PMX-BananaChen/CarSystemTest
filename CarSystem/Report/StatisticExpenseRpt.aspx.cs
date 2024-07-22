using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CarSystemTest.Common;
using CarSystemTest.Business;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace CarSystem.Report
{
    public partial class StatisticExpenseRpt : BasePage
    {
        private CarBiz cb = new CarBiz();
        private const int pageSize = 20;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               InitData();
                GetData(1);
            }
        }

        private void InitData()
        {
            string userAccount = WebUser.CurrentUser.UserAccount;
            txtStartDay.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtEndDay.Value = DateTime.Now.ToString("yyyy-MM-dd");

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
            ddlCostCenter.Items.Insert(0, new ListItem(" ", ""));

            //初始化目的地
            Region r = new Region();
            DataTable dtRegion = r.GetRegions(userAccount);
            if (dtRegion != null)
            {
                ddlDestination.DataSource = dtRegion;
                ddlDestination.DataTextField = "RegionDetailName";
                ddlDestination.DataValueField = "RegionCode";
                ddlDestination.DataBind();
            }
            ddlDestination.Items.Insert(0, new ListItem(" ", ""));

            //初始化廠商
            Vendor v = new Vendor();
            DataTable dtVendors = v.GetVendors();
            if (dtVendors != null)
            {
                ddlVendor.DataSource = dtVendors;
                ddlVendor.DataTextField = "VendorShortName";
                ddlVendor.DataValueField = "VendorCode";
                ddlVendor.DataBind();
            }
            ddlVendor.Items.Insert(0, new ListItem(" ", ""));

            ddlDriver.Items.Insert(0, new ListItem(" ", ""));

            //初始化派車用途
            CarInfo ci = new CarInfo();
            DataTable dtPurpose = ci.GetCarPurpose();
            if (dtPurpose != null)
            {
                ddlPurpose.DataSource = dtPurpose;
                ddlPurpose.DataTextField = "VehiclePurposeName";
                ddlPurpose.DataValueField = "VehiclePurposeCode";
                ddlPurpose.DataBind();
            }
            ddlPurpose.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            int currentPage = 1;
            GetData(currentPage);
        }

        private void SeachData(string startDay, string endDay, string costCenter, string destinationCode, string vendorCode, string vehicleNo, string vehiclePurpose, string businessType, int pageSize, int currentPage)
        {
            string userAccount = WebUser.CurrentUser.UserAccount;
            //string userAccount = @"pcn\ang.yu";
            DataSet dsReportData = cb.GetStaticExpense(userAccount, startDay, endDay, costCenter, destinationCode, vendorCode, vehicleNo, vehiclePurpose, businessType, pageSize, currentPage);
            {
                this.rptReportData.DataSource = dsReportData.Tables[0];
                this.rptReportData.DataBind();

                DataTable dtRecord = dsReportData.Tables[1];
                DataRow dr = dtRecord.Rows[0];
                this.lblTotal.Text = dr["TtlRecord"].ToString();
                this.lblCurrPage.Text = dr["CurrentPage"].ToString();
                this.lblTttPage.Text = dr["TtlPage"].ToString();
            }
        }

        /// <summary>
        /// 首頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbFirst_Click(object sender, EventArgs e)
        {
            int currentPage = 1;
            GetData(currentPage);
        }

        /// <summary>
        /// 上一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(this.lblCurrPage.Text);
            GetData((currentPage - 1));
        }

        /// <summary>
        /// 下一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbNext_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(this.lblCurrPage.Text);
            GetData((currentPage + 1));
        }

        /// <summary>
        /// 末頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLast_Click(object sender, EventArgs e)
        {
            int lastPage = 0;
            int leftRecord = Convert.ToInt32(lblTotal.Text) % pageSize;
            if (leftRecord != 0)
            {
                lastPage = Convert.ToInt32(lblTotal.Text) / pageSize + 1;
            }
            else
            {
                lastPage = Convert.ToInt32(lblTotal.Text) / pageSize;
            }
            GetData(lastPage);
        }

        private void GetData(int page)
        {
            string startDay = this.txtStartDay.Value;
            string endDay = this.txtEndDay.Value;
            string costCenter = this.ddlCostCenter.SelectedValue;
            string destination = this.ddlDestination.SelectedValue;
            string vendor = this.ddlVendor.SelectedValue;
            string vehicleNo = this.ddlDriver.SelectedValue;
            string purpose = this.ddlPurpose.SelectedValue;
            string businessType = this.ddlBusinessType.SelectedValue;

            SeachData(startDay, endDay, costCenter, destination, vendor, vehicleNo, purpose, businessType, pageSize, page);
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vendor = ddlVendor.SelectedValue;
            Vendor v = new Vendor();
            if (vendor != "")
            {
                DataTable dtCars = v.GetVendorCars(vendor);
                if (dtCars != null)
                {
                    ddlDriver.Items.Clear();
                    ddlDriver.DataSource = dtCars;
                    ddlDriver.DataTextField = "UserChineseName";
                    ddlDriver.DataValueField = "VehicleNO";
                    ddlDriver.DataBind();

                    ddlDriver.Items.Insert(0, new ListItem(" ", ""));
                }
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("月結費用明細");

            ICellStyle HeadercellStyle = book.CreateCellStyle();
            HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            //字体
            NPOI.SS.UserModel.IFont headerfont = book.CreateFont();
            headerfont.Boldweight = (short)FontBoldWeight.Bold;
            HeadercellStyle.SetFont(headerfont);

            NPOI.SS.UserModel.IRow row_1 = sheet.CreateRow(0);
            row_1.CreateCell(0).SetCellValue("申請單號");
            row_1.CreateCell(1).SetCellValue("申請單類型");
            row_1.CreateCell(2).SetCellValue("申請人");
            row_1.CreateCell(3).SetCellValue("用車人");
            row_1.CreateCell(4).SetCellValue("成本中心");
            row_1.CreateCell(5).SetCellValue("出行時間");
            row_1.CreateCell(6).SetCellValue("出發地");
            row_1.CreateCell(7).SetCellValue("出發地详细地址");
            row_1.CreateCell(8).SetCellValue("目的地");
            row_1.CreateCell(9).SetCellValue("目的地詳細地址");
            row_1.CreateCell(10).SetCellValue("廠商");
            row_1.CreateCell(11).SetCellValue("司機");
            row_1.CreateCell(12).SetCellValue("車牌號");
            row_1.CreateCell(13).SetCellValue("派車用途");
            row_1.CreateCell(14).SetCellValue("附註(申請人)");
            row_1.CreateCell(15).SetCellValue("基本費用");
            row_1.CreateCell(16).SetCellValue("里程費");
            row_1.CreateCell(17).SetCellValue("高速費");
            row_1.CreateCell(18).SetCellValue("候時費");
            row_1.CreateCell(19).SetCellValue("繞路費");
            row_1.CreateCell(20).SetCellValue("合計");
            row_1.CreateCell(21).SetCellValue("備註");

            for (int m = 0; m <= 20; m++)
            {
                row_1.Cells[m].CellStyle = HeadercellStyle;
            }

            ICellStyle cellStyle = book.CreateCellStyle();

            //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;


            NPOI.SS.UserModel.IFont cellfont = book.CreateFont();
            cellfont.Boldweight = (short)FontBoldWeight.Normal;
            cellStyle.SetFont(cellfont);

            DataTable dt = GetData();
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i - 1];
                IRow row = sheet.CreateRow(i);

                row.CreateCell(0).SetCellValue(dr["BusinessCode"].ToString());
                row.CreateCell(1).SetCellValue(dr["BusinessTypeName"].ToString());
                row.CreateCell(2).SetCellValue(dr["ApplicationUser"].ToString());
                row.CreateCell(3).SetCellValue(dr["PassengerName"].ToString());
                row.CreateCell(4).SetCellValue(dr["CostCenter"].ToString());
                row.CreateCell(5).SetCellValue(dr["RequireStartTime"].ToString());
                row.CreateCell(6).SetCellValue(dr["DepartureRegion"].ToString());
                row.CreateCell(7).SetCellValue(dr["DepartureDetailAddress"].ToString());
                row.CreateCell(8).SetCellValue(dr["DestinationRegion"].ToString());
                row.CreateCell(9).SetCellValue(dr["DetailAddress"].ToString());
                row.CreateCell(10).SetCellValue(dr["VendorShortName"].ToString());
                row.CreateCell(11).SetCellValue(dr["DriverUser"].ToString());
                row.CreateCell(12).SetCellValue(dr["VehicleNO"].ToString());
                row.CreateCell(13).SetCellValue(dr["VehiclePurposeName"].ToString());
                row.CreateCell(14).SetCellValue(dr["AppUserRemark"].ToString());
                row.CreateCell(15).SetCellValue(dr["BaseFee"].ToString());
                row.CreateCell(16).SetCellValue(dr["MileageFee"].ToString());
                row.CreateCell(17).SetCellValue(dr["TollFee"].ToString());
                row.CreateCell(18).SetCellValue(dr["WaitingFee"].ToString());
                row.CreateCell(19).SetCellValue(dr["DetourFee"].ToString());
                row.CreateCell(20).SetCellValue(dr["SubtotalFee"].ToString());
                row.CreateCell(21).SetCellValue(dr["ZwRemark"].ToString());

                for (int j = 0; j <= 20; j++)
                {
                    row.Cells[j].CellStyle = cellStyle;
                }
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }

        private DataTable GetData()
        {
            string startDay = this.txtStartDay.Value;
            string endDay = this.txtEndDay.Value;
            string costCenter = this.ddlCostCenter.SelectedValue;
            string destination = this.ddlDestination.SelectedValue;
            string vendor = this.ddlVendor.SelectedValue;
            string vehicleNo = this.ddlDriver.SelectedValue;
            string purpose = this.ddlPurpose.SelectedValue;
            string businessType = this.ddlBusinessType.SelectedValue;

            string userAccount = WebUser.CurrentUser.UserAccount;

            DataSet ds = cb.GetAllStaticExpense(userAccount, startDay, endDay, costCenter, destination, vendor, vehicleNo, purpose, businessType);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
    }
}