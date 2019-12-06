using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace first.Login
{
    public static class SQL
    {

        public static DataSet query(string query)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConn"].ConnectionString))
            using (SqlCommand comm = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(comm))
            using (DataSet ds = new DataSet())
                return da.Fill(ds) > 0 ? ds : null;
        }

        public static void execute(string query)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConn"].ConnectionString))
            using (SqlCommand comm = new SqlCommand(query, conn))
                comm.ExecuteNonQuery();
        }

    }
}