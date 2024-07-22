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

namespace CarSystemTest.Car
{
    public partial class AuditLine : BasePage
    {
        private CarBiz cb = new CarBiz();
        private const int pageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userAccount = WebUser.CurrentUser.UserAccount;
                InitData(userAccount);
                GetData(1, userAccount);
            }
        }

        private void InitData(string userAccount)
        {
            //DataTable dtDispatcher= cb.GetAllDispatcher().Tables[0];
            DataTable dtDispatcher = cb.GetAllPerson(userAccount).Tables[0];
            if (dtDispatcher != null)
            {
                ddlDispatcher.DataSource = dtDispatcher;
                ddlDispatcher.DataTextField = "UserChineseName";
                ddlDispatcher.DataValueField = "dispatcher";
                ddlDispatcher.DataBind();
            }
            ddlDispatcher.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            int currentPage = 1;
            string userAccount = WebUser.CurrentUser.UserAccount;
            GetData(currentPage, userAccount);
        }

        private void SeachData(string applicationUser, string approveUser, string dispatcher, string isEnabled,string userAccount, int pageSize, int currentPage)
        {           
            DataSet dsAuditData = cb.GetAuditLine(applicationUser,approveUser,dispatcher,isEnabled, userAccount,pageSize, currentPage);
            {
                this.rptAuditData.DataSource = dsAuditData.Tables[0];
                this.rptAuditData.DataBind();

                DataTable dtRecord = dsAuditData.Tables[1];
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
            string userAccount = WebUser.CurrentUser.UserAccount;
            int currentPage = 1;
            GetData(currentPage, userAccount);
        }

        /// <summary>
        /// 上一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            string userAccount = WebUser.CurrentUser.UserAccount;
            int currentPage = Convert.ToInt32(this.lblCurrPage.Text);
            GetData((currentPage - 1), userAccount);
        }

        /// <summary>
        /// 下一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbNext_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(this.lblCurrPage.Text);
            string userAccount = WebUser.CurrentUser.UserAccount;
            GetData((currentPage + 1), userAccount);
        }

        /// <summary>
        /// 末頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLast_Click(object sender, EventArgs e)
        {
            string userAccount = WebUser.CurrentUser.UserAccount;
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
            GetData(lastPage,userAccount);
        }

        private void GetData(int page,string userAccount)
        {
            string applicationUser = this.txtAssistent.Value.Trim();
            string approveUser = this.txtLeader.Value.Trim();
            string dispatcher = this.ddlDispatcher.SelectedValue.Trim();
            string isEnabled = this.ddlIsEnabled.SelectedValue.Trim();

            SeachData(applicationUser,approveUser,dispatcher,isEnabled, userAccount,pageSize, page);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string userAccount = WebUser.CurrentUser.UserAccount;

            NPOI.HSSF.UserModel.HSSFWorkbook book = new HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("簽核線明細");

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
            row_1.CreateCell(0).SetCellValue("申請人");
            row_1.CreateCell(1).SetCellValue("申請人英文名");
            row_1.CreateCell(2).SetCellValue("第一階主管");
            row_1.CreateCell(3).SetCellValue("第一階主管英文名");
            row_1.CreateCell(4).SetCellValue("第二階主管");
            row_1.CreateCell(5).SetCellValue("第二階主管英文名");
            row_1.CreateCell(6).SetCellValue("派車人");
            row_1.CreateCell(7).SetCellValue("派車人英文名");
            row_1.CreateCell(8).SetCellValue("狀態");        

            for (int m = 0; m <= 8; m++)
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

         
            DataTable dt = GetData(userAccount);
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i - 1];
                IRow row = sheet.CreateRow(i);

                row.CreateCell(0).SetCellValue(dr["AssistentName"].ToString());
                row.CreateCell(1).SetCellValue(dr["AssistenEnglishName"].ToString());
                row.CreateCell(2).SetCellValue(dr["FirstLeaderName"].ToString());
                row.CreateCell(3).SetCellValue(dr["FirstLeaderEnglishName"].ToString());
                row.CreateCell(4).SetCellValue(dr["SecondLeaderName"].ToString());
                row.CreateCell(5).SetCellValue(dr["SecondLeaderEnglishName"].ToString());
                row.CreateCell(6).SetCellValue(dr["DispatcherName"].ToString());
                row.CreateCell(7).SetCellValue(dr["DispatcherEnglishName"].ToString());
                row.CreateCell(8).SetCellValue(dr["IsEnabled"].ToString());              

                for (int j = 0; j <= 8; j++)
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

        private DataTable GetData(string userAccount)
        {
            string applicationUser = this.txtAssistent.Value.Trim();
            string approveUser = this.txtLeader.Value.Trim();
            string dispatcher = this.ddlDispatcher.SelectedValue.Trim();
            string isEnabled = this.ddlIsEnabled.SelectedValue.Trim();           

            DataSet ds = cb.GetAuditLine(applicationUser, approveUser, dispatcher, isEnabled, userAccount, 10000, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
    }
}