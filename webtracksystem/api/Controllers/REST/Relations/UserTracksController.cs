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
    [Route("api/users/{userId}/tracks")]
    [Authorize(Policy.User)]
    public class UserTracksController : AllAuthApiController
    {
        public UserTracksController(
            ILoggerFactory loggerFactory, 
            IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService) 
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
        
        [HttpGet(Name = RouteNames.GetTracksForUser)]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Users, SwaggerGroups.Tracks })]
        [MyResource("userId")]
        public async Task<IActionResult> GetAllForUser([FromRoute]int userId, [FromQuery]CollectionParameters parameters)
        {
            PagedList<Track> data = await logic.Users.GetTracksAsync(userId, parameters);           
            AddPaginationMetadata<Track>(data, parameters, RouteNames.GetTracksForUser);
            return Ok(data.ShapeCollection<Track>(parameters.Fields));
        }
    }
}