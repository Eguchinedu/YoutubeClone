using System.ComponentModel.DataAnnotations;

namespace YoutubeClone.Dtos
{
    public class UserDto
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }


    }
}
