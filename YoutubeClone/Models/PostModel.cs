using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YoutubeClone.Models
{
    public class PostModel
    {
        [Key]
        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string VideoUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public int UserId { get; set; }

        public UserModel User { get; set; }

        public int Likes { get; set; }

        public List<PostLike> PostLikes { get; set; } = new List<PostLike>();

        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
}
