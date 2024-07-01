using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class TokenResponse
    {
        public TokenResponse(string token, int expires)
        {
            access_token = token;
            expires_in = expires;
        }
        public string access_token { get; private set; }
        public int expires_in { get; private set; }
    }
}