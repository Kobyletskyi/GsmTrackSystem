using BusinessLayer.Models;
using DbModels.Entities;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface ITrackLogic
    {
        #region Read

        IList<DeviceInfo> GetDevicesWithTracks();

        IList<GpsResponseEntity> GetTrackPoints(int trackId);

        IList<GpsResponseEntity> GetNewTrackPoints(int trackId, float utcTime);

        IList<GpsResponseEntity> GetTrackPoints(int trackId, long trackUniqTicks);

        IList<GpsResponseEntity> SearchPoints(Expression<Func<GpsResponseEntity, bool>> expFunc = null);

        #endregion

        #region Update
        void ProccessDeviceGpsData(string dataStr);

        #endregion
    }
}
