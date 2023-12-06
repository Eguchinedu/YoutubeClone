using System.ComponentModel.DataAnnotations;

namespace YoutubeClone.Dtos
{
    public class PostForDisplayDto
    {
        [Key]
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int Likes { get; set; }
    }
}
