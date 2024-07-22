using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections;
using System.Reflection;

namespace CarSystemTest.DBUtility
{
    public static class DBSchema
    {
        /// <summary>
        /// 根據表名得到表架構信息,可同時獲取多個表的架構信息
        /// </summary>
        /// <param name="tableList"></param>
        /// <returns></returns>
        public static DataSet GetTableSchema(string[] tableList)
        {
            DataSet ds = new DataSet();
            DataTable dt;
            DataTable dtNew;
            if (tableList != null && tableList.Length > 0)
            {
                foreach (string sTableName in tableList)
                {
                    dtNew = new DataTable();
                    IDictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("@TABLE_NAME", sTableName);
                    dt = DBUtil.GetSearchResult("GetTableSchema", dic).Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            DataColumn dc = new DataColumn();
                            dc.ColumnName = dr["COLUMN_NAME"].ToString();
                            dc.DataType = ConvertDataType(dr["DATA_TYPE"].ToString());
                            dtNew.Columns.Add(dc);
                        }
                        dtNew.TableName = sTableName;
                        ds.Tables.Add(dtNew);
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 數據類型轉換
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private static Type ConvertDataType(string dataType)
        {
            switch (dataType)
            {
                case "char":
                case "text":
                case "varchar":
                case "ncahr":
                case "nvarchar":
                case "ntext":
                    return typeof(String);
                case "smallint":
                    return typeof(Int16);
                case "int":
                    return typeof(Int32);
                case "bigint":
                    return typeof(Int64);
                case "decimal":
                case "numeric":
                    return typeof(Decimal);
                case "datetime":
                case "date":
                    return typeof(DateTime);
                default:
                    return typeof(String);
            }
        }

        /// <summary>
        /// 將Xml數據轉換為帶架構DataTable數據類型
        /// </summary>
        /// <param name="SchemaTable">帶有數據架構的DataTable數據表</param>
        /// <param name="XmlData">Xml字符串數據</param>
        /// <param name="DataRowState">當前的操作類型</param>
        /// <returns>返回轉換後的數據</returns>
        public static DataTable ConvertXmlToDataTable(ref DataTable SchemaTable, string XmlData)
        {
            XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(XmlData);
            //找到數據項
            XmlNode dataNode = null;
            if (doc.ChildNodes.Count == 1)
            {
                dataNode = doc.ChildNodes[0];
            }
            else
            {
                dataNode = doc.ChildNodes[1];
            }
            //找到數據列表項
            XmlNodeList xmlNodes = dataNode.ChildNodes;
            foreach (XmlNode rowNode in xmlNodes)
            {
                //行狀態
                DataRowState _RowState = DataRowState.Unchanged;
                DataRow row = SchemaTable.NewRow();
                //找到列表中的數據
                foreach (XmlNode cellNode in rowNode.ChildNodes)
                {
                    //列或字段名稱
                    string columnName = cellNode.Name;
                    //如果存在此列數據才加入
                    if (SchemaTable.Columns.Contains(columnName))
                    {
                        string xmlData = cellNode.InnerText;
                        row[columnName] = string.IsNullOrEmpty(xmlData) ? DBNull.Value : row[columnName] = xmlData;
                    }
                    if (cellNode.Name == "_RowState")
                    {
                        if (cellNode.InnerText == "U") { _RowState = DataRowState.Modified; }
                        if (cellNode.InnerText == "I") { _RowState = DataRowState.Added; }
                    }
                }
                SchemaTable.Rows.Add(row);
                row.AcceptChanges();
                if (_RowState == DataRowState.Added)
                {
                    row.SetAdded();
                }
                else
                {
                    row.SetModified();
                }
            } 
            return SchemaTable;
        }

        /// <summary>
        /// 將DataTable的列名轉換為小寫字符[前台Js區分大小寫字符]
        /// </summary>
        /// <param name="table">DataTable類</param>
        /// <returns>當前DataTable</returns>
        public static DataTable ColumnNameToLower(DataTable table)
        {
            if (table == null)
            {
                return table;
            }
            foreach (DataColumn column in table.Columns)
            {
                column.ColumnName = column.ColumnName.ToLower();
            }
            return table;
        }

        /// <summary>
        /// 將DataSet中的DataTable列名轉換為小寫字符[前台Js區分大小寫字符]
        /// </summary>
        /// <param name="table">DataSet類</param>
        /// <returns>當前DataSet</returns>
        public static DataSet ColumnNameToLower(DataSet ds)
        {
            if (ds == null)
            {
                return ds;
            }
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    column.ColumnName = column.ColumnName.ToLower();
                }
            }
            return ds;
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                Type t = typeof(T);
                PropertyInfo[] propertys = t.GetProperties();
                foreach (PropertyInfo pi in propertys)
                    result.Columns.Add(pi.Name, pi.PropertyType);

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
    }
}