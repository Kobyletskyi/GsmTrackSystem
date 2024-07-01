using System;
using System.Collections.Generic;
using GeoCoordinatePortable;

namespace BusinessLayer.Helpers.Location
{
    public static class CoordinatesExtention
    {
        public static GeoCoordinate ConvertNmeaToDeg(this GeoCoordinate coord)
        {
            return ConvertNmeaToDeg(coord.Longitude, coord.Latitude);
        }

        public static double ConvertNmeaToDeg(double value)
        {
            int deg = (int)value / 100;
            decimal min = (decimal)value - (deg * 100);
            return (double)(deg + (min / 60));
        }
        public static GeoCoordinate ConvertNmeaToDeg(double longitude, double latitude)
        {
            return new GeoCoordinate(ConvertNmeaToDeg(latitude), ConvertNmeaToDeg(longitude));
        }
    }
}