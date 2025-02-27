using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using TextBlog.Models;

namespace TextBlog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }



 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("users");   // Принудительно задаем имя
            modelBuilder.Entity<Post>().ToTable("posts");
            modelBuilder.Entity<Comment>().ToTable("comments");

            modelBuilder.Entity<User>()
                .HasMany<Post>()
                .WithOne()
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<User>()
                .HasMany<Comment>()
                .WithOne()
                .HasForeignKey(c => c.AuthorId);

            modelBuilder.Entity<Post>()
                .HasMany<Comment>()
                .WithOne()
                .HasForeignKey(c => c.PostId);
        }
    }
}

