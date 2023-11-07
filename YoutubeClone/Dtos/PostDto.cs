using System.ComponentModel.DataAnnotations;

namespace YoutubeClone.Dtos
{
    public class PostDto
    {
        [Key]
        public int PostId { get; set; }

        public string VideoUrl { get; set; }

        public int Likes { get; set; }
    }
}
