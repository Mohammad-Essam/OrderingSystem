using Microsoft.AspNetCore.Mvc;
using Task.Dto;
using Task.Dto.Messages;
using Task.Models;

namespace Task.Services
{
    public interface IAuthService
    {
        Task<AuthMessageDto> AuthenticateAsync(string EmailOrUsername, string password);
        Task<AuthMessageDto> Register(UserRegisterDTO newUser);
    }
}
