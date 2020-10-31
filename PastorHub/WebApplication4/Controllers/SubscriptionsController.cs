using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class SubscriptionsController : Controller
    {
        // GET: Subscriptions
        public ActionResult Index()
        {
            ApplicationUser user = new ApplicationUser();
            ApplicationDbContext context = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            user = context.Users.Where(i => i.Id == userId).FirstOrDefault();
            return View(user);
        }
    }
}