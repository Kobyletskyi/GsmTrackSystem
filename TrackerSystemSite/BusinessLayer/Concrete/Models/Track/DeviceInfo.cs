using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class DeviceInfo
    {
        public string Title { get; set; }        
        public string IMEI { get; set; }        
        public string Description { get; set; }        
        public string SoftwareVersion { get; set; }
        public IEnumerable<TrackInfo> Tracks { get; set; }
    }
}
