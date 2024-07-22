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
    public class CorporationDao
    {
        Database db = null;

        public CorporationDao()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 獲取所有的成本中心
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCostCenter(string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetAllCostCenter");
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取默認的成本中心
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public DataSet GetDefaultCostCenter(string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_GetDefaultCostCenter");
            db.AddInParameter(cmd, "@UserAccount", DbType.String, userAccount);
            return db.ExecuteDataSet(cmd);
        }
    }
}
