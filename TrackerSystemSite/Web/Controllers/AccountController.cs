using BusinessLayer;
using BusinessLayer.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Web.Helpers.Auth;
using Web.Models;

namespace Web.Controllers
{
    //[RequireHttps]
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private AuthService _authService;
        protected IBusinessLogic _logic;

        public AccountController(IBusinessLogic logic) : base(logic)
        {
            this._logic = logic;
            _authService = new AuthService(logic);
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> Signin([FromBody]Credentials cred)
        {
            TokenResponse token = await _authService.GenerateJwtTokenAsync(cred.userName, cred.password);
            if (token != null)
            {   
                var user = _logic.UserLogic.FindByUsername(cred.userName);
                setCookieToken(token.access_token, user.Id, cred.remember ? 7 : 1);
                return Ok(user);
            }
            return Unauthorized();
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> Signup([FromBody]Credentials cred)
        {
            PasswordHash hash = new PasswordHash(cred.password);
            var user = _logic.UserLogic.CreateUser(cred.userName, hash.ToString());
            TokenResponse token = await _authService.GenerateJwtTokenAsync(cred.userName, cred.password);
            if (token != null)
            {
                setCookieToken(token.access_token, user.Id, 1);
            }
            var url = Url.Route("DefaultApiWithAction", new { controller = "user", action="get", id = user.Id });
            return Created<Account>(url, user);
        }

        public async Task<IHttpActionResult> Signout()
        {
            //if (HttpContext.Current.Request.Cookies.AllKeys.Contains("Cookie"))
            {
                setCookieToken("",0, -1);
            }
            return Ok();
        }

        private void setCookieToken(string token, int userId, int days)
        {
            HttpCookie cookie = new HttpCookie("token", token);
            cookie.Expires = DateTime.Now.AddDays(days);
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";
            HttpContext.Current.Response.Cookies.Add(cookie);

            HttpCookie cookieUserID = new HttpCookie("userId", userId.ToString());
            cookie.Expires = DateTime.Now.AddDays(days);
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";
            HttpContext.Current.Response.Cookies.Add(cookieUserID);
        }
    }
}