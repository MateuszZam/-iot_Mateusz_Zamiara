using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationPlugin;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using PaperForeverApi.Data;
using PaperForeverApi.Models;

namespace PaperForeverApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private AppDbContext _dbContext;
        private IConfiguration _config;
        private readonly AuthService _auth;

        public UsersController(AppDbContext dbContext, IConfiguration config)
        {
            _config = config;
            _auth = new AuthService(_config);
            _dbContext = dbContext;
        }

        //api/users/register
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            var userWithSameEmail = _dbContext.Users.Where(u => u.Email == user.Email).SingleOrDefault();

            if (userWithSameEmail != null)
            {
                return BadRequest("User with this e-mail address already exist");
            }

            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                Role = "Users"
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/users/login
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var userEmail = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

            if (userEmail == null)
            {
                return NotFound();
            }

            if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password))
            {
                return Unauthorized();
            }

            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userEmail.Role)
            };

            var authToken = _auth.GenerateAccessToken(authClaims);

            return new ObjectResult(new
            {
                accessToken = authToken.AccessToken,
                expiresIn = authToken.ExpiresIn,
                tokenType = authToken.TokenType,
                validationStart = authToken.ValidFrom,
                validationEnd = authToken.ValidTo,
                userId = userEmail.Id,
                userName = userEmail.Name
            });
        }
    }
}
