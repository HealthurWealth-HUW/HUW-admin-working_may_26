using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using ClosedXML.Excel;
using Utility;

public partial class Admin_SearchOrders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


        }
    }

    protected void ExportExcel(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            string query = string.Empty;
            string frmDate = Request.Form["filter_date_start"].ToString();
            string toDate = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["filter_date_end"].ToString()))
            {
                toDate = Convert.ToString(Convert.ToDateTime(Request.Form["filter_date_end"].ToString()).AddDays(1));
            }
            if (!string.IsNullOrEmpty(frmDate) && !string.IsNullOrEmpty(toDate))
                query = "SELECT * FROM(select VchType = 'Sales @', pt.PaymentTransactionId as BillNo, CONVERT(varchar, pt.CreatedOn, 106) as Date, Code = '', Name = 'HUW', Add1 = '', Add2 = '', Add3 = '',Pincode = '', state = (select StateName from UserAddresses where UserAddressId = upt.ShippingAddressId) ,RegnType = '',GSTNo = '',PartyType = 'consumer',Ecommerce = '',SaleType = 'CEN',BasicPrice = (TxnAmount - ShippingCharges),Tax = '',IGST = '',SGST = '',CGST = '',ShippingCharges,Discount = '',RoundOff = '',TxnAmount as TotalAmount,InvoiceNo = '',InvoiceDate = '',pt.PaymentTransactionId as ref, Narr = 'Towards sales', ROW_NUMBER() OVER(PARTITION BY pt.PaymentTransactionId ORDER BY pt.PaymentTransactionId ASC) rn from PaymentTransactions pt left join UserProductTransactions upt on pt.PaymentTransactionId = upt.PaymentTransactionId where pt.PaymentTransactionId in (select PaymentTransactionId from PaymentTransactions where createdon >= '" + frmDate + "' and createdon <= '" + toDate + "')) a WHERE rn = 1 ";
            else
                query = "SELECT * FROM(select VchType = 'Sales @', pt.PaymentTransactionId as BillNo, CONVERT(varchar, pt.CreatedOn, 106) as Date, Code = '', Name = 'HUW', Add1 = '', Add2 = '', Add3 = '',Pincode = '', state = (select StateName from UserAddresses where UserAddressId = upt.ShippingAddressId) ,RegnType = '',GSTNo = '',PartyType = 'consumer',Ecommerce = '',SaleType = 'CEN',BasicPrice = (TxnAmount - ShippingCharges),Tax = '',IGST = '',SGST = '',CGST = '',ShippingCharges,Discount = '',RoundOff = '',TxnAmount as TotalAmount,InvoiceNo = '',InvoiceDate = '',pt.PaymentTransactionId as ref, Narr = 'Towards sales', ROW_NUMBER() OVER(PARTITION BY pt.PaymentTransactionId ORDER BY pt.PaymentTransactionId ASC) rn from PaymentTransactions pt left join UserProductTransactions upt on pt.PaymentTransactionId = upt.PaymentTransactionId where pt.PaymentTransactionId in (select PaymentTransactionId from PaymentTransactions where createdon >= '" + DateTime.Now.Date.ToString() + "' and createdon <= '" + DateTime.Now.AddDays(1).ToString() + "')) a WHERE rn = 1 ";
               
            using (SqlCommand cmd = new SqlCommand(query))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt, "PaymentTransactions");

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=HUW_Tally.xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }
                }
            }
        }
    }
}