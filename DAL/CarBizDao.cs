using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using CarSystemTest.DBUtility;


namespace CarSystemTest.DAL
{
    public class CarBizDao
    {
        Database db = null;

        public CarBizDao()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 獲取派車主單的流水號
        /// </summary>
        /// <returns></returns>
        public long GetBizCode()
        {
            CommonGen cg = new CommonGen();
            return cg.GetGenCode(1);
        }

        /// <summary>
        /// 獲取申請單相關表的表結構
        /// </summary>
        /// <returns></returns>
        public DataSet GetAppSchema()
        {
            DataSet ds = DBUtil.GetSearchResult("GetAppSchema");
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].TableName = "Vehicle_Master";
                ds.Tables[1].TableName = "Vehicle_Destination";
                ds.Tables[2].TableName = "Vehicle_Passenger";
            }
            return ds;
        }

        /// <summary>
        /// 獲取乘客屬性
        /// </summary>
        /// <returns></returns>
        public DataSet GetPassengerAttribute()
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_GetPassengerAttribute");
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取申請單詳情
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public DataSet GetAppDetails(string businessCode)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_GetAppDetails");
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            return db.ExecuteDataSet(cmd);
        }


        /// <summary>
        /// 助理提交派車申請單
        /// </summary>
        /// <param name="dsApp"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool SubmitForm(DataSet dsApp, string remark)
        {
            //DbCommand cmd = db.GetStoredProcCommand("proc_App_SubmitForm");
            if (dsApp != null && dsApp.Tables.Count > 0)
            {
                DataTable dtVehicle = dsApp.Tables["Vehicle_Master"];
                DataTable dtDestination = dsApp.Tables["Vehicle_Destination"];
                DataTable dtPassenger = dsApp.Tables["Vehicle_Passenger"];
                DataRow dr = dtVehicle.Rows[0];
                string businessCode = dr["BusinessCode"].ToString();
                string applicationUserAccount = dr["ApplicationUserAccount"].ToString();
                DateTime requireStartTime = Convert.ToDateTime(dr["RequireStartTime"]);
                string departureRegionCode = dr["DepartureRegionCode"].ToString();
                string departureDetailAddress = dr["departureDetailAddress"].ToString();
                string isOneWay = dr["IsOneWay"].ToString();
                short businessTypeCode = Convert.ToInt16(dr["BusinessTypeCode"]);
                short vehiclePurposeCode = Convert.ToInt16(dr["VehiclePurposeCode"]);
                string isPassengerPay = dr["IsPassengerPay"].ToString();
                short passengerAttributeCode = Convert.ToInt16(dr["PassengerAttributeCode"]);
                string suggestVehicleNo = dr["SuggestVehicleNo"].ToString();
                string costCenter = dr["CostCenter"].ToString();

                string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                //int cnt = 0;
                int err = 0;
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    SqlCommand sqlCmd = new SqlCommand("proc_App_SubmitForm");
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@BusinessCode", businessCode);
                    sqlCmd.Parameters.AddWithValue("@ApplicationUserAccount", applicationUserAccount);
                    sqlCmd.Parameters.AddWithValue("@RequireStartTime", requireStartTime);
                    sqlCmd.Parameters.AddWithValue("@DepartureRegionCode", departureRegionCode);
                    sqlCmd.Parameters.AddWithValue("@DepartureDetailAddress", departureDetailAddress);
                    sqlCmd.Parameters.AddWithValue("@IsOneWay", isOneWay);
                    sqlCmd.Parameters.AddWithValue("@BusinessTypeCode", businessTypeCode);
                    sqlCmd.Parameters.AddWithValue("@VehiclePurposeCode", vehiclePurposeCode);
                    sqlCmd.Parameters.AddWithValue("@IsPassengerPay", isPassengerPay);
                    sqlCmd.Parameters.AddWithValue("@PassengerAttributeCode", passengerAttributeCode);
                    sqlCmd.Parameters.AddWithValue("@SuggestVehicleNo", suggestVehicleNo);
                    sqlCmd.Parameters.AddWithValue("@CostCenter", costCenter);
                    sqlCmd.Parameters.AddWithValue("@Remark", remark);
                    SqlParameter par = sqlCmd.Parameters.AddWithValue("@PassengerTbl", dtPassenger);
                    par.SqlDbType = SqlDbType.Structured;
                    SqlParameter pa = sqlCmd.Parameters.AddWithValue("@DestinationTbl", dtDestination);
                    pa.SqlDbType = SqlDbType.Structured;

                    SqlParameter para = new SqlParameter();
                    para.ParameterName = "@Error";
                    para.SqlDbType = SqlDbType.Int;
                    para.Direction = ParameterDirection.Output;

                    sqlCmd.Parameters.Add(para);

                    sqlCmd.ExecuteNonQuery();

                    err = Convert.ToInt32(para.Value);
                }

                if (err == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 取消申請單
        /// </summary>
        /// <param name="businessCode"></param>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public bool CancelForm(string businessCode, string userAccount, string remark)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_CancelForm");
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@Remark", DbType.String, remark);
            db.AddOutParameter(cmd, "@Error", DbType.Int32, 5);
            db.ExecuteNonQuery(cmd);
            int err = Convert.ToInt32(db.GetParameterValue(cmd, "@Error"));
            bool isSuccess = true;
            if (err != 0)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 主管審核
        /// </summary>
        /// <param name="businessCode"></param>
        /// <param name="userAccount"></param>
        /// <param name="auditResult"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool Audit(string businessCode, string userAccount, string auditResult, string remark)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_Audit");
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@AuditResult", DbType.String, auditResult);
            db.AddInParameter(cmd, "@Remark", DbType.String, remark);
            db.AddOutParameter(cmd, "@Error", DbType.Int32, 5);
            db.ExecuteNonQuery(cmd);
            int err = Convert.ToInt32(db.GetParameterValue(cmd, "@Error"));
            bool isSuccess = true;
            if (err != 0)
            {
                isSuccess = false;
            }
            return isSuccess;
        }


        /// <summary>
        /// 總務綁定廠商
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="businessA"></param>
        /// <param name="businessB"></param>
        /// <param name="businessC"></param>
        /// <param name="vendorCode"></param>
        /// <returns></returns>
        public bool BindingVendor(string userAccount, string businessA, string businessB, string businessC, string vendorCode, string vehicleNO, string remark)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_BindingVendor");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@BusinessA", DbType.Int64, businessA.Trim() == "" ? "0" : businessA);
            db.AddInParameter(cmd, "@BusinessB", DbType.Int64, businessB.Trim() == "" ? "0" : businessB);
            db.AddInParameter(cmd, "@BusinessC", DbType.Int64, businessC.Trim() == "" ? "0" : businessC);
            db.AddInParameter(cmd, "@VendorCode", DbType.String, vendorCode);
            db.AddInParameter(cmd, "@VehicleNO", DbType.String, vehicleNO);
            db.AddInParameter(cmd, "@Remark", DbType.String, remark);
            db.AddOutParameter(cmd, "@Error", DbType.Int32, 5);
            db.ExecuteNonQuery(cmd);
            int err = Convert.ToInt32(db.GetParameterValue(cmd, "@Error"));
            bool isSuccess = true;
            if (err != 0)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 廠商綁定車輛
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="businessCode"></param>
        /// <param name="vehicleNo"></param>
        /// <returns></returns>
        public bool BindingCar(string userAccount, string businessCode, string vehicleNo, string remark)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_BindingCar");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@BusinessCode", DbType.String, businessCode);
            db.AddInParameter(cmd, "@VehicleNo", DbType.String, vehicleNo);
            db.AddInParameter(cmd, "@Remark", DbType.String, remark);
            db.AddOutParameter(cmd, "@Error", DbType.Int32, 5);
            db.ExecuteNonQuery(cmd);
            int err = Convert.ToInt32(db.GetParameterValue(cmd, "@Error"));
            bool isSuccess = true;
            if (err != 0)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 待處理事項  
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public DataSet GetTodoList(string userAccount, int pageSize, int currentPage)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetToDoList");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@CurrentPage", DbType.Int32, currentPage);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取所有的流程狀態
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllFlowStatus()
        {
            return DBUtil.GetSearchResult("GetAllFlowStatus");
        }


        /// <summary>
        /// 獲取派車清單歷史記錄  總務可以查看所有的，其他都只能查看對應的派車清單
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <param name="flowStatus"></param>
        /// <returns></returns>
        public DataSet GetAppHistory(string userAccount, string startDay, string endDay, string flowStatus, int pageSize, int currentPage)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetAppHistory");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@StartDay", DbType.String, startDay);
            db.AddInParameter(cmd, "@EndDay", DbType.String, endDay);
            db.AddInParameter(cmd, "@FlowStatus", DbType.String, flowStatus);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@CurrentPage", DbType.Int32, currentPage);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 司機到達出發地
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="businessCode"></param>
        /// <param name="arriveDepartureTime"></param>
        /// <returns></returns>
        public bool ArriveDepartureplace(string userAccount, string businessCode, string arriveDepartureTime)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_ArriveDepartureplace");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            db.AddInParameter(cmd, "@ArriveDepartureTime", DbType.DateTime, arriveDepartureTime);
            db.AddOutParameter(cmd, "@Error", DbType.Int32, 5);
            db.ExecuteNonQuery(cmd);
            int err = Convert.ToInt32(db.GetParameterValue(cmd, "@Error"));
            bool isSuccess = true;
            if (err != 0)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 司機從出發地出發
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="businessCode"></param>
        /// <param name="settingOutTime"></param>
        /// <returns></returns>
        public bool SetOut(string userAccount, string businessCode, string settingOutTime)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_SetOut");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            db.AddInParameter(cmd, "@SettingOutTime", DbType.DateTime, settingOutTime);
            db.AddOutParameter(cmd, "@Error", DbType.Int32, 5);
            db.ExecuteNonQuery(cmd);
            int err = Convert.ToInt32(db.GetParameterValue(cmd, "@Error"));
            bool isSuccess = true;
            if (err != 0)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 司機結束行程,派車申請單結案
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public bool FinishDispatchment(string userAccount, string businessCode, string remark)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_FinishDispatchment");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            db.AddInParameter(cmd, "@Remark", DbType.String, remark);
            db.AddOutParameter(cmd, "@Error", DbType.Int32, 5);
            db.ExecuteNonQuery(cmd);
            int err = Convert.ToInt32(db.GetParameterValue(cmd, "@Error"));
            bool isSuccess = true;
            if (err != 0)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 獲取申請單費用
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public DataSet GetAppFee(string businessCode)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetAppFee");
            db.AddInParameter(cmd, "@BusinessCode", DbType.String, businessCode);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 更新繞路費
        /// </summary>
        /// <param name="dtFee"></param>
        /// <returns></returns>
        public bool UpdateDetourFee(DataTable dtFee)
        {
            if (dtFee != null)
            {
                string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                int cnt = 0;
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    SqlCommand sqlCmd = new SqlCommand("proc_UpdateDetourFee");
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter par = sqlCmd.Parameters.AddWithValue("@DetourFeeTbl", dtFee);
                    par.SqlDbType = SqlDbType.Structured;
                    cnt = sqlCmd.ExecuteNonQuery();
                }
                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新基本费用
        /// </summary>
        /// <param name="dtFee"></param>
        /// <returns></returns>
        public bool UpdateBaseFee(decimal BaseFee, string businessCode)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_UpdateBaseFee");
            db.AddInParameter(cmd, "@BaseFee", DbType.Decimal, BaseFee);
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            
            DataSet ds = db.ExecuteDataSet(cmd);
            bool isPendingUser = false;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                isPendingUser = true;
            }
            return isPendingUser;
        }

        /// <summary>
        /// 判斷是否是待處理用戶
        /// </summary>
        /// <param name="businessCode"></param>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public bool IsPendingUser(string businessCode, string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_GetPendingUser");
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            DataSet ds = db.ExecuteDataSet(cmd);
            bool isPendingUser = false;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                isPendingUser = true;
            }
            return isPendingUser;
        }

        /// <summary>
        /// 獲取最後上傳高速費的時間
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public DateTime GetUploadTollFeeLastTime(string businessCode)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_GetTollFeeLastTime");
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            return Convert.ToDateTime(db.ExecuteScalar(cmd));
        }

        /// <summary>
        /// 獲取可拼車的申請單的相關信息
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public DataSet GetCarpoolApps(string businessCode)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetCarpoolApps");
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 總務結案
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public bool CloseForm(string userAccount, string businessCode, string remark)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_CloseForm");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            db.AddInParameter(cmd, "@Remark", DbType.String, remark);
            db.AddOutParameter(cmd, "@Error", DbType.Int32, 5);
            db.ExecuteNonQuery(cmd);
            int err = Convert.ToInt32(db.GetParameterValue(cmd, "@Error"));
            bool isSuccess = true;
            if (err != 0)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 獲取車輛的GpsURL
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public string GetGpsURL(string businessCode)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetGpsURL");
            db.AddInParameter(cmd, "@BusinessCode", DbType.Int64, businessCode);
            string url = Convert.ToString(db.ExecuteScalar(cmd));
            return url;
        }

        /// <summary>
        /// 獲取報表數據
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <param name="costCenter"></param>
        /// <param name="destinationCode"></param>
        /// <param name="vendorCode"></param>
        /// <param name="vehicleNo"></param>
        /// <param name="vehiclePurpose"></param>
        /// <param name="businessType"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public DataSet GetStaticExpense(string userAccount, string startDay, string endDay, string costCenter, string destinationCode, string vendorCode, string vehicleNo, string vehiclePurpose, string businessType, int pageSize, int currentPage)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_Report_StatisticExpense");
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@reqStartDay", DbType.String, startDay);
            db.AddInParameter(cmd, "@reqEndDay", DbType.String, endDay);
            db.AddInParameter(cmd, "@costCenter", DbType.String, costCenter);
            db.AddInParameter(cmd, "@destinationCode", DbType.String, destinationCode);
            db.AddInParameter(cmd, "@vendorCode", DbType.String, vendorCode);
            db.AddInParameter(cmd, "@vehicleNo", DbType.String, vehicleNo);
            db.AddInParameter(cmd, "@vehiclePurpose", DbType.String, vehiclePurpose);
            db.AddInParameter(cmd, "@businessType", DbType.String, businessType);
            db.AddInParameter(cmd, "@pageSize", DbType.String, pageSize);
            db.AddInParameter(cmd, "@currentPage", DbType.String, currentPage);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 導出數據到EXCEL 獲取數據
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <param name="costCenter"></param>
        /// <param name="destinationCode"></param>
        /// <param name="vendorCode"></param>
        /// <param name="vehicleNo"></param>
        /// <param name="vehiclePurpose"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public DataSet GetAllStatisticsExpense(string userAccount, string startDay, string endDay, string costCenter, string destinationCode, string vendorCode, string vehicleNo, string vehiclePurpose, string businessType)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_Report_AllStatisticExpense");
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@reqStartDay", DbType.String, startDay);
            db.AddInParameter(cmd, "@reqEndDay", DbType.String, endDay);
            db.AddInParameter(cmd, "@costCenter", DbType.String, costCenter);
            db.AddInParameter(cmd, "@destinationCode", DbType.String, destinationCode);
            db.AddInParameter(cmd, "@vendorCode", DbType.String, vendorCode);
            db.AddInParameter(cmd, "@vehicleNo", DbType.String, vehicleNo);
            db.AddInParameter(cmd, "@vehiclePurpose", DbType.String, vehiclePurpose);
            db.AddInParameter(cmd, "@businessType", DbType.String, businessType);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取車輛報價
        /// </summary>
        /// <param name="vehicleTypeCode"></param>
        /// <param name="fromRegionCode"></param>
        /// <param name="toRegionCode"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public DataSet GetQuotation(string vehicleTypeCode, string fromRegionCode, string toRegionCode,string userAccount, int pageSize, int currentPage)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetQuotation");
            db.AddInParameter(cmd, "@vehicleTypeCode", DbType.String, vehicleTypeCode);
            db.AddInParameter(cmd, "@fromRegionCode", DbType.String, fromRegionCode);
            db.AddInParameter(cmd, "@toRegionCode", DbType.String, toRegionCode);
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@pageSize", DbType.String, pageSize);
            db.AddInParameter(cmd, "@currentPage", DbType.String, currentPage);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取簽核線信息
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="approveUser"></param>
        /// <param name="dispatcher"></param>
        /// <param name="isEnabled"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public DataSet GetAuditLine(string applicationUser, string approveUser, string dispatcher,string isEnabled,string userAccount, int pageSize, int currentPage)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetAuditLine");
            db.AddInParameter(cmd, "@applicationUser", DbType.String, applicationUser);
            db.AddInParameter(cmd, "@approveUser", DbType.String, approveUser);
            db.AddInParameter(cmd, "@dispatcher", DbType.String, dispatcher);
            db.AddInParameter(cmd, "@isEnabled", DbType.String, isEnabled);
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@pageSize", DbType.String, pageSize);
            db.AddInParameter(cmd, "@currentPage", DbType.String, currentPage);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取所有的派車人
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllDispatcher()
        {
            DataSet ds = DBUtil.GetSearchResult("GetAllDispatcher");         
            return ds;
        }

        /// <summary>
        /// 獲取簽核線信息
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public DataSet GetAllPerson(string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetAllDispatcher");
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);
            return db.ExecuteDataSet(cmd);
        }
    }
}
