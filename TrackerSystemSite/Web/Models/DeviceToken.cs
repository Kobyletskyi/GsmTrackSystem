using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DeviceToken: DeviceIdenty
    {
        [Required]
        public string refreshToken { get; set; }
    }
}