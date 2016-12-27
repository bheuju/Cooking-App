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
        List<Recipe> recipeList = new List<Recipe>();

        public RecipeController()
        {

        }

        //
        // GET: /Recipe/recipe/{id}
        public ActionResult Recipe(int id)
        {
            Recipe recipe = getRecipeFromHome(id);

            return View(recipe);
        }

        public Recipe getRecipeFromHome(int id)
        {
            return HomeController.recipeList[id - 1];
        }

    }
}