using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Helpers.GoogleApi
{
    public class TimeZoneApiResponse
    {
        /// <summary>
        /// in seconds
        /// </summary>
        public int dstOffset { get; set; }
        /// <summary>
        /// in seconds
        /// </summary>
        public int rawOffset { get; set; }
        public string timeZoneId { get; set; }
        public string timeZoneName { get; set; }
        public ApiResponseStatus status { get; set; }
        public string error_message { get; set; }
        
        public enum ApiResponseStatus
        {
            OK, //indicates that the request was successful.
            INVALID_REQUEST, //indicates that the request was malformed.
            OVER_QUERY_LIMIT, //indicates the requestor has exceeded quota.
            REQUEST_DENIED, //indicates that the the API did not complete the request. Confirm that the request was sent over HTTPS instead of HTTP.
            UNKNOWN_ERROR, //indicates an unknown error.
            ZERO_RESULTS //indicates that no time zone data could be found for the specified position or time. Confirm that the request is for a location on land, and not over water.
        }
    }    
}