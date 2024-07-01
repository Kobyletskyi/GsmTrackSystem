using System;
using BusinessLayer.Parsers.Attributes;
using Repositories.Dto;

namespace BusinessLayer.Models
{
    public class GpsLocation
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public float UtcTime { get; set; }
        public double Latitude { get; set; }
        public char NorthOrSouth { get; set; }
        public double Longitude { get; set; }
        public char EastOrWest { get; set; }
        public string NavigationStatus { get; set; }
        public float HorizontalAccuracy { get; set; }
        public float VerticalAccuracy { get; set; }
        public float SpeedOverGround { get; set; }
        public float CourseOverGround { get; set; }
    }
}