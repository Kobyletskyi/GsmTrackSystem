using DbModels.Entities;
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
    [Table("GeoLocationByCells")]
    public class GeoLocationByCellsEntity : BaseEntity<int>
    {        
        public GeoLocationByCellsEntity()
        {
            CellInfos = new Collection<NeighborCellInfo>();
        }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Accuracy { get; set; }

        [Required, ForeignKey("Track"), Column(Order = 1)]
        public int TrackId { get; set; }
        public virtual TrackEntity Track { get; set; }

        public virtual ICollection<NeighborCellInfo> CellInfos { get; set; }

    }
}
