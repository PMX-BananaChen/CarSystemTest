using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using System.Net;
using System.Diagnostics;

namespace CarSystemTest.Common
{
    public class FileHelper
    {
        /// <summary>
        /// 根據文件路徑獲取文件名稱
        /// </summary> 
        public static string GetFileName(string filePath)
        {
            if (ExistsFile(filePath))
            {
                return System.IO.Path.GetFileName(filePath);
            }
            return null;

        }

        /// <summary>
        /// 判斷文件是否存在
        /// </summary> 
        public static bool ExistsFile(string filePath)
        {
            FileInfo f = null;
            try
            {
                f = new FileInfo(filePath);
                return f.Exists;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(e);
                return false;
            }
        }



        /// <summary>
        /// 刪除文件
        /// </summary> 
        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileInfo fi = new FileInfo(filePath);
                    fi.Attributes = FileAttributes.Normal;
                    File.Delete(filePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 創建文件夾
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool CreateFolder(string Path)
        {
            try
            {
                if (!System.IO.Directory.Exists(Path))
                {
                    System.IO.Directory.CreateDirectory(Path);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 將字符串保存在文件中
        /// </summary>
        /// <param name="strContent">字符串內容</param>
        /// <param name="filePath">文件全名稱(帶路徑)</param>
        /// <returns>是否成功</returns>
        public static bool SaveStringToFile(string strContent, string filePath)
        {
            FileStream fs = null;
            try
            {
                FileHelper.DeleteFile(filePath);
                fs = new FileStream(filePath, FileMode.OpenOrCreate);
                byte[] bs = Encoding.UTF8.GetBytes(strContent);
                fs.Write(bs, 0, bs.Length);
                fs.Flush();
            }
            catch (Exception e)
            {
                 CarSystemTest.Common.LogHelper.WriteLog(e);                
                throw e;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return true;
        }
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;
        System.Security.Principal.WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern int LogonUser(String lpszUserName,
        String lpszDomain,
        String lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int DuplicateToken(IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

        public bool impersonateValidUser(String userName, String domain, String password)
        {
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
           LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            {
                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    System.Security.Principal.WindowsIdentity tempWindowsIdentity;
                    tempWindowsIdentity = new System.Security.Principal.WindowsIdentity(tokenDuplicate);
                    impersonationContext = tempWindowsIdentity.Impersonate();
                    if (impersonationContext != null)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public void undoImpersonation()
        {
            impersonationContext.Undo();//回退为未更改前账户  
        }


        public void UploadFile(HttpPostedFile FileUpload, string filepath)
        {
            //临时更改为跟网络硬盘相同用户名密码的账户（此账户必须在网络盘有写入权限）本机也需要同样帐号密码的帐户  
            if (impersonateValidUser("IT-SPteam01", "pcn", "1234.abc"))
            {
                FileUpload.SaveAs(filepath);
                undoImpersonation();//回退为未更改前账户  
            }
        }


        ///// <summary>
        ///// NetUse文件上傳的路徑
        ///// </summary>
        //public static void NetUseFilePath()
        //{
        //    Config cf = new Config();
        //    string FilePath = @"D:\Files";
        //    FilePath = FilePath.Remove(FilePath.Length - 1, 1);

        //    string UserName = "ftpUser";
        //    string Password = "ftpUser";

        //    string removePath = @"net use " + FilePath + " /delete";
        //    string addPath = @"net use " + FilePath + " " + Password + " /User:" + UserName + "";
        //    System.Diagnostics.Process process = new System.Diagnostics.Process();
        //    process.StartInfo.FileName = "cmd.exe";//调用cmd执行dos命令
        //    process.StartInfo.UseShellExecute = false;
        //    process.StartInfo.RedirectStandardInput = true;
        //    process.StartInfo.RedirectStandardOutput = true;
        //    process.StartInfo.RedirectStandardError = true;
        //    process.StartInfo.CreateNoWindow = true;//是否显示cmd窗口
        //    process.Start();
        //    process.StandardInput.WriteLine(removePath);
        //    process.StandardInput.WriteLine(addPath);
        //    process.StandardInput.WriteLine("exit");
        //    string _errmsg = process.StandardOutput.ReadToEnd();

        //    LogHelper.WriteLog(_errmsg);
        //    process.Close();
        //}
    }
}