using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace BAL
{
    public static class DbLogger
    {
        public static void LogError(
            Exception ex,
            Product product = null,
            string methodName = null)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(
                        ConfigurationManager
                            .ConnectionStrings["db_Zon_constr"]
                            .ConnectionString))
                {
                    using (SqlCommand cmd =
                        new SqlCommand(
                            "SP_ErrorLogs_Insert",
                            con))
                    {
                        cmd.CommandType =
                            CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue(
                            "@ProductId",
                            (object)product?.ProductId ?? DBNull.Value);

                        cmd.Parameters.AddWithValue(
                            "@ProductName",
                            (object)product?.ProductName ?? DBNull.Value);

                        cmd.Parameters.AddWithValue(
                            "@ExceptionType",
                            ex.GetType().FullName);

                        cmd.Parameters.AddWithValue(
                            "@ErrorMessage",
                            ex.Message);

                        cmd.Parameters.AddWithValue(
                            "@StackTrace",
                            ex.StackTrace ?? "");

                        con.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
            }
        }

        internal static void LogError(Exception ex, string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}