using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BusinessLayer;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using BusinessLayer.Models;
using DbModels.Entities;
using System.Web.Http.Filters;
using Web.Helpers.Auth;

namespace Web.Controllers
{
    [CookieTokenAuthenticate]
    public class TrackController : BaseApiController
    {
        public TrackController(IBusinessLogic logic) : base(logic)
        {

        }
        [HttpGet]
        public IEnumerable<DeviceInfo> Get()
        {
           return _logic.TrackLogic.GetDevicesWithTracks();
        }

        [HttpGet]
        public IEnumerable<GpsResponseEntity> GetPoints()
        {
            var response = _logic.TrackLogic.SearchPoints()
                .Select(x => new GpsResponseEntity
                {
                    UtcTime = x.UtcTime,
                    ageC = x.ageC,
                    AltRef = x.AltRef,
                    CourseOverGround = x.CourseOverGround,
                    CreatedUtcDate = x.CreatedUtcDate,
                    UpdatedUtcDate = x.UpdatedUtcDate,
                    EastOrWest = x.EastOrWest,
                    HorizontalAccuracy = x.HorizontalAccuracy,
                    HorizontalDilutionOfPrecision = x.HorizontalDilutionOfPrecision,
                    Id = x.Id,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    NavigationStatus = x.NavigationStatus,
                    NorthOrSouth = x.NorthOrSouth,
                    NumberGPSSatellites = x.NumberGPSSatellites,
                    SpeedOverGround = x.SpeedOverGround,
                    TimeDilutionOfPrecision = x.TimeDilutionOfPrecision,
                    TrackId = x.TrackId
                }).Where(x => !x.NavigationStatus.Contains("NF")).OrderBy(x => x.CreatedUtcDate).ToList(); //.Where(x => x.CreatedDate.Date == DateTime.Now.Date);
            return response;

        }
        [HttpGet]
        public IEnumerable<GpsResponseEntity> GetPoints(int trackId)
        {
            var response = _logic.TrackLogic.GetTrackPoints(trackId)
                .Select(x => new GpsResponseEntity
                {
                    UtcTime = x.UtcTime,
                    ageC = x.ageC,
                    AltRef = x.AltRef,
                    CourseOverGround = x.CourseOverGround,
                    CreatedUtcDate = x.CreatedUtcDate,
                    UpdatedUtcDate = x.UpdatedUtcDate,
                    EastOrWest = x.EastOrWest,
                    HorizontalAccuracy = x.HorizontalAccuracy,
                    HorizontalDilutionOfPrecision = x.HorizontalDilutionOfPrecision,
                    Id = x.Id,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    NavigationStatus = x.NavigationStatus,
                    NorthOrSouth = x.NorthOrSouth,
                    NumberGPSSatellites = x.NumberGPSSatellites,
                    SpeedOverGround = x.SpeedOverGround,
                    TimeDilutionOfPrecision = x.TimeDilutionOfPrecision,
                    TrackId = x.TrackId
                })/*.Where(x => !x.NavigationStatus.Contains("NF"))*/.OrderBy(x => x.CreatedUtcDate)
                .ToList(); //.Where(x => x.CreatedDate.Date == DateTime.Now.Date);
            return response;

        }
        [HttpGet]
        public IEnumerable<GpsResponseEntity> GetNew(int trackId, float utcTime)
        {
            var response = _logic.TrackLogic.GetNewTrackPoints(trackId, utcTime)
                .Select(x => new GpsResponseEntity
                {
                    UtcTime = x.UtcTime,
                    ageC = x.ageC,
                    AltRef = x.AltRef,
                    CourseOverGround = x.CourseOverGround,
                    CreatedUtcDate = x.CreatedUtcDate,
                    UpdatedUtcDate = x.UpdatedUtcDate,
                    EastOrWest = x.EastOrWest,
                    HorizontalAccuracy = x.HorizontalAccuracy,
                    HorizontalDilutionOfPrecision = x.HorizontalDilutionOfPrecision,
                    Id = x.Id,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    NavigationStatus = x.NavigationStatus,
                    NorthOrSouth = x.NorthOrSouth,
                    NumberGPSSatellites = x.NumberGPSSatellites,
                    SpeedOverGround = x.SpeedOverGround,
                    TimeDilutionOfPrecision = x.TimeDilutionOfPrecision,
                    TrackId = x.TrackId
                }).Where(x => !x.NavigationStatus.Contains("NF")).OrderBy(x => x.CreatedUtcDate)
                .ToList(); //.Where(x => x.CreatedDate.Date == DateTime.Now.Date);
            return response;

        }

    }
}
