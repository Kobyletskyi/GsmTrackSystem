using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class DeviceForUpdate : DeviceForManipulation
    {
        [Required]
        [MaxLength(Constants.TITLE_MAX_LENGTH)]
        public override string Title { get; set; }

        [Required]
        [MaxLength(Constants.DESCRIPTION_MAX_LENGTH)]
        public override string Description { get; set; }

        [Required]
        public override string SoftwareVersion { get; set; }
    }
}