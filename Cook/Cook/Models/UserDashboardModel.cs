using Cook.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cook.Models
{
    public class UserDashboardModel
    {
        public User user = new User();

        public List<Recipe> myRecipeList = new List<Recipe>();
        public List<Recipe> favouritedRecipeList = new List<Recipe>();

        public UserDashboardModel(List<Recipe> myRecipeList, List<Recipe> favouritedRecipeList)
        {
            this.user = UserAccess.getInstance().getUser();
            this.myRecipeList = myRecipeList;
            this.favouritedRecipeList = favouritedRecipeList;
        }
    }
}