using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    public class UsersController : AllAuthApiController
    {
        public UsersController(ILoggerFactory loggerFactory, IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService)
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
        
        // [HttpGet(Name = RouteNames.GetUsers)]
        // [Authorize(Policy = "Admin")]
        // public async Task<IActionResult> GetAll(CollectionParameters parameters)
        // {
        //     var paging = parameters as CollectionParameters;
        //     var data = await _logic.Users.GetUsersAsync(paging);
        //     AddPaginationMetadata<User>(data, parameters, RouteNames.GetUsers);
        //     return Ok(data.ShapeCollection<User>(parameters.Fields));
        // }

        [HttpGet("{id:int:min(1)}", Name = RouteNames.GetUser)]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Users })]
        public async Task<IActionResult> GetById([Required,FromRoute]int id, [FromQuery]string fields)
        {
            var user = await logic.Users.GetUserAsync(id, fields);
            if(user == null)
                return NotFound();
            return Ok(user.ShapeObject<User>(fields));
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Users })]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if(!await logic.Users.RemoveUserAsync(id)){
                return NotFound();
            }
            return NoContent();
        }
    }
    
}
