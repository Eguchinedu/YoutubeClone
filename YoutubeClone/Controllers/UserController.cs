using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YoutubeClone.Dtos;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;

namespace YoutubeClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _userData;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userData, IMapper mapper)
        {
            _userData = userData ?? throw new ArgumentNullException(nameof(userData));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserModel>))]

        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userData.GetUsers());

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]

        public IActionResult GetUserId(int userId)
        {
            if (!_userData.UserExists(userId))
            {
                return NotFound();
            }

            var user = _mapper.Map<UserDto>(_userData.GetUser(userId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(user);

        }



 

    }
}
