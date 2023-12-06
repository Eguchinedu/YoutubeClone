using YoutubeClone.Models;

namespace YoutubeClone.Interfaces
{
    public interface IPostRepository
    {
        Task<ICollection<PostModel>> GetPostsAsync(int userId);
        Task<PostModel> GetPostByIdAsync(int postId);
        Task<PostModel> GetPostForUserAsync(int userId, int postId);
        Task<ICollection<PostModel>> GetPostForAllUsersAsync();

        Task<bool> AddLikeToPost(PostLike postLike);

        Task<bool> UserLikedPost(int userId, int postId);
        Task<bool> PostExistsAsync(int postId);
        Task<bool> CreatePostAsync(PostModel post);
        Task<bool> DeletePostAsync(int postId);
        Task<bool> UpdatePostAsync(PostModel post);
        Task<bool> SaveAsync();
    }
}
