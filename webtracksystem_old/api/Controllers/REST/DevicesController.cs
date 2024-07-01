#region using
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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
#endregion

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy.User)]
    public class DevicesController : AllAuthApiController
    {
        public DevicesController(
            ILoggerFactory loggerFactory, 
            IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService) 
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
        
        [HttpGet(Name = RouteNames.GetDevices)]
        [Authorize(Policy.Admin)]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Devices })]
        [ProducesResponseType(typeof(IEnumerable<Device>), (int)HttpStatusCode.OK)]
        #endregion
        public async Task<IActionResult> GetAll(CollectionParameters parameters)
        {
            PagedList<Device> data = await logic.Devices.GetDevicesAsync(parameters);           
            AddPaginationMetadata<Device>(data, parameters, RouteNames.GetDevices);
            return Ok(data.ShapeCollection<Device>(parameters.Fields));
        }
        [HttpGet("{id}", Name= RouteNames.GetDevice)]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Devices })]
        [ProducesResponseType(typeof(Device), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        #endregion
        public async Task<IActionResult> GetById([Required,FromRoute]int id, [FromQuery]string fields)
        {
            Device device = await logic.Devices.GetDeviceAsync(id, fields);
            if(device == null)
                return NotFound();
            return Ok(device.ShapeObject<Device>(fields));
        }
        [HttpPost]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Devices })]
        [ProducesResponseType(typeof(Device), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), (int)HttpStatusCode.BadRequest)]
        #endregion
        public async Task<IActionResult> Post([FromBody]DeviceForCreation device)
        {
            if(!ModelState.IsValid){
                return new UnprocessableEntity(ModelState);
            }
            var newDevice = Mapper.Map<NewDevice>(device);
            var result = await logic.Devices.CreateAsync(newDevice);
            var url = Url.RouteUrl(RouteNames.GetDevice, new { id = result.Id });
            return Created(url, result);
        }
        [HttpPut("{id}", Name= RouteNames.PutDevice)]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Devices })]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(Device), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), (int)HttpStatusCode.BadRequest)]
        #endregion
        public async Task<IActionResult> Put([Required,FromRoute]int id,[FromBody]DeviceForUpdate deviceUpdates)
        {
            if(!ModelState.IsValid){
                return new UnprocessableEntity(ModelState);
            }
            var device = Mapper.Map<DeviceUpdate>(deviceUpdates);
            try {
                var result = await logic.Devices.UpdateAsync(id, device);
                return NoContent();
            } catch (/*NotFound*/Exception) {
                return NotFound();
            }
        }
        [HttpPatch("{id}", Name= RouteNames.PatchDevice)]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Devices })]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(Device), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), (int)HttpStatusCode.BadRequest)]
        #endregion
        public async Task<IActionResult> Patch([Required,FromRoute]int id,
            [FromBody]JsonPatchDocument<DeviceForCreation> patchDoc)
        {
            if(patchDoc == null){
                return BadRequest();
            }
            Device device = await logic.Devices.GetDeviceAsync(id);
            if(device == null){
                DeviceForCreation deviceDto = new DeviceForCreation();
                patchDoc.ApplyTo(deviceDto, ModelState);
                //ToDo: implement Upsert deviceDto.Id = id;
                TryValidateModel(deviceDto);
                if(!ModelState.IsValid){
                    return new UnprocessableEntity(ModelState);
                }
                NewDevice deviceToAdd = Mapper.Map<NewDevice>(deviceDto);
                //deviceToAdd.Id = id;
                var createResult = await logic.Devices.CreateAsync(deviceToAdd);
                var url = Url.RouteUrl(RouteNames.GetDevice, new { id = createResult.Id });
                return Created(url, createResult);
            }
            
            var deviceForPatch = Mapper.Map<DeviceForUpdate>(device);
            var updatePatchOperations =  Mapper.Map<List<Operation<DeviceForUpdate>>>(patchDoc.Operations); 
            var updatePatchDoc = new JsonPatchDocument<DeviceForUpdate>(updatePatchOperations, patchDoc.ContractResolver);
            updatePatchDoc.ApplyTo(deviceForPatch, ModelState);
            TryValidateModel(deviceForPatch);
            if(!ModelState.IsValid){
                return new UnprocessableEntity(ModelState);
            }
            DeviceUpdate updates = new DeviceUpdate();
            Mapper.Map(deviceForPatch, updates);
            var result = await logic.Devices.UpdateAsync(id, updates);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        #region Swagger
        [SwaggerOperation(Tags = new[] { SwaggerGroups.Devices })]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        #endregion
        public async Task<IActionResult> Delete([Required,FromRoute]int id)
        {
            if(!await logic.Devices.RemoveAsync(id)){
                return NotFound();
            }
            return NoContent();
        }
    }
}
