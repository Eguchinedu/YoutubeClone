namespace YoutubeClone.Models
{
    public class PostLike
    {
        public int PostId { get; set; }
        public PostModel Post { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
