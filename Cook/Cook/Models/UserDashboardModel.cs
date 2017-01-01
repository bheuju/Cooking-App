using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cook.Models
{
    public class UserDashboardModel
    {
        public List<Recipe> myRecipeList = new List<Recipe>();
        public List<Recipe> favouritedRecipeList = new List<Recipe>();

        public UserDashboardModel(List<Recipe> myRecipeList, List<Recipe> favouritedRecipeList)
        {
            this.myRecipeList = myRecipeList;
            this.favouritedRecipeList = favouritedRecipeList;
        }
    }
}