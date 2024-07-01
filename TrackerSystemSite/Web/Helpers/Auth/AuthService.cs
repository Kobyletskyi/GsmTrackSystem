using BusinessLayer;
using BusinessLayer.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Web.Models;

namespace Web.Helpers.Auth
{
    public class AuthService : IAuthService
    {
        public static string ScopeClaim = "scope";
        public static string ImeiClaim = "imei";
        public static string UserIdClaim = "userId";
        protected IBusinessLogic logic;

        public AuthService(IBusinessLogic logic)
        {
            this.logic = logic;
        }

        public async Task<TokenResponse> GenerateJwtTokenAsync(string username, string password)
        {
            
            Account user = logic.UserLogic.FindByUsername(username);
            if (!String.IsNullOrWhiteSpace(user.PasswordSalt))
            {
                PasswordHash hash = new PasswordHash(Convert.FromBase64String(user.PasswordSalt));
                if (hash.Verify(password))
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(user.Id.ToString(), "JWT"),
                        new[] {
                            new Claim(ScopeClaim, Enum.GetName(typeof(Scopes), Scopes.User))
                            //new Claim(UserIdClaim, user.Id.ToString())
                        });
                    return createJwtToken(identity);
                }
            }
            return null;
        }

        public async Task<TokenResponse> RefreshDeviceJwtTokenAsync(string imei, string refreshToken)
        {
            if (logic.DeviceLogic.ValidateRefreshToken(imei, refreshToken))
            {
                var identity = new ClaimsIdentity(
                    new GenericIdentity(imei, "JWT")
                    //new[] { new Claim(ScopeClaim, Enum.GetName(typeof(Scopes), Scopes.Device)) }
                    );

                return createJwtToken(identity);
            }
            return null;
        }

        public async Task<string> GetDeviceRefreshTokenAsync(string imei, int code)
        {
            return logic.DeviceLogic.GenerateRefreshToken(imei, code);
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string TokenString)
        {
            ClaimsPrincipal result = null;
            try
            {
                SecurityToken securityToken = new JwtSecurityToken(TokenString);
                JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();

                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = AuthConfig.Issuer,
                    ValidAudience = AuthConfig.Audience,
                    IssuerSigningKey = AuthConfig.SigningKey,
                    AuthenticationType = "JWT",
                    //NameClaimType = "JWT"
                };
                result = securityTokenHandler.ValidateToken(TokenString, validationParameters, out securityToken);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public TokenResponse createJwtToken(ClaimsIdentity identity)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = null;
            token = handler.CreateJwtSecurityToken(
               subject: identity,
               signingCredentials: AuthConfig.SigningCredentials,
               audience: AuthConfig.Audience,
               issuer: AuthConfig.Issuer,
               expires: DateTime.UtcNow.AddSeconds(AuthConfig.ValidFor.TotalSeconds));

            return new TokenResponse(handler.WriteToken(token), (int)AuthConfig.ValidFor.TotalSeconds);
        }

    }
}