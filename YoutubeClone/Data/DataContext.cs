using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using YoutubeClone.Models;

namespace YoutubeClone.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }

       

        public DbSet<UserModel> UserModels { get; set; }

        public DbSet<PostModel> PostModels { get; set; }

        public DbSet<CommentModel> CommentModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User-Post relationship
            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //  User-Comment relationship
            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post-Comment relationship
            modelBuilder.Entity<PostModel>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
