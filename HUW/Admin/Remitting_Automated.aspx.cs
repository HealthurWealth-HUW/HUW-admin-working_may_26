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

public partial class Admin_Remitting_Automated : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnUpload_Click(object sender, EventArgs e)

    {

        if (FileUpload1.HasFile)

        {

            string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

            string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);

            //string FolderPath = ConfigurationManager.AppSettings["FolderPath"];



            string FilePath = Server.MapPath("../ExcellSheets/Path" + FileName);

            FileUpload1.SaveAs(FilePath);

            Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);

        }

    }
    private void Import_To_Grid(string FilePath, string Extension, string isHDR)

    {


        //public static string path = @"C:\src\RedirectApplication\RedirectApplication\301s.xlsx";
        string conStr = "";

        switch (Extension)

        {

            case ".xls": //Excel 97-03

                conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"]

                         .ConnectionString;

                break;

            case ".xlsx": //Excel 07

                conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"]

                          .ConnectionString;

                break;

        }


        //conStr= "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=Excel 12.0;";
        conStr = String.Format(conStr, FilePath, isHDR);

        OleDbConnection connExcel = new OleDbConnection(conStr);

        OleDbCommand cmdExcel = new OleDbCommand();

        OleDbDataAdapter oda = new OleDbDataAdapter();

        DataTable dt = new DataTable();

        cmdExcel.Connection = connExcel;



        //Get the name of First Sheet

        connExcel.Open();

        DataTable dtExcelSchema;

        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

        //connExcel.Close();



        //Read Data from First Sheet

        connExcel.Open();

        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

        oda.SelectCommand = cmdExcel;

        oda.Fill(dt);

        connExcel.Close();



        //Bind Data to GridView

        GridView1.Caption = Path.GetFileName(FilePath);

        GridView1.DataSource = dt;

        GridView1.DataBind();

    }
    protected void PageIndexChanging(object sender, GridViewPageEventArgs e)

    {

        string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

        string FileName = GridView1.Caption;

        string Extension = Path.GetExtension(FileName);

        string FilePath = Server.MapPath(FolderPath + FileName);



        Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);

        GridView1.PageIndex = e.NewPageIndex;

        GridView1.DataBind();

    }
}