using YoutubeClone.Models;

namespace YoutubeClone.Interfaces
{
    public interface IPostRepository
    {
        ICollection<PostModel> GetPosts(int userId);
        PostModel GetPostById(int postId);

        PostModel GetPostForUser(int userId, int postId);

        bool PostExists(int postId);

        bool CreatePost(PostModel post);

        bool DeletePost(PostModel id);

        bool UpdatePost(PostModel post);

        bool Save();
    }
}
