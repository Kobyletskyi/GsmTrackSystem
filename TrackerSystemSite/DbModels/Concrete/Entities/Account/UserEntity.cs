using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.Entities
{
    [Table("Users")]
    public partial class UserEntity : BaseEntity<int>
    {
        public UserEntity()
        {
            Roles = new Collection<UserRoleEntity>();
            OAuthUsers = new Collection<OAuthUserEntity>();
        }

        [Required]
        public string UserName { get; set; }

        public string ConfirmationToken { get; set; }

        public string RefreshToken { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }

        public DateTime? LastPasswordFailureDate { get; set; }

        [Required]
        public int PasswordFailuresSinceLastSuccess { get; set; }

        public DateTime? PasswordChangedDate { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        public string PasswordVerificationToken { get; set; }

        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        //public virtual UserProfileEntity Profile { get; set; }

        public virtual ICollection<OAuthUserEntity> OAuthUsers { get; set; }

        [Association("UsersToRoles", "UserId", "UserId")]
        public virtual ICollection<UserRoleEntity> Roles { get; set; }

    }
}
