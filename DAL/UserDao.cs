using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using CarSystemTest.DBUtility;
using CarSystemTest.Model;


namespace CarSystemTest.DAL
{
    public class UserDao
    {
        Database db;
        public UserDao()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 記錄用戶登錄信息
        /// </summary>
        /// <param name="userAccount">用戶帳號</param>
        /// <param name="password">帳號密碼</param>
        /// <param name="ip">用戶電腦ip</param>
        /// <param name="hostName">用戶電腦名稱</param>
        /// <param name="remark">備註</param>
        /// <param name="lockCount">帳號被鎖次數</param>
        /// <param name="userID">用戶ID</param>
        /// <param name="message"></param>
        public void UserLogin(string userAccount, string password, string ip, string hostName, string remark, int lockCount, ref int userID, ref string message)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_UserLogin");
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);
            db.AddInParameter(cmd, "@password", DbType.String, password);
            db.AddInParameter(cmd, "@ip", DbType.String, ip);
            db.AddInParameter(cmd, "@hostName", DbType.String, hostName);
            db.AddInParameter(cmd, "@remark", DbType.String, remark);
            db.AddInParameter(cmd, "@lockCount", DbType.Int32, lockCount);
            db.AddOutParameter(cmd, "@userID", DbType.Int32, 5);
            db.AddOutParameter(cmd, "@message", DbType.String, 100);

            db.ExecuteNonQuery(cmd);

            userID = (int)db.GetParameterValue(cmd, "@userID");
            message = db.GetParameterValue(cmd, "@message").ToString();
        }

        /// <summary>
        /// 根據用戶ID 獲取用戶信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet GetUserInfoById(int userID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@UserID", userID.ToString());
            return DBUtil.GetSearchResult("GetUserInfoById", dic);
        }

        /// <summary>
        /// 域登陸
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public int UserADLogin(string userAccount)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_UserADLogin");
            db.AddInParameter(cmd, "@userAccount", DbType.String, userAccount);
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }

        /// <summary>
        /// 獲取用戶菜單
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserMenus(int userId)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetUserMenus");
            db.AddInParameter(cmd, "@UserID", DbType.Int32, userId);
            DataSet ds = db.ExecuteDataSet(cmd);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判斷用戶是否有頁面的操作權限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool HasPagePrivilege(int userId, string url)
        {
            bool hasNoPrivilege = false;
            DbCommand cmd = db.GetStoredProcCommand("proc_CommHasPagePrivilege");
            db.AddInParameter(cmd, "@UserID", DbType.Int32, userId);
            db.AddInParameter(cmd, "@Url", DbType.String, url);
            int flag = Convert.ToInt32(db.ExecuteScalar(cmd));
            if (flag == 1)
            {
                hasNoPrivilege = true;
            }
            return hasNoPrivilege;
        }

        /// <summary>
        /// 修改權限
        /// </summary>
        /// <param name="dtPrivilege"></param>
        /// <returns></returns>
        public bool ModifyPrivilege(DataSet dsPrivilege, string[] tblName, bool flag)
        {
            bool isSuccess = false;
            if (flag)
            {
                isSuccess = DBUtil.UpdateDataSet(dsPrivilege, tblName, new string[,] { { "InsertSys_Privilege", "", "" } });
            }
            else
            {
                isSuccess = DBUtil.UpdateDataSet(dsPrivilege, tblName, new string[,] { { "", "UpdateSys_Privilege", "" } });
            }
            return isSuccess;
        }

        /// <summary>
        /// 獲取GenCode 2：用戶ID 3:角色ID 4:權限ID
        /// </summary>
        /// <param name="genType"></param>
        /// <returns></returns>
        public int GetGenCode(int genType)
        {
            CommonGen cg = new CommonGen();
            return Convert.ToInt32(cg.GetGenCode(genType));
        }

        /// <summary>
        /// 獲取父級權限對應的下一個子權限ID
        /// </summary>
        /// <param name="privilegeId"></param>
        /// <returns></returns>
        public short GetNextSonPrivilege(short privilegeId)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_GetNextSonPrivilege");
            db.AddInParameter(cmd, "@ParentPrivilegeCode", DbType.Int16, privilegeId);
            return Convert.ToInt16(db.ExecuteScalar(cmd));
            //IDictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("@GetNextSonPrivilege", privilegeId.ToString());
            //DataTable dt = DBUtil.GetSearchResult("GetNextSonPrivilege", dic).Tables[0];
            //return Convert.ToInt16(dt.Rows[0]["PrivilegeCode"]);
        }

        /// <summary>
        /// 獲取所有的父級權限
        /// </summary>
        /// <returns></returns>
        public DataTable GetParentPrivileges()
        {
            return DBUtil.GetSearchResult("GetParentPrivileges").Tables[0];
        }

        /// <summary>
        /// 根據ID獲取對應權限
        /// </summary>
        /// <param name="privilegeId"></param>
        /// <returns></returns>
        public DataTable GetPrivilegeById(int privilegeId)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@PrivilegeCode", privilegeId.ToString());
            return DBUtil.GetSearchResult("GetPrivilegeById", dic).Tables[0];
        }

        /// <summary>
        /// 根據RoleCode獲取對應角色，權限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataSet GetRolePrivileges(int roleId)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@RoleCode", roleId.ToString());
            return DBUtil.GetSearchResult("GetRoleInfoById", dic);
        }

        /// <summary>
        /// 更改用戶密碼
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool UpdatePwd(int userId, string pwd)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@UserID", userId.ToString());
            dic.Add("@Password", pwd);
            return DBUtil.Update("UpdatePwd", dic);
        }


        /// <summary>
        /// 修改角色以及對應的權限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <param name="remark"></param>
        /// <param name="privileges"></param>
        /// <returns></returns>
        public bool ModifyRolePrivilege(int roleId, string roleName, string remark, string privileges)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_RolePrivilegesModify");
            db.AddInParameter(cmd, "@RoleCode", DbType.Int16, roleId);
            db.AddInParameter(cmd, "@RoleName", DbType.String, roleName);
            db.AddInParameter(cmd, "@Remark", DbType.String, remark);
            db.AddInParameter(cmd, "@Privileges", DbType.String, privileges);
            int cnt = db.ExecuteNonQuery(cmd);
            bool isSuceess = false;
            if (cnt > 0)
            {
                isSuceess = true;
            }
            return isSuceess;
        }

        /// <summary>
        /// 數據綁定時 獲取用戶類型，用戶級別，用戶所屬部門或廠商
        /// </summary>
        /// <returns></returns>
        public DataSet GetUserTypeGradeEmp()
        {
            return DBUtil.GetSearchResult("GetUserTypeGradeEmp");
        }


        /// <summary>
        /// 獲取所有的部門信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllDept()
        {
            return DBUtil.GetSearchResult("GetAllDepts").Tables[0];
        }

        /// <summary>
        /// 根據用戶Id，獲取對應的用戶信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserinfoById(int userId)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@UserID", userId.ToString());
            return DBUtil.GetSearchResult("GetUserInfoByUserId", dic).Tables[0];
        }

        /// <summary>
        /// 用戶信息更新 包括新增和修改
        /// </summary>
        /// <param name="ue"></param>
        /// <param name="empNo"></param>
        /// <param name="depOrVendor"></param>
        /// <returns></returns>
        public bool ModifyUserInfo(UserEntity ue, string empNo, string depOrVendor)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_UserInfoModify");
            db.AddInParameter(cmd, "@UserID", DbType.Int32, ue.UserID);
            db.AddInParameter(cmd, "@UserAccount", DbType.String, ue.UserAccount);
            db.AddInParameter(cmd, "@UserChineseName", DbType.String, ue.UserChineseName);
            db.AddInParameter(cmd, "@UserEnglishName", DbType.String, ue.UserEnglishName);
            db.AddInParameter(cmd, "@IsEnabled", DbType.String, ue.IsEnabled);
            db.AddInParameter(cmd, "@EffectiveTime", DbType.DateTime, ue.EffectiveTime);
            db.AddInParameter(cmd, "@ExpireTime", DbType.DateTime, ue.ExpireTime);
            db.AddInParameter(cmd, "@Email", DbType.String, ue.Email);
            db.AddInParameter(cmd, "@Mobile", DbType.String, ue.Mobile);
            db.AddInParameter(cmd, "@Tel", DbType.String, ue.Tel);
            db.AddInParameter(cmd, "@Password", DbType.String, ue.Password);
            db.AddInParameter(cmd, "@Sex", DbType.String, ue.Sex);
            db.AddInParameter(cmd, "@UserTypeCode", DbType.Int16, ue.UserTypeCode);
            db.AddInParameter(cmd, "@UserGradeCode", DbType.Int16, ue.UserGradeCode);
            db.AddInParameter(cmd, "@Remark", DbType.String, ue.Remark);
            db.AddInParameter(cmd, "@EmpNo", DbType.String, empNo);
            db.AddInParameter(cmd, "@DepOrVendor", DbType.String, depOrVendor);
            int cnt = db.ExecuteNonQuery(cmd);
            if (cnt > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 獲取用戶綁定的角色或權限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetUserRolePrivilege(int userId)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@UserID", userId.ToString());
            return DBUtil.GetSearchResult("GetRoleInfoByUserId", dic);
        }

        /// <summary>
        /// 修改用戶對應的角色和權限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        /// <param name="privileges"></param>
        /// <returns></returns>
        public bool ModifyUserRolePrivilege(int userId, string roles, string privileges)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_UserRolePrivilegeModify");
            db.AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            db.AddInParameter(cmd, "@Roles", DbType.String, roles);
            db.AddInParameter(cmd, "@Privileges", DbType.String, privileges);
            int cnt = db.ExecuteNonQuery(cmd);
            bool flag = false;
            if (cnt > 0)
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 獲取用戶對應的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetRolesByUserId(int userId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@UserID", userId.ToString());
            return DBUtil.GetSearchResult("GetRolesByUserId", dic);
        }

        /// <summary>
        /// 獲取權限明細
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public DataSet GetPrivileges(int pageSize, int currentPage)
        {
            DbCommand cmd = db.GetStoredProcCommand("proc_getPrivileges");
            db.AddInParameter(cmd, "@pageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@currentPage", DbType.Int32, currentPage);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 獲取角色信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public DataSet GetRoles(int pageSize, int currentPage) {
            DbCommand cmd = db.GetStoredProcCommand("proc_getRoles");
            db.AddInParameter(cmd, "@pageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@currentPage", DbType.Int32, currentPage);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 刪掉角色及角色對應的權限
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public bool DeleteRole(int roleCode)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@RoleCode",roleCode.ToString());
            return DBUtil.Delete("DeleteRole", dic);
        }

        /// <summary>
        /// 禁用用戶
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteUser(int userId) {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@UserID", userId.ToString());
            return DBUtil.Update("DeleteUser", dic);
        }


        /// <summary>
        /// 獲取用戶信息記錄
        /// </summary>
        /// <param name="englishName"></param>
        /// <param name="isEnabled"></param>
        /// <param name="area"></param>
        /// <param name="userType"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public DataSet getUserInfo(string englishName, string isEnabled, string area, string userType, int pageSize, int currentPage) {
            DbCommand cmd = db.GetStoredProcCommand("proc_getUserInfo");
            db.AddInParameter(cmd, "@englishName", DbType.String, englishName);
            db.AddInParameter(cmd, "@isEnabled", DbType.String, isEnabled);
            db.AddInParameter(cmd, "@area", DbType.String, area);
            db.AddInParameter(cmd, "@userType", DbType.String, userType);
            db.AddInParameter(cmd, "@pageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@currentPage", DbType.Int32, currentPage);
            return db.ExecuteDataSet(cmd);
        }
    }
}
