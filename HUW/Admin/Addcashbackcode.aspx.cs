using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Addcashbackcode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ddlDiscountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDiscountType.SelectedValue == "0")
        {
            lblError.Text = "Please select discount type";
        }
        else if (ddlDiscountType.SelectedValue == "1")
        {
            lblError.Text = "";
            divAmount.Visible = true;
            divPercentage.Visible = false;
        }
        else if (ddlDiscountType.SelectedValue == "2")
        {
            lblError.Text = "";
            divAmount.Visible = false;
            divPercentage.Visible = true;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        using(db_Zon_HuwEntities context = new db_Zon_HuwEntities()) {
            if (ddlDiscountType.SelectedValue == "0")
            {
                lblError.Text = "Please select discount type";
            }
            else
            {
                if (ddlDiscountType.SelectedValue == "1")
                {
                    txtPercentage.Text = "0";
                }
                else if (ddlDiscountType.SelectedValue == "2")
                {
                    txtAmount.Text = "0";
                }
            }
            CashbackRepository repository = new CashbackRepository();
            Cashback_Coupons cashbacknames = repository.First(u => u.Coup_Code == txtCouponCode.Text);
            if (cashbacknames == null) { 
            int max = Convert.ToInt16(context.Cashback_Coupons.Max(p => p.Defaultid));
            Cashback_Coupons cback = new Cashback_Coupons();
            cback.Coup_Code = txtCouponCode.Text;
            cback.Min_Prod_Val = Convert.ToInt16(txtCartAMount.Text);
            string iDate = txtValidfrom.Text;
            DateTime oDate = DateTime.Parse(iDate);
            cback.From_Date = oDate;
            string pdate = txtValidTo.Text;
            DateTime atime = DateTime.Parse(pdate);
            cback.To_Date = atime;
            cback.Coup_Amount = Convert.ToInt64(txtAmount.Text);
            cback.Coup_Percentage= Convert.ToInt64(txtPercentage.Text);
            cback.Total_Times_Usable = Convert.ToInt16(txtUsage.Text);
            cback.Total_Times_Usable_Per_User = Convert.ToInt16(txtUserUsage.Text);
            cback.Min_Qty = Convert.ToInt16(TextBox1.Text);
            cback.Max_Cashback = Convert.ToInt64(txtmaxcashback.Text);
            cback.Cashbackdesc = txtDescription.InnerText;
            cback.TC = txttc.InnerText;
            cback.New_User = chknuser.Checked;
            cback.Is_Active = true;
            cback.Created_Date = DateTime.Now;
            cback.Defaultid = Convert.ToInt16(max) + 1;
            context.Cashback_Coupons.Add(cback);
            context.SaveChanges();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Cashback coupon sucessfully added');window.location ='Home.aspx';", true);
            }
            else
            {
                hiddenlbl.Visible = true;
            }
        }
        

    }
}