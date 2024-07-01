using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Helpers.GoogleApi
{
    public class GoogleApi
    {
        private static string key = "AIzaSyAhVxfWmjiKUHT-TmxtBUKwa5TSz3Ox-yg";
        private static string googleMapsApiServer = "https://maps.googleapis.com";
        private static string googleApiServer = "https://www.googleapis.com";


        public class TimeZone
        {
            private string timezoneApiUri = "maps/api/timezone/{outputFormat}";
            private string locationParam = "location";
            private string timestampParam = "timestamp";
            private string keyParam = "key";
            private string outputFormat = "json";

            public TimeZoneApiResponse GetTimeZone()
            {
                var client = new RestClient(googleMapsApiServer);

                var request = new RestRequest(timezoneApiUri, Method.GET);
                request.AddUrlSegment("outputFormat", outputFormat);

                request.AddParameter(locationParam, "49.84067816666666,24.007570333333334"); // adds to POST or URL querystring based on Method
                request.AddParameter(timestampParam, ToTimestamp(DateTime.UtcNow));
                request.AddParameter(keyParam, key);

                IRestResponse<TimeZoneApiResponse> response = client.Execute<TimeZoneApiResponse>(request);
                return response.Data;
            }
            
            private double ToTimestamp(DateTime date)
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                TimeSpan diff = date.ToUniversalTime() - origin;
                return Math.Floor(diff.TotalSeconds);
            }

        }

        public class Geolocation
        {
            private string uri = "geolocation/v1/geolocate?key={keyParam}";

            public GeolocationApiResponse GetLocation()
            {
                var client = new RestClient(googleApiServer);

                var request = new RestRequest(uri, Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-type", "application/json");
                request.AddUrlSegment("keyParam", key);

                request.AddJsonBody(new GeolocationRequest());

                IRestResponse<GeolocationApiResponse> response = client.Execute<GeolocationApiResponse>(request);
                return response.Data;
            }                        
        }
    }
}