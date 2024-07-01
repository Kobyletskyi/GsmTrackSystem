using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Dto
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
        DateTime CreatedUtcDate { get; set; }
        DateTime UpdatedUtcDate { get; set; }
        int CompareTo(IEntity<TKey> other);
        bool Equals(IEntity<TKey> other);
    }
}
