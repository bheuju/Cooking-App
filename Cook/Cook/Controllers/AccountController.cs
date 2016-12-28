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

            User user = new User() { email = userModel.email, password = userModel.password };

            //get user details

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(userModel.email, false);

                var authTicket = new FormsAuthenticationTicket(1, user.email, DateTime.Now, DateTime.Now.AddMinutes(20), false, "user");
                //var authTicket = new FormsAuthenticationTicket(1, user.email, DateTime.Now, DateTime.Now.AddMinutes(20), false, user.roles);
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
        [HttpPost]
        public ActionResult Signup()
        {
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
    }
}