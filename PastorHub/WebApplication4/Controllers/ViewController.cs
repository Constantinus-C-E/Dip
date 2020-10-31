using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication4.Controllers
{
    public class ViewController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: View
        public ActionResult Index(int Id)
        {
            return View(context.Posts.Where(i => i.Id == Id).FirstOrDefault());
        }
      
        [HttpPost]
        public ActionResult WriteComment(int Id, string Text)
        {
            string UserId = User.Identity.GetUserId();
            Comment comment = new Comment();
            comment.Post = context.Posts.Where(i => i.Id == Id).FirstOrDefault();
            comment.User = context.Users.Where(i => i.Id == UserId).FirstOrDefault();
            comment.Text = Text;
            comment.Date = DateTime.Now;
            context.Comments.Add(comment);
            context.SaveChanges();
            return PartialView(comment);
        }
    }
}