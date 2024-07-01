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
    [Route("api/tracks/{trackId}/points")]
    [Authorize(Policy.User)]
    public class TrackPointsController : AllAuthApiController
    {
        public TrackPointsController(
            ILoggerFactory loggerFactory, 
            IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService) 
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
        
        [HttpGet(Name = RouteNames.GetPointsForTrack)]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Points, SwaggerGroups.Tracks })]
        public async Task<IActionResult> GetAllForTrack([FromRoute]int trackId, [FromQuery]CollectionParameters parameters)
        {
            PagedList<GpsLocation> data = await logic.Tracks.GetPointsAsync(trackId, parameters);           
            AddPaginationMetadata<GpsLocation>(data, parameters, RouteNames.GetPointsForTrack);
            return Ok(data.ShapeCollection<GpsLocation>(parameters.Fields));
        }
        [HttpGet("new/{lastUtc}", Name = RouteNames.GetNewPointsForTrack)]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Points, SwaggerGroups.Tracks })]
        public async Task<IActionResult> GetNewForTrack([FromRoute]int trackId, [FromRoute]int lastUtc, [FromQuery]CollectionParameters parameters)
        {
            PagedList<GpsLocation> data = await logic.Tracks.GetNewPointsAsync(trackId, lastUtc, parameters);           
            AddPaginationMetadata<GpsLocation>(data, parameters, RouteNames.GetNewPointsForTrack);
            return Ok(data.ShapeCollection<GpsLocation>(parameters.Fields));
        }
    }
}