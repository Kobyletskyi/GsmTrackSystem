using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessLayer.Models;
using BusinessLayer.Transformation;
using Repositories.Dto;

namespace BusinessLayer
{
    public interface IDevicesLogic
    {
        Task<bool> DeviceExistsAsync(int id);
        Task<PagedList<Device>> GetDevicesAsync(CollectionParameters parameters);
        Task<Device> GetDeviceAsync(int deviceId, string fields = null);
        Task<Device> GetDeviceAsync(string imei, string fields = null);
        Task<DeviceCode> CreateCodeAsync(string imei);
        Task<DeviceCode> GetCodeAsync(int id, string fields);
        Task<PagedList<DeviceCode>> GetCodesAsync(CollectionParameters parameters);
        Task<PagedList<Track>> GetTracksAsync(int deviceId, CollectionParameters parameters);
        Task<bool> ValidateRefreshTokenAsync(string imei, string token);
        Task<string> GenerateRefreshTokenAsync(string imei, int code);
        Task<Device> CreateAsync(NewDevice device);
        Task<Device> UpdateAsync(int deviceId, DeviceUpdate device);
        Task<GpsLocation> ProccessGpsDataAsync(string dataStr);
        Task<bool> RemoveAsync(int deviceId);
        Task<bool> RemoveCodeAsync(int codeId);
    }
}