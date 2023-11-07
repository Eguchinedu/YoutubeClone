using System.ComponentModel.DataAnnotations;

namespace YoutubeClone.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public ICollection<PostModel> Posts { get; set; } = new List<PostModel>();
    }
}
