using System.ComponentModel.DataAnnotations;

namespace YoutubeClone.Dtos
{
    public class CommentDto
    {
        [Key]
        public int CommentId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
