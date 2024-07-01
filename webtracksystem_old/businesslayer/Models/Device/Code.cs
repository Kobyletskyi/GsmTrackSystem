
using System;

namespace BusinessLayer.Models
{
    public class DeviceCode
    {
        public int Id { get; set; }
        public string IMEI { get; set; }
        public int Code { get; set; }
        public DateTime Expiration { get; set; }
        public Device Device { get; set; }
    }
}