using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.Entities
{
    [Table("Devices")]
    public class DeviceEntity : BaseEntity<int>
    {
        public DeviceEntity()
        {
            Tracks = new Collection<TrackEntity>();
            AuthCode = null;
        }
        
        [Required, Index("IX_IMEI", IsUnique = true)]
        [MaxLength(20)]
        public string IMEI { get; set; }

        public virtual AuthCodeEntity AuthCode { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public virtual UserEntity Owner { get; set; }

        [Required]
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        public virtual UserEntity Creator { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        //[Required]
        public string SoftwareVersion { get; set; }

        public virtual ICollection<TrackEntity> Tracks { get; set; }
    }
}
