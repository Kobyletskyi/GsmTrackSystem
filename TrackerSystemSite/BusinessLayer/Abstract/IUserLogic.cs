using BusinessLayer.Models;
using DbModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IUserLogic
    {

        #region User

        IEnumerable<Account> GetUsers();
        Account FindById(int id);
        Account FindByUsername(string userName);
        bool UserExists(string userName);
        Account CreateUser(string userName, string password, string confirmationToken = null);

        #endregion

        #region OAuth

        void CreateOAuthUser();

        #endregion

        #region UserProfile



        void CreateUserProfile(UserProfileEntity profile);

        UserProfileEntity GetUserProfile(int userId);

        void SaveUserProfile(UserProfileEntity profile);

        #endregion

        #region Role

        void AddRoleToUser(int roleId, int userId);

        void RemoveRoleFromUser(int roleId, int userId);

        #endregion

    }
}
