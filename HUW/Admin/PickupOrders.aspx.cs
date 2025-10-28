using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using Utility;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Web.Services;
using DAL;

public partial class Admin_PickupOrders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            db_Zon_HuwEntities context = new db_Zon_HuwEntities();
            lblAWBCOD.Text = context.Tbl_AWB.Where(x => x.OrderId == 0 && x.PaymentMode == 1 && x.IsActive == true).Count().ToString();
            lblAWBPPD.Text = context.Tbl_AWB.Where(x => x.OrderId == 0 && x.PaymentMode == 2 && x.IsActive == true).Count().ToString();

        }
    }
}