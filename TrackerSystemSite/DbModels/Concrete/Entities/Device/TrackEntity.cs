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
    [Table("Tracks")]
    public class TrackEntity : BaseEntity<int>
    {
        public TrackEntity()
        {
            GpsLocations = new Collection<GpsResponseEntity>();
            LocationsByCells = new Collection<GeoLocationByCellsEntity>();
        }
        
        [Required, Index("IX_TRACK_TICKS", IsUnique = true)]
        public long UniqCreatedTicks { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, ForeignKey("Device")]
        public int DeviceId { get; set; }
        public virtual DeviceEntity Device { get; set; }

        //[ForeignKey("Creator")/*, Required*/]
        //public int CreatorId { get; set; }
        //public virtual UserEntity Creator { get; set; }
        
        public virtual ICollection<GpsResponseEntity> GpsLocations { get; set; }

        public virtual ICollection<GeoLocationByCellsEntity> LocationsByCells { get; set; }
    }
}
