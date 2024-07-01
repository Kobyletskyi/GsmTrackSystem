#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Dto;
using Repositories.UnitOfWork;
using BusinessLayer.Transformation;
#endregion

namespace BusinessLayer
{
    public class UsersLogic : BaseLogic, IUsersLogic
    {
        public UsersLogic(IUnitOfWorkRepositories repositories, IPropertyMappingService propertyMappingService)
            : base(repositories, propertyMappingService){ }

        #region Fetch

        public async Task<PagedList<User>> GetUsersAsync(CollectionParameters parameters)
        {
            var include = DataShapingExtensions.IncludeQuery<UserEntity>(parameters.Fields);
            IQueryable<UserEntity> users = _repositories.Users.FindAll(include);
            var sortedEntities = users.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<User, UserEntity>());
            return await PagedList<User>.CreateAsync<UserEntity>(sortedEntities, parameters.PageNumber, parameters.PageSize);
        }
        public async Task<User> GetUserAsync(int id, string fields)
        {
            var include = DataShapingExtensions.IncludeQuery<UserEntity>(fields);
            var entity = await Task.Run(() => _repositories.Users.FindSingle(x=>x.Id == id, include));
            return Mapper.Map<User>(entity);

        }
        public async Task<PagedList<Track>> GetTracksAsync(int userId, CollectionParameters parameters)
        {
            var include = DataShapingExtensions.IncludeQuery<TrackEntity>(parameters.Fields);
            IQueryable<TrackEntity> entities = _repositories.Tracks.Find(t => t.OwnerId == userId, include);
            var sortedEntities = entities.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<Track, TrackEntity>());
            return await PagedList<Track>.CreateAsync<TrackEntity>(sortedEntities, parameters.PageNumber, parameters.PageSize);
        }
        public async Task<PagedList<Device>> GetDevicesAsync(int userId, CollectionParameters parameters)
        {
            var include = DataShapingExtensions.IncludeQuery<DeviceEntity>(parameters.Fields);
            IQueryable<DeviceEntity> devices = _repositories.Devices.Find(d => d.OwnerId == userId, include);
            var sortedEntities = devices.ApplySort(parameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<Device, DeviceEntity>());
            return await PagedList<Device>.CreateAsync<DeviceEntity>(sortedEntities, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<User> FindByUsernameAsync(string userName, string fields = null)
        {
            var include = DataShapingExtensions.IncludeQuery<UserEntity>(fields);
            var entity = await Task.Run(() => _repositories.Users.FindSingle(u => u.UserName == userName, include));
            return Mapper.Map<User>(entity);
        }
        public async Task<bool> UserExistsAsync(string userName)
        {
            return await Task.Run(() => _repositories.Users.Count(u => u.UserName == userName) > 0);
        }
        #endregion
        public async Task<User> CreateAsync(string userName, string passwordHash, string confirmationToken = null)
        {
            UserEntity newUser = new UserEntity()
            {
                UserName = userName,
                PasswordSalt = passwordHash,
                IsConfirmed = (confirmationToken == null),
                ConfirmationToken = confirmationToken
            };
            _repositories.Users.Add(newUser);
            await _repositories.SaveChangesAsync();
            return Mapper.Map<User>(newUser);
        }

        public async Task<bool> RemoveUserAsync(int id)
        {
            _repositories.Users.Remove(id);
            return await _repositories.SaveChangesAsync() > 0;
        }
    }
}
