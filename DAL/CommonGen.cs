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
    public class CommonGen
    {
        Database db = null;

        public CommonGen()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 獲取流水號
        /// </summary>
        /// <param name="genType"></param>
        /// <returns></returns>
        public long GetGenCode(int genType)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetGenCode");
            db.AddInParameter(cmd, "@GenType", DbType.Int32, genType);
            return Convert.ToInt64(db.ExecuteScalar(cmd));
        }
    }
}
