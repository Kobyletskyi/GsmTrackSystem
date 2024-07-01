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
    [Route("api/[controller]")]
    [Authorize(Policy.User)]
    public class TracksController : AllAuthApiController
    {
        public TracksController(
            ILoggerFactory loggerFactory, 
            IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService) 
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
        
        // [HttpGet(Name = RouteNames.GetTracks)]
        // [Authorize(Policy = "Admin")]
        // public async Task<IActionResult> GetAll(CollectionParameters parameters)
        // {
        //     PagedList<Track> data = await _logic.Tracks.GetTracksAsync(parameters as CollectionParameters);           
        //     AddPaginationMetadata<Track>(data, parameters, RouteNames.GetDevices);
        //     return Ok(data.ShapeCollection<Track>(parameters.Fields));
        // }
        [HttpGet("{id}", Name= RouteNames.GetTrack)]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Tracks })]
        public async Task<IActionResult> GetById([Required,FromRoute]int id, [FromQuery]string fields)
        {
            Track track = await logic.Tracks.GetTrackAsync(id, fields);
            if(track == null)
                return NotFound();
            return Ok(track.ShapeObject<Track>(fields));
        }
        // [HttpPost]
        // public async Task<IActionResult> Post([FromBody]Track track)
        // {
        //     track = await _logic.Devices.CreateAsync(track);
        //     var url = Url.RouteUrl(RouteNames.GetTrack, new { id = track.Id });
        //     return Created(url, track);
        // }
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Tracks })]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if(!await logic.Tracks.RemoveAsync(id)){
                return NotFound();
            }
            return NoContent();
        }
    }
}
