using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class Device
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int CreatorId { get; set; }
        public string IMEI { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SoftwareVersion { get; set; }
    }
}
