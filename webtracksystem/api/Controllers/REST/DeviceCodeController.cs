using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Helpers;
using BusinessLayer;
using BusinessLayer.Transformation;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
    [Route("api/devicecode")]
    [Authorize(Policy.User)]
    public class DeviceCodeController : AllAuthApiController
    {
        public DeviceCodeController(
            ILoggerFactory loggerFactory, 
            IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService) 
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }

        [HttpGet("{id}", Name= RouteNames.GetDeviceCode)]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Devices })]
        public async Task<IActionResult> GetById([FromRoute]int id, [FromQuery]string fields)
        {
            DeviceCode code = await logic.Devices.GetCodeAsync(id, fields);
            if(code == null)
            {
                return NotFound();
            }
            return Ok(code.ShapeObject<DeviceCode>(fields));
        }
    }
}
