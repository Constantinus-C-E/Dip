using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ChannelController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        [HttpGet]
        [Authorize]
        public ActionResult Unsubscribe(string Id)
        {
            string Sid = User.Identity.GetUserId();

            ApplicationUser Subscriber = context.Users.Where(i => i.Id == Sid).FirstOrDefault();//user who subscribing
            ApplicationUser Subscription = context.Users.Where(i => i.Id == Id).FirstOrDefault();
            context.Users.Where(i => i.Id == Id).FirstOrDefault().Subscribers.Remove(Subscriber);
            context.Users.Where(i => i.Id == Sid).FirstOrDefault().Subscriptions.Remove(Subscription);
            context.SaveChangesAsync();
            return RedirectToAction("Index/" + Id);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Subscribe(string Id)
        {
            string Sid = User.Identity.GetUserId();
            ApplicationUser Subscriber = context.Users.Where(i => i.Id == Sid).FirstOrDefault();//user who subscribing
            ApplicationUser Subscription = context.Users.Where(i => i.Id == Id).FirstOrDefault();
            context.Users.Where(i => i.Id == Id).FirstOrDefault().Subscribers.Add(Subscriber);
            context.Users.Where(i => i.Id == Sid).FirstOrDefault().Subscriptions.Add(Subscription);
            context.SaveChangesAsync();
            return RedirectToAction("Index/" + Id); 
        }

        [HttpGet]
        [Authorize(Roles = "pastor")]
        public ActionResult PostList()
        {
            string Sid = User.Identity.GetUserId();
            return View(context.Posts.Where(i => i.Pastor.Id == Sid).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "pastor")]
        public ActionResult Create()
        {
            return View(context.Categories.ToList());
        }

        [HttpPost]
        [Authorize(Roles = "pastor")]
        public ActionResult Create(string Title, HttpPostedFileBase PreviewImage, IEnumerable<string> Categories, IEnumerable<HttpPostedFileBase> Files, string Text)
        {
            string PastorId = User.Identity.GetUserId(), fileName;
            Post post = new Post() { Title = Title, Text = Text, Pastor = context.Users.Where(i => i.Id == PastorId).FirstOrDefault(), Date = DateTime.Now, PreviewImage = "standartPreview.png" };
            List<PostFile> files = new List<PostFile>();

            if(Categories != null)
            foreach (string i in Categories)
                post.Categories.Add(context.Categories.Where(j => j.Name.Equals(i)).FirstOrDefault());

            if (PreviewImage != null)
            {
                fileName = System.IO.Path.GetFileName(PreviewImage.FileName);
                if (context.Posts.Count() == 0)
                {
                    fileName = fileName.Insert(0, "Preview_for_1_post_");
                }
                else
                {
                    fileName = fileName.Insert(0, "Preview_for_" + context.Posts.ToList().Last().Id.ToString() + "_post_");
                }

                PreviewImage.SaveAs(Server.MapPath("~/Files/PostFiles/" + fileName));

                post.PreviewImage = fileName;

            }


            if (Files != null)
            {
                foreach (var file in Files)
                {
                    if (file != null)
                    {
                        fileName = System.IO.Path.GetFileName(file.FileName);

                        if (context.Posts.Count() == 0)
                        {
                            fileName = fileName.Insert(0, "1");
                        }
                        else
                        {
                            fileName = fileName.Insert(0, context.Posts.ToList().Last().Id.ToString());
                        }

                        file.SaveAs(Server.MapPath("~/Files/PostFiles/" + fileName));

                        files.Add(new PostFile { FileName = fileName, Posts = post });
                    }
                }

                post.Files = files;
            }

            context.Posts.Add(post);
            context.SaveChangesAsync();

            return RedirectToAction("PostList");
        }

        public ActionResult Error()
        {
            return View();
        }

        [Authorize(Roles = "pastor")]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Post post = context.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [Authorize(Roles = "pastor")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Post post = context.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            context.Posts.Remove(post);
            context.SaveChangesAsync();

            return RedirectToAction("PostList");
        }

        [Authorize(Roles = "pastor")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(context.Posts.Where(i => i.Id == id).FirstOrDefault());
        }

        // GET: Channel
        [HttpGet]
        public ActionResult Index(string id)
        {
            try
            {
                if (User.IsInRole("User"))
                {
                    return RedirectToAction("Error");
                }

                return View(context.Users.Where(i => i.Id == id).FirstOrDefault());
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }
    }
}