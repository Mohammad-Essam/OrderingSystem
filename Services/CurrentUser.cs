using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Task.Dto;
using Task.Models;

namespace Task.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;
        public CurrentUser(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

        }

        public bool IsAuthenticated {
            get {
                var authorizeKey = _httpContextAccessor.HttpContext.User;
                return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;            
            }
        }

        public User User
        {
            get
            {
                if (!IsAuthenticated)
                {
                    return null;
                }

                var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var usernameClaim = _httpContextAccessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Name);
                var user = _dbContext.Users.Where(u => u.Id == userIdClaim.Value).FirstOrDefault();
                return user;
            }
        }

        public IEnumerable<string> Roles
        {
            get
            {
                if (!IsAuthenticated)
                {
                    return Enumerable.Empty<string>();
                }

                return _httpContextAccessor.HttpContext?.User?.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value) ?? Enumerable.Empty<string>();
            }
        }
    }
}

