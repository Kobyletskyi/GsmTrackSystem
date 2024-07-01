using BusinessLayer.Models;
using DbModels.Entities;
using RepositoriesLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserLogic : IUserLogic
    {
        public UserLogic(IUnitOfWorkRepositories repositories)
        {
            _repositories = repositories;
        }

        private IUnitOfWorkRepositories _repositories;

        #region User

        public IEnumerable<Account> GetUsers()
        {
            IEnumerable<UserEntity> users = _repositories.Users.FindAll();
            return users.Select(u => new Account() { Id = u.Id, UserName = u.UserName });
        }
        public Account FindById(int id)
        {
            UserEntity user = _repositories.Users.FindByKey(id);
            return user != null ? new Account() { Id = user.Id, UserName = user.UserName } : null;
        }
        public Account FindByUsername(string userName)
        {
            UserEntity user = _repositories.Users.FindSingle(u => u.UserName == userName);
            return user != null ? new Account() { Id = user.Id, UserName = user.UserName, PasswordSalt = user.PasswordSalt } : null;
        }
        public bool UserExists(string userName)
        {
            return _repositories.Users.Count(u => u.UserName == userName) > 0;
        }
        public Account CreateUser(string userName, string passwordHash, string confirmationToken = null)
        {
            UserEntity newUser = new UserEntity()
            {
                UserName = userName,
                PasswordSalt = passwordHash,
                IsConfirmed = (confirmationToken == null),
                ConfirmationToken = confirmationToken
            };
            _repositories.Users.Add(newUser);
            _repositories.SaveChanges();
            return new Account()
            {
                Id = newUser.Id,
                UserName = newUser.UserName
            };
        }

        
        #endregion

        #region OAuth

        public void CreateOAuthUser()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UserProfile

        public void CreateUserProfile(UserProfileEntity profile)
        {
            _repositories.UserProfiles.Add(profile);
        }

        public UserProfileEntity GetUserProfile(int userId)
        {
            UserProfileEntity profile = _repositories.UserProfiles.FindSingle(p => p.Id == userId);
            return profile;
        }

        public void SaveUserProfile(UserProfileEntity profile)
        {
            _repositories.UserProfiles.Update(profile);
        }

        #endregion

        #region Role

        public void CreateRole(string name)
        {
            throw new NotImplementedException();
        }

        public void RemoveRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public void AddRoleToUser(int roleId, int userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveRoleFromUser(int roleId, int userId)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
