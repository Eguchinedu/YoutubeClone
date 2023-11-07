using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutubeClone.Dtos;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;

namespace YoutubeClone.Controllers
{
    [Route("api/user/{userId}/[controller]")]
    [ApiController]
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

       


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]

        public IActionResult GetPosts( int userId)
        {
            if (!_userRepository.UserExists(userId))
            {
                return NotFound("User does not exist");
            }

            var userPosts = _postRepository.GetPosts(userId);

            if (userPosts == null)
            {
                return NotFound("this user has no posts");
            }

            return Ok(userPosts);
        }


        [HttpGet("{postId}", Name ="GetPosts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostModel>))]
        [ProducesResponseType(400)]

        public IActionResult GetPost(int userId, int postId)
        {
            if (!_userRepository.UserExists(userId))
            {
                return NotFound();
            }

            var post = _mapper.Map<PostModel>(_postRepository.GetPostForUser(userId,postId));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (post == null)
            {
                return NotFound("Post does not exist");
            }
            return Ok(post);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]

        public IActionResult CreatePost([FromBody] PostForCreationDto postForCreation, int userId)
        {



            if (postForCreation == null)
            {
                return BadRequest();
            }

            var user = _userRepository.GetUser(userId);

            if (user == null)
            {
                return NotFound("User does not exist");
            }


            var postMap = _mapper.Map<PostModel>(postForCreation);

            var createdPost = _postRepository.CreatePost(postMap);

            if (!ModelState.IsValid)
            {
                return BadRequest(createdPost); 
            }

            if (createdPost == null)
            {
                return BadRequest("Failed to create the post.");
            }

            return CreatedAtRoute("GetPosts", new { userId = userId, postId = postMap.PostId }, createdPost);
        }



        [HttpGet("{postId}/View-comments", Name ="GetComments")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CommentDto>))]
        [ProducesResponseType(404)]

        public IActionResult GetComments(int userId, int postId)
        {
            if (!_userRepository.UserExists(userId))
            {
                return NotFound("User does not exist");
            }

            var post = _postRepository.GetPostById(postId);

            if (post == null)
            {
                return NotFound("Post not found");
            }

            var userComments = _commentRepository.GetCommentForPost(postId);

            if (userComments == null || !userComments.Any())
            {
                return NotFound("No comments yet");
            }

            var userCommentsDto = _mapper.Map<IEnumerable<CommentDto>>(userComments);

            return Ok(userCommentsDto);
        }





        [HttpPost("{postId}/Add-comment")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]

        public IActionResult AddComment([FromBody] CommentForCreationDto commentForCreation, int postId, int userId)
        {
            if (!_userRepository.UserExists(userId))
            {
                return NotFound();
            }

            var post = _postRepository.GetPostById(postId);

            if (post == null)
            {
                
                return null;
            }


            if (commentForCreation == null)
            {
                return BadRequest();
            }



            var commentMap = _mapper.Map<CommentModel>(commentForCreation);

            var createdComment = _commentRepository.AddCommentToPost(commentMap);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (createdComment == null)
            {
                return BadRequest("Failed to create comment.");
            }

            return CreatedAtAction("GetComments", new {commentId = commentMap.CommentId }, createdComment); 
        }

        [HttpPost("{postId}/add-likes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult AddLikes(int postId)
        {
            
            var post = _postRepository.GetPostById(postId);

            if (post == null)
            {
                return NotFound("Post not found");
            }

            
            post.Likes++; 

           
            if (!_postRepository.UpdatePost(post))
            {
                return BadRequest("Failed to update likes");
            }

            return Ok(post);
        }


    }
}
