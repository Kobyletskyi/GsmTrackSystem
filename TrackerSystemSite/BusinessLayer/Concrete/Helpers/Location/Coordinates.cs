using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace BusinessLayer.Helpers.Location
{
    public static class CoordinatesExtention
    {
        public static GeoCoordinate ConvertNmeaToDeg(this GeoCoordinate coord)
        {
            return ConvertNmeaToDeg(coord.Longitude, coord.Latitude);
        }

        public static GeoCoordinate ConvertNmeaToDeg(double longitude, double latitude)
        {
            int deg = (int)latitude / 100;
            decimal min = (decimal)latitude - (deg * 100);
            decimal lat = (deg + (min / 60));

            deg = (int)longitude / 100;
            min = (decimal)longitude - (deg * 100);
            decimal lon = (deg + (min / 60));

            return new GeoCoordinate((double)lat, (double)lon);
        }
    }
}