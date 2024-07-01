#region using
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Models;
using Api.Providers;
using Api.Providers.Auth;
using BusinessLayer;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Api.Extentions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Api.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
#endregion

namespace Api.Controllers
{
    [Route("auth")]
    //[RequireHttps]
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IAuthService _authService;
        public AuthController(
            IOptions<JwtIssuerOptions> jwtOptions, 
            ILoggerFactory loggerFactory, 
            IBusinessLogic logic,
            IAuthService authService,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService)
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        {
            _jwtOptions = jwtOptions.Value;
            _authService = authService;
        }

        [Route("token")]
        [HttpPost]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Auth })]
        [ProducesResponseType(typeof(Token), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), (int)HttpStatusCode.BadRequest)]
        #endregion
        public async Task<IActionResult> GetTokenWithJson([FromBody]Credentials cred)
        {
            return await getToken(cred);
        }
        [Route("token/form")]
        [HttpPost]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Auth })]
        [ProducesResponseType(typeof(Token), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        #endregion
        public async Task<IActionResult> GetTokenWithForm([FromForm]Credentials cred)
        {
            return await getToken(cred);
        }
        [Route("signin")]
        [HttpPost]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Auth })]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        #endregion
        //public async Task<IActionResult> Signin([FromBody]Credentials cred)
        public async Task<IActionResult> Signin([FromBody]Credentials cred)
        {
            if(!ModelState.IsValid){
                return new UnprocessableEntity(ModelState);
            }
            //var token = await _authService.GenerateJwtToken("firstUser", cred.password);
            Token token = await _authService.GenerateJwtToken(cred.userName, cred.password);
            if(token != null){
                ClaimsPrincipal principal = _authService.ValidateToken(token.AccessToken);
                string id = principal.Claims.First(x => x.Type == "userId").Value;
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Ok(new {id});
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Unauthorized();
        }

        [Route("signup")]
        [HttpPost]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Auth })]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), (int)HttpStatusCode.BadRequest)]
        #endregion
        public async Task<IActionResult> Signup([FromBody]Credentials cred)
        {
            if(!ModelState.IsValid){
                return new UnprocessableEntity(ModelState);
            }
            User account = await logic.Users.CreateAsync(cred.userName, new PasswordHash(cred.password).ToString());
            Token token = await _authService.GenerateJwtToken(cred.userName, cred.password);
            if(token != null) {
                ClaimsPrincipal principal = _authService.ValidateToken(token.AccessToken);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                string accountUrl = Url.RouteUrl(RouteNames.GetUser, new { id = account.Id });
                return Created(accountUrl, account);
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [Route("signout")]
        [HttpPost]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Auth })]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
        private async Task<IActionResult> getToken(Credentials cred)
        {
            if(!ModelState.IsValid){
                return new UnprocessableEntity(ModelState);
            }
            Token token = await _authService.GenerateJwtToken(cred.userName, cred.password);    
            if(token != null){
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}