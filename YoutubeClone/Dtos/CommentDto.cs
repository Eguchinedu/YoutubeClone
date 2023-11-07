using System.ComponentModel.DataAnnotations;

namespace YoutubeClone.Dtos
{
    public class CommentDto
    {
        [Key]
        public int CommentId { get; set; }

        public string Comment { get; set; }
    }
}
