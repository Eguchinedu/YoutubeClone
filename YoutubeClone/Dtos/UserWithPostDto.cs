using System.ComponentModel.DataAnnotations;
using YoutubeClone.Models;

namespace YoutubeClone.Dtos
{
    public class UserWithPostDto
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public List<PostForDisplayDto> Posts { get; set; } = new List<PostForDisplayDto>();


    }
}
