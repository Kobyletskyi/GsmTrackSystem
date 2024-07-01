using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class DeviceCode
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string IMEI { get; set; }
        public string Title { get; set; }
        public int Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}
