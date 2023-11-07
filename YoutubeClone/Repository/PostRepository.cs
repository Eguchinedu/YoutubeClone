using YoutubeClone.Data;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;

namespace YoutubeClone.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;

        }
        public bool CreatePost(PostModel post)
        {
            _context.PostModels.Add(post);

            return Save();
        }

        public bool DeletePost(PostModel id)
        {
            _context.PostModels.Remove(id);

            return Save();
        }

     

        public PostModel GetPostById(int postId)
        {
            return _context.PostModels.Where(p => p.PostId == postId).FirstOrDefault();
        }

        public PostModel GetPostForUser(int userId, int postId)
        {
            return _context.PostModels.Where(p => p.UserId == userId && p.PostId == postId).FirstOrDefault();
        }

        public ICollection<PostModel> GetPosts(int userId)
        {
            return _context.PostModels.Where(p => p.UserId == userId).ToList();
        }

        public bool PostExists(int id)
        {
           return _context.PostModels.Any(p => p.PostId == id);
        }

        

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePost(PostModel post)
        {
            _context.PostModels.Update(post);

            return Save();
        }
    }
}
