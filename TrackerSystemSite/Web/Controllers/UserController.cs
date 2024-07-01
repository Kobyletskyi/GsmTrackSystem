using BusinessLayer;
using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Controllers
{
    public class UserController : BaseApiController
    {
        public UserController(IBusinessLogic logic) : base(logic)
        {

        }
        // GET: api/User
        public IEnumerable<Account> Get()
        {
            return _logic.UserLogic.GetUsers();
        }

        // GET: api/User/5
        public Account Get(int id)
        {
            return _logic.UserLogic.FindById(id);
        }
        
    }
}
