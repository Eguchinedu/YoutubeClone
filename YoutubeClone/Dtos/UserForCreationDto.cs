﻿using System.ComponentModel.DataAnnotations;

namespace YoutubeClone.Dtos
{
    public class UserForCreationDto
    {
        

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage ="Please enter a valid email")]
        public string Email { get; set; }
    }
}
