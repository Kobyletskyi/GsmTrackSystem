using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class DeviceForCreation : DeviceForPostWithoutUser
    {
        [Range(1, int.MaxValue)]
        public int OwnerId { get; set; }
    }
}