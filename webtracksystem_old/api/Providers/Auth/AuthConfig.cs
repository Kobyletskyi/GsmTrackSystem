using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Api.Providers.Auth
{
    public static class AuthConfig
    {
        private const string SecretKey = "needtogetthisfromenvironment";

        public static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public static readonly string Issuer = "SuperAwesomeTokenServer";
        public static readonly string Audience = "http://localhost:5000";
        public static readonly string Authority = "http://localhost:5000";
        public static TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);
        public static readonly SigningCredentials SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);

        public static readonly TokenValidationParameters ValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = AuthConfig.Issuer,
            ValidAudience = AuthConfig.Audience,
            IssuerSigningKey = AuthConfig.SigningKey,
            AuthenticationType = "JWT",
            //NameClaimType = "JWT"
        };
    }
}