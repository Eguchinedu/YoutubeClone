using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoutubeClone.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentId { get; set; }

        public string Comment { get; set; }

        [ForeignKey("PostId")]
        public int PostId { get; set; }

        public PostModel Post { get; set; }


    }
}
