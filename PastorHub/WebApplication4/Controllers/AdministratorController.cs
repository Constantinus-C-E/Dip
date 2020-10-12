using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class AdministratorController : Controller
    {

        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin
        [Authorize(Roles = "administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "administrator")]
        public ActionResult Categories()
        {
            return View(context.Categories.ToList());
        }

        [Authorize(Roles = "administrator")]
        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [Authorize(Roles = "administrator")]
        [HttpPost]
        public ActionResult CreateCategory(string Name)
        {
          
            Category category = new Category() { Name = Name };

            context.Categories.Add(category);
            context.SaveChangesAsync();

            return RedirectToAction("Categories");
        }

        [Authorize(Roles = "administrator")]
        [HttpGet]
        public ActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Category category = context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [Authorize(Roles = "administrator")]
        [HttpPost, ActionName("DeleteCategory")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Category category = context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            context.Categories.Remove(category);
            context.SaveChangesAsync();

            return RedirectToAction("Categories");
        }

        [Authorize(Roles = "administrator")]
        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Category category = context.Categories.Find(id);
            
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [Authorize(Roles = "administrator")]
        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            context.Entry(category).State = EntityState.Modified;
            context.SaveChangesAsync();
            return RedirectToAction("Categories");
        }
    }
}