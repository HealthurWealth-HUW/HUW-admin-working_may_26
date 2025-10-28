using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BAL;

using Utility;

public partial class Admin_RecordMaster : System.Web.UI.Page
    {
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString);
    decimal sum = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind_Document();
        }
        BindSum();
    }
    void bind_Document()
    {
        ddlDocumnetMonth.Items.Clear();
        ddlDocumnetMonth.DataTextField = "File_Date";
        ddlDocumnetMonth.DataValueField = "Id";
        var repository = new DocumnetMonthRepository();
        var data = repository.GetDocuemnts();
        ddlDocumnetMonth.DataSource = data;
        ddlDocumnetMonth.DataBind();

        ddlDocumnetMonth_SelectedIndexChanged(null, null);
        }
        void bindPin()
        {
        if (ddlDocumnetMonth.SelectedValue != "")
        {
            ddlpincode.Items.Clear();
            ddlpincode.DataTextField = "R_Pin";
            ddlpincode.DataValueField = "R_Pin";
            int DocumentId = Convert.ToInt32(ddlDocumnetMonth.SelectedValue.ToString());
            var repository = new RPinRepository();
            var userdata = repository.GetPin(DocumentId);
            ddlpincode.DataSource = userdata;
            ddlpincode.DataBind();
            ddlpincode_SelectedIndexChanged(null, null);
        }
        else
        {
            
        }
    }

        void bindData()
    {
        if(ddlpincode.SelectedValue!="")
        {

            int RPin = Convert.ToInt32(ddlpincode.SelectedValue.ToString());
            var repository = new DocumentDetailsRepository();
            var userdata = repository.GetDetails(RPin);
            rptDetailsTbl.DataSource = userdata;
            rptDetailsTbl.DataBind();
        }
        else
        {
            rptDetailsTbl.DataSource = null;
            rptDetailsTbl.DataBind();
        }
    }
    void BindSum()
    {
        if (ddlpincode.SelectedValue != "")
        {
            int PinCode = Convert.ToInt32(ddlpincode.SelectedValue.ToString());
            var repository = new SumRepository();
            var dt = ToDataTable.LINQResultToDataTable<Delivery_Invoice_Excel>(repository.GetSum(PinCode).AsEnumerable());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sum = sum + Convert.ToDecimal(dt.Rows[i]["Sum"]);
            }
            Lblsum.Text = Convert.ToString(sum);
        }
        else
        {
            Lblsum.Text = "0";
        }
    }


    protected void ddlpincode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindData();
        }

        protected void ddlDocumnetMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindPin();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Response.Redirect("FileUload.aspx");
        }




    }