using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CarSystemTest.DBUtility
{
    class SQLFilter
    {
        private ArrayList SQLArray = new ArrayList();

        public String filter(String sql, IDictionary<string, string> dicParam)
        {
            parse(sql);
            int count = 1;

            foreach (KeyValuePair<string, string> kvp in dicParam)
            {
                if (kvp.Value == "")
                {
                    drop(count);
                }
                count++;
            }


            return getRstSql();
        }

        private String getRstSql()
        {
            int count = SQLArray.Count;
            String rstSql = "";
            for (int i = 0; i < count; i++)
            {
                String tmpStr = SQLArray[i].ToString();
                if (tmpStr != " ")
                {
                    rstSql += tmpStr;
                }
            }

            rstSql = rstSql.Replace("=@null", "");
            rstSql = clearRedundantKey(rstSql);

            return rstSql;
        }

        private int findParamIndex(int index)
        {
            int count = SQLArray.Count;
            int paramCount = 0;
            for (int i = 0; i < count; i++)
            {
                String tmpStr = SQLArray[i].ToString();
                if (tmpStr.IndexOf("@") != -1 || tmpStr.IndexOf("@null") != -1)
                {
                    paramCount++;
                }

                if (paramCount == index)
                {
                    return i;
                }
            }

            return count;
        }

        private void drop(int i)
        {
            int paramIndex = findParamIndex(i);
            if (paramIndex >= SQLArray.Count)
            {
                return;
            }

            String tmpStr = SQLArray[paramIndex].ToString();
            int index;
            if ((index = tmpStr.IndexOf("(")) != -1)
            {                
                SQLArray[paramIndex] = tmpStr.Substring(0,index) + "(=@null";
            }
            else if (tmpStr.IndexOf(")") != -1)
            {
                SQLArray[paramIndex] = "=@null)";
            }
            else
            {
                SQLArray[paramIndex] = "=@null";
            }
        }

        private void parse(String sql)
        {
            String tmpSQL = sql.ToLower();

            int index;
            String tmpCon;
            index = tmpSQL.IndexOf("where");

            if (index >= 0)
            {
                SQLArray.Add(tmpSQL.Substring(0, index));
                SQLArray.Add("where");
                tmpSQL = tmpSQL.Substring(index + 5);
            }

            while (true)
            {
                index = tmpSQL.IndexOf("where");

                if (index > 0)
                {
                    tmpCon = tmpSQL.Substring(0, index);
                    tmpCon = tmpCon.Replace("and ", "A ");
                    tmpCon = tmpCon.Replace("or ", "O ");
                    tmpSQL = tmpSQL.Substring(index + 5);
                }
                else
                {
                    tmpCon = tmpSQL;
                    tmpCon = tmpCon.Replace("and ", "A");
                    tmpCon = tmpCon.Replace("or ", "O");
                }

                int index_where = 0;
                String[] arrAnd = tmpCon.Split(new char[] { 'A' });
                foreach (String str in arrAnd)
                {
                    String[] arrOr = str.Split(new char[] { 'O' });
                    int count = 0;

                    index_where++;

                    foreach (String strOr in arrOr)
                    {
                        String strORItem = strOr;
                        if (count == 0)
                        {
                            if (index_where > 1)
                            {
                                SQLArray.Add(" and " + strORItem);
                            }
                            else
                            {
                                SQLArray.Add(strORItem);
                            }
                        }
                        else
                        {
                            strORItem = " or " + strORItem;
                            SQLArray.Add(strORItem);
                        }
                        count++;
                    }
                }
                if (index > 0)
                {
                    SQLArray.Add("where");
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Clear redundant key in sql string
        /// </summary>
        /// <param name="argSql">result of filter sql string</param>
        /// <returns></returns>
        private string clearRedundantKey(string argSql)
        {
            String rstSql = argSql;    
            rstSql = Regex.Replace(rstSql, "\\(\\s*or\\s", "( ", RegexOptions.None);
            rstSql = Regex.Replace(rstSql, "\\s+\\(\\s*\\)", "");
            rstSql = Regex.Replace(rstSql, "\\s+\\w+\\s+in\\s*\\(\\s+\\)*", " ", RegexOptions.None);//Replace 'select * from t where f1 in( and f2=@f2'  with 'select * from t where and f2=@f2'
            rstSql = Regex.Replace(rstSql, "\\s+\\w+\\s+in\\s*\\($", " ", RegexOptions.None);//Replace 'select * from t where f1 in('  with 'select * from t'
            rstSql = Regex.Replace(rstSql, "and\\s*or", "and ", RegexOptions.None);
            rstSql = Regex.Replace(rstSql, "and\\s*and", "and ", RegexOptions.None);
            rstSql = Regex.Replace(rstSql, "\\s+and\\s*$", "", RegexOptions.None);  //Replace 'Or' with '' if sql string is end with 'and'
            rstSql = Regex.Replace(rstSql, "\\s+or\\s*$", "", RegexOptions.None);   //Replace 'Or' with '' if sql string is end with 'or'
            rstSql = Regex.Replace(rstSql, "where\\s*and", "where", RegexOptions.None);//Replace 'select * from t where and with 'select * from t'
            rstSql = Regex.Replace(rstSql, "where\\s*or", "where", RegexOptions.None);
            rstSql = Regex.Replace(rstSql, "where\\s*\\)", " )", RegexOptions.None);
            //rstSql = Regex.Replace(rstSql, " where\\s*$", "", RegexOptions.None);
            rstSql = Regex.Replace(rstSql, "\\s+where\\s*$", "", RegexOptions.None);

            return rstSql;
        }
    }
}

