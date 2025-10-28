using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BAL;
using Utility;
using DAL;

public partial class Admin_RemittanceFileUpload : System.Web.UI.Page
{
    private OleDbConnection Econ;
    private string constr;
    private string Query;
    private string Count;
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            if (!this.fupload.HasFile)
            {
                Response.Write("<script>alert('Please Select file for upload.');</script> ");
                return;
            }
            string excelPath = base.Server.MapPath(string.Concat("~/Admin/Files/", this.fupload.FileName));
            fupload.SaveAs(excelPath);
            base.Response.Write(string.Concat("path=", excelPath));
            string lower = Path.GetExtension(this.fupload.FileName).ToLower();
            string fileName = this.fupload.PostedFile.FileName;
            if (lower.Trim() == ".xls")
            {
                this.constr = string.Concat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=", excelPath, ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
                this.Econ = new OleDbConnection(this.constr);
            }
            if (lower.Trim() == ".xlsx")
            {
                this.constr = string.Concat("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=", excelPath, ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"");
                this.Econ = new OleDbConnection(this.constr);
            }
            constr = string.Format(constr, excelPath);
            using (OleDbConnection excel_con = new OleDbConnection(constr))
            {
                excel_con.Open();
                string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                DataTable dtExcelData = new DataTable();
                dtExcelData.Columns.AddRange(new DataColumn[9] {
                    new DataColumn("CRF ID", typeof(string)),
        new DataColumn("AWB", typeof(string)),
         new DataColumn("Delivered Date", typeof(string)),
          new DataColumn("Order Id", typeof(string)),
           new DataColumn("Courier", typeof(string)),
            new DataColumn("Order Value", typeof(string)),
            new DataColumn("Remittance Date", typeof(string)),
            new DataColumn("UTR", typeof(string)),
            new DataColumn("Channel Name", typeof(string))

                });

                using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                {
                    oda.Fill(dtExcelData);
                    System.Data.DataColumn newColumn = new System.Data.DataColumn("FileName", typeof(System.String));
                    newColumn.DefaultValue = fupload.FileName;
                    dtExcelData.Columns.Add(newColumn);
                }
                excel_con.Close();



                //Upload and save the file
                //        string csvPath = Server.MapPath("~/Admin/Files/") + Path.GetFileName(fupload.PostedFile.FileName);
                //    fupload.SaveAs(csvPath);

                //    //Create a DataTable.
                //    DataTable dt = new DataTable();
                //    dt.Columns.AddRange(new DataColumn[9] {
                //new DataColumn("CRF ID", typeof(string)),
                //new DataColumn("AWB", typeof(string)),
                // new DataColumn("Delivered Date", typeof(string)),
                //  new DataColumn("Order Id", typeof(string)),
                //   new DataColumn("Courier", typeof(string)),
                //    new DataColumn("Order Value", typeof(string)),
                //    new DataColumn("Remittance Date", typeof(string)),
                //    new DataColumn("UTR", typeof(string)),
                //    new DataColumn("Channel Name", typeof(string))



                //});

                //    //Read the contents of CSV file.
                //    string csvData = File.ReadAllText(csvPath);

                //    //Execute a loop over the rows.
                //    foreach (string row in csvData.Split('\n'))
                //    {
                //        if (!string.IsNullOrEmpty(row))
                //        {
                //            dt.Rows.Add();
                //            int i = 0;

                //            //Execute a loop over the columns.
                //            foreach (string cell in row.Split(','))
                //            {
                //                dt.Rows[dt.Rows.Count - 1][i] = cell;
                //                i++;
                //            }
                //        }
                //    }
                db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                int ir = 0;
                foreach (DataRow row in dtExcelData.Rows)
                {

                    string CodAmount = row["Order Value"].ToString();
                    if (CodAmount == "")
                    {
                        string OrderNumber = row["Order Id"].ToString();
                        long transid = Convert.ToInt64(OrderNumber);
                        PaymentTransaction data = context.PaymentTransactions.Where(x => x.PaymentTransactionId == transid).FirstOrDefault();
                        data.RemittenceAmountReceived = CodAmount;
                        data.RemittenceReferenceId = txtRemId.Text;
                        context.SaveChanges();
                    }
                    ir++;
                }
                //Bind the DataTable.
                Response.Write("<script>alert('File Uploaded Successfully');</script>");

            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.Message + "');</script>");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            if (!this.fupload.HasFile)
            {
                Response.Write("<script>alert('Please Select file for upload.');</script> ");
                return;
            }
            //Upload and save the file
            string csvPath = Server.MapPath("~/Admin/Files/") + Path.GetFileName(fupload.PostedFile.FileName);
            fupload.SaveAs(csvPath);

            //Create a DataTable.
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[10] {
        new DataColumn("Description", typeof(string)),
        new DataColumn("Payment Mode", typeof(string)),
         new DataColumn("Client", typeof(string)),
          new DataColumn("Pincode", typeof(string)),
           new DataColumn("Amount Payable", typeof(string)),
            new DataColumn("City", typeof(string)),
            new DataColumn("Status", typeof(string)),
            new DataColumn("COD Amount", typeof(string)),
            new DataColumn("Waybill Number", typeof(string)),
            new DataColumn("Order Number", typeof(string))


        });

            //Read the contents of CSV file.
            string csvData = File.ReadAllText(csvPath);

            //Execute a loop over the rows.
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    dt.Rows.Add();
                    int i = 0;

                    //Execute a loop over the columns.
                    foreach (string cell in row.Split(','))
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell;
                        i++;
                    }
                }
            }
            db_Zon_HuwEntities context = new db_Zon_HuwEntities();
            int ir = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (ir != 0)
                {
                    string CodAmount = row["COD Amount"].ToString();
                    string OrderNumber = row["Order Number"].ToString();
                    long transid = Convert.ToInt64(OrderNumber);
                    PaymentTransaction data = context.PaymentTransactions.Where(x => x.PaymentTransactionId == transid).FirstOrDefault();
                    data.RemittenceAmountReceived = CodAmount;
                    data.RemittenceReferenceId = txtRemId.Text;
                    context.SaveChanges();
                }
                ir++;
            }
            //Bind the DataTable.
            Response.Write("<script>alert('File Uploaded Successfully');</script>");

        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.Message + "');</script>");
        }
    }


}
