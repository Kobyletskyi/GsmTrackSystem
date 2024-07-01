#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessLayer.Models;
using Repositories.UnitOfWork;
using System.Security.Cryptography;
using Repositories.Dto;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BusinessLayer.Parsers;
using BusinessLayer.Helpers.Location;
using GeoCoordinatePortable;
using System.Reflection;
using BusinessLayer.Transformation;
#endregion

namespace BusinessLayer
{
    public class DevicesLogic : BaseLogic, IDevicesLogic
    {
        public DevicesLogic(IUnitOfWorkRepositories repositories, IPropertyMappingService propertyMappingService)
            : base(repositories, propertyMappingService){ }

        #region Fetch
        public async Task<bool> DeviceExistsAsync(int id)
        {
            return await Task.Run(() => _repositories.Devices.ExistByKey(id));
        }
        public async Task<PagedList<Device>> GetDevicesAsync(CollectionParameters parameters)
        {
            var include = DataShapingExtensions.IncludeQuery<Device, DeviceEntity>(parameters.Fields, 
            _propertyMappingService.GetPropertyMapping<Device, DeviceEntity>());
            IQueryable<DeviceEntity> devices = _repositories.Devices.FindAll(include);

            var sortedEntities = devices.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<Device, DeviceEntity>());
            return await PagedList<Device>.CreateAsync<DeviceEntity>(sortedEntities, parameters.PageNumber, parameters.PageSize);
        }
        public async Task<Device> GetDeviceAsync(int id, string fields = null)
        {
            var include = DataShapingExtensions.IncludeQuery<Device, DeviceEntity>(fields, 
                _propertyMappingService.GetPropertyMapping<Device, DeviceEntity>());
            var entity = await Task.Run(() => _repositories.Devices.FindSingle(x=>x.Id == id, include));
            return Mapper.Map<Device>(entity); 
        }
        public async Task<Device> GetDeviceAsync(string imei, string fields = null)
        {
            var include = DataShapingExtensions.IncludeQuery<DeviceEntity>(fields);
            var entity = await Task.Run(() => _repositories.Devices.Find(x => x.IMEI == imei, include).FirstOrDefault());
            return Mapper.Map<Device>(entity);
        }
        public async Task<PagedList<DeviceCode>> GetCodesAsync(CollectionParameters parameters)
        {
            var include = DataShapingExtensions.IncludeQuery<AuthCodeEntity>(parameters.Fields);
            IQueryable<AuthCodeEntity> codes = _repositories.AuthCodes.FindAll();
            var sortedEntities = codes.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<DeviceCode, AuthCodeEntity>());
            return await PagedList<DeviceCode>.CreateAsync<AuthCodeEntity>(sortedEntities, parameters.PageNumber, parameters.PageSize);
        }
        public async Task<DeviceCode> CreateCodeAsync(string imei)
        {
            const int codeMinRange = 1000;
            const int codeMaxRange = 10000;
            const int codeExpHours = 24;

            Func<IQueryable<DeviceEntity>, IQueryable<DeviceEntity>> include = 
                d => d.Include(x => x.AuthCode);

            DeviceEntity device = await Task.Run(() => _repositories.Devices.FindSingle(d => d.IMEI == imei, include));
            if (device != null)
            {
                Random rnd = new Random();
                if (device.AuthCode == null)
                {
                    device.AuthCode = new AuthCodeEntity()
                    {
                        Code = rnd.Next(codeMinRange, codeMaxRange),
                        Id = device.Id,
                        IMEI = device.IMEI,
                        Expiration = DateTime.UtcNow.AddHours(codeExpHours)
                    };
                }
                else
                {
                    device.AuthCode.Code = rnd.Next(codeMinRange, codeMaxRange);
                    device.AuthCode.Expiration = DateTime.UtcNow.AddHours(codeExpHours);
                }
                await _repositories.SaveChangesAsync();
                return Mapper.Map<DeviceCode>(device.AuthCode);
            }
            else
            {
                throw new Exception();//not found
            }
        }
        public async Task<DeviceCode> GetCodeAsync(string imei, string fields)
        {
            var include = DataShapingExtensions.IncludeQuery<AuthCodeEntity>(fields);
            var entity = await Task.Run(() => _repositories.AuthCodes
                .FindSingle(c=>c.IMEI == imei, include));
            return Mapper.Map<DeviceCode>(entity);
        }
        public async Task<DeviceCode> GetCodeAsync(int id, string fields)
        {
            var include = DataShapingExtensions.IncludeQuery<AuthCodeEntity>(fields);
            var entity = await Task.Run(() => _repositories.AuthCodes.FindByKey(id, include));
            return Mapper.Map<DeviceCode>(entity);
        }
        public async Task<PagedList<Track>> GetTracksAsync(int deviceId, CollectionParameters parameters){
            var include = DataShapingExtensions.IncludeQuery<TrackEntity>(parameters.Fields);
            var tracks = await Task.Run(() => _repositories.Tracks.Find(t => t.DeviceId == deviceId, include));
            var sortedEntities = tracks.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<Track, TrackEntity>());
            return await PagedList<Track>.CreateAsync<TrackEntity>(tracks, parameters.PageNumber, parameters.PageSize);
        }
        public async Task<bool> ValidateRefreshTokenAsync(string imei, string token)
        {
            Func<IQueryable<DeviceEntity>, IQueryable<DeviceEntity>> include = 
                d => d.Include(x => x.Owner);
            DeviceEntity device = await Task.Run(() => _repositories.Devices.FindSingle(c => c.IMEI == imei, include));
            return (device != null && device.Owner != null && device.Owner.RefreshToken == token);
        }
        #endregion
        #region Create
        public async Task<string> GenerateRefreshTokenAsync(string imei, int code)
        {
            string token = null;
            Func<IQueryable<DeviceEntity>, IQueryable<DeviceEntity>> include = 
                d => d.Include(x => x.AuthCode).Include(x => x.Owner);
            DeviceEntity device = await Task.Run(() => _repositories.Devices.FindSingle(d => d.IMEI == imei, include));
            if (device != null && device.AuthCode != null)
            {
                if (device.AuthCode.Code == code)
                {
                    if (device.AuthCode.Expiration > DateTime.UtcNow)
                    {
                        
                        if (String.IsNullOrWhiteSpace(device.Owner.RefreshToken))
                        {
                            token = generateRefreshToken();
                            device.Owner.RefreshToken = token;
                        }else
                        {
                            token = device.Owner.RefreshToken;
                        }
                    }
                    _repositories.AuthCodes.Remove(device.AuthCode);
                    await _repositories.SaveChangesAsync();
                }
            }
            return token;
        }
        public async Task<Device> CreateAsync(NewDevice device)
        {
            DeviceEntity entity = Mapper.Map<DeviceEntity>(device);
            _repositories.Devices.Add(entity);
            await _repositories.SaveChangesAsync();
            return Mapper.Map<Device>(entity);
        }
        public async Task<GpsLocation> ProccessGpsDataAsync(string dataStr)
        {
            //'012207000000015,2016/3/11 21:33:11,193416.00,0000.00000,N,00000.00000,E,0.000,NF,5303302,3750001,0.000,0.00,0.000,,99.99,99.99,99.99,0,0,0*20'
            //$PUBX,00,hhmmss.ss,Latitude,N,Longitude,E,AltRef,NavStat,Hacc,Vacc,SOG,COG,Vvel,ageC,HDOP,VDOP,TDOP,GU,RU,DR,*cs<CR><LF>
            var p = new SeparValuesParser<GpsResponse>();
            var obj = p.Parse(dataStr);

            DeviceEntity device = await Task.Run(() => _repositories.Devices.FindSingle(d => d.IMEI == obj.DeviceImei));
            if (device == null)
            {
                throw new Exception();
            }
            //TimeZoneInfo dtz = TimeZoneInfo.CreateCustomTimeZone(obj.DeviceImei, new TimeSpan(obj.TimeZone, 0, 0), obj.DeviceImei, obj.DeviceImei);
            //DateTime dateUtc = TimeZoneInfo.ConvertTimeToUtc(obj.TrackLocalDateTime, dtz);
            DateTime dateUtc = TimeZoneInfo.ConvertTime(obj.TrackLocalDateTime, /*ToDo:  dtz,*/ TimeZoneInfo.Utc);
            long trackUtcTicks = dateUtc.Ticks;
            try{
            TrackEntity track = _repositories.Tracks.FindSingle(t => t.UniqCreatedTicks == trackUtcTicks);
            if (track == null)
            {
                track = new TrackEntity()
                {
                    OwnerId = device.OwnerId,
                    DeviceId = device.Id,
                    UniqCreatedTicks = trackUtcTicks,
                    Title = dateUtc.ToString(),
                    Description = dateUtc.ToString(),
                };
                _repositories.Tracks.Add(track);
            }
            var entity = Mapper.Map<GpsResponseEntity>(obj);
            track.GpsLocations.Add(entity);
            //_repositories.GpsResponses.Add(entity);
            await _repositories.SaveChangesAsync();

            return Mapper.Map<GpsLocation>(entity);
            }catch(Exception ex){
               return null; 
            }
        }
        #endregion
        #region Update
        public async Task<Device> UpdateAsync(int id, DeviceUpdate updates)
        {
            var deviceFromRepo = _repositories.Devices.FindByKey(id);
            Mapper.Map(updates, deviceFromRepo);
            _repositories.Devices.Update(deviceFromRepo);
            await _repositories.SaveChangesAsync();
            return Mapper.Map<Device>(deviceFromRepo);
        }
        
        #endregion

        #region Delete
        public async Task<bool> RemoveAsync(int deviceId)
        {
            _repositories.Devices.Remove(deviceId);
            return await _repositories.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveCodeAsync(int codeId)
        {
            _repositories.AuthCodes.Remove(codeId);
            return await _repositories.SaveChangesAsync() > 0;
        }

        #endregion

        private string generateRefreshToken()
        {
            var guid = Guid.NewGuid();
            byte[] source = guid.ToByteArray();
            var encoder = SHA256.Create();
            byte[] encoded = encoder.ComputeHash(source);
            return Convert.ToBase64String(encoded);
        }
    }
}
