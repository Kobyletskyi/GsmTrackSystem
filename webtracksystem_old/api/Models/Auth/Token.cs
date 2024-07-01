using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Token
    {
        // public Token(string token, int expires)
        // {           
        //     AccessToken = token;
        //     Expires = expires;
        //     //AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjE2NSIsImNsaWVudCI6IlVzZXIiLCJ1c2VySWQiOiIxNjUiLCJuYmYiOjE1MDQ4MTQ1MzUsImV4cCI6MTUwNDgxNDgzNSwiaWF0IjoxNTA0ODE0NTM1LCJpc3MiOiJTdXBlckF3ZXNvbWVUb2tlblNlcnZlciIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.WxZjE2Fwp0xo9l6r_dQPXpdxpopDphMjb87HQVFNaok";
        // }
        public string AccessToken { get; set; }
        public int Expires { get; set; }
    }
}