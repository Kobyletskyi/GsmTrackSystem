using DbModels.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.DataBase
{
    public class MyDbContextInitializerIfNotExists : CreateDatabaseIfNotExists<DbScheme>
    {
        public static void Initializing(DbScheme context)
        {
            //var user = new UserEntity
            //{
            //    UserName = "user@email.com",
            //    IsConfirmed = true,
            //    PasswordFailuresSinceLastSuccess = 0,
            //    PasswordSalt = "PasswordSalt"
            //};
            //context.Users.Add(user);
            //context.SaveChanges();


            //var device = new DeviceEntity
            //{
            //    OwnerId = user.Id,
            //    Title = "Test",
            //    Description = "Test",
            //    IMEI = "866104028656646",
            //    SoftwareVersion = "1137B04V01SIM900M64_ST_DTMF_JD_EAT"
            //};
            //context.Devices.Add(device);
            //context.SaveChanges();

        }
        protected override void Seed(DbScheme context)
        {
            Initializing(context);
        }
    }

    public class MyDbContextInitializerIfChanges : DropCreateDatabaseIfModelChanges<DbScheme>
    {
        protected override void Seed(DbScheme context)
        {
            MyDbContextInitializerIfNotExists.Initializing(context);
        }
    }

    public class MyDbContextInitializerAlways : DropCreateDatabaseAlways<DbScheme>
    {
        protected override void Seed(DbScheme context)
        {
            MyDbContextInitializerIfNotExists.Initializing(context);
        }
    }
}
