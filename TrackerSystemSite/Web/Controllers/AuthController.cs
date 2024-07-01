using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BusinessLayer;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using BusinessLayer.Models;
using DbModels.Entities;
using System.Web.Http.Filters;
using System.Threading.Tasks;
using Web.Helpers.Auth;
using Web.Models;

using WEB = Web.Models;

namespace Web.Controllers
{
    
    public class AuthController : ApiController
    {
        //ToDo: move to dependency resolver
        private AuthService _authService;
        protected IBusinessLogic logic;

        public AuthController(IBusinessLogic logic)
        {
            this.logic = logic;
            _authService = new AuthService(logic);
        }
        //[RequireHttps]
        [Route("auth/token")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetTokenWithJson([FromBody]Credentials cred)
        {
            return Json(await _authService.GenerateJwtTokenAsync(cred.userName, cred.password));
        }

        //Device
        [Route("auth/device/token")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetTokenForDevice([FromBody]WEB.DeviceCode cred)
        {
            string token = await _authService.GetDeviceRefreshTokenAsync(cred.imei, cred.code);
            if (!String.IsNullOrWhiteSpace(token))
            {
                return Ok(token);
            }
            return Unauthorized();
        }

        [Route("auth/device/refreshtoken")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RefreshTokenForDevice([FromBody]DeviceToken cred)
        {
            var token = await _authService.RefreshDeviceJwtTokenAsync(cred.imei, cred.refreshToken);
            if (token != null)
            {
                return Ok(token.access_token);
            }
            return Unauthorized();
        }

        [Route("auth/device/code")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Code([FromBody]string imei)
        {
            var code = logic.DeviceLogic.CreateDeviceCode(imei);
            return Created<AuthCodeEntity>("Link", null);
        }
    }
}
