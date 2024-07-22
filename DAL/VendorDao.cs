using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using CarSystemTest.DBUtility;

namespace CarSystemTest.DAL
{
    public class VendorDao
    {
        Database db;

        public VendorDao()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 獲取所有的廠商信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendors()
        {
            return DBUtil.GetSearchResult("GetAllVendors").Tables[0];
        }

        /// <summary>
        /// 獲取可供選擇的廠商
        /// </summary>
        /// <returns></returns>
        public DataSet GetAvailableVendors(string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetAvailableVendors");
            db.AddInParameter(cmd, "@userAccount", DbType.String,userAccount);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取某廠商下的車輛
        /// </summary>
        /// <param name="vendorCode"></param>
        /// <returns></returns>
        public DataSet GetVendorCars(string vendorCode)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_GetVendorCars");
            db.AddInParameter(cmd, "@VendorCode", DbType.String, vendorCode);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取司機用戶對應的車輛信息
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public DataSet GetCarByUserAccount(string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetCarByUserAccount");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取廠商用戶對應的廠商信息
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public DataSet GetVendorInfo(string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_GetVendorInfo");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            return db.ExecuteDataSet(cmd);
        }
    }
}
