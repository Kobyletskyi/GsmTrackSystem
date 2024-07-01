/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Providers.Auth;
using BusinessLayer;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Api.Extentions;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace Api.Controllers
{
    
    public class SamplesController : BaseApiController
    {
        public SamplesController(ILoggerFactory loggerFactory, IBusinessLogic logic)
            : base(loggerFactory, logic)
        { }

        [HttpGet]
        ToDo[PagedResult]
        public IActionResult GetEntities(ResourceParameters parameters){
            var prevPage = data.HasPrev ? CreateResourceUri("", parameters, ResourceUriType.PreviousPage): null;
            var nextPage = data.hasNext ? CreateResourceUri("", parameters, ResourceUriType.NextPage):null;

            var paginationMetadata = new {
                totalCount = data.Count,
                pageSize = data.pageSize,
                curentPage = data.curentPage,
                totalPages = data.totalPages,
                previousPageLink = prevPage,
                nextPageLink = nextPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(Collection.ShapeData<object>(parameters.Fields));
        }

        [HttpGet("{id}")]
        public IActionResult GetEntity([FromRoute]int id, [FromQuery]string fields){
            return Ok();
        }

        [HttpPatch]
        public IActionResult PatchEntity([FromBody]JsonPatchDocument<object> patchDoc){
            patchDoc.ApplyTo(new object());
            return Ok();
        }
        
    }
    public class SamplesCollectionController : BaseApiController{
        public SamplesCollectionController(ILoggerFactory loggerFactory, IBusinessLogic logic)
            : base(loggerFactory, logic)
        { }

        [HttpPost]
        public IActionResult CreateEntityCollection([FromBody]IEnumerable<object> collection){
            return Ok();
        }

        [HttpGet("({ids})")]
        public IActionResult GetEntityCollection(
            [ModelBinder(BinderType=typeof(ArrayModelBinder))]IEnumerable<int> ids)
        {
           return Ok();;
        }
    }
}
*/