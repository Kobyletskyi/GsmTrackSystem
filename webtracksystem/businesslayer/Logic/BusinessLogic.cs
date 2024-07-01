using Repositories.UnitOfWork;
using BusinessLayer.Transformation;

namespace BusinessLayer
{
    public class BusinessLogic : IBusinessLogic
    {
        public BusinessLogic(IUnitOfWorkRepositories repositories, IPropertyMappingService propertyMappingService)
        {
            _repositories = repositories;
            _propertyMappingService = propertyMappingService;
        }

        private IUnitOfWorkRepositories _repositories;
        protected IPropertyMappingService _propertyMappingService;

        private IUsersLogic _usersLogic;
        private ITracksLogic _tracksLogic;
        private IDevicesLogic _devicesLogic;

        public IUsersLogic Users
        {
            get { return _usersLogic ?? (_usersLogic = new UsersLogic(_repositories, _propertyMappingService)); }
        }
        public ITracksLogic Tracks
        {
            get { return _tracksLogic ?? (_tracksLogic = new TracksLogic(_repositories, _propertyMappingService)); }
        }
        public IDevicesLogic Devices
        {
            get { return _devicesLogic ?? (_devicesLogic = new DevicesLogic(_repositories, _propertyMappingService)); }
        }
    }
}
