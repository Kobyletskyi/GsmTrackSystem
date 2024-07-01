using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Helpers.GoogleApi
{
    public class GeolocationApiResponse
    {
        /// <summary>
        /// The user’s estimated latitude and longitude, in degrees. Contains one lat and one lng subfield.
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// The accuracy of the estimated location, in meters. This represents the radius of a circle around the given location
        /// </summary>
        public float accuracy { get; set; }

        public class Location
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }
    }
}