using System.Collections.Generic;

namespace BusinessLayer.Models
{
    public class Device
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        //public int CreatorId { get; set; }
        public string IMEI { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SoftwareVersion { get; set; }
        public ICollection<Track> Tracks { get; set; }
    }
}