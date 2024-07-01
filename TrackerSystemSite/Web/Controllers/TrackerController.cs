using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BusinessLayer;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.Web;
using BusinessLayer.Models;
using DbModels.Entities;
using System.Web.Http.Filters;
using Web.Helpers.Auth;
using System.Net.Http;
using System.Net.Http.Headers;

public class CustomHeaderModule : IHttpModule
{
    public void Init(HttpApplication context)
    {
        context.PreSendRequestHeaders += OnPreSendRequestHeaders;
    }

    public void Dispose() { }

    void OnPreSendRequestHeaders(object sender, EventArgs e)
    {
        //HttpContext.Current.Response.Headers.Clear();
        foreach (var key in HttpContext.Current.Response.Headers.AllKeys)
        {
            HttpContext.Current.Response.Headers.Remove(key);
        }
    }
}

namespace Web.Controllers
{
    [TokenAuthenticate]
    public class TrackerController : BaseApiController
    {
        public TrackerController(IBusinessLogic logic) : base(logic)
        {
        }

        // GET api/values
        // POST api/values
        //public void Post(DeviceCellsInfo info)
        //{
        //    _logic.SetServiceCell(info.Arfcn, info.RxLevel, info.Bsic, info.CellID, info.Mcc, info.Mnc, info.Lac, info.Ber, info.RxLevAccessMin, info.MsTxpwrMaxCch, info.Ta);
        //    info.cells.ForEach(v => _logic.SetNeighborCell(v.Arfcn, v.RxLevel, v.Bsic, v.CellId, v.Mcc, v.Mnc, v.Lac));
        //    var gP = new
        //    {
        //        homeMobileCountryCode = info.Mcc,
        //        homeMobileNetworkCode = info.Mnc,
        //        radioType = "gsm",
        //        carrier = "MTS+UKR",
        //        considerIp = "false",
        //        cellTowers = info.cells.Select(v => (object)new
        //        {
        //            cellId = v.CellId,
        //            locationAreaCode = v.Lac,
        //            mobileCountryCode = v.Mcc,
        //            mobileNetworkCode = v.Mnc,
        //            //signalStrength = (-110 + v.RxLevel)
        //        }).ToList()
        //    };
        //    gP.cellTowers.Add((object)new
        //    {
        //        cellId = info.CellID,
        //        locationAreaCode = info.Lac,
        //        mobileCountryCode = info.Mcc,
        //        mobileNetworkCode = info.Mnc,
        //        //signalStrength = (-110 + info.RxLevel),
        //        timingAdvance = info.Ta
        //    });


        //    var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/geolocation/v1/geolocate?key=AIzaSyAhVxfWmjiKUHT-TmxtBUKwa5TSz3Ox-yg");

        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    string json = js.Serialize(gP);
        //    var data = Encoding.ASCII.GetBytes(json);

        //    request.Method = "POST";
        //    request.ContentType = "application/json";
        //    request.ContentLength = data.Length;

        //    using (var stream = request.GetRequestStream())
        //    {
        //        stream.Write(data, 0, data.Length);
        //    }

        //    var response = (HttpWebResponse)request.GetResponse();

        //    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        //    gResp resp = js.Deserialize<gResp>(responseString);

        //    _logic.SetGeolocation(resp.location.lng, resp.location.lat, resp.accuracy);
        //}

        // PUT api/values/5


        // DELETE api/values/5

        //[TokenAuthenticate(Scopes.Device)]
        [HttpPost]
        public void Point([FromBody]/*to get single string*/ string str)
        {
            _logic.TrackLogic.ProccessDeviceGpsData(str);
        }



        //public void setPositio2()
        //{
        //    _logic.TrackLogic.ProccessDeviceGpsData("111107000000015,2016/3/11 21:33:11,1,193416.00,0000.00000,N,00000.00000,E,0.000,NF,5303302,3750001,0.000,0.00,0.000,,99.99,99.99,99.99,0,0,0*20");
        //}

        [HttpGet]
        public string Time(DeviceCellsInfo cellInfo)
        {
            TimeZoneInfo.TransitionTime transitionStart = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 3, 0, 0), 3, 5, DayOfWeek.Sunday);
            TimeZoneInfo.TransitionTime transitionEnd = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 4, 0, 0), 10, 5, DayOfWeek.Sunday);
            TimeZoneInfo.AdjustmentRule adjustmentRule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(DateTime.MinValue.Date, DateTime.MaxValue.Date, new TimeSpan(1, 0, 0), transitionStart, transitionEnd);
            TimeZoneInfo.AdjustmentRule[] adjustmentRules = { adjustmentRule };
            TimeSpan offset = new TimeSpan(2, 0, 0);
            var customTz = TimeZoneInfo.CreateCustomTimeZone("id", offset, "displayName", "standartDisplayName", "daylightDisplayName", adjustmentRules);

            var date = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, customTz);

            return date.ToString("yy'/'MM'/'dd,HH:mm:sszz");
        }

        [HttpGet]
        public IHttpActionResult GpsAssist()
        {
            string url = "http://online-live1.services.u-blox.com/GetOnlineData.ashx?token=07txQ58CnEWPmLIdS15V9Q;gnss=gps;datatype=eph,alm,aux,pos;filteronpos;format=aid";
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpRequest.Method = "GET";
            WebResponse res = httpRequest.GetResponse();
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(res.GetResponseStream())
            };
            result.Content.Headers.ContentLength = long.Parse(res.Headers.Get("Content-Length"));
            result.Content.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse(res.Headers.Get("Content-Disposition"));
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(res.Headers.Get("Content-Type"));

            return ResponseMessage(result);
        }

    }
}
