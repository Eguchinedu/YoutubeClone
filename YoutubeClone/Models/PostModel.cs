using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoutubeClone.Models
{
    public class PostModel
    {
        [Key]
        public int PostId { get; set; }

        public string VideoUrl { get; set; }

        public DateTime? CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public UserModel User { get; set; }

        public int Likes { get; set; }

        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
}
