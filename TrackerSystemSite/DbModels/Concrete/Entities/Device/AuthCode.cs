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
    [Table("AuthCode")]
    public class AuthCodeEntity : BaseEntity<int>
    {
        [Required, Key, ForeignKey("Device")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }
        public virtual DeviceEntity Device { get; set; }
        [Required, Index("IX_IMEI", IsUnique = true)]
        [MaxLength(20)]
        public string DeviceIMEI { get; set; }

        [Required]
        public int Code { get; set; }

        [Required]
        public DateTime Expiration { get; set; }
        
    }
}
