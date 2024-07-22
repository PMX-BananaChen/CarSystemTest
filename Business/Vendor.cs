using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CarSystemTest.DAL;

namespace CarSystemTest.Business
{
    public class Vendor
    {
        private VendorDao vd = new VendorDao();

        /// <summary>
        /// 獲取所有的廠商信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendors()
        {
            return vd.GetVendors();
        }

        /// <summary>
        /// 獲取可供選擇的廠商
        /// </summary>
        /// <returns></returns>
        public DataTable GetAvailabeVendors(string userAccount)
        {
            DataSet ds = vd.GetAvailableVendors(userAccount);
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
        /// 獲取某個廠商下對應的車輛
        /// </summary>
        /// <param name="vendorCode"></param>
        /// <returns></returns>
        public DataTable GetVendorCars(string vendorCode)
        {
            DataSet ds = vd.GetVendorCars(vendorCode);
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
        /// 獲取司機用戶對應的車輛信息
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public DataTable GetCarByUserAccount(string userAccount)
        {
            DataSet ds = vd.GetCarByUserAccount(userAccount);
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
        /// 獲取廠商用戶對應的廠商信息
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public DataTable GetVendorInfo(string userAccount)
        {
            DataSet ds = vd.GetVendorInfo(userAccount);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

    }
}
