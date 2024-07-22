using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Xml;
using System.IO;
using System.Transactions;
using System.Linq;

namespace CarSystemTest.DBUtility
{
    public static class DBUtil
    {
        // sql resource file relative path,these file will be deployed to the SQL subfolder under application root
        private const string SQLRESOURCEFILERELATIVEPATH = "SQL";
        private static readonly Database db = DatabaseFactory.CreateDatabase();
        private static readonly IDictionary<string, StatementNode> statementDic = new Dictionary<string, StatementNode>();

        /// <summary>
        /// Initializes statemendDic variable.
        /// </summary>
        static DBUtil()
        {
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + SQLRESOURCEFILERELATIVEPATH);
            foreach (FileInfo fi in di.GetFiles("*.xml"))
            {
                //add exception report ,by mingliang.qu 2010-04-23.
                try
                {
                    //load from sql resource file
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fi.FullName);
                    XmlElement root = xmlDoc.DocumentElement;
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        //create StatementNode instance
                        StatementNode statement = new StatementNode();
                        statement.IsProcedure = node.Attributes["IsProc"].Value == "1" ? true : false;
                        statement.Title = node.FirstChild.InnerText;
                        statement.Comment = node.FirstChild.NextSibling.InnerText;
                        statement.Sql = node.LastChild.PreviousSibling.InnerText;

                        foreach (XmlNode paramNode in node.LastChild.ChildNodes)
                        {
                            //create StatementParamNode instance
                            StatementParamNode param = new StatementParamNode();
                            param.Name = paramNode.Attributes["Name"].Value;
                            param.Pos = paramNode.Attributes["Pos"].Value;
                            param.DbType = paramNode.Attributes["DbType"].Value;
                            param.Direction = paramNode.Attributes["Direction"].Value;
                            param.Size = paramNode.Attributes["Size"].Value;
                            statement.Paramlst.Add(param);
                        }

                        //caution:if the same key existed, an exception will be thrown
                        statementDic.Add(statement.Title, statement);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(fi.Name + ":" + ex.Message, ex);
                }

            }
        }

        public static IDictionary<string, StatementNode> StatementDic
        {
            get
            {
                return statementDic;
            }
        }

        /// <summary>
        /// Update the whole DataSet in one batch.
        /// </summary>
        /// <example>
        /// Dao layer will call this method like this:
        /// Define a method for get po info assuming GetPO as its name, implements its method body
        /// GetPO(string condition)
        /// {
        ///  ....
        ///  //construct your PO dataset, then call DBUtil method
        ///  DBUtil.UpdateDataSet(dsPO,
        ///                       new string[]{ds.Tables[0].TableName,ds.Tables[1].TableName},
        ///                       new string[,]{{"insertPOMaster","",""},{"insertPODetail","",""}})
        ///  
        ///   
        /// }
        /// 
        /// define a section in your sql template file(residing in the SQL sub folder under the web root)
        /// for the corresponding operation(insert/update/delete).
        /// </example>
        /// <param name="ds">DataSet to be updated</param>
        /// <param name="tblName">DataTable to be updated</param>
        /// <param name="arrTitle">Array of statement title representing insert/update/delete operation</param>
        /// <returns>true represents succeed,false represents fail.</returns>
        public static bool UpdateDataSet(DataSet ds, string[] tblName, string[,] arrTitle, bool existEmpty = false)
        {
            int dimension1Size = arrTitle.GetUpperBound(0) + 1;
            int dimension2Size = arrTitle.GetUpperBound(1) + 1;

            DbCommand[,] arrCmd = new DbCommand[dimension1Size, dimension2Size];

            for (int i = 0; i < dimension1Size; i++)
            {
                for (int j = 0; j < dimension2Size; j++)
                {
                    DbCommand cmd = null;
                    string title = arrTitle.GetValue(i, j).ToString();
                    string sql = GetSqlStrByTitle(title);
                    bool isProcedure = IsProcedure(title);
                    IList<StatementParamNode> paramList = GetParamListByTitle(title);
                    cmd = BuildCommand(sql, false, isProcedure, null, paramList, existEmpty);

                    arrCmd.SetValue(cmd, i, j);
                }
            }

            int succeedCount = 0;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                for (int m = 0; m < tblName.Length; m++)
                {
                    int count = db.UpdateDataSet(ds, tblName[m], (DbCommand)(arrCmd.GetValue(m, 0)), (DbCommand)(arrCmd.GetValue(m, 1)), (DbCommand)(arrCmd.GetValue(m, 2)), UpdateBehavior.Standard);
                    if (count > 0)
                    {
                        succeedCount++;
                    }
                }
                ts.Complete();
            }

            return succeedCount == tblName.Length ? true : false;

        }

        /// <summary>
        /// Update the whole DataSet in one batch, you can specify which columns will be updated
        /// usually the whole DataSet update behavior contains three related DbCommand(insert/update/delete),
        /// using this overloaded method, you can construct your update command to update specified columns you
        /// designated, also you can reuse your update sql statement in your sql template file.
        /// </summary>
        /// <example>
        /// Dao layer will call this method like this:
        /// Define a method for get po info assuming GetPO as its name, implements its method body
        /// GetPO(string condition)
        /// {
        ///  ....
        ///  //construct your PO dataset, then call DBUtil method
        ///  DBUtil.UpdateDataSet(dsPO,
        ///                       new string[]{ds.Tables[0].TableName,ds.Tables[1].TableName},
        ///                       new string[,]{{"","updatePOMaster",""},{"","updatePODetail",""}},
        ///                       new string[][]{new string[]{"@VendorCode","@VendorName"},new string[]{"@DeliveryDate"}})
        ///  
        ///   
        /// }
        /// 
        /// Note:1 the length of tblName must be equal to the length of arrUpdateArgs;
        ///      2 the sequence of arrUpdateArgs array must align with the corresponding table
        /// define a section in your sql template file(residing in the SQL sub folder under the web root)
        /// for the corresponding operation(insert/update/delete).
        /// </example>
        /// <param name="ds">DataSet to be updated</param>
        /// <param name="tblName">DataTable to be updated</param>
        /// <param name="arrTitle">Array of statement title representing insert/update/delete operation</param>
        /// <param name="arrUpdateArgs">The jagged array of columns which you can specify to updated</param>
        /// <returns>true represents succeed,false represents fail.</returns>        
        public static bool UpdateDataSet(DataSet ds, string[] tblName, string[,] arrTitle, string[][] arrUpdateArgs,bool existEmpty=false)
        {
            int dimension1Size = arrTitle.GetUpperBound(0) + 1;
            int dimension2Size = arrTitle.GetUpperBound(1) + 1;

            DbCommand[,] arrCmd = new DbCommand[dimension1Size, dimension2Size];

            for (int i = 0; i < dimension1Size; i++)
            {
                for (int j = 0; j < dimension2Size; j++)
                {
                    DbCommand cmd = null;
                    string title = arrTitle.GetValue(i, j).ToString();
                    string sql = GetSqlStrByTitle(title);
                    bool isProcedure = IsProcedure(title);
                    IList<StatementParamNode> paramList = GetParamListByTitle(title);
                    if (!sql.Trim().ToLower().StartsWith("update")) //indicating not a update statement
                    {
                        cmd = BuildCommand(sql, false, isProcedure, null, paramList,existEmpty);
                    }
                    else
                    {
                        //Modify by Wuzhuohui 2008-09-19
                        if (arrUpdateArgs[i] == null)
                        {
                            cmd = BuildCommand(sql, false, isProcedure, null, paramList, existEmpty);
                        }
                        else
                        {
                            UpdateSQLFilter filter = new UpdateSQLFilter();
                            sql = filter.filter(sql, arrUpdateArgs[i]);
                            cmd = BuildUpdateCommand(sql, arrUpdateArgs[i], paramList);
                        }
                    }

                    arrCmd.SetValue(cmd, i, j);
                }
            }

            int succeedCount = 0;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                for (int m = 0; m < tblName.Length; m++)
                {
                    int count = db.UpdateDataSet(ds, tblName[m], (DbCommand)(arrCmd.GetValue(m, 0)), (DbCommand)(arrCmd.GetValue(m, 1)), (DbCommand)(arrCmd.GetValue(m, 2)), UpdateBehavior.Standard);
                    if (count > 0)
                    {
                        succeedCount++;
                    }

                }

                ts.Complete();
            }

            return succeedCount == tblName.Length ? true : false;

        }

        /// <summary>
        /// Execute delete operation.
        /// </summary>
        /// <param name="title">sql title</param>
        /// <param name="dicCondition">dictionary of parameter name and value</param>
        /// <returns>true represents succeed,false represents fail.</returns>
        public static bool Delete(string title, IDictionary<string, string> dicCondition,bool existEmpty=false)
        {
            String sql = GetSqlStrByTitle(title);
            IList<StatementParamNode> paramList = GetParamListByTitle(title);
            IDictionary<string, string> newArgs = GenRealParamArgs(dicCondition, paramList);

            DbCommand cmd = BuildCommand(sql, true, false, newArgs, paramList, existEmpty);

            int count = db.ExecuteNonQuery(cmd);

            return count > 0 ? true : false;
        }
        /// <summary>
        /// Execute Update operation.
        /// </summary>
        /// <param name="title">sql title</param>
        /// <param name="dicCondition">dictionary of parameter name and value</param>
        /// <returns>true represents succeed,false represents fail.</returns>
        public static bool Update(string title, IDictionary<string, string> dicCondition, bool existEmpty=false)
        {
            String sql = GetSqlStrByTitle(title);
            IList<StatementParamNode> paramList = GetParamListByTitle(title);
            IDictionary<string, string> newArgs = GenRealParamArgs(dicCondition, paramList);

            DbCommand cmd = BuildCommand(sql, true, false, newArgs, paramList, existEmpty);

            int count = db.ExecuteNonQuery(cmd);

            return count > 0 ? true : false;
        }

        /// <summary>
        /// Execute Update4ID operation.
        /// </summary>
        /// <param name="title">sql title</param>
        /// <param name="dicCondition">dictionary of parameter name and value</param>
        /// <returns>true represents succeed,false represents fail.</returns>
        public static int Update4ID(string title, IDictionary<string, string> dicCondition, bool existEmpty=false)
        {
            String sql = GetSqlStrByTitle(title);
            IList<StatementParamNode> paramList = GetParamListByTitle(title);
            IDictionary<string, string> newArgs = GenRealParamArgs(dicCondition, paramList);

            DbCommand cmd = BuildCommand(sql, true, false, newArgs, paramList,existEmpty);

            int Identity = Int32.Parse(db.ExecuteScalar(cmd).ToString());
            return Identity;
        }

        /// <summary>
        /// Retrieve query result as DataSet while no search condition designated. Use this overloaded method when
        /// you don't need to provide search condition for your search operation.
        /// </summary>
        /// <param name="title">sql title</param>
        /// <returns>instance of DataSet containing query result</returns>        
        public static DataSet GetSearchResult(string title)
        {
            String sql = GetSqlStrByTitle(title);
            bool isProcedure = IsProcedure(title);

            DbCommand cmd = null;
            if (isProcedure)
            {
                cmd = db.GetStoredProcCommand(sql);
            }
            else
            {
                cmd = db.GetSqlStringCommand(sql);
            }

            return db.ExecuteDataSet(cmd);

        }

        /// <summary>
        /// Retrieve query result as DataSet given concreate sql title and dictionary of parameter name and value.
        /// </summary>
        /// <param name="title">sql title</param>
        /// <param name="dicCondition">dictionary of parameter name and value</param>
        /// <returns>instance of DataSet containing query result</returns>
        public static DataSet GetSearchResult(string title, IDictionary<string, string> dicCondition,bool existEmpty=false)
        {
            String sql = GetSqlStrByTitle(title);
            bool isProcedure = IsProcedure(title);
            IList<StatementParamNode> paramList = GetParamListByTitle(title);
            IDictionary<string, string> newArgs = GenRealParamArgs(dicCondition, paramList);

            if (!isProcedure)
            {
                SQLFilter filter = new SQLFilter();
                sql = filter.filter(sql, newArgs);
            }

            DbCommand cmd = BuildCommand(sql, true, isProcedure, newArgs, paramList, existEmpty);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// overload GetSearchResult(string title, IDictionary<string, string> dicCondition), supports pagination
        /// </summary>
        /// <param name="title">sql title</param>
        /// <param name="dicCondition">dictionary of parameter name and value</param>
        /// <param name="cPage">current page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>instance of DataSet containing query result</returns>
        public static DataSet GetSearchResult(string title, IDictionary<string, string> dicCondition, int cPage, int pageSize,bool existEmpty=false)
        {
            String sql = GetSqlStrByTitle(title);
            bool isProcedure = IsProcedure(title);
            IList<StatementParamNode> paramList = GetParamListByTitle(title);
            IDictionary<string, string> newArgs = GenRealParamArgs(dicCondition, paramList);

            if (!isProcedure)
            {
                SQLFilter filter = new SQLFilter();
                sql = filter.filter(sql, newArgs);
                int startRowId = (cPage - 1) * pageSize + 1; //start row id
                int endRowId = cPage * pageSize; //end row id
                sql = "select t.* from (" + sql + ") t where rowid between " + startRowId + " and " + endRowId;
            }

            DbCommand cmd = BuildCommand(sql, true, isProcedure, newArgs, paramList,existEmpty);

            if (cPage == 1)
            {
                _totalRecords = GetTotalRecords(title, dicCondition);
            }

            return db.ExecuteDataSet(cmd);
        }
        /// <summary>
        /// Total records for pagging
        /// </summary>
        private static int _totalRecords;
        public static int TotalRecords
        {
            get { return _totalRecords; }
            set { _totalRecords = value; }
        }
        /// <summary>
        /// Retrieve total records of query result
        /// </summary>
        /// <param name="title">sql title</param>
        /// <param name="dicCondition">dictionary of parameter name and value</param>
        /// <returns>total records</returns>
        public static int GetTotalRecords(string title, IDictionary<string, string> dicCondition,bool existEmpty=false)
        {
            String sql = GetSqlStrByTitle(title);
            bool isProcedure = IsProcedure(title);
            if (isProcedure)
            {
                return 0; //TODO
            }
            else
            {
                IList<StatementParamNode> paramList = GetParamListByTitle(title);
                IDictionary<string, string> newArgs = GenRealParamArgs(dicCondition, paramList);

                SQLFilter filter = new SQLFilter();
                sql = filter.filter(sql, newArgs);
                sql = "select count(rowid) from (" + sql + ") t";

                DbCommand cmd = BuildCommand(sql, true, isProcedure, newArgs, paramList,existEmpty);

                return Convert.ToInt32(db.ExecuteScalar(cmd).ToString());
            }
        }

       

        /// <summary>
        /// Get sql string from sql template according title
        /// </summary>
        /// <param name="title">sql title</param>
        /// <returns>sql</returns>
        public static String GetSqlStrByTitle(String title)
        {
            String sqlstr = "";
            if (!string.IsNullOrEmpty(title) && StatementDic.Keys.Contains(title))
            {
                StatementNode statement = StatementDic[title];
                sqlstr = statement.Sql;
            }

            return sqlstr;
        }

        /// <summary>
        /// Get sql parameter list from sql template according title
        /// </summary>
        /// <param name="title">sql title</param>
        /// <returns>parameter list</returns>
        private static IList<StatementParamNode> GetParamListByTitle(String title)
        {
            IList<StatementParamNode> paramList = null;
            if (!string.IsNullOrEmpty(title) && StatementDic.Keys.Contains(title))
            {
                StatementNode statement = StatementDic[title];
                paramList = StatementDic[title].Paramlst;
            }
            return paramList;
        }

        /// <summary>
        /// whether the given sql statement is procedure or not.
        /// </summary>
        /// <param name="title">sql title</param>
        /// <returns>true represents procedure, otherwise sql statement</returns>
        private static bool IsProcedure(String title)
        {
            bool isProcedure = false;
            if (!string.IsNullOrEmpty(title) && StatementDic.Keys.Contains(title))
            {
                StatementNode statement = StatementDic[title];
                isProcedure = statement.IsProcedure;
            }
            return isProcedure;
        }

        /// <summary>
        /// Generate real parameter key/value Dictionary according parameter list which config in sql template.
        /// </summary>
        /// <param name="args">args needs to be query</param>
        /// <param name="Paramlst">parameters configured in sql template</param>
        /// <returns></returns>
        private static IDictionary<string, string> GenRealParamArgs(IDictionary<string, string> args, IList<StatementParamNode> Paramlst)
        {
            IDictionary<string, string> newArgs = new Dictionary<string, string>();
            foreach (StatementParamNode paramNode in Paramlst)
            {
                string key = "@" + paramNode.Name;
                if (args.ContainsKey(key))
                {
                    newArgs.Add(key, args[key]);
                }
                else
                {
                    newArgs.Add(key, "");
                }
            }
            return newArgs;
        }

        /// <summary>
        /// Build Update DbCommand.
        /// </summary>
        /// <param name="sql">the filtered update statement</param>
        /// <param name="updatgeArgs">chosen arguments list,for example update only two column</param>
        /// <param name="Paramlst">parameter list config in sql template</param>
        /// <returns>DbCommand</returns>
        private static DbCommand BuildUpdateCommand(string sql, string[] updateArgs, IList<StatementParamNode> Paramlst)
        {
            DbCommand cmd = null;
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }
            cmd = db.GetSqlStringCommand(sql);
            foreach (string arg in updateArgs)
            {
                string s = arg.Replace("@", "");
                foreach (StatementParamNode paramNode in Paramlst)
                {
                    if (string.Compare(paramNode.Name, s, true) == 0)
                    {
                        db.AddInParameter(cmd, paramNode.Name, ConvertToDbType(paramNode.DbType), paramNode.Name, DataRowVersion.Current);
                        break;
                    }
                }
            }

            cmd.CommandTimeout = 1000;   //批量更新數據時速度慢,延長連接時間（臨時解決方案）by gaocaihui 2009-5-27
            return cmd;

        }

        /// <summary>
        /// Build corresponding DbCommand given statement identifier.
        /// </summary>
        /// <param name="title">identifier of statement</param>
        /// <param name="isQuery">whether or not query operation</param>
        /// <param name="isProcedure">whether or not Procedure</param>
        /// <param name="args">DbCommand parameter name value collections</param>
        /// <param name="Paramlst">parameter list config in sql template</param>
        /// <returns>DbCommand</returns>   
        private static DbCommand BuildCommand(string sql, bool isQuery, bool isProcedure, IDictionary<string, string> args, IList<StatementParamNode> Paramlst, bool existEmpty=false)
        {
            DbCommand cmd = null;
            if (string.IsNullOrEmpty(sql))
            {
                return cmd;
            }

            if (!isQuery) //indicating not query operation, no need to do sql filting
            {
                if (isProcedure)
                {
                    cmd = db.GetStoredProcCommand(sql);

                }
                else
                {
                    cmd = db.GetSqlStringCommand(sql);
                }

                foreach (StatementParamNode paramNode in Paramlst)
                {
                    if (string.Compare(paramNode.Direction, "Input", true) == 0)
                    {
                        db.AddInParameter(cmd, paramNode.Name, ConvertToDbType(paramNode.DbType), paramNode.Name, DataRowVersion.Current);

                    }

                    if (string.Compare(paramNode.Direction, "Output", true) == 0)
                    {
                        db.AddOutParameter(cmd, paramNode.Name, ConvertToDbType(paramNode.DbType), Convert.ToInt32(paramNode.Size));
                    }

                }
            }
            else
            {
                if (isProcedure)
                {
                    cmd = db.GetStoredProcCommand(sql);
                    foreach (StatementParamNode paramNode in Paramlst)
                    {
                        string key = "@" + paramNode.Name;
                        db.AddInParameter(cmd, paramNode.Name, ConvertToDbType(paramNode.DbType), args[key]);
                    }
                }
                else
                {
                    cmd = db.GetSqlStringCommand(sql);
                    foreach (StatementParamNode paramNode in Paramlst)
                    {
                        string key = "@" + paramNode.Name;
                        if (existEmpty)
                        {
                            if (args.ContainsKey(key))
                            {
                                db.AddInParameter(cmd, paramNode.Name, ConvertToDbType(paramNode.DbType), args[key]);
                            }
                        }
                        else
                        {
                            if (args.ContainsKey(key) && args[key] != "")
                            {
                                db.AddInParameter(cmd, paramNode.Name, ConvertToDbType(paramNode.DbType), args[key]);
                            }

                        }
                    }
                }

            }

            cmd.CommandTimeout = 240;
            return cmd;
        }
        /// <summary>
        /// Convert direction string from sql resource file to corresponding ParameterDirection.
        /// </summary>
        /// <param name="direction">direction string</param>
        /// <returns>enumerator of ParameterDirection</returns>
        private static ParameterDirection ConvertDirection(string direction)
        {
            ParameterDirection dir = ParameterDirection.Input;
            switch (direction)
            {
                case "Output": dir = ParameterDirection.Output; break;
                case "ReturnValue": dir = ParameterDirection.ReturnValue; break;
                default:
                    break;
            }
            return dir;
        }

        /// <summary>
        /// Convert dbtype string from sql resource file to corresponding SqlDbType.
        /// </summary>
        /// <param name="dbType">dbtype string</param>
        /// <returns>enumerator of ParameterDirection</returns>
        private static DbType ConvertToDbType(string dbType)
        {
            DbType tp = DbType.String;

            switch (dbType.ToLower())
            {
                case "int": tp = DbType.Int32; break;
                case "long": tp = DbType.Int64; break;
                case "bigint": tp = DbType.Int64; break;
                case "decimal": tp = DbType.Decimal; break;
                case "datetime": tp = DbType.DateTime; break;
                case "double": tp = DbType.Double; break;
                case "float": tp = DbType.Double; break;
                default:
                    break;
            }

            return tp;

        }
    }
}

