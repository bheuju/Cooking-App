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

            //Check user already exists
            if (!UserAccess.getInstance().checkUserExists(user))
            {
                //saveSql.cmdExecute("insert into users values ('" + user.username + "','" + user.password + "','" + "user" + "')");

                List<KeyValuePair<string, object>> param = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@username", user.username),
                    new KeyValuePair<string, object>("@password", user.password),
                    new KeyValuePair<string, object>("@roles", "user")
                };
                saveSql.executeStoredProcedure("SignUpUser", param);


                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Username already exists");
            }

            //return RedirectToAction("Index", "Home");
            return View();
        }

        //POST: /Account/Logout
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        public ActionResult Dashboard()
        {
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