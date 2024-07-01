using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Dto
{
    public class AuthCodeEntity : BaseEntity<int>
    {
        //[Required, Key, ForeignKey("Device")]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }
        public virtual DeviceEntity Device { get; set; }
        //[Required, Index("IX_IMEI", IsUnique = true)]
        //[MaxLength(20)]
        public string IMEI { get; set; }
        //[Required]
        public int Code { get; set; }
        //[Required]
        public DateTime Expiration { get; set; }
        
    }
}
