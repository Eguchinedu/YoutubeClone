using YoutubeClone.Models;

namespace YoutubeClone.Interfaces
{
    public interface ICommentRepository
    {
        bool AddCommentToPost( CommentModel comment);

        ICollection<CommentModel> GetCommentForPost( int postId );

        bool Save();
    }
}
