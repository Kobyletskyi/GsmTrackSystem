using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Dto
{
    public partial class UserEntity : BaseEntity<int>
    {
        public UserEntity()
        {
            Devices = new Collection<DeviceEntity>();
            Tracks = new Collection<TrackEntity>();
            //Roles = new Collection<UserRoleEntity>();
            //OAuthUsers = new Collection<OAuthUserEntity>();
        }
        //[Required]
        public string UserName { get; set; }
        public string ConfirmationToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordVerificationToken { get; set; }
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
        //public virtual UserProfileEntity Profile { get; set; }
        //public virtual ICollection<OAuthUserEntity> OAuthUsers { get; set; }
        //[Association("UsersToRoles", "UserId", "UserId")]
        //public virtual ICollection<UserRoleEntity> Roles { get; set; }

        public ICollection<TrackEntity> Tracks { get; set; }
        public ICollection<DeviceEntity> Devices { get; set; }
    }
}
