using ProductDemo.API.Models;

namespace ProductDemo.API.Services.Interfaces
{
    public interface IAuthTokenService
    {
        string CreateToken(AppUser user);
    }
}
