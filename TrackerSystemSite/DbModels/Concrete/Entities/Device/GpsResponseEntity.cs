using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.Entities
{
    [Table("GpsResponse")]
    public class GpsResponseEntity : BaseEntity<int>
    {
        [Required]
        public float UtcTime { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public char NorthOrSouth { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public char EastOrWest { get; set; }
        public float AltRef { get; set; }
        [Required]
        public string NavigationStatus { get; set; }
        [Required]
        public float HorizontalAccuracy { get; set; }        
        public float VerticalAccuracy { get; set; }       
        public float SpeedOverGround { get; set; }        
        public float CourseOverGround { get; set; }        
        public float VerticalVelocity { get; set; }
        public int ageC { get; set; }
        public float HorizontalDilutionOfPrecision { get; set; }
        public float VerticalDilutionOfPrecision { get; set; }
        public float TimeDilutionOfPrecision { get; set; }
        public int NumberGPSSatellites { get; set; }
        public float NumberGLONASSSatellites { get; set; }

        [Required, ForeignKey("Track"), Column(Order = 1)]
        public int TrackId { get; set; }
        public virtual TrackEntity Track { get; set; }
    }
}
