using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YoutubeClone.Dtos;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;
using Microsoft.EntityFrameworkCore;

namespace YoutubeClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
      
            private readonly IConfiguration _configuration;
            private readonly IUserRepository _userData;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public class AuthenticationRequestBody
            {
                public string? UserName { get; set; }
                public string? Password { get; set; }
            }

            public AuthController(IConfiguration configuration, IUserRepository userData, IPasswordHasher passwordHasher, IMapper mapper, IEmailService emailService)
            {
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
                _userData = userData ?? throw new ArgumentNullException(nameof(userData));
                _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }


            [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(ResponseDto))]
        [ProducesResponseType(401, Type = typeof(ResponseDto))]
        public async  Task<ActionResult<string>> Authenticate(AuthenticationRequestBody authenticationRequestBody)
            {
                if (authenticationRequestBody.UserName == null || authenticationRequestBody.Password == null)
                {
                    return BadRequest("Username and password cannot be null.");
                }

                var user = await ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);

                if (user == null)
                {
                var errorResponse = new ResponseDto()
                {
                    errorReason = "Username or password incorrect",
                    success = false,
                    token = "",
                };
                
                    return Unauthorized(errorResponse);
                }

                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                var claimsForToken = new List<Claim>();
                claimsForToken.Add(new Claim("Id", user.UserId.ToString()));
                claimsForToken.Add(new Claim("Firstname", user.FirstName));
                claimsForToken.Add(new Claim("lastname", user.LastName));
            claimsForToken.Add(new Claim("Username", user.UserName));


            var jwtSecurityToken = new JwtSecurityToken(
                    _configuration["Authentication:Issuer"],
                    _configuration["Authentication:Audience"],
                    claimsForToken,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1),
                    signingCredentials
                    );

                var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var response = new ResponseDto()
            {
                errorReason = "",
                success = true,
                token = tokenToReturn,
            };

                return Ok(response);

            }

            private async Task<UserModel?> ValidateUserCredentials(string? userName, string? password)
            {

            var users = await _userData.GetUsers();
                var user =  users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

                if (user == null || !_passwordHasher.Verify(user.Password, password))
                {
                    return null;
                }


                return user;

            }


        [HttpPost("Register")]
        [ProducesResponseType(204, Type = typeof(ResponseDto))]
        [ProducesResponseType(400, Type = typeof(ResponseDto))]
        [ProducesResponseType(500, Type = typeof(ResponseDto))]
        public async Task<IActionResult> CreateUser([FromBody] UserForCreationDto userCreate)
        {
            if (userCreate == null)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Invalid data received",
                    success = false,
                    token = ""
                });
            }

            var users = await _userData.GetUsers();

            var user = users.FirstOrDefault(c => c.UserName.Trim().ToLower() == userCreate.UserName.ToLower() || c.Email.Trim().ToLower() == userCreate.Email.ToLower());

            if (user != null)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "User already exists",
                    success = false,
                    token = ""
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Invalid model state",
                    success = false,
                    token = ""
                });
            }

            userCreate.Password = _passwordHasher.Hash(userCreate.Password);

            var userMap = _mapper.Map<UserModel>(userCreate);

            var newUser = await _userData.AddUser(userMap);

            if (!newUser)
            {
                return StatusCode(500, new ResponseDto
                {
                    errorReason = "Something went wrong while creating",
                    success = false,
                    token = ""
                });
            }

            return Ok(new ResponseDto
            {
                errorReason = "",
                success = true,
                token = ""
            });
        }

        [HttpPost("Change-Password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(ResponseDto))]
        [ProducesResponseType(422, Type = typeof(ResponseDto))]
        public async Task<IActionResult> ChangePassword(PasswordChangeDto passwordUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    errorReason = "Invalid model state",
                    success = false,
                    token = ""
                });
            }

            var user = await _userData.GetUserByName(passwordUpdate.UserName);

            if (user == null)
            {
                return StatusCode(422, new ResponseDto
                {
                    errorReason = "User not found",
                    success = false,
                    token = ""
                });
            }

            var passwordChanged = await _userData.ChangePassword(user, passwordUpdate.Password);

            if (!passwordChanged)
            {
                return StatusCode(422, new ResponseDto
                {
                    errorReason = "Something went wrong while changing the password",
                    success = false,
                    token = ""
                });
            }

            return NoContent();
        }

    }
}
