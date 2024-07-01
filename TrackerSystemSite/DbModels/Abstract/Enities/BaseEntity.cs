using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbModels.Entities
{
    public abstract class BaseEntity<TKey> : IEntity<TKey>
        where TKey : struct
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }

        [Required]
        public DateTime CreatedUtcDate { get; set; }

        [Required]
        public DateTime UpdatedUtcDate { get; set; }

        public int CompareTo(IEntity<TKey> other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IEntity<TKey> other)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            var props = this.GetType().GetProperties();
            int hash = 0;
            foreach (var prop in props)
            {
                var value = prop.GetValue(this);
                if (value != null)
                {
                    hash += prop.GetValue(this).GetHashCode();
                }
            }

            return hash;
        }
    }
}
