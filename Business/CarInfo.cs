using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CarSystemTest.DAL;

namespace CarSystemTest.Business
{
    public class CarInfo
    {
        private CarInfoDao cid = new CarInfoDao();

        /// <summary>
        /// 獲取所有的車輛，作為助理申請的建議車輛
        /// </summary>
        /// <returns></returns>
        public DataTable GetSuggestCar(string userAccount)
        {
            DataSet ds = cid.GetSuggestCar(userAccount);
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
        /// 獲取派車用途
        /// </summary>
        /// <returns></returns>
        public DataTable GetCarPurpose()
        {
            DataSet ds = cid.GetCarPurpose();
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
