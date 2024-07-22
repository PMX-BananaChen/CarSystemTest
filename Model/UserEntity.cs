using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSystemTest.Model
{
    public class UserEntity
    {
        public int UserID { get; set; }
        public string UserAccount { get; set; }
        public string UserChineseName { get; set; }
        public string UserEnglishName { get; set; }
        public char IsEnabled { get; set; }
        public DateTime EffectiveTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Password { get; set; }
        public char Sex { get; set; }
        public int UserTypeCode { get; set; }
        public int UserGradeCode { get; set; }
        public string Remark { get; set; }
    }
}
