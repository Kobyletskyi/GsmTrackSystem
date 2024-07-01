namespace BusinessLayer.Models
{
    public class NewDevice : DeviceUpdate
    {
        public string IMEI { get; set; }
        public int OwnerId { get; set; }
    }
}