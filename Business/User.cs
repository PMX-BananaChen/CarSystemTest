using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CarSystemTest.DAL;
using CarSystemTest.Common;
using CarSystemTest.Model;

namespace CarSystemTest.Business
{
    public class User
    {
        private UserDao userDao = new UserDao();
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
            userDao.UserLogin(userAccount, Security.EncryptPassword(password, "MD5"), ip, hostName, remark, lockCount, ref userID, ref message);
        }

        /// <summary>
        /// 綁定用戶信息
        /// </summary>
        /// <param name="userID"></param>
        public void SetUserInfo(int userID)
        {
            DataSet ds = userDao.GetUserInfoById(userID);
            DataTable dt = ds.Tables[0];
            UserEntity ue = new UserEntity();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ue.UserID = Convert.ToInt32(dr["UserID"]);
                ue.UserAccount = dr["UserAccount"].ToString();
                ue.UserChineseName = dr["UserChineseName"].ToString();
                ue.UserEnglishName = dr["UserEnglishName"].ToString();
                ue.IsEnabled = Convert.ToChar(dr["IsEnabled"]);
                ue.EffectiveTime = Convert.ToDateTime(dr["EffectiveTime"]);
                ue.ExpireTime = Convert.ToDateTime(dr["ExpireTime"]);
                ue.Email = dr["Email"].ToString();
                ue.Mobile = dr["Mobile"].ToString();
                ue.Tel = dr["Tel"].ToString();
                ue.Password = dr["Password"].ToString();
                ue.Sex = Convert.ToChar(dr["Sex"]);
                ue.UserTypeCode = Convert.ToInt32(dr["UserTypeCode"]);
                ue.UserGradeCode = Convert.ToInt32(dr["UserGradeCode"]);
                ue.Remark = dr["Remark"].ToString();
            }
            else
            {
                ue = null;
            }
            WebUser.CurrentUser = ue;
        }

        /// <summary>
        /// 域登陸
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public int UserADLogin(string userAccount)
        {
            return userDao.UserADLogin(userAccount);
        }

        /// <summary>
        /// 獲取用戶菜單
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserMenus(int userId)
        {
            return userDao.GetUserMenus(userId);
        }

        /// <summary>
        /// 修改權限
        /// </summary>
        /// <param name="dsPrivilege"></param>
        /// <param name="tblName"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool ModifyPrivilege(DataSet dsPrivilege, string[] tblName, bool flag)
        {
            return userDao.ModifyPrivilege(dsPrivilege, tblName, flag);
        }

        /// <summary>
        /// 獲取GenCode 2：用戶ID 3:角色ID 4:權限ID
        /// </summary>
        /// <param name="genType"></param>
        /// <returns></returns>
        public int GetGenCode(int genType)
        {
            return userDao.GetGenCode(genType);
        }

        /// <summary>
        /// 獲取父級權限對應的下一個子權限ID
        /// </summary>
        /// <returns></returns>
        public short GetNextSonPrivilege(short privilegeId)
        {
            return userDao.GetNextSonPrivilege(privilegeId);
        }

        /// <summary>
        /// 獲取所有的父級權限
        /// </summary>
        /// <returns></returns>
        public DataTable GetParentPrivileges()
        {
            return userDao.GetParentPrivileges();
        }

        /// <summary>
        /// 根據ID獲取對應權限
        /// </summary>
        /// <param name="privilegeId"></param>
        /// <returns></returns>
        public DataTable GetPrivilegeById(int privilegeId)
        {
            return userDao.GetPrivilegeById(privilegeId);
        }

        /// <summary>
        /// 根據RoleCode獲取對應角色，權限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataSet GetRolePrivileges(int roleId)
        {
            return userDao.GetRolePrivileges(roleId);
        }

        /// <summary>
        /// 更改用戶密碼
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool UpdatePwd(int userId, string pwd)
        {
            return userDao.UpdatePwd(userId, pwd);
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
            return userDao.ModifyRolePrivilege(roleId, roleName, remark, privileges);
        }

        /// <summary>
        /// 數據綁定時 獲取用戶類型，用戶級別，用戶所屬部門或廠商
        /// </summary>
        /// <returns></returns>
        public DataSet GetUserTypeGradeEmp()
        {
            return userDao.GetUserTypeGradeEmp();
        }

        /// <summary>
        /// 獲取所有的部門信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllDepts()
        {
            return userDao.GetAllDept();
        }

        /// <summary>
        /// 根據用戶Id，獲取對應的用戶信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserInfoById(int userId)
        {
            return userDao.GetUserinfoById(userId);
        }

        /// <summary>
        /// 更新用戶信息 包括新增和修改
        /// </summary>
        /// <param name="ue"></param>
        /// <param name="empNo"></param>
        /// <param name="depOrVendor"></param>
        /// <returns></returns>
        public bool ModifyUserInfo(UserEntity ue, string empNo, string depOrVendor)
        {
            return userDao.ModifyUserInfo(ue, empNo, depOrVendor);
        }

        /// <summary>
        /// 獲取用戶對應的角色或權限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetUserRolePrivilege(int userId)
        {
            return userDao.GetUserRolePrivilege(userId);
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
            return userDao.ModifyUserRolePrivilege(userId, roles, privileges);
        }

        /// <summary>
        /// 獲取用戶對應的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetRolesByUserId(int userId)
        {
            DataSet ds = userDao.GetRolesByUserId(userId);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 獲取權限明細
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public DataSet GetPrivilege(int pageSize, int currentPage)
        {
            return userDao.GetPrivileges(pageSize, currentPage);
        }

        /// <summary>
        /// 獲取角色信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public DataSet GetRoles(int pageSize, int currentPage)
        {
            return userDao.GetRoles(pageSize, currentPage);
        }

        /// <summary>
        /// 刪除角色及角色對應的權限
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public bool DeleteRole(int roleCode)
        {
            return userDao.DeleteRole(roleCode);
        }

        /// <summary>
        /// 禁用用戶
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteUser(int userId) {
            return userDao.DeleteUser(userId);
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
            return userDao.getUserInfo(englishName, isEnabled, area, userType, pageSize, currentPage);
        }
    }
}
