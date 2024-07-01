using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Helpers;
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
    [Route("api/devices/{deviceId}/tracks")]
    [Authorize(Policy.User)]
    public class DeviceTracksController : AllAuthApiController
    {
        public DeviceTracksController(
            ILoggerFactory loggerFactory, 
            IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService) 
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
        
        [HttpGet(Name = RouteNames.GetTracksForDevice)]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Devices, SwaggerGroups.Tracks })]
        public async Task<IActionResult> GetAllForDevice(int deviceId, CollectionParameters parameters)
        {
            PagedList<Track> data = await logic.Devices.GetTracksAsync(deviceId, parameters);           
            AddPaginationMetadata<Track>(data, parameters, RouteNames.GetTracksForDevice);
            return Ok(data.ShapeCollection<Track>(parameters.Fields));
        }
    }
}