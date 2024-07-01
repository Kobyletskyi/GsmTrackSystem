using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Dto
{
    public class GeoLocationByCellsEntity : BaseEntity<int>
    {        
        public GeoLocationByCellsEntity()
        {
            CellInfos = new Collection<NeighborCellInfoEntity>();
        }
        //[Required]
        public double Longitude { get; set; }
        //[Required]
        public double Latitude { get; set; }
        //[Required]
        public double Accuracy { get; set; }
        //[Required, ForeignKey("Track"), Column(Order = 1)]
        public int TrackId { get; set; }
        public virtual TrackEntity Track { get; set; }
        public virtual ICollection<NeighborCellInfoEntity> CellInfos { get; set; }
    }
}
