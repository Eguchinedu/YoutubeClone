using Microsoft.EntityFrameworkCore;
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
    }
}
