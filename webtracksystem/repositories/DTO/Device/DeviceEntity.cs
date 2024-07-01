using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Dto
{
    public class DeviceEntity : BaseEntity<int>
    {
        public DeviceEntity()
        {
            Tracks = new Collection<TrackEntity>();
            AuthCode = null;
        }
        
        //[Required, Index("IX_IMEI", IsUnique = true)]
        //[MaxLength(20)]
        public string IMEI { get; set; }

        public AuthCodeEntity AuthCode { get; set; }

        //[Required]
        //[ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public UserEntity Owner { get; set; }

        //[Required]
        //[ForeignKey("Creator")]
        //public int CreatorId { get; set; }
        //public virtual UserEntity Creator { get; set; }

        //[Required]
        public string Title { get; set; }

        //[Required]
        public string Description { get; set; }

        //[Required]
        public string SoftwareVersion { get; set; }

        public virtual ICollection<TrackEntity> Tracks { get; set; }
    }
}
