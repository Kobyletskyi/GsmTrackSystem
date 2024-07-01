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
    [Table("UserRoles")]
    public class UserRoleEntity : BaseEntity<int>
    {
        public UserRoleEntity()
        {
            Users = new Collection<UserEntity>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
