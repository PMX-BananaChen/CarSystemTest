using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CarSystemTest.Common;
using CarSystemTest.Business;

namespace CarSystemTest.Car
{
    public partial class CalFee : BasePage
    {

        private CarBiz cb = new CarBiz();
        private string businesCode = "";
        private string userAccount = "";
        private float baseFeeTtl = 0f;
        private float MileageFeeTtl = 0f;
        private float detourFeeTtl = 0f;
        private float tollFeeTtl = 0f;
        private float waitingFeeTtl = 0f;
        private float subtotalFeeTtl = 0f;
        protected void Page_Load(object sender, EventArgs e)
        {
            Security sec = new Security();
            businesCode = sec.Decode64(Request["BusinessCode"].ToString());
            userAccount = WebUser.CurrentUser.UserAccount;
            if (!IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {
            DataTable dtAppFee = cb.GetAppFee(businesCode);
            if (dtAppFee != null && dtAppFee.Rows.Count > 0)
            {
                string remark = "";
                for (int i = 0; i < dtAppFee.Rows.Count; i++)
                {
                    DataRow dr = dtAppFee.Rows[i];
                    if (remark != Convert.ToString(dr["ZwRemark"]))
                    {
                        remark = remark + Convert.ToString(dr["ZwRemark"]);
                    }
                }
                tbZwRemark.Text = remark;
                this.rptAppFee.DataSource = dtAppFee;
                this.rptAppFee.DataBind();
            }
        }

        protected void AppFee_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1)
            {
                float baseFee = 0;
                float MileageFee = 0;
                float detourFee = 0;
                float tollFee = 0;
                float waitingFee = 0;
                float subtotalFee=0;
                if (((HtmlInputText)e.Item.FindControl("txtBaseFee")).Value.Trim()!="")
                    baseFee = float.Parse(((HtmlInputText)e.Item.FindControl("txtBaseFee")).Value.Trim());
              
                if (((HtmlInputText)e.Item.FindControl("txtMileageFee")).Value.Trim() != "")
                      MileageFee = float.Parse(((HtmlInputText)e.Item.FindControl("txtMileageFee")).Value.Trim());
                 
                if (((HtmlInputText)e.Item.FindControl("txtDetourFee")).Value.Trim() != "")
                    detourFee = float.Parse(((HtmlInputText)e.Item.FindControl("txtDetourFee")).Value.Trim());
                
                if (((HtmlInputText)e.Item.FindControl("txtTollFee")).Value.Trim() != "")
                    tollFee = float.Parse(((HtmlInputText)e.Item.FindControl("txtTollFee")).Value.Trim());
               
                if (((HtmlInputText)e.Item.FindControl("txtWaitingFee")).Value.Trim() != "")
                    waitingFee = float.Parse(((HtmlInputText)e.Item.FindControl("txtWaitingFee")).Value.Trim());

                if (((Label)e.Item.FindControl("lblSubtotalFee")).Text.Trim() != "")
                      subtotalFee = float.Parse(((Label)e.Item.FindControl("lblSubtotalFee")).Text.Trim());

                //float baseFee = float.Parse(((Label)e.Item.FindControl("lblBaseFee")).Text.Trim());
                //float detourFee = float.Parse(((Label)e.Item.FindControl("lblDetourFee")).Text.Trim());
                //float tollFee = float.Parse(((Label)e.Item.FindControl("lblTollFee")).Text.Trim());
                //float waitingFee = float.Parse(((Label)e.Item.FindControl("lblWaitingFee")).Text.Trim());
                //float subtotalFee = float.Parse(((Label)e.Item.FindControl("lblSubtotalFee")).Text.Trim());
                baseFeeTtl += baseFee;
                MileageFeeTtl += MileageFee;
                detourFeeTtl += detourFee;
                tollFeeTtl += tollFee;
                waitingFeeTtl += waitingFee;
                subtotalFeeTtl += subtotalFee;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                ((HtmlInputText)e.Item.FindControl("txtBaseFeeTtl")).Value = Convert.ToDecimal(baseFeeTtl).ToString();
                ((HtmlInputText)e.Item.FindControl("txtMileageFeeTtl")).Value = MileageFeeTtl.ToString();
                ((HtmlInputText)e.Item.FindControl("txtDetourFeeTtl")).Value = detourFeeTtl.ToString();
                ((HtmlInputText)e.Item.FindControl("txtTollFeeTtl")).Value = tollFeeTtl.ToString();
                ((HtmlInputText)e.Item.FindControl("txtWaitingFeeTtl")).Value = waitingFeeTtl.ToString();
                ((Label)e.Item.FindControl("lblSubtotalFeeTtl")).Text = subtotalFeeTtl.ToString();

                //((Label)e.Item.FindControl("lblBaseFeeTtl")).Text = Convert.ToDecimal(baseFeeTtl).ToString();
                //((TextBox)e.Item.FindControl("tbDetourFeeTtl")).Text = detourFeeTtl.ToString();
                ////((HtmlInputText)e.Item.FindControl("tbTollFeeTtl")).Value = tollFeeTtl.ToString();
                //((Label)e.Item.FindControl("lblTollFeeTtl")).Text = tollFeeTtl.ToString();
                //((Label)e.Item.FindControl("lblWaitingFeeTtl")).Text = waitingFeeTtl.ToString();
                //((Label)e.Item.FindControl("lblSubtotalFeeTtl")).Text = subtotalFeeTtl.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtFee = new DataTable();
            dtFee.Columns.Add("BusinessCode", typeof(System.Int64));
            dtFee.Columns.Add("DetourFee", typeof(System.Decimal));
            dtFee.Columns.Add("MileageFee", typeof(System.Decimal));
            dtFee.Columns.Add("ZwRemark", typeof(System.String));

            decimal BaseFee;

            for (int i = 0; i < rptAppFee.Items.Count; i++)
            {
                DataRow dr = dtFee.NewRow();
                dr["BusinessCode"] = Convert.ToInt64(((Label)rptAppFee.Items[i].FindControl("lblBusinessCode")).Text);
                if (((HtmlInputText)rptAppFee.Items[i].FindControl("txtDetourFee")).Value != "")
                {
                    dr["DetourFee"] = Convert.ToDecimal(((HtmlInputText)rptAppFee.Items[i].FindControl("txtDetourFee")).Value);
                }
                else
                {
                    dr["DetourFee"] = 0;
                }
                if (((HtmlInputText)rptAppFee.Items[i].FindControl("txtMileageFee")).Value != "")
                {
                    dr["MileageFee"] = ((HtmlInputText)rptAppFee.Items[i].FindControl("txtMileageFee")).Value;
                }
                else
                {
                    dr["MileageFee"] = 0;
                }
                dr["ZwRemark"] = this.tbZwRemark.Text.Trim();
                dtFee.Rows.Add(dr);
            }

            bool isSuccess = cb.UpdateDetourFee(dtFee);
            if (isSuccess)
            {
                lblMsg.Text = "保存成功！";
                if (((HtmlInputText)rptAppFee.Items[0].FindControl("txtBaseFee")).Value != "")
                {
                    BaseFee = Convert.ToDecimal(((HtmlInputText)rptAppFee.Items[0].FindControl("txtBaseFee")).Value);
                    if(BaseFee>0)
                    {
                        cb.UpdateBaseFee(BaseFee,businesCode);
                    }
                }
            }
            else
            {
                lblMsg.Text = "保存失敗！";
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>$('.tip').fadeIn(300);</script>");
            Security sec = new Security();
            string url = "AppDetail.aspx?BusinessCode=" + sec.Encode64(businesCode) + "&FlowStatus=Ng==";
            Response.Redirect(url);
        }
    }
}