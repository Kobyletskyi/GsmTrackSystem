using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Models;
using Api.Providers.Auth;
using BusinessLayer;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodaTime;
using NodaTime.TimeZones;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
    [Route("api/device")]
    [Authorize(Policy.Device)]
    public class TrackerController : BearerAuthController
    {
        private readonly IAuthService authService;
        public TrackerController(ILoggerFactory loggerFactory, IBusinessLogic logic,
            IAuthService authService,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService)
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        {
            this.authService = authService;
        }

        [Route("code")]
        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Tracker })]
        public async Task<IActionResult> Code([Required, FromBody]string imei)
        {
            try{
            DeviceCode code = await logic.Devices.CreateCodeAsync(imei);
            string codeUrl = Url.RouteUrl(RouteNames.GetDeviceCode, new { id = code.Id });
            return Created(codeUrl, code);
            }catch{
                return NotFound();
            }
        }
        [Route("token")]
        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Tracker })]
        public async Task<IActionResult> GetRefreshTokenForDevice([FromBody]DeviceAuthCode cred)
        {
            string token = await authService.GetDeviceRefreshToken(cred.IMEI, cred.Code);
            if (!String.IsNullOrWhiteSpace(token))
            {
                return Ok(token);
            }
            return Unauthorized();
        }
        [Route("refreshtoken")]
        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Tracker })]
        public async Task<IActionResult> RefreshTokenForDevice([FromBody]DeviceToken cred)
        {
            var token = await authService.RefreshDeviceJwtToken(cred.IMEI, cred.RefreshToken);
            if (token != null)
            {
                return Ok(token.AccessToken);
            }
            return Unauthorized();
        }
        
        [HttpPost("location")]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Tracker })]
        public async Task<IActionResult> SetLocation([Required, FromBody] string str)
        {
            GpsLocation location = await logic.Devices.ProccessGpsDataAsync(str);
            string url = Url.RouteUrl(RouteNames.GetPoint, new { id = location.Id });
            return Created(url, null);
        }

        [Route("time")]
        [HttpGet]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Tracker })]
        public string Time()
        {
            string timezone = "Europe/Kiev";
            Instant now = SystemClock.Instance.GetCurrentInstant();
            var tz = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timezone) ;
            var date = now.InZone(tz);
            return date.ToString("yy'/'MM'/'dd,HH:mm:sso<g>", CultureInfo.InvariantCulture);
        }

        [Route("gpsassist")]
        [HttpGet]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Tracker })]
        public async Task<IActionResult> GpsAssist()
        {
            string url = "http://online-live1.services.u-blox.com/GetOnlineData.ashx?token=07txQ58CnEWPmLIdS15V9Q;gnss=gps;datatype=eph,alm,aux,pos;filteronpos;format=aid";
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpRequest.Method = "GET";
            try {
                WebResponse res = await httpRequest.GetResponseAsync();
                var contentType = res.Headers["Content-Type"];
                var contentDisposition = res.Headers["Content-Disposition"];
                return File(res.GetResponseStream(),contentType, contentDisposition);
            } catch {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}
