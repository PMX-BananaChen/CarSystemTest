using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CarSystemTest.Model;


namespace CarSystemTest.Common
{
    public class WebUser
    {
        private const string SESSIONKEY_USER = "CurrentUser";

        /// <summary>
        /// 當前用戶Entity對象
        /// </summary>
        public static UserEntity CurrentUser
        {
            get
            {
                return HttpContext.Current.Session[SESSIONKEY_USER] as UserEntity;
            }
            set
            {
                HttpContext.Current.Session[SESSIONKEY_USER] = value;
            }
        }
    }
}
