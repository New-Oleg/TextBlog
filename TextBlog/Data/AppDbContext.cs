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
        public DbSet<Coment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany<Post>()
                .WithOne()
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<User>()
                .HasMany<Coment>()
                .WithOne()
                .HasForeignKey(c => c.AuthorId);

            modelBuilder.Entity<Post>()
                .HasMany<Coment>()
                .WithOne()
                .HasForeignKey(c => c.PostId);
        }
    }
}

