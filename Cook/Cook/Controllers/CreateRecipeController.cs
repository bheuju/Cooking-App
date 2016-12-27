using Cook.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cook.Controllers
{
    public class CreateRecipeController : Controller
    {
        //
        // GET: /home/create
        public ActionResult Create()
        {
            //ViewBag.id = HomeController.recipeList.Count;
            return View();
        }

        [HttpPost]
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

            SqlConnect sql = new SqlConnect();

            //Write to database
            //Write recipe to [Recipe]
            sql.cmdExecute("insert into recipe values ('" + recipe.id + "','" + recipe.name + "','" + recipe.img + "','" + recipe.description + "','" + recipe.process + "')");

            //Write ingedrients to [Ingedrients]
            foreach (string ingedrient in ingedrientsList)
            {
                sql.cmdExecute("insert into Ingedrients values('" + recipe.id + "','" + ingedrient + "')");
            }

            Response.Write("After post request: ");
            Response.Write(recipe.id);

            return View();
        }
    }
}