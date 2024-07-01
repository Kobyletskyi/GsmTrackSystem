using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Web.Controllers
{
    public class HomeController : ApiController
    {
        // GET: Home
        [HttpGet]
        public HttpResponseMessage Index()
        {
            var path = "~/Default.html";
            var response = new HttpResponseMessage();
            response.Content = new StringContent(File.ReadAllText(HttpContext.Current.Server.MapPath(path)));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
