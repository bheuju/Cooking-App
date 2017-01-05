using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cook.Models
{
    public class RecipeFactory
    {
        public Recipe Get(string recipeType)
        {
            switch (recipeType)
            {
                case "food":
                    return new Food();
                case "drinks":
                    return new Drinks();
                default:
                    return null;
            }
        }
    }
}