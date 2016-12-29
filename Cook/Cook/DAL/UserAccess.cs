using Cook.Controllers;
using Cook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cook.DAL
{
    public class UserAccess
    {
        static UserAccess pInstance;

        public static UserAccess getInstance()
        {
            if (pInstance == null)
            {
                pInstance = new UserAccess();
            }
            return pInstance;
        }

        public User getUserDetails(User user)
        {
            SqlConnect sql = new SqlConnect();

            sql.retriveData("select * from Users where email='" + user.email + "' and password='" + user.password + "'");
            int count = sql.sqlTable.Rows.Count;

            if (count > 0)
            {
                //user exists
                return new User()
                {
                    id = Convert.ToInt32(sql.sqlTable.Rows[0]["id"]),
                    email = sql.sqlTable.Rows[0]["email"].ToString(),
                    password = sql.sqlTable.Rows[0]["password"].ToString(),
                    roles = sql.sqlTable.Rows[0]["roles"].ToString()
                };
            }


            //user not found ... return null
            return null;
        }
    }
}