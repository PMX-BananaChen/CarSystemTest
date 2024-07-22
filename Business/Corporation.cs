using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CarSystemTest.DAL;

namespace CarSystemTest.Business
{
    public class Corporation
    {
        private CorporationDao cd = new CorporationDao();

        /// <summary>
        /// 獲取所有的成本中心
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCostCenter(string userAccount)
        {
            DataTable dt = null;
            DataSet ds = cd.GetAllCostCenter(userAccount);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        /// <summary>
        /// 獲取默認的成本中心
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public DataTable GetDefaultCostCenter(string userAccount)
        {
            return cd.GetDefaultCostCenter(userAccount).Tables[0];
        }
    }
}
