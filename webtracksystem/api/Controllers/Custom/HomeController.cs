using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    //[RequireHttps]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            var file = @"Default.html";
            var contentType = "text/html";
            return File(file, contentType);
        }
    }
}