﻿using System.ComponentModel.DataAnnotations;

namespace YoutubeClone.Dtos
{
    public class PostForCreationDto
    {
        [Required]
        public string VideoUrl { get; set; }

        [Required]
        public int UserId { get; set; }

        public int Likes { get; set; }
    }
}
