using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Dto;
using Task.Models;
using System.Security.Cryptography;
using System.Text;
using Task.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly ICurrentUser _currentUser;
        private readonly UserManager<User> _userManager;

        public UsersController(ApplicationDbContext context, IAuthService authService, ICurrentUser currentUser, UserManager<User> userManager)
        {
            _context = context;
            _authService = authService;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]

        public async Task<IActionResult> Register([FromBody] UserRegisterDTO user)
        {
            var result = await _authService.Register(user);
            if (result.Status == true)
                return Ok(result);
            else
                return BadRequest(result);

            //var dbUser = await _context.Users.SingleOrDefaultAsync(
            //    u => u.Email == user.Email
            //    );

            //if (dbUser != null)
            //    return BadRequest("The email is already in use");
            ////hash the password
            //string hashedPassword = hash(user.Password);

            ////generate random api token
            //string randomChars = ApiTokenGenerator();

            //User newUser = new User
            //{
            //    Name = user.Name,
            //    Email = user.Email,
            //    PasswordHash = hashedPassword,
            //    Api_token = randomChars
            //};
            //await _context.Users.AddAsync(newUser);
            //_context.SaveChanges();
            //return Ok(newUser);
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            var result = await _authService.AuthenticateAsync(user.Email,user.Password);
            if (result.Status == true)
                return Ok(result);
            else
                return BadRequest(result);
            
            //string hashedPassword = hash(user.Password);

            //var dbUser = await _context.Users.SingleOrDefaultAsync(
            //   u => u.Email == user.Email && u.PasswordHash == hashedPassword
            //    );

            //if (dbUser == null)
            //    return BadRequest("invalid credentials");

            //string api_token = ApiTokenGenerator();
            //dbUser.Api_token = api_token;
            //_context.SaveChanges();
            //return Ok(new { user = dbUser, api_token = api_token });
        }

        [HttpGet]
        [Route("whoami")]
        [Authorize]
        public IActionResult whoami()
        {
            var claims = User.Claims.ToList();
            var result = _currentUser;
            return Ok(result);
            //if(HttpContext.Request.Headers.Authorization.ToString() == "")
            //    return NotFound(new { Name = "Guest" });

            //var user = await _context.Users.FirstOrDefaultAsync(u =>
            //    u.Api_token == HttpContext.Request.Headers.Authorization.ToString());
            //if (user == null)
            //    return NotFound(new { Name = "Guest" });
            //return Ok(user);
        }



        [Authorize]
        [Route("me")]
        [HttpGet]
        public async Task<IActionResult> me()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles
            });
        }




        [HttpDelete]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //if (HttpContext.Request.Headers.Authorization.ToString() == "")
            //    return BadRequest("not logged in");

            //var user = await _context.Users.FirstOrDefaultAsync(u =>
            //    u.Api_token == HttpContext.Request.Headers.Authorization.ToString());
            //if (user == null)
            //    return NotFound("Guest");

            //var result = _currentUser;
            //return Ok(result);
            //user.Api_token = "";
            //_context.SaveChanges();
            return Ok("Logged out succefully");
        }
        //private string ApiTokenGenerator(int length = 100)
        //{
        //    Random random = new Random();
        //    string randomChars = new string(
        //        Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
        //                  .Select(s => s[random.Next(s.Length)])
        //                  .ToArray());
        //    return randomChars;
        //}
        //private string hash(string password)
        //{
        //    SHA256 sha256 = SHA256.Create();
        //    byte[] bytes = Encoding.UTF8.GetBytes(password);
        //    byte[] hash = sha256.ComputeHash(bytes);
        //    string hashedPassword = Convert.ToBase64String(hash);
        //    return hashedPassword;
        //}
    }


}
