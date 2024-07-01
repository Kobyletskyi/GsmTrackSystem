using RepositoriesLayer.Repositories;
using RepositoriesLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BusinessLogic : IBusinessLogic
    {
        public BusinessLogic(IUnitOfWorkRepositories repositories)
        {
            _repositories = repositories;
        }

        private IUnitOfWorkRepositories _repositories;

        private IUserLogic _userLogic;

        private ITrackLogic _trackLogic;

        private IDeviceLogic _deviceLogic;

        public IUserLogic UserLogic
        {
            get { return _userLogic ?? (_userLogic = new UserLogic(_repositories)); }
        }

        public ITrackLogic TrackLogic
        {
            get { return _trackLogic ?? new TrackLogic(_repositories); }
        }

        public IDeviceLogic DeviceLogic
        {
            get { return _deviceLogic ?? new DeviceLogic(_repositories); }
        }
    }
}
