using Kashmira_Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kashmira_Restaurant.Controllers
{
    public class FoodController : Controller
    {
        DBFoodEntities context = new DBFoodEntities();
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(TBLUser dr)
        {
            TBLUser user = context.TBLUsers.Where(a => a.UserName == dr.UserName && a.Password == dr.Password).FirstOrDefault();
            if (user != null)
            {
                Session["user"] = user.UserName;
                return RedirectToAction("Food_Items");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(TBLUser ur)
        {
            context.TBLUsers.Add(ur);
            context.SaveChanges();
            return RedirectToAction("Login","Food");
        }

        public ActionResult Food_Items()
        {
            if (Session["user"] != null)
            {
                List<TBLFood> dishes = context.TBLFoods.ToList();
                return View(dishes);
            }
            else
            {
                return View("Login");
            }
        }
        [Route("dish/new")]
        public ActionResult Insert_Food_Items()
        {

            if (Session["user"] != null)
            {
                return View();

            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult Insert(TBLFood dr)
        {
            if (dr.Cuisine == "Mexican" && dr.Price >300)
            {
                dr.Discount = 40;
            }
            else if(dr.Cuisine == "Mexican" || dr.Cuisine == "Italian")
            {
                dr.Discount = 20;
            }
            else if(dr.Cuisine == "Nonveg_Mexican")
            {
                return View("Error");
            }
            else
            {
                dr.Discount = 0;
            }

            if (Session["user"] != null)
            {
                context.TBLFoods.Add(dr);
                context.SaveChanges();

                return RedirectToAction("Food_Items","Food");
            }
            else
            {
                return View("Login");
            }

        }
        public ActionResult Update_Food_Items(int id)
        {
            if (Session["user"] != null)
            {
                TBLFood dr= context.TBLFoods.Where(a => a.ID_PK == id).FirstOrDefault();
                return View(dr);
            }
            else
            {
                return View("Login");
            }

        }
        public ActionResult Update(TBLFood newData)
        {



            if (Session["user"] != null)
            {
                TBLFood olddata = context.TBLFoods.Find(newData.ID_PK);
                olddata.DishName = newData.DishName;
                olddata.Cuisine = newData.Cuisine;
                olddata.Price = newData.Price;

                if (newData.Cuisine == "Mexican" && newData.Price > 300)
                {
                    newData.Discount = 40;
                }
                else if (newData.Cuisine == "Mexican" || newData.Cuisine == "Italian")
                {
                    newData.Discount = 20;
                }
                else if (newData.Cuisine == "Nonveg_Mexican")
                {
                    return View("Error");
                }
                else
                {
                    newData.Discount = 0;
                }
                olddata.Discount = newData.Discount;
                context.SaveChanges();

                return RedirectToAction("Food_Items","Food");
            }
            else
            {
                return View("Login");
            }
        }
        public ActionResult Delete(int id)
        {


            if (Session["user"] != null)
            {
                TBLFood e = context.TBLFoods.Find(id);
                context.TBLFoods.Remove(e);
                context.SaveChanges();

                return RedirectToAction("Food_Items", "Food");
            }
            else
            {
                return View("Login");
            }
        }
    }
}