using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarSystemTest.Common;

namespace CarSystemTest.Car
{
    public partial class Redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string businessCode = Convert.ToString(Request["BusinessCode"]);
            string currFlowStatus = Convert.ToString(Request["FlowStatusCode"]);
            if (!IsPostBack)
            {
                RedirectURL(businessCode, currFlowStatus);
            }
        }

        private void RedirectURL(string businessCode, string currFlowStatus)
        {
            Security sec = new Security();
            string url = "";
            businessCode = sec.Encode64(businessCode);
            currFlowStatus = sec.Encode64(currFlowStatus);
            url = "AppDetail.aspx?BusinessCode=" + businessCode + "&FlowStatus=" + currFlowStatus;
            Response.Redirect(url);
        }
    }
}