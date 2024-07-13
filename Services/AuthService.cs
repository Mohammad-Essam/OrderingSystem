using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Task.Dto.Messages;
using Task.Models;

namespace Task.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthService(UserManager<Models.User> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }

        public async Task<AuthMessageDto> AuthenticateAsync(string UsernameOrEmail, string password)
        {
            User user = userManager.Users.Where(x => x.UserName == UsernameOrEmail || x.Email == UsernameOrEmail).FirstOrDefault();
            
            if (user == null)
            {
                return new AuthMessageDto { Status = false, ErrorMessage = "Invalide Credential" };
            }
            if (await userManager.CheckPasswordAsync(user, password))
                return new AuthMessageDto { Status = true, User = user, token = await GenerateTokenAsync(user) };
            else
                return new AuthMessageDto { Status = false, ErrorMessage = "Invalide Credential" };

        }

        public  async Task<AuthMessageDto> Register(UserRegisterDTO newUser)
        {
            if (userManager.Users.Any(x => x.UserName == newUser.Email))
            {
                return new AuthMessageDto { Status = false, ErrorMessage = "This email is already in use" };
            }

            var user = new User
            {
                UserName = newUser.Username,
                Email = newUser.Email,
            };

            var result = await userManager.CreateAsync(user, newUser.Password);
            if (result.Succeeded)
            {
                foreach (var role in newUser.Roles)
                {
                    await userManager.AddToRoleAsync(user, role);
                }

                return new AuthMessageDto{Status = true, token = await GenerateTokenAsync(user), User = user};
            }
            else
            {
                var er = result.Errors.Select(x => x.Description).ToList();
                string errorMessage = string.Join("\n", er);
                return new AuthMessageDto { Status = false, ErrorMessage = errorMessage };
            }
        }

        
        private async Task<string> GenerateTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)

            };
            IEnumerable<string> userRoles = await userManager.GetRolesAsync(user);
            foreach (string role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: configuration.GetSection("Jwt:Issuer").Value,
                audience: configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCredentials
                );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }


    }
}
