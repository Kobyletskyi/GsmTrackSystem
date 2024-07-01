using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Web.Models;

namespace Web.Helpers.Auth
{
    public interface IAuthService
    {
        Task<TokenResponse> GenerateJwtTokenAsync(string username, string password);
        Task<ClaimsPrincipal> ValidateTokenAsync(string tokenString);
        Task<string> GetDeviceRefreshTokenAsync(string imei, int code);
        Task<TokenResponse> RefreshDeviceJwtTokenAsync(string imei, string refreshToken);
    }
}