using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace CarSystemTest.Common
{
    public class Util
    {
        public static void ShowMessage(string message)
        {
            HttpContext.Current.Response.Write("<script language=javascript> alert('" + message + "');</script>");
        }
        public static Int64 GetNewBusinessId(int creater)
        {
            return Convert.ToInt64(string.Format("{0}{1:yyMMddHHmmssff}", creater, DateTime.Now));
        }

        public static void ClearCtl(System.Web.UI.ControlCollection ctls)
        {
            foreach (Control ctl in ctls)
            {
                if (ctl is MasterPage)
                {
                    foreach (ContentPlaceHolder cph in ctl.Controls)
                    {
                        ClearCtl(cph.Controls);
                    }
                }
                if (ctl is HtmlInputText)
                    ((HtmlInputText)ctl).Value = "";
                if (ctl is HtmlInputCheckBox)
                    ((HtmlInputCheckBox)ctl).Checked = false;
                if (ctl is HtmlSelect)
                    ((HtmlSelect)ctl).SelectedIndex = -1;
                if (ctl is TextBox)
                    ((TextBox)ctl).Text = "";
                if (ctl is DropDownList)
                    ((DropDownList)ctl).SelectedIndex = -1;
                if (ctl is CheckBox)
                    ((CheckBox)ctl).Checked = false;
            }
        }
        /// <summary>
        /// 合併repetear 單元個
        /// </summary>
        /// <param name="rpt"></param>
        /// <param name="cellName">td 的ID</param>
        public static void UniteRepeaterCell(Repeater rpt, string cellName)
        {
            for (int i = rpt.Items.Count - 1; i > 0; i--)
            {
                HtmlTableCell upCell = rpt.Items[i - 1].FindControl(cellName) as HtmlTableCell;
                HtmlTableCell currCell = rpt.Items[i].FindControl(cellName) as HtmlTableCell;
                currCell.RowSpan = (currCell.RowSpan == -1) ? 1 : currCell.RowSpan;
                upCell.RowSpan = (upCell.RowSpan == -1) ? 1 : upCell.RowSpan;
                if (currCell.InnerText == upCell.InnerText)
                {
                    currCell.Visible = false;
                    upCell.RowSpan += currCell.RowSpan;
                }

            }
        }

        #region JS彈出窗體、刪除提示
        /// <summary>
        /// 确认删除数据记录
        /// </summary>
        /// <returns></returns>
        public static string GetJScriptConfirm()
        {
            string sScript = "javascript:if(!window.confirm('您确认要删除吗？')) return false;";
            return sScript;
        }

        /// <summary>
        /// 确认删除数据记录
        /// </summary>
        /// <returns></returns>
        public static string GetJScriptConfirm(string sMsg)
        {
            string sScript = "javascript:if(!window.confirm('" + sMsg + "')) return false;";
            return sScript;
        }

        /// <summary>
        /// 弹出修改窗口
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetJScriptWindowOpen(string sUrl)
        {
            string sScript = "javascript:window.open('" + sUrl + "')";
            return sScript;
        }

        /// <summary>
        /// 弹出非模式对话框
        /// </summary>
        /// <param name="response"></param>
        /// <param name="sUrl"></param>
        public static string OpenWinModelessDialog(string sUrl)
        {
            string sScript = "javascript:window.showModelessDialog('" + sUrl + "','','edge:raised;dialogHeight:180px;dialogWidth:400px;status:no;help:no;scroll:no');return false;";
            return sScript;
        }

        /// <summary>
        /// 弹出模式对话框
        /// </summary>
        /// <param name="response"></param>
        /// <param name="sUrl"></param>
        public static string OpenWinModalDialog(string sUrl)
        {
            string sShowFrameDailog = "Web/ShowFrameDailog.aspx?sUrl=" + sUrl;
            string sScript = "javascript:window.showModalDialog('" + sShowFrameDailog + "','','dialogHeight:580px;dialogWidth:780px;status:no;help:no;scroll:yes');return false;";
            return sScript;
        }

        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="sUrl"></param>
        /// <returns></returns>
        public static string OpenWinDialog(string sUrl)
        {
            string sScript = "javascript:window.open('" + sUrl + "','ShowWinDialog','left=40,top=40,width=760,height=580,toolbar=0,scrollbars=1,z-look=1,resizable=1');return false;";
            return sScript;
        }

        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="sUrl"></param>
        /// <returns></returns>
        public static string OpenWinDialog(string sWindowName, string sUrl)
        {
            string sScript = "javascript:window.open('" + sUrl + "','" + sWindowName + "','left=40,top=40,width=760,height=580,toolbar=0,scrollbars=1,z-look=1,resizable=1');return false;";
            return sScript;
        }

        /// <summary>
        /// 指定窗口大小，弹出窗口
        /// </summary>
        /// <param name="sUrl"></param>
        /// <returns></returns>
        public static string OpenWinDialog(string sWindowName, string sUrl, string sWidth, string sHeight)
        {
            string sScript = "<script language='javascript'>window.open('" + sUrl + "','" + sWindowName + "','width=" + sWidth + ",height=" + sHeight + ",toolbar=0,scrollbars=1,z-look=1,resizable=1');</script>";
            return sScript;
        }
        #endregion JS彈出窗體、刪除提示

        /// <summary>
        ///根据传入的字符串得到分隔符左边或者分隔符右边的字符串0表示左边，1表示右边。
        /// </summary>
        /// <param name="original"></param>
        /// <param name="split"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSubStrByStr(string original, string split, int type)
        {
            if (original.Trim().Length > 0)
            {
                int i = original.IndexOf(split);
                if (i != -1)
                {
                    if (type == 0)
                        return original.Substring(0, i);
                    else
                        return original.Substring(i + 1);
                }
                else
                {
                    if (type == 0)
                        return original;
                    else
                        return "";
                }
            }
            else
                return "";
        }
    }
}
