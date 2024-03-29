﻿using System.ComponentModel.DataAnnotations;
using YoutubeClone.Models;

namespace YoutubeClone.Dtos
{
    public class PostDto
    {
        [Key]
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int Likes { get; set; }

        public List<UserLikeDto> PostLikes { get; set; } = new List<UserLikeDto>();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}
