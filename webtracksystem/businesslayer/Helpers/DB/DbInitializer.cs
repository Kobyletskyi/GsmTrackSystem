using System.Threading.Tasks;
using System.Security.Cryptography;
using System;
using BusinessLayer.Models;
using Repositories.DataBase;
using System.Linq;
using Repositories.Dto;

namespace BusinessLayer.DB
{
    public class DbInitializer
    {
        private DbScheme _context;
 
        public DbInitializer(DbScheme context)
        {
            _context = context;
        }
 
        public async Task Seed(string adminName, string passwordSalt)
        {
            var res = await _context.Database.EnsureCreatedAsync();
            if ( _context.Users.FirstOrDefault(u=> u.UserName == adminName)==null)
            {
                var user = new UserEntity {
                    UserName=adminName,
                    PasswordSalt=passwordSalt
                };
                _context.Users.Add(user);
                _context.Devices.Add(new DeviceEntity {
                    OwnerId=user.Id,
                    IMEI="123456789",
                    Title="demo",
                    Description="demo",
                    SoftwareVersion="1"
                });
                
                await _context.SaveChangesAsync();
 
            }
        }
 
    }
}