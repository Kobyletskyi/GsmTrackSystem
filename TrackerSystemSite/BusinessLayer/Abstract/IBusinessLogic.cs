using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IBusinessLogic
    {
        ITrackLogic TrackLogic { get; }

        IDeviceLogic DeviceLogic { get; }

        IUserLogic UserLogic { get; }
    }
}
