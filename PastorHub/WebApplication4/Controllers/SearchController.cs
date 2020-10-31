using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class SearchController : Controller
    {
        [HttpPost]
        public ActionResult Index(string SearchText)
        {
            List<Post> result = new List<Post>();

            ApplicationDbContext context = new ApplicationDbContext();
            result = context.Posts.Where(i => i.Title.Contains(SearchText) || i.Categories.Where(j => j.Name.Contains(SearchText)).Count() > 0 || i.Pastor.UserName.Contains(SearchText)).ToList();
            ViewBag.Text = SearchText;

            return View(result);
        }
    }
}