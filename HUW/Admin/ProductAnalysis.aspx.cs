using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_ProductAnalysis : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Assuming 'chartData' contains the data (month names and product counts)
            Dictionary<string, int> chartData = GetDataFromSQL(); // Your method to retrieve SQL data

            // Bind chartData to the ASP.NET Chart control
            Chart1.Series["Series1"].Points.DataBindXY(chartData.Keys, chartData.Values);
lblname.Text=Request.QueryString["nm"];
        }
    }

    // Method to fetch data from SQL (sample method)
    private Dictionary<string, int> GetDataFromSQL()
    {
        Dictionary<string, int> data = new Dictionary<string, int>();
        int month = Convert.ToInt32(Request.QueryString["mn"]);
        int Productid = Convert.ToInt32(Request.QueryString["id"]);
        string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        // Replace with your SQL Server connection string
        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("ProductAnalysis", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Productid", Productid);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string monthName = reader["MonthName"].ToString();
                int productCount = Convert.ToInt32(reader["ProductsDelivered"]);
                // Add data to dictionary
                data.Add(monthName, productCount);
            }
            reader.Close();
        }

        return data;
    }
}