using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class DeviceForPostWithoutUser : DeviceForUpdate
    {        
        [Required]
        [MaxLength(Constants.IMEI_MAX_LENGTH)]
        public string IMEI { get; set; }
    }
}
