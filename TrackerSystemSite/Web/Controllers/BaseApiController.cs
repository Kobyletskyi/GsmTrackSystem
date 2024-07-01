using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLayer;
using Web.Helpers.Auth;
using System.Security.Claims;

namespace Web.Controllers
{
    //[TokenAuthenticate]
    public class BaseApiController : ApiController
    {
        protected IBusinessLogic _logic;

        public BaseApiController(IBusinessLogic logic)
            : base()
        {
            this._logic = logic;
        }

        public int? AuthUserId {
            get
            {
                int id;
                return (User.Identity != null && Int32.TryParse(User.Identity.Name, out id)) ? (int?)id: null;
            }
        }
    }
}
