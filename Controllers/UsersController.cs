using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Dto;
using Task.Models;
using System.Security.Cryptography;
using System.Text;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDto user)
        {
            var dbUser = await _context.Users.SingleOrDefaultAsync(
                u => u.Email == user.Email
                );

            if (dbUser != null)
                return BadRequest("The email is already in use");
            //hash the password
            string hashedPassword = hash(user.Password);

            //generate random api token
            string randomChars = ApiTokenGenerator();

            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = hashedPassword,
                Api_token = randomChars
            };
            await _context.Users.AddAsync(newUser);
            _context.SaveChanges();
            return Ok(newUser);
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            string hashedPassword = hash(user.Password);

            var dbUser = await _context.Users.SingleOrDefaultAsync(
               u => u.Email == user.Email && u.Password == hashedPassword
                );

            if (dbUser == null)
                return BadRequest("invalid credentials");

            string api_token = ApiTokenGenerator();
            dbUser.Api_token = api_token;
            _context.SaveChanges();
            return Ok(new { user = dbUser, api_token = api_token });
        }

        [HttpGet]
        [Route("whoami")]
        public async Task<IActionResult> whoami()
        {
            if(HttpContext.Request.Headers.Authorization.ToString() == "")
                return NotFound(new { Name = "Guest" });

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Api_token == HttpContext.Request.Headers.Authorization.ToString());
            if (user == null)
                return NotFound(new { Name = "Guest" });
            return Ok(user);
        }
        [HttpDelete]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.Request.Headers.Authorization.ToString() == "")
                return BadRequest("not logged in");

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Api_token == HttpContext.Request.Headers.Authorization.ToString());
            if (user == null)
                return NotFound("Guest");

            user.Api_token = "";
            _context.SaveChanges();
            return Ok("Logged out succefully");
        }
        private string ApiTokenGenerator(int length = 100)
        {
            Random random = new Random();
            string randomChars = new string(
                Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return randomChars;
        }
        private string hash(string password)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            string hashedPassword = Convert.ToBase64String(hash);
            return hashedPassword;
        }
    }

}
