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
    public class PointsController : AllAuthApiController
    {
        public PointsController(
            ILoggerFactory loggerFactory, 
            IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService) 
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
        //To0 many data not good for perfomance
        // [HttpGet(Name = RouteNames.GetPoints)]
        // [Authorize(Policy.Admin)]
        // public async Task<IActionResult> GetAll(CollectionParameters parameters)
        // {
        //     var pager = parameters as CollectionParameters;
        //     PagedList<GpsLocation> data = await _logic.Tracks.GetPointsAsync(pager);           
        //     AddPaginationMetadata<GpsLocation>(data, parameters, RouteNames.GetDevices);
        //     return Ok(data.ShapeCollection<GpsLocation>(parameters.Fields));
        // }
        [HttpGet("{id}", Name= RouteNames.GetPoint)]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Points })]
        public async Task<IActionResult> GetById([Required,FromRoute]int id, [FromQuery]string fields)
        {
            GpsLocation point = await logic.Tracks.GetPointAsync(id, fields);
            if(point == null)
                return NotFound();
            return Ok(point.ShapeObject<GpsLocation>(fields));
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Points })]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if(!await logic.Tracks.RemovePointAsync(id)){
                return NotFound();
            }
            return NoContent();
        }
    }
}
