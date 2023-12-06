using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutubeClone.Dtos;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;

namespace YoutubeClone.Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    [Authorize]
   
    public class PostController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public PostController(IUserRepository userRepository, IMapper mapper, IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        }

        /*

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostForDisplayDto>))]
        [ProducesResponseType(404, Type = typeof(ResponseDto))] 
        public async Task<IActionResult> GetPostForAllUsers()
        {
            var getAllPostForUsers = await _postRepository.GetPostForAllUsersAsync();

            if (!getAllPostForUsers.Any())
            {
                return NotFound(new ResponseDto
                {
                    errorReason = "No posts found",
                    success = false,
                    token = ""
                });
            }

            var allUserPost = _mapper.Map<List<PostForDisplayDto>>(getAllPostForUsers);

            return Ok(allUserPost);
        }

        */
     

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostForDisplayDto>))]
        [ProducesResponseType(404, Type = typeof(ResponseDto))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPosts(int userId)
        {
            

            var userFound = await _userRepository.UserExists(userId);
            if (!userFound)
            {
                return NotFound(new ResponseDto
                {
                    errorReason = "User does not exist",
                    success = false,
                    token = ""
                });
            }

            var userPosts = _mapper.Map<List<PostForDisplayDto>>(await _postRepository.GetPostsAsync(userId));

            if (userPosts == null)
            {
                return NotFound(new ResponseDto
                {
                    errorReason = "This user has no posts",
                    success = false,
                    token = ""
                });
            }

            return Ok(userPosts);
        }


        [HttpGet("{userId}/{postId}", Name = "GetPostById")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostDto>))]
        [ProducesResponseType(404, Type = typeof(ResponseDto))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPost(int userId, int postId)
        {
            var userFound = await _userRepository.UserExists(userId);
            if (!userFound)
            {
                return NotFound(new ResponseDto
                {
                    errorReason = "User does not exist",
                    success = false,
                    token = ""
                });
            }

            var postEntity = await _postRepository.GetPostForUserAsync(userId, postId);
            if (postEntity == null)
            {
                return NotFound(new ResponseDto
                {
                    errorReason = "Post does not exist",
                    success = false,
                    token = ""
                });
            }

            var userEntity = await _userRepository.GetUser(postEntity.UserId);

            // Map Post and Comments to DTO
            var postDto = _mapper.Map<PostDto>(postEntity);
            postDto.User = _mapper.Map<UserDto>(userEntity);
            postDto.Comments = _mapper.Map<List<CommentDto>>(postEntity.Comments);
            postDto.PostLikes = _mapper.Map<List<UserLikeDto>>(postEntity.PostLikes);
            foreach (var commentDto in postDto.Comments)
            {
                var userComment = await _userRepository.GetUser(commentDto.UserId);
                if (userEntity != null)
                {
                    commentDto.User = _mapper.Map<UserDto>(userComment);
                }
            }

            return Ok(postDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PostModel))]
        [ProducesResponseType(400, Type = typeof(ResponseDto))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreatePost([FromBody] PostForCreationDto postForCreation)
        {
            if (postForCreation == null)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Invalid data received",
                    success = false,
                    token = ""
                });
            }

            var user = await _userRepository.GetUser(postForCreation.UserId);

            if (user == null)
            {
                return NotFound(new ResponseDto
                {
                    errorReason = "User does not exist",
                    success = false,
                    token = ""
                });
            }

            var postMap = _mapper.Map<PostModel>(postForCreation);

            var createdPost = await _postRepository.CreatePostAsync(postMap);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Invalid data received",
                    success = false,
                    token = ""
                });
            }

            if (createdPost == null)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Failed to create the post",
                    success = false,
                    token = ""
                });
            }

            return CreatedAtRoute("GetPostById", new { userId = postForCreation.UserId, postId = postMap.PostId }, createdPost);
        }


        [HttpGet("{userId}/{postId}/comments/{commentId}", Name = "GetCommentById")]
        [ProducesResponseType(200, Type = typeof(CommentDto))]
        [ProducesResponseType(404, Type = typeof(ResponseDto))]
        public async Task<IActionResult> GetCommentById(int userId, int postId, int commentId)
        {
            try
            {
                var userExists = await _userRepository.UserExists(userId);
                if (!userExists)
                {
                    return NotFound(new ResponseDto
                    {
                        errorReason = "User does not exist",
                        success = false,
                        token = ""
                    });
                }

                var post = await _postRepository.GetPostByIdAsync(postId);
                if (post == null)
                {
                    return NotFound(new ResponseDto
                    {
                        errorReason = "Post does not exist",
                        success = false,
                        token = ""
                    });
                }

                var comment = await _commentRepository.GetCommentByIdAsync(commentId);

                if (comment == null)
                {
                    return NotFound(new ResponseDto
                    {
                        errorReason = "Comment does not exist",
                        success = false,
                        token = ""
                    });
                }

                var commentDto = _mapper.Map<CommentDto>(comment);

                return Ok(commentDto);
            }
            catch (Exception ex)
            {
                // Log the exception for further analysis
                return StatusCode(500, new ResponseDto
                {
                    errorReason = "Internal server error",
                    success = false,
                    token = "",
                });
            }
        }


        [HttpPost("{userId}/{postId}/Add-comment")]
        [ProducesResponseType(201, Type = typeof(CommentDto))]
        [ProducesResponseType(400, Type = typeof(ResponseDto))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AddComment([FromBody] CommentForCreationDto commentForCreation, int userId, int postId)
        {
            try
            {
                var userExists = await _userRepository.UserExists(userId);
                if (!userExists)
                {
                    return NotFound(new ResponseDto
                    {
                        errorReason = "User does not exist",
                        success = false,
                        token = ""
                    });
                }

                var post = await _postRepository.GetPostByIdAsync(postId);
                if (post == null)
                {
                    return NotFound(new ResponseDto
                    {
                        errorReason = "Post does not exist",
                        success = false,
                        token = ""
                    });
                }

                if (commentForCreation == null || commentForCreation.PostId != postId)
                {
                    return BadRequest(new ResponseDto
                    {
                        errorReason = "Invalid data received",
                        success = false,
                        token = ""
                    });
                }

                var commentMap = _mapper.Map<CommentModel>(commentForCreation);

    
                var createdComment = await _commentRepository.AddCommentToPostAsync(commentMap);

                if (!ModelState.IsValid || createdComment == null)
                {
                    return BadRequest(new ResponseDto
                    {
                        errorReason = "Failed to create comment",
                        success = false,
                        token = ""
                    });
                }

                var commentDto = _mapper.Map<CommentDto>(commentMap);

                return CreatedAtRoute("GetCommentById", new { userId, postId, commentId = commentMap.CommentId }, commentDto);
            }
            catch (Exception ex)
            {
                // Log the exception for further analysis
                return StatusCode(500, new ResponseDto
                {
                    errorReason = "Internal server error",
                    success = false,
                    token = "",
                });
            }
        }


        [HttpPost("{postId}/likes")]
        [ProducesResponseType(201, Type = typeof(PostDto))]
        [ProducesResponseType(404, Type = typeof(ResponseDto))]
        [ProducesResponseType(400, Type = typeof(ResponseDto))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AddLikes(int postId, [FromBody] UserLikeDto userLikeDto)
        {
            if (userLikeDto == null)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Invalid request body",
                    success = false,
                    token = ""
                });
            }

            var userExists = await _userRepository.UserExists(userLikeDto.UserId);
            if (!userExists)
            {
                return NotFound(new ResponseDto
                {
                    errorReason = "User not found",
                    success = false,
                    token = ""
                });
            }

            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound(new ResponseDto
                {
                    errorReason = "Post not found",
                    success = false,
                    token = ""
                });
            }

            var userLikedPost = await _postRepository.UserLikedPost(userLikeDto.UserId, postId);
            if (userLikedPost)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "You Have already liked this post",
                    success = false,
                    token = ""
                });
            }

            var postLikeMap = _mapper.Map<PostLike>(userLikeDto);
            var createdLike = await _postRepository.AddLikeToPost(postLikeMap);

            if (!createdLike)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Failed to update likes",
                    success = false,
                    token = ""
                });
            }

            post.Likes++;
            if (!await _postRepository.UpdatePostAsync(post))
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Failed to update likes",
                    success = false,
                    token = ""
                });
            }

            // Return the updated post
            var postDto = _mapper.Map<PostDto>(post);

            return CreatedAtRoute("GetPostById", new { userId = post.UserId, postId = post.PostId }, postDto);
        }

    }
}
