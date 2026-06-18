using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Utility;

public partial class Admin_Excell2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uploader.Visible = false;
            pending_records_div.Visible = false;
            successmessage.Visible = false;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        uploader.Visible = true;
        pending_records_div.Visible = false;
        if (fileAWB.PostedFile != null)
        {
            //try
            //{
            string folderPath = Server.MapPath("~/UploadFiles");
            string path = folderPath + Guid.NewGuid() + Path.GetFileName(fileAWB.FileName);
            fileAWB.SaveAs(path);
            
            // Connection String to Excel Workbook  
            string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", path);
            OleDbConnection Econ = new OleDbConnection(constr);
            //string Query = string.Format("Select [AWB_Number],[PaymentMode],[OrderId] FROM [{0}]", "Sheet1$");
            string Query = string.Format("Select [Sr],[AWB Number],[Order Number],[Pickup Date],[Origin],[Shipper],[Consignee],[COD Due],[Remitted Amount],[Balance],[Dest Centre],[Status],[Del Date],[Payment Ref & Date],[Bank Name],[Bank Ref] FROM [{0}]", "Sheet1$");
            OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable Exceldt = ds.Tables[0];

            List<Remitting_Excel_Data> New_Data_Obj = new List<Remitting_Excel_Data>();
            List<Remitting_Excel_Data> Existing_Data_Obj = new List<Remitting_Excel_Data>();

            for (int i = 0; i <= Exceldt.Rows.Count - 2; i++)
            {
                db_Zon_HuwEntities context = new db_Zon_HuwEntities();


                Remitting_Excel_Data obj = new Remitting_Excel_Data();

                obj.AWB_number = Convert.ToInt64(Exceldt.Rows[i]["AWB Number"].ToString().Trim());
                obj.Order_Number = Convert.ToInt32(Exceldt.Rows[i]["Order Number"].ToString().Trim());
                obj.Pickup_Date = Convert.ToDateTime(Exceldt.Rows[i]["Pickup Date"].ToString().Trim());
                obj.Origin = Exceldt.Rows[i]["Origin"].ToString().Trim();
                obj.Shipper = Exceldt.Rows[i]["Shipper"].ToString().Trim();
                obj.Consignee = Exceldt.Rows[i]["Consignee"].ToString().Trim();
                obj.COD_Due = Convert.ToInt32(Exceldt.Rows[i]["COD Due"].ToString().Trim());
                obj.Remitted_Amount = Convert.ToInt32(Exceldt.Rows[i]["Remitted Amount"].ToString().Trim());
                obj.Balance = Convert.ToInt32(Exceldt.Rows[i]["Balance"].ToString().Trim());
                obj.Dest_Centre = Exceldt.Rows[i]["Dest Centre"].ToString().Trim();
                obj.Status = Exceldt.Rows[i]["Status"].ToString().Trim();
                obj.Del_Date = Convert.ToDateTime(Exceldt.Rows[i]["Del Date"].ToString().Trim());
                obj.Payment_Ref_and_Date = Exceldt.Rows[i]["Payment Ref & Date"].ToString().Trim();
                obj.Bank_Name = Exceldt.Rows[i]["Bank Name"].ToString().Trim();
                obj.Bank_Ref = Exceldt.Rows[i]["Bank Ref"].ToString().Trim();


                var paymentrec = context.PaymentTransactions.Where(x => x.PaymentTransactionId == obj.Order_Number).FirstOrDefault();

                if (paymentrec != null)
                {
                    var pgtxnid = obj.Payment_Ref_and_Date;

                    var substrng = pgtxnid.Substring(0, 16);


                    paymentrec.PGTxnId = substrng;
                    paymentrec.TxnRefNo = substrng;
                    paymentrec.TxnStatus = "SUCCESS(amount collected)";

                    paymentrec.UpdatedOn = DateTime.Now;

                    context.SaveChanges();
                }






                var y = context.Remitting_Excel_Data.Where(x => x.AWB_number == obj.AWB_number).Count();
                if (y != 0)
                {
                    Existing_Data_Obj.Add(obj);
                }
                else
                {
                    New_Data_Obj.Add(obj);
                    context.Remitting_Excel_Data.Add(obj);
                    context.SaveChanges();
                }
            }
            lblMessage.InnerText = "Uploaded Successfully";
            successmessage.Visible = true;
            Existing_Data.DataSource = Existing_Data_Obj;
            Existing_Data.DataBind();
            New_Data.DataSource = New_Data_Obj;
            New_Data.DataBind();
            uploader.Visible = true;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/xls";
        Response.AppendHeader("Content-Disposition", "attachment; filename=AWB_UploadExcel_Format.xls");
        Response.TransmitFile(Server.MapPath("~/UploadFiles/AWB_UploadExcel_Format.xls"));
        Response.End();
    }
    protected void pending_records_bind(object sender, EventArgs e)
    {
        uploader.Visible = false;
        pending_records_div.Visible = true;
        var junedate = Convert.ToDateTime("2019-06-01 00:00:00.000");
        ctl00_ctl00_Main_Main_legSelProductt.InnerText = "Pending Payment";

        var orderid_s = 0;
        var from_date_s = Convert.ToDateTime("2019-06-01 00:00:00.000");
        var to_date_s = Convert.ToDateTime("2099-06-01 00:00:00.000");
        var dropdown = DropDownList1.SelectedItem.Value;

        if (OrderID_txt.Text.Trim()!="")
        {
            orderid_s = Convert.ToInt32(OrderID_txt.Text.Trim());
        }
        if (From_Date_txt.Text.Trim() != "")
        {
            from_date_s = Convert.ToDateTime(From_Date_txt.Text);
        }
        if (To_Date_txt.Text.Trim() != "")
        {
            to_date_s = Convert.ToDateTime(To_Date_txt.Text);
        }


        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        List<PaymentTransaction> pending_records = new List<PaymentTransaction>();

        if (OrderID_txt.Text != "")
        {
            pending_records = context.PaymentTransactions.Where(x => x.PGTxnId == null && x.PaymentMode == "Cash On Delivery" && x.DeliveredDate >= junedate && x.PaymentTransactionId == orderid_s).ToList();
        }
        else if(From_Date_txt.Text.Trim() != "" || To_Date_txt.Text.Trim() != "")
        {
            pending_records = context.PaymentTransactions.Where(x => x.PGTxnId == null && x.PaymentMode == "Cash On Delivery" && x.DeliveredDate >= from_date_s && x.DeliveredDate<=to_date_s).ToList();
        }
        else if(dropdown == "Paid")
        {
            ctl00_ctl00_Main_Main_legSelProductt.InnerText = "Recieved Payment";
            if (OrderID_txt.Text != "")
            {
                pending_records = context.PaymentTransactions.Where(x => x.PGTxnId != null && x.PaymentMode == "Cash On Delivery" && x.DeliveredDate >= from_date_s && x.DeliveredDate <= to_date_s && x.PaymentTransactionId==orderid_s).ToList();
            }
            else
            {
                pending_records = context.PaymentTransactions.Where(x => x.PGTxnId != null && x.PaymentMode == "Cash On Delivery" && x.DeliveredDate >= from_date_s && x.DeliveredDate <= to_date_s).ToList();
            }
        }
        else
        {
            pending_records = context.PaymentTransactions.Where(x => x.PGTxnId == null && x.PaymentMode == "Cash On Delivery" && x.DeliveredDate >= from_date_s && x.DeliveredDate <= to_date_s).ToList();
        }

        pending_records_grid.DataSource = pending_records;
        pending_records_grid.DataBind();
    }
}