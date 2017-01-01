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
        private User pUser = null;
        private User guestUser = new User
        {
            id = 0,
            username = "guest",
            password = "guest",
            roles = "guest"
        };

        static UserAccess pInstance = null;
        UserAccess() { }

        public User getUser()
        {
            if (pUser == null)
            {
                return guestUser;
            }
            return pUser;
        }

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

            sql.retriveData("select * from Users where username='" + user.username + "' and password='" + user.password + "'");
            int count = sql.sqlTable.Rows.Count;

            if (count > 0)
            {
                //user exists
                pUser = new User()
                {
                    id = Convert.ToInt32(sql.sqlTable.Rows[0]["id"]),
                    username = sql.sqlTable.Rows[0]["username"].ToString(),
                    password = sql.sqlTable.Rows[0]["password"].ToString(),
                    roles = sql.sqlTable.Rows[0]["roles"].ToString()
                };
                return pUser;
            }

            //user not found ... return null
            return null;
        }
    }
}