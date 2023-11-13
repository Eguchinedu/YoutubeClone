using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoutubeClone.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentId { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public int PostId { get; set; }

        public PostModel Post { get; set; }


        
        public int UserId { get; set; }

        public UserModel User { get; set; }

    }
}
