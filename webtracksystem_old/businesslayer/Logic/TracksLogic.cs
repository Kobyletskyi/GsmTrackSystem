using Repositories.UnitOfWork;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using BusinessLayer.Models;
using BusinessLayer.Parsers;
using BusinessLayer.Helpers.Location;
using GeoCoordinatePortable;
using Repositories.Dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Transformation;

namespace BusinessLayer
{
    public class TracksLogic : BaseLogic, ITracksLogic
    {
        public TracksLogic(IUnitOfWorkRepositories repositories, IPropertyMappingService propertyMappingService)
            : base(repositories, propertyMappingService){ }

        #region Fetch
        public async Task<PagedList<Track>> GetTracksAsync(CollectionParameters parameters){
            var include = DataShapingExtensions.IncludeQuery<TrackEntity>(parameters.Fields);
            var tracks = await Task.Run(() => _repositories.Tracks.FindAll(include));
            var sortedEntities = tracks.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<Track, TrackEntity>());
            return await PagedList<Track>.CreateAsync<TrackEntity>(sortedEntities, parameters.PageNumber, parameters.PageSize);
        }
        public async Task<Track> GetTrackAsync(int id, string fields)
        {
            var include = DataShapingExtensions.IncludeQuery<TrackEntity>(fields);
            var entity = await Task.Run(() => _repositories.Tracks.FindByKey(id, include));
            return Mapper.Map<Track>(entity);
        }
        public async Task<GpsLocation> GetPointAsync(int id, string fields)
        {
            var include = DataShapingExtensions.IncludeQuery<GpsResponseEntity>(fields);
            var entity = await Task.Run(() => _repositories.GpsResponses.FindByKey(id, include));
            return Mapper.Map<GpsLocation>(entity);
        }
        public async Task<PagedList<GpsLocation>> GetPointsAsync(int trackId, CollectionParameters parameters)
        {
            var include = DataShapingExtensions.IncludeQuery<GpsResponseEntity>(parameters.Fields);
            var entities = await Task.Run(() => _repositories.GpsResponses.Find(t => t.TrackId == trackId, include));
            var sortedEntities = entities.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<GpsLocation, GpsResponseEntity>());
            return await PagedList<GpsLocation>.CreateAsync<GpsResponseEntity>(sortedEntities, parameters.PageNumber, parameters.PageSize);
        }
        public async Task<PagedList<GpsLocation>> GetPointsAsync(CollectionParameters parameters)
        {
            var include = DataShapingExtensions.IncludeQuery<GpsResponseEntity>(parameters.Fields);
            var entities = await Task.Run(() => _repositories.GpsResponses.FindAll(include));
            var sortedEntities = entities.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<GpsLocation, GpsResponseEntity>());
            return await PagedList<GpsLocation>.CreateAsync<GpsResponseEntity>(sortedEntities, parameters.PageNumber, parameters.PageSize);
        }
        public async Task<PagedList<GpsLocation>> GetNewPointsAsync(int trackId, float utcTime, CollectionParameters parameters)
        {
            var include = DataShapingExtensions.IncludeQuery<GpsResponseEntity>(parameters.Fields);
            var entities = await Task.Run(() => _repositories.GpsResponses.Find(t => t.TrackId == trackId && t.UtcTime > utcTime, include));
            var sortedEntities = entities.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<GpsLocation, GpsResponseEntity>());
            return await PagedList<GpsLocation>.CreateAsync<GpsResponseEntity>(sortedEntities, parameters.PageNumber, parameters.PageSize);
        }
        public async Task<GpsLocation> GetTrackPointAsync(int id, string fields)
        {
            var include = DataShapingExtensions.IncludeQuery<GpsResponseEntity>(fields);
            var entity = await Task.Run(() => _repositories.GpsResponses.FindByKey(id, include));
            return Mapper.Map<GpsLocation>(entity);
        }
        #endregion
        public async Task<bool> RemoveAsync(int id)
        {
            _repositories.Tracks.Remove(id);
            return await _repositories.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemovePointAsync(int id)
        {
            _repositories.GpsResponses.Remove(id);
            return await _repositories.SaveChangesAsync() > 0;
        }
    }
}