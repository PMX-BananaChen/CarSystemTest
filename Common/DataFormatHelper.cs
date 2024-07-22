
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarSystemTest.Common
{
    public class DataFormatHelper
    {
        /// <summary>
        /// Add thousands separator
        /// </summary>
        /// <returns></returns>
        public static string AddSeperator(Decimal num)
        {
            int tempnum = Convert.ToInt32(num);
            if (num != tempnum)
            {
                return num.ToString("N3").TrimEnd('0');
            }
            else
            {
                return num.ToString("N").Split('.')[0];
            }
        }

        public static string AddSeperator(Double num)
        {
            int tempnum = Convert.ToInt32(num);
            if (num != tempnum)
            {
                return num.ToString("N3").TrimEnd('0');
            }
            else
            {
                return num.ToString("N").Split('.')[0];
            }
        }

        /// <summary>
        /// 增加千位符號，保留小數點后面的0
        /// </summary>
        /// <param name="num"></param>
        /// <param name="decimalLen"></param>
        /// <returns></returns>
        public static string AddSeperator(Double num, int decimalLen)
        {
            return num.ToString("N" + decimalLen.ToString());
        }

        /// <summary>
        /// Add thousands separator
        /// </summary>
        /// <returns></returns>
        public static string AddSeperator(int num)
        {
            return num.ToString("N").Split('.')[0];
        }

        /// <summary>
        /// Remove thousands separator
        /// </summary>
        /// <returns></returns>
        public static Decimal RemoveSeperator(String numStr)
        {
            return Decimal.Parse(numStr.Replace(",", ""));
        }

        /// <summary>
        /// Convert the string to date type.
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDate(string strDate)
        {
            DateTime outDt;
            if (DateTime.TryParse(strDate, out outDt))
            {
                return outDt;
            }
            else
            {
                if (strDate.Length == 8)
                {
                    strDate = strDate.Substring(0, 4) + "-" + strDate.Substring(4, 2) + "-" + strDate.Substring(6, 2);
                }
                if (DateTime.TryParse(strDate, out outDt))
                {
                    return outDt;
                }
                else
                {
                    return Convert.ToDateTime("1900-01-01");
                }
            }
            //return Convert.ToDateTime(strDate);
        }

        /// <summary>
        /// Convert SAP Date format to DateTime       
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="dtOut"></param>
        /// <returns></returns>
        public static bool ConvertSAPDate(string strDate, out DateTime dtOut)
        {
            if (DateTime.TryParse(strDate, out dtOut))
            {
                return true;
            }
            strDate = strDate.Substring(0, 4) + "-" + strDate.Substring(4, 2) + "-" + strDate.Substring(6, 2);
            return DateTime.TryParse(strDate,out dtOut);
        }
        /// <summary>
        /// Convert the date to string type(20080101).
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static string ConvertDateToString(DateTime dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.Year.ToString());
            sb.Append(dt.Month.ToString().PadLeft(2, '0'));
            sb.Append(dt.Day.ToString().PadLeft(2, '0'));
            return sb.ToString();
        }

        /// <summary>
        /// 转换成标准日期型的字符串格式(2010-08-08)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDateToStandardString(DateTime dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.Year.ToString() + "-");
            sb.Append(dt.Month.ToString().PadLeft(2, '0') + "-");
            sb.Append(dt.Day.ToString().PadLeft(2, '0'));
            return sb.ToString();
        }

        /// <summary>
        /// 转换成时间型字符串(08:30)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDateToHourString(DateTime dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.Hour.ToString().PadLeft(2, '0') + ":");
            sb.Append(dt.Minute.ToString().PadLeft(2, '0'));
            return sb.ToString();
        }

        /// <summary>
        /// Convert object to data time string, for page to display
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String ToDateTimeStr(object val)
        {
            if (val != null && val.ToString() != "")
            {
                return Convert.ToDateTime(val).ToString("yyyy-MM-dd HH:mm:ss");
            }
            return "";
        }

        /// <summary>
        /// 轉換成2010-06-06 17:30:30格式
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String ToLongDateTimeStr(DateTime val)
        {
            if (val != null && val.ToString() != "")
            {
                return val.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return "";
        }

        /// <summary>
        /// Convert object to data string, for page to display
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String ToDateStr(object val)
        {
            if (val != null && val.ToString() != "")
            {
                return Convert.ToDateTime(val).ToString("yyyy-MM-dd");
            }
            return "";
        }

        /// <summary>
        /// Convert object to int
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt(object val)
        {
            try
            {
                return Convert.ToInt32(val);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// whethor or not date
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsDate(string strDate)
        {
            DateTime dtDate;
            bool bValid = true;
            try
            {
                dtDate = DateTime.Parse(strDate);
            }
            catch (FormatException)
            {
                bValid = false;
            }
            return bValid;
        }
    }
}

