using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace CarSystemTest.DAL
{
    public class RegionDao
    {
        Database db = null;
        public RegionDao()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 獲取所有的區域
        /// </summary>
        /// <returns></returns>
        public DataSet GetRegions(string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetAllArea");
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);
            return db.ExecuteDataSet(cmd);
        }
    }
}
