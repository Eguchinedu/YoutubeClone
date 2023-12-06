using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddCommentToPostAsync(CommentModel comment)
        {
            _context.CommentModels.Add(comment);

           return await SaveAsync();
           
        }

        public async Task<ICollection<CommentModel>> GetCommentForPostAsync(int postId)
        {
            
            return await _context.CommentModels.Where(p => p.PostId == postId).ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            // Use SaveChangesAsync for asynchronous saving
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
        public async Task<CommentModel> GetCommentByIdAsync(int commentId)
        {
            return await _context.CommentModels.FirstOrDefaultAsync(c => c.CommentId == commentId);
        }
    }
}
