﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cook.Models
{
    public class Drinks : Recipe
    {
        public override void setType()
        {
            type = "drinks";
        }
    }
}