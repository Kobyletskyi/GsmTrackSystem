using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Models;
using BusinessLayer.Transformation;
using Repositories.Dto;

namespace BusinessLayer
{
    public interface IUsersLogic
    {
        #region User
        Task<PagedList<User>> GetUsersAsync(CollectionParameters parameters);
        Task<User> GetUserAsync(int id, string fields);
        Task<PagedList<Track>> GetTracksAsync(int userId, CollectionParameters parameters);
        Task<PagedList<Device>> GetDevicesAsync(int userId, CollectionParameters parameters);
        Task<User> FindByUsernameAsync(string userName, string fields=null);
        Task<bool> UserExistsAsync(string userName);
        Task<User> CreateAsync(string userName, string password, string confirmationToken = null);
        Task<bool> RemoveUserAsync(int id);

        #endregion
    }
}