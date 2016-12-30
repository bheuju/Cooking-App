using Cook.DAL;
using Cook.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cook.Controllers
{
    public class RecipeController : Controller
    {

        static int recipeId = 0;

        // GET: /Recipe/recipe/{id}
        public ActionResult Recipe(int id)
        {
            Recipe recipe = getRecipeFromHome(id);

            recipeId = id;

            return View(recipe);
        }

        //POST: /Recipe/recipe/{id}
        [HttpPost]
        public ActionResult Recipe()
        {
            Recipe recipe = getRecipeFromHome(recipeId);
            Response.Write("Favourited");

            //Favourited recipe by the user
            //Assign in favourite list

            int user_id = UserAccess.getInstance().pUser.id;

            SqlConnect favouriteSQL = new SqlConnect();
            favouriteSQL.cmdExecute("insert into favourites values('" + user_id + "','" + recipeId + "')");

            return View(recipe);
        }

        public Recipe getRecipeFromHome(int id)
        {
            return HomeController.recipeList[id - 1];
        }

    }
}