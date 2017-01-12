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

        public static UserAccess getInstance()
        {
            if (pInstance == null)
            {
                pInstance = new UserAccess();
            }
            return pInstance;
        }


        public User getUser()
        {
            if (pUser == null)
            {
                return guestUser;
            }
            return pUser;
        }


        public bool getUser(string username)
        {
            SqlConnect sql = new SqlConnect();

            sql.retriveData("select * from Users where username='" + username + "'");
            int count = sql.sqlTable.Rows.Count;

            if (count > 0)
            {
                //user exists
                return true;
            }
            return false;
        }


        public User getUser(string username, string password)
        {
            User user = null;

            SqlConnect sql = new SqlConnect();

            sql.retriveData("select * from Users where username='" + username + "' and password='" + password + "'");
            int count = sql.sqlTable.Rows.Count;

            if (count > 0)
            {
                //user exists
                user = new User()
                {
                    id = Convert.ToInt32(sql.sqlTable.Rows[0]["id"]),
                    username = sql.sqlTable.Rows[0]["username"].ToString(),
                    password = sql.sqlTable.Rows[0]["password"].ToString(),
                    roles = sql.sqlTable.Rows[0]["roles"].ToString()
                };

                pUser = user;

                return user;
            }

            return user;
        }
    }
}