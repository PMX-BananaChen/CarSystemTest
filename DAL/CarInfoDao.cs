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
    public class CarInfoDao
    {
        Database db = null;

        public CarInfoDao()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 獲取所有的車輛，作為助理申請的建議車輛
        /// </summary>
        /// <returns></returns>
        public DataSet GetSuggestCar(string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetAllCars");
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);       
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取派車用途
        /// </summary>
        /// <returns></returns>
        public DataSet GetCarPurpose()
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_App_GetVehiclePurpose");
            return db.ExecuteDataSet(cmd);
        }
    }
}
