using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarSystemTest.Common;
using CarSystemTest.Business;

namespace CarSystemTest.GPS
{
    public partial class CarTracking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Security sec = new Security();
            string businessCode = sec.Decode64(Request["BusinessCode"]);
            if (!IsPostBack)
            {
                InitData(businessCode);
            }
        }

        private void InitData(string businessCode)
        {
            CarBiz cb = new CarBiz();
            string url = cb.GetGpsURL(businessCode);
            this.frm.Src = url;
        }     

    }
}