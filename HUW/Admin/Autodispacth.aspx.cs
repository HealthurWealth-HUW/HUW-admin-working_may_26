using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Autodispacth : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (fileAWB.PostedFile != null)
        {
            try
            {
                string folderPath = Server.MapPath("~/UploadFiles/AWB/");
                string path = folderPath + Guid.NewGuid() + Path.GetFileName(fileAWB.FileName);
                fileAWB.SaveAs(path);
               
                // Connection String to Excel Workbook  
                string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
                string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", path);
                OleDbConnection Econ = new OleDbConnection(constr);
                string Query = string.Format("Select ReferenceNo,Waybill FROM [{0}]", "Sheet1$");
                OleDbCommand Ecom = new OleDbCommand(Query, Econ);
                Econ.Open();

                DataSet ds = new DataSet();
                OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
                Econ.Close();
                oda.Fill(ds);
                DataTable Exceldt = ds.Tables[0];

                for (int i = Exceldt.Rows.Count - 1; i >= 0; i--)
                {
                    if (Exceldt.Rows[i]["ReferenceNo"] == DBNull.Value || Exceldt.Rows[i]["Waybill"] == DBNull.Value)
                    {
                        Exceldt.Rows[i].Delete();
                    }
                    else
                    {
                        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                        string awb = Exceldt.Rows[i]["Waybill"].ToString().Trim();
                        long orderid = Convert.ToInt64(Exceldt.Rows[i]["ReferenceNo"]);
                        PaymentTransaction pt = context.PaymentTransactions.Where(x => x.PaymentTransactionId == orderid).FirstOrDefault();
                        pt.ShipmentId = awb;
                        pt.PickupID = awb;
                        pt.Pickup = true;
                        pt.Dispatched = true;
                        pt.PickupDate = DateTime.Now;
                        pt.DispatchedDate = DateTime.Now;
                        pt.OrderCurrentStatus = (int)Shared.OrderStatus.Dispatched;
                        context.SaveChanges();
                        BalUtility.SendMailForProductStatus(
                        pt,
                        Shared.GethtmlPage(Shared.ProductStatusForSendMail.Dispatched),
                        Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Dispatched)
                        );
                        var Repsitory2 = new UserRepository();
                        var UsrDtls = Repsitory2.First(u => u.UserId == pt.UserId);
                        string Message = "";
                        var repository = new PaymentTransactionRepository();

                        string couriername = repository.CourierName(Convert.ToInt64(orderid));
                        //Message = "Your Product is Dispatched. You can track your shipment at https://www.delhivery.com/track/package/" + awb + " . Your Product sent through Delhivery. Your Tracking ID is " + awb + " .Healthurwealth.com (SONAL ENTERPRISES)";
                        if (couriername == "Delhivery")
                        {
                            Message = "Your " + pt.PaymentTransactionId + " is Dispatched. Tracking Url : https://www.delhivery.com/track/package/" + awb + " .Tracking ID " + awb + " .Healthurwealth.com (SONAL ENTERPRISES)";
                            string Url = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].ToString();
                            string UserName = System.Configuration.ConfigurationManager.AppSettings["SmsId"].ToString();
                            string password = System.Configuration.ConfigurationManager.AppSettings["SmsPwd"].ToString();
                            string Status = Utility.MailMessage.SendSms(Url, UserName, password, UsrDtls.MobileNo, Message, "N", "1707162122732475842");
                            //Message = "Your Product is Dispatched. You can track your shipment at https://www.delhivery.com/track/package/" + awb + " . Your Product sent through Delhivery. Your Tracking ID is " + awb + " .Healthurwealth.com (SONAL ENTERPRISES)";
                        }
                        else
                        {
                            Message = "Your " + pt.PaymentTransactionId + " is Dispatched. Tracking Url : https://ecomexpress.in/tracking/?awb_field=" + awb + " .Tracking ID " + awb + " .Healthurwealth.com (SONAL ENTERPRISES)";

                            string Url = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].ToString();
                            string UserName = System.Configuration.ConfigurationManager.AppSettings["SmsId"].ToString();
                            string password = System.Configuration.ConfigurationManager.AppSettings["SmsPwd"].ToString();
                            string Status = Utility.MailMessage.SendSms(Url, UserName, password, UsrDtls.MobileNo, Message, "N", "1707162122737017503");

                            //Message = "Your Product is ready to Dispatch. You can track your shipment at https://ecomexpress.in/tracking/?awb_field=" + awb + " . Your Product sent through " + couriername + ". Your Tracking ID is " + ShipmentID + " .";
                        }
                    //    string Url = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].ToString();
                    //    string UserName = System.Configuration.ConfigurationManager.AppSettings["SmsId"].ToString();
                    //    string password = System.Configuration.ConfigurationManager.AppSettings["SmsPwd"].ToString();
                    //    string Status = Utility.MailMessage.SendSms(Url, UserName, password, UsrDtls.MobileNo, Message, "N", "1707160960575460833");
                    }
                }
                Exceldt.AcceptChanges();

                lblMessage.InnerText = "Uploaded Successfully";
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message.ToString();
            }
        }
    }
}