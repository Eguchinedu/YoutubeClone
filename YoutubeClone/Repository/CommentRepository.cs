using YoutubeClone.Data;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;

namespace YoutubeClone.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public bool AddCommentToPost(CommentModel comment)
        {
            _context.CommentModels.Add(comment);

            return Save();
        }

        public ICollection<CommentModel> GetCommentForPost(int postId)
        {
            return _context.CommentModels.Where(p => p.PostId == postId).ToList(); ;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
