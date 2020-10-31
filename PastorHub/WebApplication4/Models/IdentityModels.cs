using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication4.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {

        public virtual List<ApplicationUser> Subscriptions { get; set; }
        public virtual List<ApplicationUser> Subscribers { get; set; }

        public string AvatarImage { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostFile> Files { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class Post
    {
        public int Id { get; set; }
        public virtual ApplicationUser Pastor { get; set; }
        public virtual List<Category> Categories { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string PreviewImage { get; set; }
        virtual public List<PostFile> Files { get; set; }
        virtual public List<Comment> Comments { get; set; }
        public DateTime Date { get; set; }

        public Post()
        {
            Files = new List<PostFile>();
            Categories = new List<Category>();
            Comments = new List<Comment>();
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        virtual public List<Post> Posts { get; set; }


        public Category()
        {
            Posts = new List<Post>();
        }
    }

    public class PostFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        virtual public Post Posts { get; set; }

    }

    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        virtual public Post Post { get; set; }

        virtual public ApplicationUser User { get; set; }

        public DateTime Date { get; set; }

        public Comment()
        {
            Post = new Post();
        }
    }
}