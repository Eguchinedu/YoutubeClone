using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddLikeToPost(PostLike postLike)
        {
             await _context.PostLikes.AddAsync(postLike);

            return await SaveAsync();
        }

        public async Task<bool> CreatePostAsync(PostModel post)
        {
            await _context.PostModels.AddAsync(post);
            return await SaveAsync();
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var post = await _context.PostModels.FindAsync(postId);
            if (post == null)
            {
                return false; // Post not found
            }

            _context.PostModels.Remove(post);
            return await SaveAsync();
        }

        

        public async Task<PostModel> GetPostByIdAsync(int postId)
        {
            return await _context.PostModels.Where(p => p.PostId == postId).FirstOrDefaultAsync();
        }



        public async Task<ICollection<PostModel>> GetPostForAllUsersAsync()
        {
            return await _context.PostModels.ToListAsync();
        }

        public async Task<PostModel> GetPostForUserAsync(int userId, int postId)
        {
            return await _context.PostModels
                .Include(p => p.Comments).Include(p => p.PostLikes)
                .Where(p => p.UserId == userId && p.PostId == postId)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<PostModel>> GetPostsAsync(int userId)
        {
            return await _context.PostModels.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<bool> PostExistsAsync(int postId)
        {
            return await _context.PostModels.AnyAsync(p => p.PostId == postId);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePostAsync(PostModel post)
        {
            _context.PostModels.Update(post);
            return await SaveAsync();
        }

        public async Task<bool> UserLikedPost(int userId, int postId)
        {
            return await _context.PostLikes.AnyAsync(p => p.UserId == userId && p.PostId == postId);
        }
    }
}
