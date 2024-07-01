using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.Entities
{
    [Table("UserProfiles")]
    public class UserProfileEntity : BaseEntity<int>
    {
        [ForeignKey("Id")]
        public virtual UserEntity User { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        //[Required]
        public string Fathername { get; set; }
    }
}
