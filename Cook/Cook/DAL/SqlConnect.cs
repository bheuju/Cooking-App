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
        public SqlConnection sqlCon = new SqlConnection();
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


        public void executeStoredProcedure(string procedure, List<KeyValuePair<string, object>> param)
        {
            //procedure name
            //string sqlCmd = "GetRecipe";
            SqlCommand cmd = new SqlCommand(procedure, sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, object> p in param)
            {
                //System.Diagnostics.Debug.Print("" + p.Key + p.Value);
                cmd.Parameters.AddWithValue(p.Key, p.Value);
            }


            try
            {
                sqlCon.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("<script>alert('" + "Error Executing" + "')</script>");
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