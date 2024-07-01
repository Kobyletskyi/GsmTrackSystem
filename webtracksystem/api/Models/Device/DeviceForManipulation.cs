using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public abstract class DeviceForManipulation
    {
        [MaxLength(Constants.TITLE_MAX_LENGTH)]
        public virtual string Title { get; set; }

        [MaxLength(Constants.DESCRIPTION_MAX_LENGTH)]
        public virtual string Description { get; set; }

        public virtual string SoftwareVersion { get; set; }
    }
}