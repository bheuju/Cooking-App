using Cook.DAL;
using Cook.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
            //Recipe recipe = getRecipeFromHome(id);
            Recipe recipe = RecipeAccess.getInstance().getRecipe(id);

            recipeId = id;

            return View(recipe);
        }

        //POST: /Recipe/recipe/{id}
        [HttpPost]
        public ActionResult Recipe()
        {
            //Loads recipe from home recipeList
            //Recipe recipe = getRecipeFromHome(recipeId);

            //loads recipe from database
            Recipe recipe = RecipeAccess.getInstance().getRecipe(recipeId);

            //Response.Write("Favourited");

            //Favourited recipe by the user
            //Assign in favourite list
            int user_id = UserAccess.getInstance().getUser().id;

            //Toggle Favourites from database
            if (recipe.isFavourited)
            {
                //do unfavourite (remove)
                SqlConnect unfavouriteSQL = new SqlConnect();
                unfavouriteSQL.cmdExecute("delete from favourites where user_id='" + user_id + "'and recipe_id='" + recipeId + "'", "Error removing from favourites");
            }
            else
            {
                //do favourite (add)
                SqlConnect favouriteSQL = new SqlConnect();
                favouriteSQL.cmdExecute("insert into favourites values('" + user_id + "','" + recipeId + "')", "Error saving to favourites");
            }

            return View(recipe);
        }

        [Authorize(Roles = "user,admin")]
        // GET: /recipe/create
        public ActionResult Create()
        {
            return View();
        }

        //POST: /recipe/create
        [HttpPost]
        [Authorize(Roles = "user,admin")]
        public ActionResult Create(Recipe recipe)
        {
            //upload image
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    file.SaveAs(path);

                    recipe.img = fileName;  // [IMAGE]
                }
            }

            List<string> ingedrientsList = new List<string>();
            //get list of ingedrients
            string ingedrients = Request["ingedrients"].ToString();

            //System.Diagnostics.Debug.Print(ingedrients);

            string[] str = ingedrients.Split(',');      //separating ingedrients

            foreach (string s in str)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    ingedrientsList.Add(s);     // [INGEDRIENTS]
                }
            }

            Response.Write("Valid Ingedrients count: " + ingedrientsList.Count);



            recipe.id = HomeController.recipeList.Count + 1;    // [ID]
            recipe.creatorId = UserAccess.getInstance().getUser().id;   // [CREATOR ID]

            SqlConnect sql = new SqlConnect();

            //Write to database
            //Write recipe to [Recipe]
            sql.cmdExecute("insert into recipe values ('" + recipe.id + "','" + recipe.creatorId + "','" + recipe.name + "','" + recipe.img + "','" + recipe.description + "','" + recipe.process + "')");

            //Write ingedrients to [Ingedrients]
            foreach (string ingedrient in ingedrientsList)
            {
                sql.cmdExecute("insert into Ingedrients values('" + recipe.id + "','" + ingedrient + "')");
            }

            Response.Write("After post request: ");
            Response.Write(recipe.id);

            return View();
        }

        //GET: /Recipe/Edit/{id}
        public ActionResult Edit(int recipeId)
        {
            //int recipeId = 1;
            //System.Diagnostics.Debug.Print("Id: " + recipeId);

            Recipe recipe = RecipeAccess.getInstance().getRecipe(recipeId);

            //SqlConnect recipeLoader = new SqlConnect();
            //SqlConnect ingedrientsLoader = new SqlConnect();

            //recipeLoader.retriveData("select * from Recipe where id='" + recipeId + "'");

            //int recipeCount = recipeLoader.sqlTable.Rows.Count;

            //if (recipeCount > 0)
            //{
            //    //prepare recipe data
            //    int id = Convert.ToInt32(recipeLoader.sqlTable.Rows[0]["id"].ToString());
            //    string name = recipeLoader.sqlTable.Rows[0]["name"].ToString();
            //    string img = recipeLoader.sqlTable.Rows[0]["img"].ToString();
            //    string description = recipeLoader.sqlTable.Rows[0]["description"].ToString();
            //    string process = recipeLoader.sqlTable.Rows[0]["process"].ToString();

            //    ingedrientsLoader.retriveData("select ingedrients from Ingedrients where recipe_id = " + id);
            //    List<string> ingedrientsList = new List<string>();
            //    ingedrientsList.Clear();

            //    int ingedrientsCount = ingedrientsLoader.sqlTable.Rows.Count;

            //    if (ingedrientsCount > 0)
            //    {
            //        //prepare ingedrients data
            //        for (int j = 0; j < ingedrientsCount; j++)
            //        {
            //            string ingedrient = ingedrientsLoader.sqlTable.Rows[j]["ingedrients"].ToString();
            //            ingedrientsList.Add(ingedrient);
            //        }
            //        ingedrientsLoader.sqlTable.Clear();
            //    }

            //    recipe = new Recipe()
            //    {
            //        id = id,
            //        name = name,
            //        img = img,
            //        description = description,
            //        process = process,
            //        ingredients = ingedrientsList
            //    };

            //}

            return View(recipe);
        }

        //POST: recipe/edit
        [HttpPost]
        public ActionResult Edit(Recipe recipe)
        {
            //update database
            //Response.Write("Image: " + recipe.img);
            //Response.Write("<script>alert('Image: " + recipe.img + "')</script>");

            SqlConnect updateSql = new SqlConnect();

            //upload image
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    file.SaveAs(path);

                    recipe.img = fileName;  // [IMAGE]

                    //if image file loaded then change otherwise nothing
                    updateSql.cmdExecute("update Recipe set img='" + recipe.img + "' where id='" + recipe.id + "'");
                }
            }

            List<string> ingedrientsList = new List<string>();
            //get list of ingedrients
            string ingedrients = Request["ingedrients"].ToString();

            //System.Diagnostics.Debug.Print(ingedrients);

            string[] str = ingedrients.Split(',');      //separating ingedrients

            foreach (string s in str)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    ingedrientsList.Add(s);     // [INGEDRIENTS]
                }
            }

            Response.Write("Valid Ingedrients count: " + ingedrientsList.Count);

            //TODO: need to get count from databse directly / or generate a random unique key
            //recipe.id = HomeController.recipeList.Count + 1;    // [ID]
            //System.Diagnostics.Debug.Print("Recipe id: " + recipe.id);
            recipe.creatorId = UserAccess.getInstance().getUser().id;   // [CREATOR ID]


            //Write to database
            //Write recipe to [Recipe]
            //sql.cmdExecute("insert into Recipe values ('" + recipe.id + "','" + recipe.creatorId + "','" + recipe.name + "','" + recipe.img + "','" + recipe.description + "','" + recipe.process + "')");
            updateSql.cmdExecute("update Recipe set name='" + recipe.name + "', description='" + recipe.description + "', process='" + recipe.process + "' where id='" + recipe.id + "'");

            //clear ingedrients table and repopulate ingedrients table
            updateSql.cmdExecute("delete from Ingedrients where recipe_id='" + recipe.id + "'");

            //Write ingedrients to [Ingedrients]
            foreach (string ingedrient in ingedrientsList)
            {
                updateSql.cmdExecute("insert into Ingedrients values('" + recipe.id + "','" + ingedrient + "')");
            }

            return RedirectToAction("Index", "Home");
        }


        public Recipe getRecipeFromHome(int id)
        {
            return HomeController.recipeList[id - 1];
        }

    }
}