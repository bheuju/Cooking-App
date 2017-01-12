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


        /************/
        /*** READ ***/
        /************/

        // GET: /Recipe/recipe/{id}
        public ActionResult Recipe(int id)
        {
            //Recipe recipe = getRecipeFromHome(id);
            Recipe recipe = RecipeAccess.getInstance().getRecipe(id);

            recipeId = id;

            return View(recipe);
        }


        /**************/
        /*** CREATE ***/
        /**************/

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

                if (file.ContentType.ToLower() != "image/jpg" &&
                       file.ContentType.ToLower() != "image/jpeg" &&
                       file.ContentType.ToLower() != "image/pjpeg" &&
                       file.ContentType.ToLower() != "image/gif" &&
                       file.ContentType.ToLower() != "image/x-png" &&
                       file.ContentType.ToLower() != "image/png")
                {
                    ModelState.AddModelError("", "Not a valid image file");
                    return View();
                }

                //string dd = DateTime.Now();

                if (file != null && file.ContentLength > 0)
                {
                    var imgName = Path.GetFileName(file.FileName);
                    var fileName = generateUniqueID() + imgName;
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

            //Response.Write("Valid Ingedrients count: " + ingedrientsList.Count);

            //recipe.id = HomeController.recipeList.Count + 1;    // [ID]
            recipe.id = RecipeAccess.getInstance().getNewRecipeId();
            recipe.creatorId = UserAccess.getInstance().getUser().id;   // [CREATOR ID]

            /*************************************/
            /*************************************/
            //Write to database
            //Write recipe to [Recipe]

            //SqlConnect sql = new SqlConnect();
            //sql.cmdExecute("insert into recipe values ('" + recipe.id + "','" + recipe.creatorId + "','" + recipe.name + "','" + recipe.type + "','" + recipe.img + "','" + recipe.description + "','" + recipe.process + "')");

            //Write ingedrients to [Ingedrients]
            //foreach (string ingedrient in ingedrientsList)
            //{
            //    sql.cmdExecute("insert into Ingedrients values('" + recipe.id + "','" + ingedrient + "')");
            //}
            /*************************************/

            SqlConnect createSql = new SqlConnect();

            System.Diagnostics.Debug.Print("Type: " + recipe.type);

            List<KeyValuePair<string, object>> param = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("@id", recipe.id),
                new KeyValuePair<string, object>("@creatorId", recipe.creatorId),
                new KeyValuePair<string, object>("@name", recipe.name),
                new KeyValuePair<string, object>("@type", recipe.type),
                new KeyValuePair<string, object>("@img", recipe.img),
                new KeyValuePair<string, object>("@description", recipe.description),
                new KeyValuePair<string, object>("@process", recipe.process)
            };
            createSql.executeStoredProcedure("CreateRecipe", param);

            foreach (string ingedrient in ingedrientsList)
            {
                param.Clear();
                param.Add(new KeyValuePair<string, object>("@recipe_id", recipe.id));
                param.Add(new KeyValuePair<string, object>("@ingedrient", ingedrient));

                createSql.executeStoredProcedure("CreateRecipeIngedrients", param);
            }

            //return View();
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Recipe", "Recipe", new { id = recipe.id });
        }



        /**************/
        /*** UPDATE ***/
        /**************/

        [Authorize(Roles = "user,admin")]
        //GET: /Recipe/Edit/{id}
        public ActionResult Edit(int recipeId)
        {
            //int recipeId = 1;
            //System.Diagnostics.Debug.Print("Id: " + recipeId);

            Recipe recipe = RecipeAccess.getInstance().getRecipe(recipeId);

            return View(recipe);
        }

        [Authorize(Roles = "user,admin")]
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

                //if (file.ContentType.ToLower() != "image/jpg" &&
                //       file.ContentType.ToLower() != "image/jpeg" &&
                //       file.ContentType.ToLower() != "image/pjpeg" &&
                //       file.ContentType.ToLower() != "image/gif" &&
                //       file.ContentType.ToLower() != "image/x-png" &&
                //       file.ContentType.ToLower() != "image/png")
                //{
                //    ModelState.AddModelError("", "Not a valid image file");
                //    return View();
                //}

                if (file != null && file.ContentLength > 0)
                {
                    var imgName = Path.GetFileName(file.FileName);
                    var fileName = generateUniqueID() + imgName;
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    file.SaveAs(path);

                    recipe.img = fileName;  // [IMAGE]

                    //if image file loaded then change otherwise nothing
                    //updateSql.cmdExecute("update Recipe set img='" + recipe.img + "' where id='" + recipe.id + "'");
                    List<KeyValuePair<string, object>> param = new List<KeyValuePair<string, object>>()
                    {
                        new KeyValuePair<string, object>("@image", recipe.img),
                        new KeyValuePair<string, object>("@recipe_id", recipe.id)
                    };

                    updateSql.executeStoredProcedure("UpdateImage", param);

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


            ////Write to database
            ////Write recipe to [Recipe]
            //updateSql.cmdExecute("update Recipe set name='" + recipe.name + "', description='" + recipe.description + "', process='" + recipe.process + "' where id='" + recipe.id + "'");

            ////clear ingedrients table and repopulate ingedrients table
            //updateSql.cmdExecute("delete from Ingedrients where recipe_id='" + recipe.id + "'");

            ////Write ingedrients to [Ingedrients]
            //foreach (string ingedrient in ingedrientsList)
            //{
            //    updateSql.cmdExecute("insert into Ingedrients values('" + recipe.id + "','" + ingedrient + "')");
            //}


            //Update Recipe
            List<KeyValuePair<string, object>> param1 = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("@recipe_name", recipe.name),
                new KeyValuePair<string, object>("@recipe_description", recipe.description),
                new KeyValuePair<string, object>("@recipe_process", recipe.process),
                new KeyValuePair<string, object>("@recipe_id", recipe.id)
            };
            updateSql.executeStoredProcedure("UpdateRecipe", param1);

            //Delete Ingedrients
            param1.Clear();
            param1.Add(new KeyValuePair<string, object>("@recipe_id", recipe.id));
            updateSql.executeStoredProcedure("DeleteRecipeIngedrients", param1);

            //Add modified Ingedrients
            foreach (string ingedrient in ingedrientsList)
            {
                param1.Clear();
                param1.Add(new KeyValuePair<string, object>("@recipe_id", recipe.id));
                param1.Add(new KeyValuePair<string, object>("@ingedrient", ingedrient));

                updateSql.executeStoredProcedure("CreateRecipeIngedrients", param1);
            }

            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Recipe", "Recipe", new { id = recipe.id });
        }




        /**************/
        /*** DELETE ***/
        /**************/

        [Authorize(Roles = "user,admin")]
        //GET: /recipe/delete
        public ActionResult Delete()
        {
            //Are you sure?
            return View(RecipeAccess.getInstance().getRecipe(recipeId));
        }

        [Authorize(Roles = "user,admin")]
        //POST: /recipe/delete/{id}
        [HttpPost]
        public ActionResult Delete(int recipeId)
        {
            SqlConnect deleteSql = new SqlConnect();
            //delete ingedrients, favourites and recipe
            //deleteSql.cmdExecute("delete from Ingedrients where recipe_id='" + recipeId + "'");
            //deleteSql.cmdExecute("delete from Favourites where recipe_id='" + recipeId + "'");
            //deleteSql.cmdExecute("delete from Recipe where id='" + recipeId + "'");


            List<KeyValuePair<string, object>> param = new List<KeyValuePair<string, object>>();
            param.Add(new KeyValuePair<string, object>("@recipe_id", recipeId));

            deleteSql.executeStoredProcedure("DeleteRecipeIngedrients", param);
            deleteSql.executeStoredProcedure("DeleteFavouritesByRecipe", param);
            deleteSql.executeStoredProcedure("DeleteRecipe", param);

            return RedirectToAction("Index", "Home");
        }




        [HttpPost]
        public void AlterFav(string id)
        {
            int recipe_id = Convert.ToInt32(id);
            Recipe recipe = RecipeAccess.getInstance().getRecipe(recipe_id);

            int user_id = UserAccess.getInstance().getUser().id;

            //Toggle Favourites from database
            if (recipe.isFavourited)
            {
                //do unfavourite (remove)
                SqlConnect unfavouriteSQL = new SqlConnect();
                //unfavouriteSQL.cmdExecute("delete from favourites where user_id='" + user_id + "'and recipe_id='" + recipeId + "'", "Error removing from favourites");
                List<KeyValuePair<string, object>> param = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@user_id", user_id),
                    new KeyValuePair<string, object>("@recipe_id", recipeId)
                };

                unfavouriteSQL.executeStoredProcedure("DeleteFavourites", param);
            }
            else
            {
                //do favourite (add)
                SqlConnect favouriteSQL = new SqlConnect();
                //favouriteSQL.cmdExecute("insert into favourites values('" + user_id + "','" + recipeId + "')", "Error saving to favourites");
                List<KeyValuePair<string, object>> param = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@user_id", user_id),
                    new KeyValuePair<string, object>("@recipe_id", recipeId)
                };

                favouriteSQL.executeStoredProcedure("SetFavourites", param);
            }
        }

        /** EXTRA **/

        public Recipe getRecipeFromHome(int id)
        {
            return HomeController.recipeList[id - 1];
        }


        public string generateUniqueID()
        {
            return Guid.NewGuid().ToString("N");
        }

    }
}