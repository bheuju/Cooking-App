using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cook.Models
{
    public class RecipeFactory
    {
        public enum RecipeType
        {
            FOOD = 0,
            DRINKS = 1
        }
        public Recipe Get(RecipeType recipeType)
        {
            switch (recipeType)
            {
                case RecipeType.FOOD:
                    return new Food();
                case RecipeType.DRINKS:
                    return new Drinks();
                default:
                    return null;
            }
        }
    }
}