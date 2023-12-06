using YoutubeClone.Models;

namespace YoutubeClone.Interfaces
{
    public interface ICommentRepository
    {
        Task<bool> AddCommentToPostAsync(CommentModel comment);
        Task<ICollection<CommentModel>> GetCommentForPostAsync(int postId);
        Task<bool> SaveAsync();
        Task<CommentModel> GetCommentByIdAsync(int commentId);
    }
}
