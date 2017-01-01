using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Cook.Controllers
{
    public class SqlConnect
    {
        SqlConnection sqlCon = new SqlConnection();
        public DataTable sqlTable = new DataTable();

        public SqlConnect()
        {
            //System.Diagnostics.Debug.Print("Sql connect constructor");
            sqlCon.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        }

        /**
         * Retrive data from database
         * stores in "sqlTable"
         */
        public void retriveData(string cmd, string errorMessage = "Error Executing")
        {
            try
            {
                sqlCon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd, sqlCon);
                da.Fill(sqlTable);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                HttpContext.Current.Response.Write("<script>alert('" + errorMessage + "')</script>");
            }
            finally
            {
                sqlCon.Close();
            }
        }

        /**
         * For insert, update, delete execution
         */
        public void cmdExecute(string cmd, string errorMessage = "Error Executing")
        {
            try
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand(cmd, sqlCon);

                int rowInfected = sqlCmd.ExecuteNonQuery();

                if (rowInfected > 0)
                {
                    //Command execution successful
                    //HttpContext.Current.Response.Write("<script>alert('Added')</script>");
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                HttpContext.Current.Response.Write("<script>alert('" + errorMessage + "')</script>");
            }
            finally
            {
                sqlCon.Close();
            }
        }
    }
}