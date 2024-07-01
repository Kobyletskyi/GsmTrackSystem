using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Models;
using BusinessLayer;
using BusinessLayer.Models;
using Microsoft.IdentityModel.Tokens;
using Api.Extentions;

namespace Api.Providers.Auth
{
    public class AuthService : IAuthService
    {
        public static string ClientClaim = "client";
        public static string ImeiClaim = "imei";
        public static string UserIdClaim = "userId";
        private IBusinessLogic _logic;
        public AuthService(IBusinessLogic logic)
        {
            this._logic = logic;
        }
        public async Task<Token> GenerateJwtToken(string username, string password)
        {   
            User user = await _logic.Users.FindByUsernameAsync(username);
            if (user != null && !String.IsNullOrWhiteSpace(user.PasswordSalt))
            {
                PasswordHash hash = new PasswordHash(Convert.FromBase64String(user.PasswordSalt));
                if (hash.Verify(password))
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(user.Id.ToString(), "JWT"),
                        new[] {
                            //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClientClaim, Scopes.User.GetName<Scopes>()),
                            new Claim(UserIdClaim, user.Id.ToString())
                        });
                    return createJwtToken(identity);
                }
            }
            return null;
        }
        public async Task<Token> RefreshDeviceJwtToken(string imei, string refreshToken)
        {
            if (await _logic.Devices.ValidateRefreshTokenAsync(imei, refreshToken))
            {
                var identity = new ClaimsIdentity(
                    new GenericIdentity("userId", "JWT"),
                    new[] { new Claim(ClientClaim, Enum.GetName(typeof(Scopes), Scopes.Device)),
                    new Claim(ImeiClaim, imei) }
                    );

                return createJwtToken(identity);
            }
            return null;
        }
        public async Task<string> GetDeviceRefreshToken(string imei, int code)
        {
            return await _logic.Devices.GenerateRefreshTokenAsync(imei, code);
        }

        public ClaimsPrincipal ValidateToken(string tokenString)
        {
            ClaimsPrincipal result = null;
            try
            {
                SecurityToken securityToken = new JwtSecurityToken(tokenString);
                JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();               
                result = securityTokenHandler.ValidateToken(tokenString, AuthConfig.ValidationParameters, out securityToken);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        private Token createJwtToken(ClaimsIdentity identity)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(
               subject: identity,
               signingCredentials: AuthConfig.SigningCredentials,
               audience: AuthConfig.Audience,
               issuer: AuthConfig.Issuer,
               expires: DateTime.UtcNow.AddSeconds(AuthConfig.ValidFor.TotalSeconds));
            string accessToken = handler.WriteToken(token);
            return  new Token(){
                AccessToken = accessToken,
                Expires = (int)AuthConfig.ValidFor.TotalSeconds
            };
        }
    }
}