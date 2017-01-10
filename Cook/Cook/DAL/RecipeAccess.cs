using Cook.Controllers;
using Cook.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cook.DAL
{
    public class RecipeAccess
    {
        /** Singleton pattern **/
        private RecipeAccess() { }

        private static RecipeAccess pInstance = null;

        public static RecipeAccess getInstance()
        {
            if (pInstance == null)
            {
                pInstance = new RecipeAccess();
            }
            return pInstance;
        }


        /***
         * 
         **/

        public int getNewRecipeId()
        {
            int id;

            SqlConnect sql = new SqlConnect();
            //sql.executeStoredPrcedure("GetRecipe");

            sql.retriveData("select top 1 id from Recipe order by id desc");
            id = Convert.ToInt32(sql.sqlTable.Rows[0]["id"]);


            System.Diagnostics.Debug.Print("Next ID: " + (id + 1));

            return (id + 1);
        }

        public Recipe getRecipe(int recipeId)
        {
            SqlConnect recipeLoader = new SqlConnect();
            recipeLoader.retriveData("select * from Recipe where id='" + recipeId + "'");

            Recipe recipe = getRecipeList(recipeLoader.sqlTable)[0];
            //Recipe recipe = new Drinks();

            return recipe;
        }

        public List<Recipe> getRecipeList(DataTable recipeTable)
        {
            List<Recipe> recipeList = new List<Recipe>();
            recipeList.Clear();

            SqlConnect ingedrientsLoader = new SqlConnect();

            int recipeCount = recipeTable.Rows.Count;

            if (recipeCount > 0)
            {
                for (int i = 0; i < recipeCount; i++)
                {
                    //prepare recipe data

                    //fetch recipe data from table
                    int id = Convert.ToInt32(recipeTable.Rows[i]["id"].ToString());
                    int creatorId = Convert.ToInt32(recipeTable.Rows[i]["creator_id"].ToString());
                    string name = recipeTable.Rows[i]["name"].ToString();
                    string img = recipeTable.Rows[i]["img"].ToString();
                    string description = recipeTable.Rows[i]["description"].ToString();
                    string process = recipeTable.Rows[i]["process"].ToString();
                    string type = recipeTable.Rows[i]["type"].ToString();

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


                    //Get Recipe object from RecipeFactory
                    RecipeFactory factory = new RecipeFactory();
                    Recipe recipe = factory.Get(type.ToLower());

                    recipe.id = id;
                    recipe.creatorId = creatorId;
                    recipe.name = name;
                    recipe.img = img;
                    recipe.description = description;
                    recipe.process = process;
                    recipe.ingredients = ingedrientsList;


                    //check and assign for favourites and editable recipes (if user is logged in)
                    if (HttpContext.Current.Request.IsAuthenticated)
                    {
                        int userId = UserAccess.getInstance().getUser().id;

                        //check favouite
                        SqlConnect checkFavourite = new SqlConnect();
                        checkFavourite.retriveData("select * from favourites where user_id='" + userId + "' and recipe_id='" + recipe.id + "'");
                        if (checkFavourite.sqlTable.Rows.Count > 0)
                        {
                            //This is the Favourited recipe by the user
                            //set favourited = true

                            recipe.isFavourited = true;
                        }

                        //check editable

                        if (userId == recipe.creatorId)
                        {
                            recipe.isEditable = true;
                        }

                    }

                    recipeList.Add(recipe);
                }
            }

            return recipeList;
        }

    }
}