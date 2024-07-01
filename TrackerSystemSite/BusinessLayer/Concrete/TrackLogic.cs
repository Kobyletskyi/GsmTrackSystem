using BusinessLayer.Helpers.Location;
using BusinessLayer.Models;
using BusinessLayer.Parsers;
using DbModels.Entities;
using RepositoriesLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class TrackLogic : ITrackLogic
    {

        public TrackLogic(IUnitOfWorkRepositories repositories)
        {
            _repositories = repositories;
        }

        private IUnitOfWorkRepositories _repositories;

        #region Read

        public IList<DeviceInfo> GetDevicesWithTracks()
        {
            var query = _repositories.Devices.FindAll();
            var result = query.Select(d => new DeviceInfo()
            {
                Description = d.Description,
                IMEI = d.IMEI,
                SoftwareVersion = d.SoftwareVersion,
                Title = d.Title,
                Tracks = d.Tracks.Select(t => new TrackInfo()
                {
                    Id = t.Id,
                    Description = t.Description,
                    Name = t.Name,
                    UniqCreatedTicks = t.UniqCreatedTicks,
                    UpdatedUtcDate = t.UpdatedUtcDate
                })
                .OrderByDescending(t => t.UpdatedUtcDate)
                .ToList()
            })

                .OrderByDescending(d => d.Tracks.Max(t => t.UpdatedUtcDate))
            .ToList();

            return result;
        }

        public IList<GpsResponseEntity> GetTrackPoints(int trackId)
        {
            TrackEntity track = _repositories.Tracks.FindSingle(t => t.Id == trackId);
            if (track != null)
            {
                return track.GpsLocations.ToList();
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public IList<GpsResponseEntity> GetNewTrackPoints(int trackId, float utcTime)
        {
            IList<GpsResponseEntity> result = _repositories.GpsResponses.Find(p => p.TrackId == trackId && p.UtcTime > utcTime).ToList();
            return result;
        }

        public IList<GpsResponseEntity> GetTrackPoints(int trackId, long trackUniqTicks)
        {
            TrackEntity track = _repositories.Tracks.FindByKeys(trackId, trackUniqTicks);
            if (track != null)
            {
                return track.GpsLocations.ToList();
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public IList<GpsResponseEntity> SearchPoints(Expression<Func<GpsResponseEntity, bool>> expFunc = null)
        {
            IEnumerable<GpsResponseEntity> points = null;

            if (expFunc != null)
            {
                points = _repositories.GpsResponses.Find(expFunc);
            }
            else
            {
                points = _repositories.GpsResponses.FindAll();
            }

            return points.ToList();
        }


        #endregion


        #region Update

        //'012207000000015,2016/3/11 21:33:11,193416.00,0000.00000,N,00000.00000,E,0.000,NF,5303302,3750001,0.000,0.00,0.000,,99.99,99.99,99.99,0,0,0*20'
        public void ProccessDeviceGpsData(string dataStr)
        {
            //$PUBX,00,hhmmss.ss,Latitude,N,Longitude,E,AltRef,NavStat,Hacc,Vacc,SOG,COG,Vvel,ageC,HDOP,VDOP,TDOP,GU,RU,DR,*cs<CR><LF>
            var p = new SeparValuesParser<GpsResponse>();
            var obj = p.Parse(dataStr);

            DeviceEntity device = _repositories.Devices.Find(d => d.IMEI == obj.DeviceImei).FirstOrDefault();
            if (device == null)
            {
                device = new DeviceEntity()
                {
                    IMEI = obj.DeviceImei,
                    SoftwareVersion = "dgfhdfg",
                    Title = "dzfshdfh",
                    Description = "sdghdfs"
                };
            }
            TimeZoneInfo dtz = TimeZoneInfo.CreateCustomTimeZone(obj.DeviceImei, new TimeSpan(obj.TimeZone, 0, 0), obj.DeviceImei, obj.DeviceImei);
            DateTime dateUtc = TimeZoneInfo.ConvertTimeToUtc(obj.TrackLocalDateTime, dtz);
            long trackUtcTicks = dateUtc.Ticks;
            TrackEntity track = device.Tracks.FirstOrDefault(t => t.UniqCreatedTicks == trackUtcTicks);
            if (track == null)
            {
                track = new TrackEntity()
                {
                    UniqCreatedTicks = trackUtcTicks,
                    Name = dateUtc.ToString(),
                    Description = dateUtc.ToString(),
                };
                device.Tracks.Add(track);
            }

            GeoCoordinate coord = CoordinatesExtention.ConvertNmeaToDeg(obj.Longitude, obj.Latitude);

            var entity = new GpsResponseEntity()
            {
                UtcTime = obj.UtcTime,
                Latitude = coord.Latitude,
                NorthOrSouth = obj.NorthOrSouth,
                Longitude = coord.Longitude,
                EastOrWest = obj.EastOrWest,
                AltRef = obj.AltRef,
                NavigationStatus = obj.NavigationStatus,
                HorizontalAccuracy = obj.HorizontalAccuracy,
                VerticalAccuracy = obj.VerticalAccuracy,
                SpeedOverGround = obj.SpeedOverGround,
                CourseOverGround = obj.CourseOverGround,
                VerticalVelocity = obj.VerticalVelocity,
                ageC = obj.ageC,
                HorizontalDilutionOfPrecision = obj.HorizontalDilutionOfPrecision,
                VerticalDilutionOfPrecision = obj.VerticalDilutionOfPrecision,
                TimeDilutionOfPrecision = obj.TimeDilutionOfPrecision,
                NumberGPSSatellites = obj.NumberGPSSatellites,
                NumberGLONASSSatellites = obj.NumberGLONASSSatellites
            };

            track.GpsLocations.Add(entity);

            if (device.Id == default(int))
            {
                _repositories.Devices.Add(device);
            }
            else
            {
                _repositories.Devices.Update(device);
            }
            _repositories.SaveChanges();

        }

        #endregion
        
        public IList<GpsResponseEntity> GetAllPoints()
        {
            return SearchPoints();
        }

        public IList<TrackEntity> SearchTrackes(Expression<Func<TrackEntity, bool>> expFunc = null)
        {
            IEnumerable<TrackEntity> tracks = null;

            if (expFunc != null)
            {
                tracks = _repositories.Tracks.Find(expFunc);
            }
            else
            {
                tracks = _repositories.Tracks.FindAll();
            }

            return tracks.ToList();
        }

        #region Update

        public void SetGeolocation(Guid deviceId, Guid trackId, GeoCoordinate coordinate)
        {
            //if (coordinate != null)
            //{
            //    var geoLocation = new GeoLocationEntity();
            //    geoLocation.Coordinate = coordinate;
            //    geoLocation.DeviceId = deviceId;
            //    geoLocation.TrackId = trackId;

            //    _repositories.GeoLocations.Add(geoLocation);
            //}
            //else
            //{
            //    throw new Exception();
            //}
        }
        public void SetServiceCell(Int32 Arfcn, Byte RxLevel, Byte Bsic, Int32 CellId, Int32 Mcc, Int32 Mnc, Int32 Lac, byte Ber, byte RxLevAccessMin, byte MsTxpwrMaxCch, byte Ta)
        {
            var entity = new NeighborCellInfo();
            entity.Arfcn = Arfcn;
            entity.RxLevel = RxLevel;
            entity.Bsic = Bsic;
            entity.CellId = CellId;
            entity.Mcc = Mcc;
            entity.Mnc = Mnc;
            entity.Lac = Lac;
            entity.Ber = Ber;
            entity.RxLevAccessMin = RxLevAccessMin;
            entity.MsTxpwrMaxCch = MsTxpwrMaxCch;
            entity.Ta = Ta;
            entity.CreatedUtcDate = DateTime.Now;

            _repositories.NeighborCellInfo.Add(entity);
            _repositories.SaveChanges();
        }
        public void SetNeighborCell(Int32 Arfcn, Byte RxLevel, Byte Bsic, Int32 CellId, Int32 Mcc, Int32 Mnc, Int32 Lac)
        {
            var entity = new NeighborCellInfo();
            entity.Arfcn = Arfcn;
            entity.RxLevel = RxLevel;
            entity.Bsic = Bsic;
            entity.CellId = CellId;
            entity.Mcc = Mcc;
            entity.Mnc = Mnc;
            entity.Lac = Lac;
            entity.CreatedUtcDate = DateTime.Now;

            _repositories.NeighborCellInfo.Add(entity);
            _repositories.SaveChanges();
        }
        public void SetGeolocation(double Longitude, double Latitude, double Accuracy)
        {
            GeoLocationByCellsEntity ent = new GeoLocationByCellsEntity()
            {
                Accuracy = Accuracy,
                Latitude = Latitude,
                Longitude = Longitude,
                CreatedUtcDate = DateTime.UtcNow
            };
            //CreatePoint(ent);
        }

        public void GpsResp(GpsResponseEntity gps)
        {
            _repositories.GpsResponses.Add(gps);
            _repositories.SaveChanges();
        }

        #endregion
    }
}
