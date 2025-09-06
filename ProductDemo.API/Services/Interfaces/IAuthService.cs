using ProductDemo.API.DTOs.Auth;
using ProductDemo.API.Models;

namespace ProductDemo.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AppUser> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }

}