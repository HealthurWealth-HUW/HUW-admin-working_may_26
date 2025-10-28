using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using Utility;

public partial class Admin_DispatchedOrders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["RoleID"]) == 3)
        {
            liemplist.Visible = true;
        }
        else
        {
            liemplist.Visible = false;

        }
    }
}