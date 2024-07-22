using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CarSystemTest.DAL;

namespace CarSystemTest.Business
{
    public class Region
    {

        private RegionDao rd = new RegionDao();

        /// <summary>
        /// 獲取所有的區域地址
        /// </summary>
        /// <returns></returns>
        public DataTable GetRegions(string userAccount)
        {
            DataSet ds = rd.GetRegions(userAccount);
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
