using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_NeedHelpRequests : System.Web.UI.Page
{
    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
    string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];

    protected void Page_Load(object sender, EventArgs e)
    {
        List<Tbl_Need_Help> data = (from nh in Entity.Tbl_Need_Help
                                    where !string.IsNullOrEmpty(nh.Person_Contact)
                                    select nh).ToList();
        foreach (Tbl_Need_Help help in data)
        {
            help.Help_Date = help.Help_Date.Value.Date;
        }
        data = data.Distinct().OrderByDescending(x => x.Help_Id).ToList();
        gdvNeedHelp.DataSource = data;
        gdvNeedHelp.DataBind();
    }

    protected void btnNeedHelp_Click(object sender, EventArgs e)
    {
        string Name = txtName.Text;
        string Mobile = txtMobile.Text;
        int status = Convert.ToInt32(ddlNeedStatus.SelectedValue);
        long helpId = 0;
        if (txtHelpId.Text == "")
        {
            helpId = 0;
        }
        else
        {
            helpId = Convert.ToInt64(txtHelpId.Text);
        }

        List<Tbl_Need_Help> data = (from nh in Entity.Tbl_Need_Help
                                    where !string.IsNullOrEmpty(nh.Person_Contact)
                                    && (Name == "" ? true : (nh.Person_Name.Contains(Name)))
                                    && (Mobile == "" ? true : (nh.Person_Contact.Contains(Mobile)))
                                    && (status == 0 ? true : (nh.Status == status))
                                    && (helpId == 0 ? true : (nh.Help_Id == helpId))
                                    select nh).ToList();
        foreach (Tbl_Need_Help help in data)
        {
            help.Help_Date = help.Help_Date.Value.Date;
        }
        data = data.Distinct().OrderByDescending(x => x.Help_Id).ToList();
        gdvNeedHelp.DataSource = data;
        gdvNeedHelp.DataBind();
    }

    protected void OnPaging(object sender, GridViewPageEventArgs e) { 
        gdvNeedHelp.PageIndex = e.NewPageIndex;
        gdvNeedHelp.DataBind();
    }
   
}