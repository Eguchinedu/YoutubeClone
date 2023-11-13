using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using YoutubeClone.Models;

namespace YoutubeClone.Dtos
{
    public class CommentForCreationDto
    {


        [Required]
        public string Comment { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
