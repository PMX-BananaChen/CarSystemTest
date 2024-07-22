using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSystemTest.DAL;
using System.Data;

namespace CarSystemTest.Business
{
    public class CarBiz
    {
        private CarBizDao cbd = new CarBizDao();

        /// <summary>
        /// 獲取派車主單的流水號
        /// </summary>
        /// <returns></returns>
        public long GetBizCode()
        {
            return cbd.GetBizCode();
        }

        /// <summary>
        /// 獲取乘客屬性
        /// </summary>
        /// <returns></returns>
        public DataTable GetPassengerAttribute()
        {
            return cbd.GetPassengerAttribute().Tables[0];
        }

        /// <summary>
        /// 獲取申請單相關表的表結構
        /// </summary>
        /// <returns></returns>
        public DataSet GetAppSchema()
        {
            return cbd.GetAppSchema();
        }

        public void ApplyCar()
        {

        }

        /// <summary>
        /// 獲取申請單詳情
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public DataSet GetAppDetails(string businessCode)
        {
            DataSet ds = cbd.GetAppDetails(businessCode);
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].TableName = "Vehicle_Master";
                ds.Tables[1].TableName = "Vehicle_Destination";
                ds.Tables[2].TableName = "Vehicle_Passenger";
                ds.Tables[3].TableName = "Vehicle_Remark";
            }
            return ds;
        }

        /// <summary>
        /// 助理提交派車申請單
        /// </summary>
        /// <param name="dsApp"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool SubmitForm(DataSet dsApp, string remark)
        {
            return cbd.SubmitForm(dsApp, remark);
        }

        /// <summary>
        /// 取消申請單
        /// </summary>
        /// <param name="businessCode"></param>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public bool CancelForm(string businessCode, string userAccount, string remark)
        {
            return cbd.CancelForm(businessCode, userAccount, remark);
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
            return cbd.Audit(businessCode, userAccount, auditResult, remark);
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
            return cbd.BindingVendor(userAccount, businessA, businessB, businessC, vendorCode, vehicleNO, remark);
        }

        /// <summary>
        /// 廠商派車
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="businessCode"></param>
        /// <param name="vehicleNo"></param>
        /// <returns></returns>
        public bool BindingCar(string userAccount, string businessCode, string vehicleNo, string remark)
        {
            return cbd.BindingCar(userAccount, businessCode, vehicleNo, remark);
        }

        /// <summary>
        /// 獲取代辦事項列表
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public DataSet GetToDoList(string userAccount, int pageSize, int currentPage)
        {
            DataSet ds = cbd.GetTodoList(userAccount, pageSize, currentPage);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 獲取所有的流程狀態
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllFlowStatus()
        {
            DataSet ds = cbd.GetAllFlowStatus();
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        /// <summary>
        /// 獲取派車清單歷史記錄  總務可以查看所有的，其他都只能查看對應的派車清單
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <param name="flowStatus"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public DataSet GetAppHistory(string userAccount, string startDay, string endDay, string flowStatus, int pageSize, int currentPage)
        {
            DataSet ds = cbd.GetAppHistory(userAccount, startDay, endDay, flowStatus, pageSize, currentPage);
            return ds;
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
            return cbd.ArriveDepartureplace(userAccount, businessCode, arriveDepartureTime);
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
            return cbd.SetOut(userAccount, businessCode, settingOutTime);
        }

        /// <summary>
        /// 司機結束行程,派車申請單結案
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public bool FinishDispatchment(string userAccount, string businessCode, string remark)
        {
            return cbd.FinishDispatchment(userAccount, businessCode, remark);
        }

        /// <summary>
        /// 獲取申請單費用
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public DataTable GetAppFee(string businessCode)
        {
            DataSet ds = cbd.GetAppFee(businessCode);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新繞路費
        /// </summary>
        /// <param name="dtFee"></param>
        /// <returns></returns>
        public bool UpdateDetourFee(DataTable dtFee)
        {
            return cbd.UpdateDetourFee(dtFee);
        }

        /// <summary>
        /// 更新基本费用
        /// </summary>
        /// <param name="dtFee"></param>
        /// <returns></returns>
        public bool UpdateBaseFee(decimal BaseFee,string businessCode)
        {
            return cbd.UpdateBaseFee(BaseFee, businessCode);
        }

        /// <summary>
        /// 判斷是否是當前處理用戶
        /// </summary>
        /// <param name="businessCode"></param>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public bool IsPendingUser(string businessCode, string userAccount)
        {
            return cbd.IsPendingUser(businessCode, userAccount);
        }

        /// <summary>
        /// 獲取最後上傳高速費的時間
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public DateTime GetUploadTollFeeLastTime(string businessCode)
        {
            return cbd.GetUploadTollFeeLastTime(businessCode);
        }

        /// <summary>
        /// 獲取可拼車的申請單的相關信息
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public DataTable GetCarpoolApps(string businessCode)
        {
            DataSet ds = cbd.GetCarpoolApps(businessCode);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 結案
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public bool CloseForm(string userAccount, string businessCode, string remark)
        {
            return cbd.CloseForm(userAccount, businessCode, remark);
        }

        /// <summary>
        /// 獲取車輛的GpsURL
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public string GetGpsURL(string businessCode)
        {
            return cbd.GetGpsURL(businessCode);
        }

        /// <summary>
        /// 獲取報表數據 分頁顯示
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
            return cbd.GetStaticExpense(userAccount, startDay, endDay, costCenter, destinationCode, vendorCode, vehicleNo, vehiclePurpose, businessType, pageSize, currentPage);
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
        public DataSet GetAllStaticExpense(string userAccount, string startDay, string endDay, string costCenter, string destinationCode, string vendorCode, string vehicleNo, string vehiclePurpose, string businessType)
        {
            return cbd.GetAllStatisticsExpense(userAccount, startDay, endDay, costCenter, destinationCode, vendorCode, vehicleNo, vehiclePurpose, businessType);
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
        public DataSet GetQuotation(string vehicleTypeCode, string fromRegionCode, string toRegionCode,string Area, int pageSize, int currentPage)
        {
            return cbd.GetQuotation(vehicleTypeCode, fromRegionCode, toRegionCode,Area, pageSize, currentPage);
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
        public DataSet GetAuditLine(string applicationUser, string approveUser, string dispatcher, string isEnabled,string userAccount, int pageSize, int currentPage)
        {
            return cbd.GetAuditLine(applicationUser, approveUser, dispatcher,isEnabled, userAccount,pageSize, currentPage);
        }

        /// <summary>
        /// 獲取所有的派車人
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllDispatcher()
        {
            return cbd.GetAllDispatcher();
        }

        /// <summary>
        /// 獲取所有的派車人
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllPerson(string userAccount)
        {
            return cbd.GetAllPerson(userAccount);
        }
    }
}
