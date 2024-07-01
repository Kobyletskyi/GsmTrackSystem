using BusinessLayer.Helpers.Location;
using BusinessLayer.Models;
using BusinessLayer.Parsers;
using DbModels.Entities;
using RepositoriesLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class DeviceLogic : IDeviceLogic
    {

        public DeviceLogic(IUnitOfWorkRepositories repositories)
        {
            _repositories = repositories;
        }

        private IUnitOfWorkRepositories _repositories;

        public IList<Device> SearchDevices(Expression<Func<DeviceEntity, bool>> expFunc = null)
        {
            IEnumerable<DeviceEntity> devices = null;

            if (expFunc != null)
            {
                devices = _repositories.Devices.Find(expFunc);
            }
            else
            {
                devices = _repositories.Devices.FindAll();
            }

            return devices.Select(d => new Device()
            {
                Id = d.Id,
                IMEI = d.IMEI,
                Title = d.Title,
                Description = d.Description,
                SoftwareVersion = d.SoftwareVersion
            }).ToList();
        }

        public Device GetDeviceById(int deviceId)
        {
            DeviceEntity device = _repositories.Devices.FindByKey(deviceId);

            return new Device()
            {
                Id = device.Id,
                IMEI = device.IMEI,
                Title = device.Title,
                Description = device.Description,
                SoftwareVersion = device.SoftwareVersion
            };
        }

        public Device GetDeviceByIMEI(string imei)
        {
            return SearchDevices(x => x.IMEI == imei).FirstOrDefault();
        }

        public AuthCodeEntity CreateDeviceCode(string imei)
        {
            const int codeMinRange = 1000;
            const int codeMaxRange = 10000;
            const int codeExpHours = 24;

            DeviceEntity device = _repositories.Devices.FindSingle(d => d.IMEI == imei);
            if (device != null)
            {
                Random rnd = new Random();
                if (device.AuthCode == null)
                {
                    device.AuthCode = new AuthCodeEntity()
                    {
                        Code = rnd.Next(codeMinRange, codeMaxRange),
                        Id = device.Id,
                        DeviceIMEI = device.IMEI,
                        Expiration = DateTime.UtcNow.AddHours(codeExpHours)
                    };
                }
                else
                {
                    device.AuthCode.Code = rnd.Next(codeMinRange, codeMaxRange);
                    device.AuthCode.Expiration = DateTime.UtcNow.AddHours(codeExpHours);
                }
                _repositories.SaveChanges();
                return device.AuthCode;
            }
            else
            {
                throw new Exception();//not found
            }
        }

        public DeviceCode GetDeviceCode(int deviceId)
        {
            AuthCodeEntity code = _repositories.AuthCodes.FindByKey(deviceId);

            return new DeviceCode()
            {
                Id = code.Device.Id,
                OwnerId = code.Device.OwnerId,
                IMEI = code.DeviceIMEI,
                Title = code.Device.Title,
                Code = code.Code,
                Expiration = code.Expiration
            };
        }
        public bool ValidateRefreshToken(string imei, string token)
        {
            DeviceEntity device = _repositories.Devices.FindSingle(c => c.IMEI == imei);
            UserEntity user = device != null ? device.Owner : null;
            return (user != null && user.RefreshToken == token);
        }

        public string GenerateRefreshToken(string imei, int code)
        {
            string token = null;
            DeviceEntity device = _repositories.Devices.FindSingle(c => c.IMEI == imei);
            if (device != null && device.AuthCode != null)
            {
                if (device.AuthCode.Code == code)
                {
                    if (device.AuthCode.Expiration > DateTime.UtcNow)
                    {
                        
                        if (String.IsNullOrWhiteSpace(device.Owner.RefreshToken))
                        {
                            token = generateRefreshToken();
                            device.Owner.RefreshToken = token;
                        }else
                        {
                            token = device.Owner.RefreshToken;
                        }
                    }
                    _repositories.AuthCodes.Remove(device.AuthCode);
                    _repositories.SaveChanges();
                }
            }
            return token;
        }

        public Device CreateDevice(Device device)
        {
            DeviceEntity entity = new DeviceEntity()
            {
                OwnerId = device.OwnerId,
                CreatorId = device.CreatorId,
                IMEI = device.IMEI,
                Title = device.Title,
                Description = device.Description,
            };
            _repositories.Devices.Add(entity);
            _repositories.SaveChanges();
            return new Device()
            {
                Id = entity.Id,
                OwnerId = entity.OwnerId,
                CreatorId = device.CreatorId,
                IMEI = entity.IMEI,
                Title = entity.Title,
                Description = entity.Description,
            };
        }

        #region Update

        public void UpdateDevice(Device device)
        {
            DeviceEntity entity = new DeviceEntity()
            {
                //IMEI = device.IMEI,
                Title = device.Title,
                Description = device.Description,
            };
            _repositories.Devices.Update(entity);
            _repositories.SaveChanges();
        }
        #endregion

        #region Delete

        public void Delete(DeviceEntity device)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid deviceId)
        {
            throw new NotImplementedException();
        }


        #endregion

        private string generateRefreshToken()
        {
            var guid = Guid.NewGuid();
            byte[] source = guid.ToByteArray();
            var encoder = new SHA256Managed();
            byte[] encoded = SHA256.Create().ComputeHash(source);
            return Convert.ToBase64String(encoded);
        }
    }
}
