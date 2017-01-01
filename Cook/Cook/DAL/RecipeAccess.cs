using Cook.Controllers;
using Cook.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Cook.DAL
{
    public class RecipeAccess
    {
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

        public Recipe getRecipe(int recipeId)
        {
            SqlConnect recipeLoader = new SqlConnect();
            recipeLoader.retriveData("select * from Recipe where id='" + recipeId + "'");

            Recipe recipe = getRecipeList(recipeLoader.sqlTable)[0];

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
                        creatorId = creatorId,
                        name = name,
                        img = img,
                        description = description,
                        process = process,
                        ingredients = ingedrientsList
                    };

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