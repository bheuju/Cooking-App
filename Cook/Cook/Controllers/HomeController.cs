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
        public ActionResult Index()
        {
            SqlConnect recipeLoader = new SqlConnect();
            recipeLoader.retriveData("select * from Recipe");

            int recipeCount = recipeLoader.sqlTable.Rows.Count;

            SqlConnect ingedrientsLoader = new SqlConnect();


            if (recipeCount > 0)
            {
                //To clear recipeList Each time browser refreshes
                //because static => keeps on adding
                recipeList.Clear();

                for (int i = 0; i < recipeCount; i++)
                {
                    //prepare recipe data
                    int id = Convert.ToInt32(recipeLoader.sqlTable.Rows[i]["id"].ToString());
                    string name = recipeLoader.sqlTable.Rows[i]["name"].ToString();
                    string img = recipeLoader.sqlTable.Rows[i]["img"].ToString();
                    string description = recipeLoader.sqlTable.Rows[i]["description"].ToString();
                    string process = recipeLoader.sqlTable.Rows[i]["process"].ToString();

                    ingedrientsLoader.retriveData("select ingedrients from Ingedrients where recipe_id = " + id);
                    List<string> ingedrientsList = new List<string>();
                    ingedrientsList.Clear();

                    int ingedrientsCount = ingedrientsLoader.sqlTable.Rows.Count;

                    if (ingedrientsCount > 0)
                    {
                        //prepare ingedrients data
                        for (int j = 0; j < ingedrientsCount; j++)
                        {
                            string ingedrient = ingedrientsLoader.sqlTable.Rows[j]["ingedrients"].ToString();
                            ingedrientsList.Add(ingedrient);
                        }
                        ingedrientsLoader.sqlTable.Clear();
                    }

                    var recipe = new Recipe()
                    {
                        id = id,
                        name = name,
                        img = img,
                        description = description,
                        process = process,
                        ingredients = ingedrientsList
                    };

                    recipeList.Add(recipe);

                }
            }

            return View(recipeList);
        }
    }
}