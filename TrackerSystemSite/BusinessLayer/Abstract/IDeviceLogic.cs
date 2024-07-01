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
    public interface IDeviceLogic
    {
        IList<Device> SearchDevices(Expression<Func<DeviceEntity, bool>> expFunc = null);
        Device GetDeviceById(int deviceId);
        Device GetDeviceByIMEI(string imei);
        AuthCodeEntity CreateDeviceCode(string imei);
        DeviceCode GetDeviceCode(int deviceId);
        string GenerateRefreshToken(string imei, int code);
        bool ValidateRefreshToken(string imei, string token);

        Device CreateDevice(Device device);
    }
}
