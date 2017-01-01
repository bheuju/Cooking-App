using Cook.DAL;
using Cook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Cook.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        //POST: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            User user = new User() { username = userModel.username, password = userModel.password };

            //get user details
            //check user credentials
            user = UserAccess.getInstance().getUserDetails(user);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(userModel.username, false);

                //var authTicket = new FormsAuthenticationTicket(1, user.email, DateTime.Now, DateTime.Now.AddMinutes(20), false, "user");
                var authTicket = new FormsAuthenticationTicket(1, user.username, DateTime.Now, DateTime.Now.AddMinutes(20), false, user.roles);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(userModel);
            }
        }

        //POST: /Account/Signup
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User user)
        {
            SqlConnect saveSql = new SqlConnect();
            saveSql.cmdExecute("insert into users values ('" + user.username + "','" + user.password + "','" + "user" + "')");

            return RedirectToAction("Index", "Home");
        }

        //POST: /Account/Logout
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }



        public ActionResult Dashboard()
        {
            //List and Display favourited recipes

            //List<Recipe> recipeList = new List<Recipe>();

            //favouritedRecipeLoader.retriveData("select * from Favourites where user_id = '" + UserAccess.getInstance().getUser().id + "'");


            /*

            //SqlConnect ingedrientsLoader = new SqlConnect();

            //SqlConnect favouritedRecipeLoader = new SqlConnect();
            //favouritedRecipeLoader.retriveData("select * from Favourites where user_id = '" + UserAccess.getInstance().getUser().id + "'");

            //int favRecipeCount = favouritedRecipeLoader.sqlTable.Rows.Count;

            //if (favRecipeCount > 0)
            //{
            //    for (int i = 0; i < favRecipeCount; i++)
            //    {

            //        SqlConnect recipeLoader = new SqlConnect();
            //        recipeLoader.retriveData("select * from Recipe where id = '" + favouritedRecipeLoader.sqlTable.Rows[i]["recipe_id"] + "'");

            //        int recipeCount = recipeLoader.sqlTable.Rows.Count;

            //        if (recipeCount < 0)
            //        {
            //            break;
            //        }

            //        //prepare recipe data
            //        int id = Convert.ToInt32(recipeLoader.sqlTable.Rows[0]["id"].ToString());
            //        string name = recipeLoader.sqlTable.Rows[0]["name"].ToString();
            //        string img = recipeLoader.sqlTable.Rows[0]["img"].ToString();
            //        string description = recipeLoader.sqlTable.Rows[0]["description"].ToString();
            //        string process = recipeLoader.sqlTable.Rows[0]["process"].ToString();

            //        ingedrientsLoader.retriveData("select ingedrients from Ingedrients where recipe_id = " + id);
            //        List<string> ingedrientsList = new List<string>();
            //        ingedrientsList.Clear();

            //        int ingedrientsCount = ingedrientsLoader.sqlTable.Rows.Count;

            //        if (ingedrientsCount > 0)
            //        {
            //            //prepare ingedrients data
            //            for (int j = 0; j < ingedrientsCount; j++)
            //            {
            //                string ingedrient = ingedrientsLoader.sqlTable.Rows[j]["ingedrients"].ToString();
            //                ingedrientsList.Add(ingedrient);
            //            }
            //            ingedrientsLoader.sqlTable.Clear();
            //        }

            //        var recipe = new Recipe()
            //        {
            //            id = id,
            //            name = name,
            //            img = img,
            //            description = description,
            //            process = process,
            //            ingredients = ingedrientsList
            //        };

            //        recipeList.Add(recipe);

            //    }
            //}

            //return View(recipeList);
             */

            List<Recipe> favouritedRecipeList = new List<Recipe>();
            List<Recipe> myRecipeList = new List<Recipe>();

            //load from home recipe list
            //List<Recipe> recipeList = HomeController.recipeList;

            //load from database recipe list
            SqlConnect recipeLoader = new SqlConnect();
            recipeLoader.retriveData("select * from Recipe");

            List<Recipe> recipeList = RecipeAccess.getInstance().getRecipeList(recipeLoader.sqlTable);

            foreach (Recipe recipe in recipeList)
            {
                if (recipe.isEditable)
                {
                    myRecipeList.Add(recipe);
                }
                if (recipe.isFavourited)
                {
                    favouritedRecipeList.Add(recipe);
                }
            }

            return View(new UserDashboardModel(myRecipeList, favouritedRecipeList));

        }

    }
}