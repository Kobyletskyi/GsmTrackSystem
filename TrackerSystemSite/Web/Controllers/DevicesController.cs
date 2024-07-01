using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BusinessLayer;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.Web;
using BusinessLayer.Models;
using DbModels.Entities;
using System.Web.Http.Filters;
using Web.Helpers.Auth;

namespace Web.Controllers
{
    [CookieTokenAuthenticate]
    public class DevicesController : BaseApiController
    {
        public DevicesController(IBusinessLogic logic) : base(logic)
        {

        }
        
        public IEnumerable<Device> Get()
        {
            return _logic.DeviceLogic.SearchDevices();
        }
        
        public Device Get(int id)
        {
            return _logic.DeviceLogic.GetDeviceById(id);
        }

        [HttpGet]
        public DeviceCode Code(int id)
        {
            return _logic.DeviceLogic.GetDeviceCode(id);
        }

        public IHttpActionResult Post(Device device)
        {
            if (AuthUserId.HasValue)
            {
                device.OwnerId = AuthUserId.Value;
                device.CreatorId = AuthUserId.Value;
                device = _logic.DeviceLogic.CreateDevice(device);
                var url = Url.Route("DefaultApi", new { controller = "devices", action = "get", id = device.Id });
                return Created<Device>(url, device);
            }
            return Unauthorized();
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]Device device)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }

    }
}
