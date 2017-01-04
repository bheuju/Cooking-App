﻿using Cook.DAL;
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
            sql.cmdExecute("insert into recipe values ('" + recipe.id + "','" + recipe.creatorId + "','" + recipe.name + "','" + recipe.type + "','" + recipe.img + "','" + recipe.description + "','" + recipe.process + "')");

            //Write ingedrients to [Ingedrients]
            foreach (string ingedrient in ingedrientsList)
            {
                sql.cmdExecute("insert into Ingedrients values('" + recipe.id + "','" + ingedrient + "')");
            }

            Response.Write("After post request: ");
            Response.Write(recipe.id);

            return View();
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

                //if (file.ContentType != "image/jpeg")
                //{
                //    ModelState.AddModelError("", "Not an image file");
                //    return View();
                //}

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




        /**************/
        /*** DELETE ***/
        /**************/

        [Authorize(Roles = "user,admin")]
        //GET: /recipe/delete
        public ActionResult Delete()
        {
            //Are you sure?
            return View();
        }

        [Authorize(Roles = "user,admin")]
        //POST: /recipe/delete/{id}
        [HttpPost]
        public ActionResult Delete(int recipeId)
        {
            SqlConnect deleteSql = new SqlConnect();
            //delete ingedrients, favourites and recipe
            deleteSql.cmdExecute("delete from Ingedrients where recipe_id='" + recipeId + "'");
            deleteSql.cmdExecute("delete from Favourites where recipe_id='" + recipeId + "'");
            deleteSql.cmdExecute("delete from Recipe where id='" + recipeId + "'");

            return RedirectToAction("Index", "Home");
        }



        /** EXTRA **/

        public Recipe getRecipeFromHome(int id)
        {
            return HomeController.recipeList[id - 1];
        }

    }
}