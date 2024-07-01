using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.Net.Http.Headers;

namespace Api.Integration.Tests.Extentions
{
    public static class HttpResponseHeadersExtention {
        public static string GetCookie(this HttpResponseHeaders headers, string name){
            string groupName = "value";
            IEnumerable<string> cookieValues;
            headers.TryGetValues(HeaderNames.SetCookie, out cookieValues);
            var setCookieExists = headers.Contains(HeaderNames.SetCookie);
            if(cookieValues != null && cookieValues.Count() > 0){
                string cookie = cookieValues.First();
                string validValue = string.Format("{0};", cookie);
                string pattern = string.Format("{0}=(?<{1}>.*?);", name, groupName);
                Match match = Regex.Match(validValue, pattern);
                Group valueGroup = match.Groups.FirstOrDefault(g => g.Name == groupName);
                if(valueGroup != null){
                    return valueGroup.Value;
                }
            }
            return null;      
        }
        public static string GetHeaderLocation(this HttpResponseHeaders headers){
            IEnumerable<string> locationHeader;
            if(headers.TryGetValues(HeaderNames.Location, out locationHeader)){
                return locationHeader.FirstOrDefault();
            }
            return null;     
        }
        public static string GetHeaderValue(this HttpResponseHeaders headers, string header){
            IEnumerable<string> headerValues;
            if(headers.TryGetValues(header, out headerValues)){
                return headerValues.FirstOrDefault();
            }
            return null;     
        }
    }
}