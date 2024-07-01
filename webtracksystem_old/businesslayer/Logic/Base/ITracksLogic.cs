using System.Collections.Generic;
using Repositories.Dto;
using System.Linq.Expressions;
using System;
using BusinessLayer.Models;
using System.Threading.Tasks;
using BusinessLayer.Transformation;

namespace BusinessLayer
{
    public interface ITracksLogic
    {
        Task<PagedList<Track>> GetTracksAsync(CollectionParameters parameters);
        Task<Track> GetTrackAsync(int id, string fields);
        Task<GpsLocation> GetPointAsync(int id, string fields);
        Task<PagedList<GpsLocation>> GetPointsAsync(int trackId, CollectionParameters parameters);
        Task<PagedList<GpsLocation>> GetPointsAsync(CollectionParameters parameters);
        Task<PagedList<GpsLocation>> GetNewPointsAsync(int trackId, float utcTime, CollectionParameters parameters);
        Task<bool> RemoveAsync(int id);
        Task<bool> RemovePointAsync(int id);
    }
}