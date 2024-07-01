using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class TrackInfo
    {
        public int Id { get; set; }
        public long UniqCreatedTicks { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime UpdatedUtcDate { get; set; }
    }
}
