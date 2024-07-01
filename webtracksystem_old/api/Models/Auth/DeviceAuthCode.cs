using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class DeviceAuthCode: DeviceIdenty
    {
        [Required]
        public int Code { get; set; }
    }
}