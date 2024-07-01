using System.Security.Claims;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Providers.Auth
{
    public interface IAuthService
    {
        Task<Token> GenerateJwtToken(string username, string password);
        ClaimsPrincipal ValidateToken(string tokenString);
        Task<string> GetDeviceRefreshToken(string imei, int code);
        Task<Token> RefreshDeviceJwtToken(string imei, string refreshToken);
    }
}