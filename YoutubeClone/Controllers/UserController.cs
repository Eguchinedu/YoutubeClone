using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YoutubeClone.Dtos;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;
using YoutubeClone.Repository;

namespace YoutubeClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _userData;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IPostRepository _postRepository;

        public UserController(IUserRepository userData, IMapper mapper, IEmailService emailService, IPostRepository postRepository)
        {
            _userData = userData ?? throw new ArgumentNullException(nameof(userData));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        }

        [HttpGet("post")]
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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(400, Type = typeof(ResponseDto))]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var userGet = await _userData.GetUsers();
                var users = _mapper.Map<List<UserDto>>(userGet);

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Failed to get users",
                    success = false,
                    token = "",
      
                });
            }
        }


        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(UserWithPostDto))]
        [ProducesResponseType(400, Type = typeof(ResponseDto))]
        public async Task<IActionResult> GetUserId(int userId)
        {
            try
            {
                var userFound = await _userData.UserExists(userId);
                if (!userFound)
                {
                    return NotFound(new ResponseDto
                    {
                        errorReason = "User does not exist",
                        success = false,
                        token = ""
                    });
                }

                var userCheck = await _userData.GetUser(userId);

                /*
                UserEmailOptions options = new UserEmailOptions
                {
                    ToEmails = new List<string>() {userCheck.Email},
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("{{UserName}}", userCheck.UserName)
                    }
                };
                await _emailService.SendTestEmail(options);
                */

                var user = _mapper.Map<UserWithPostDto>(userCheck);
                userCheck.Posts = _mapper.Map<List<PostModel>>(userCheck.Posts);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Failed to get user by ID",
                    success = false,
                    token = "",
                });
            }
        }





    }
}
