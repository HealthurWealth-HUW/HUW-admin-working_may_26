using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Pincode_Configuration : System.Web.UI.Page
{
    db_Zon_HuwEntities se = new db_Zon_HuwEntities();
    PinCodesInformation pin = new PinCodesInformation();
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}