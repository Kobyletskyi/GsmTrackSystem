using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Helpers.Auth
{
    public static class AuthConfig
    {
        private const string SecretKey = "needtogetthisfromenvironment";

        public static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public static readonly string Issuer = "SuperAwesomeTokenServer";
        public static readonly string Audience = "http://localhost:9966";
        public static TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);
        public static readonly SigningCredentials SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
    }
}