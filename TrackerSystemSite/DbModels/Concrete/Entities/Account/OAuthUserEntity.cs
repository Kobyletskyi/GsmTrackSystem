using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.Entities
{
    [Table("OAuthUsers")]
    public class OAuthUserEntity : BaseEntity<int>
    {
        [ForeignKey("User"), Required]
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string ProviderUserId { get; set; }


    }
}
