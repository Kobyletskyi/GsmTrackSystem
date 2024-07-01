using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Models;
using AutoMapper;
using BusinessLayer;
using BusinessLayer.Models;
using BusinessLayer.Transformation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
    [Route("api/users/{userId:me}/devices")]
    [Authorize(Policy.User)]
    public class UserDevicesController : AllAuthApiController
    {
        public UserDevicesController(
            ILoggerFactory loggerFactory, 
            IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService) 
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
        
        [HttpGet(Name = RouteNames.GetDevicesForUser)]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Users, SwaggerGroups.Devices })]
        [MyResource("userId")]
        public async Task<IActionResult> GetAll([FromRoute]int userId, [FromQuery]CollectionParameters parameters)
        {
            PagedList<Device> data = await logic.Users.GetDevicesAsync(userId, parameters);           
            AddPaginationMetadata<Device>(data, parameters, RouteNames.GetDevicesForUser);
            return Ok(data.ShapeCollection<Device>(parameters.Fields));
        }
        [HttpPost]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Devices })]
        [MyResource("userId")]
        public async Task<IActionResult> Post([FromRoute]int userId, [FromBody]DeviceForPostWithoutUser device)
        {
            NewDevice newDevice = Mapper.Map<NewDevice>(device);
            newDevice.OwnerId = userId;
            Device entity = await logic.Devices.CreateAsync(newDevice);
            string url = Url.RouteUrl(RouteNames.GetDevice, new { id = entity.Id });
            return Created(url, entity);
        }
    }
}
