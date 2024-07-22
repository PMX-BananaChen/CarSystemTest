
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CarSystemTest.DBUtility
{
    class UpdateSQLFilter
    {
        private ArrayList SQLArray = new ArrayList();

        /// <summary>
        /// Filter redundant update parameters and where condition parameters. 
        /// eg. an update sql sting config in sql template looks like: 
        /// sql = "update t set f1=@f1, f2=@f2,f3=@f3,f4=@f4 where id=@id and f5=@f5";
        /// updateArgs ={@f2,@id}, 
        /// 'f1=@f1,';'f3=@f3,';'f4=@f4,'; 'and f5=@f5' should be remove from the sql string 
        /// </summary>
        /// <param name="sql">update sql string</param>
        /// <param name="updateArgs">parameter names need to be update or parameter names in where condition </param>
        /// <returns></returns>
        public String filter(String sql, String[] updateArgs)
        {
            if (sql.ToLower().IndexOf("update") == -1)
            {
                return sql;
            }

            parse(sql, updateArgs);

            String rstSql = SQLArray[0].ToString();
            rstSql += GenUpdateSetParams(updateArgs);
            rstSql += SQLArray[SQLArray.Count - 1].ToString();
            
            //After filter sql string may looks like: update t set f1=@f1, f2=@f2, where id=@id
            //So we must replace ', where' to ' where'
            rstSql = Regex.Replace(rstSql, ",\\s+where"," where");
            rstSql = Regex.Replace(rstSql, " where\\s*$", "", RegexOptions.None);

            return rstSql;
        }

        /// <summary>
        /// Generate update sql according updateArgs
        /// </summary>
        /// <param name="updateArgs">parameter names need to be update</param>
        /// <returns>A string looks like 'f1=@f1,f2=@f2'</returns>
        private string GenUpdateSetParams(String[] updateArgs)
        {
            StringBuilder stb = new StringBuilder();
            int updateArgsLen = updateArgs.Length;
            for (int i = 0; i < updateArgsLen; i++)
            {
                string paramName = updateArgs[i].ToLower();
                int sqlArrayCount = SQLArray.Count;
                String appendStr = "";

                for (int y = 1; y < sqlArrayCount - 1; y++)
                {
                    string tmpstr = SQLArray[y].ToString().ToLower();
                    if (tmpstr.IndexOf(paramName) != -1)
                    {
                        appendStr = " " + SQLArray[y].ToString() + " ";
                        SQLArray.RemoveAt(y);
                        break;
                    }
                }
               
                stb.Append(appendStr);
            }

            return stb.ToString();
        }

        /// <summary>
        /// Parase sql to sqlArray
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="updateArgs"></param>
        private void parse(String sql, String[] updateArgs)
        {
            String tmpSQL = sql.ToLower();
            String whereStr;
            int setIndex = tmpSQL.IndexOf("set");
            SQLArray.Add(tmpSQL.Substring(0, setIndex + 3));
            int whereIndex = tmpSQL.IndexOf("where");
            if (whereIndex > 0)
            {
                whereStr = tmpSQL.Substring(whereIndex);
                whereStr = filterWhereCondition(whereStr, updateArgs);//Filter where condition
                tmpSQL = tmpSQL.Substring(setIndex + 3, whereIndex - setIndex - 3);
            }
            else
            {
                tmpSQL = tmpSQL.Substring(setIndex + 3);
                whereStr = " where 1=1";
            }

            String[] arrStr = tmpSQL.Split(new char[] { ',' });
            int arrStrLen = arrStr.Length;

            for (int i = 0; i < arrStrLen; i++)
            {
                String str = arrStr[i];
                if (i < arrStrLen - 1)
                {
                    SQLArray.Add(str + ",");
                }
                else
                {
                    SQLArray.Add(str);
                }
            }

            SQLArray.Add(whereStr);

        }
        /// <summary>
        /// Filter parameter in where condition string. eg.
        ///a sql where condition string looks like: where id=@id and f4=@f4 and (f5=@f5 or f6=@f6)
        /// and updateArgs = {'@id','@f5'}
        /// after call this function, the result sql condition string should be 'where id=@id and f5=@f5'
        /// </summary>
        /// <param name="whereStrArg">String of where condition</param>
        /// <param name="updateArgs">Args of where condition</param>
        /// <returns></returns>
        private String filterWhereCondition(String whereStrArg,String[] updateArgs)
        {
            String pattern = "@[0-9a-zA-Z]+";
            MatchCollection matchs = Regex.Matches(whereStrArg, pattern);
           
            IDictionary<string, string> dicParam = new Dictionary<string, string>();

            int len = updateArgs.Length;
            //
            foreach (Match match in matchs)
            {
                String paramName = match.Value;
                bool existFlg = false;
                for (int i = len - 1; i >= 0; i--)
                {
                    String updateParamName = updateArgs[i];
                    if (String.Compare(paramName,updateParamName,true) == 0)
                    {
                        existFlg = true;
                        break;
                    }
                }
                if (existFlg)
                {
                    //Set to any non-null value, it will be remained 
                    dicParam.Add(paramName, "1");
                }
                else
                {
                    //Set to empty value, it will be dropped by sqlFilter.
                    dicParam.Add(paramName, "");
                }
            }
            
            SQLFilter sqlFilter = new SQLFilter();
            return sqlFilter.filter(whereStrArg,dicParam);
        }
    }
}

