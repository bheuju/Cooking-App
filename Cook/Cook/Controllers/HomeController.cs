using Cook.DAL;
using Cook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Cook.Controllers
{
    public class HomeController : Controller
    {
        public static List<Recipe> recipeList = new List<Recipe>();

        //GET: home/index
        public ActionResult Index(string searchString)
        {
            SqlConnect recipeLoader = new SqlConnect();
            recipeLoader.retriveData("select * from Recipe");

            recipeList = RecipeAccess.getInstance().getRecipeList(recipeLoader.sqlTable);


            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchString.ToLower())
                {
                    case "all":
                        break;
                    case "food":
                        recipeList = recipeList.Where(s => s.type.Equals("food")).ToList();
                        break;
                    case "drinks":
                        recipeList = recipeList.Where(s => s.type.Equals("drinks")).ToList();
                        break;
                    default:
                        recipeList = recipeList.Where(s => s.name.ToLower().Contains(searchString.ToLower())).ToList<Recipe>();
                        break;
                }

            }

            return View(recipeList);
        }
    }
}