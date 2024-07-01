namespace BusinessLayer
{
    public interface IBusinessLogic
    {
        ITracksLogic Tracks { get; }
        IDevicesLogic Devices { get; }
        IUsersLogic Users { get; }
    }
}