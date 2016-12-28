using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cook.Models
{
    public class User
    {
        public int id { get; set; }
        public string email { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        public string roles { get; set; }       //EG: ("admin,user, guest")
    }
}