using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Parsers.Attributes;

namespace BusinessLayer.Models
{
    [Separator(',')]
    public class GpsResponse
    {
        [Index(0)]
        public string DeviceImei { get; set; }
        [Index(1)]
        public DateTime TrackLocalDateTime { get; set; }
        [Index(2)]
        public byte TimeZone { get; set; }
        [Index(3)]
        public float UtcTime { get; set; }
        [Index(4)]
        public double Latitude { get; set; }
        [Index(5)]
        public char NorthOrSouth { get; set; }
        [Index(6)]
        public double Longitude { get; set; }
        [Index(7)]
        public char EastOrWest { get; set; }
        [Index(8)]
        public float AltRef { get; set; }
        [Index(9)]
        public string NavigationStatus { get; set; }
        [Index(10)]
        public float HorizontalAccuracy { get; set; }
        [Index(11)]
        public float VerticalAccuracy { get; set; }
        [Index(12)]
        public float SpeedOverGround { get; set; }
        [Index(13)]
        public float CourseOverGround { get; set; }
        [Index(14)]
        public float VerticalVelocity { get; set; }
        [Index(15)]
        public int ageC { get; set; }
        [Index(16)]
        public float HorizontalDilutionOfPrecision { get; set; }
        [Index(17)]
        public float VerticalDilutionOfPrecision { get; set; }
        [Index(18)]
        public float TimeDilutionOfPrecision { get; set; }
        [Index(19)]
        public int NumberGPSSatellites { get; set; }
        [Index(20)]
        public float NumberGLONASSSatellites { get; set; }
        //[Index(21)]
        //float DR { get; set; }
    }
}