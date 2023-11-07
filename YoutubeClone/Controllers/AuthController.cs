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


            public class AuthenticationRequestBody
            {
                public string? UserName { get; set; }
                public string? Password { get; set; }
            }

            public AuthController(IConfiguration configuration, IUserRepository userData, IPasswordHasher passwordHasher, IMapper mapper)
            {
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
                _userData = userData ?? throw new ArgumentNullException(nameof(userData));
                _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }


            [HttpPost("login")]
            public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
            {
                if (authenticationRequestBody.UserName == null || authenticationRequestBody.Password == null)
                {
                    return BadRequest("Username and password cannot be null.");
                }

                var user = ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "username or password is incorrect");
                    return Unauthorized(ModelState);
                }

                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                var claimsForToken = new List<Claim>();
                claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
                claimsForToken.Add(new Claim("given_name", user.FirstName));
                claimsForToken.Add(new Claim("family_name", user.LastName));

                var jwtSecurityToken = new JwtSecurityToken(
                    _configuration["Authentication:Issuer"],
                    _configuration["Authentication:Audience"],
                    claimsForToken,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1),
                    signingCredentials
                    );

                var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return Ok(tokenToReturn);

            }

            private UserModel? ValidateUserCredentials(string? userName, string? password)
            {
                var user = _userData.GetUsers()
           .FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

                if (user == null || !_passwordHasher.Verify(user.Password, password))
                {
                    return null;
                }


                return user;

            }
            


        [HttpPost("Register")]
            [ProducesResponseType(204)]
            [ProducesResponseType(400)]

            public IActionResult CreateUser([FromBody] UserForCreationDto userCreate)
            {
                if (userCreate == null)
                {
                    return BadRequest();
                }

                var user = _userData.GetUsers().Where(c => c.UserName.Trim().ToLower() == userCreate.UserName.ToLower()).FirstOrDefault();

                if (user != null)
                {
                    ModelState.AddModelError("", "Username Already exists");
                    return StatusCode(422, ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                userCreate.Password = _passwordHasher.Hash(userCreate.Password);

                var userMap = _mapper.Map<UserModel>(userCreate);

                if (!_userData.AddUser(userMap))
                {
                    ModelState.AddModelError("", "Something went wrong while creating");
                    return StatusCode(500, ModelState);
                }
                return Ok("User Successfully created");
            }


        [HttpPost("Change-Password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult ChangePassword(PasswordChangeDto passwordUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userData.GetUserByName(passwordUpdate.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return StatusCode(422, ModelState);
            }
            

            var passwordChanged = _userData.ChangePassword(user, passwordUpdate.Password);

            if (!passwordChanged)
            {
                ModelState.AddModelError("", "Something went wrong while changing the password");
                return StatusCode(422, ModelState);
            }

            return NoContent(); 
        }

    }
}
