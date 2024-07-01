using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositories.DataBase;
using Repositories.Dto;
using Repositories.UnitOfWork;

namespace Repositories
{
    public class Repositories : IUnitOfWorkRepositories
    {
        public Repositories(DbScheme dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        #region Private fields
        private DbScheme _dbContext;
        private Repository<UserEntity> usersRepository;
        // private Repository<OAuthUserEntity> oAuthUserRepository;
        // private Repository<UserProfileEntity> userProfilesRepository;
        // private Repository<UserRoleEntity> userRolesRepository;
        private Repository<GpsResponseEntity> gpsResponse;
        private Repository<DeviceEntity> devicesRepository;
        private Repository<AuthCodeEntity> codesRepository;
        private Repository<TrackEntity> tracksRepository;
        private Repository<GeoLocationByCellsEntity> geoLocationsRepository;
        private Repository<NeighborCellInfoEntity> neighborCellInfoRepository;

        #endregion

        #region Public properties

        public IRepository<UserEntity, int> Users
        {
            get
            {
                if (usersRepository == null) usersRepository = new Repository<UserEntity>(_dbContext);
                return usersRepository;
            }
        }
        public IRepository<NeighborCellInfoEntity, int> NeighborCellInfo
        {
            get
            {
                if (neighborCellInfoRepository == null) neighborCellInfoRepository = new Repository<NeighborCellInfoEntity>(_dbContext);
                return neighborCellInfoRepository;
            }
        }
        public IRepository<DeviceEntity, int> Devices
        {
            get
            {
                if (devicesRepository == null) devicesRepository = new Repository<DeviceEntity>(_dbContext);
                return devicesRepository;
            }
        }
        public IRepository<AuthCodeEntity, int> AuthCodes
        {
            get
            {
                if (codesRepository == null) codesRepository = new Repository<AuthCodeEntity>(_dbContext);
                return codesRepository;
            }
        }
        public IRepository<TrackEntity, int> Tracks
        {
            get
            {
                if (tracksRepository == null) tracksRepository = new Repository<TrackEntity>(_dbContext);
                return tracksRepository;
            }
        }
        public IRepository<GeoLocationByCellsEntity, int> GeoLocations
        {
            get
            {
                if (geoLocationsRepository == null) geoLocationsRepository = new Repository<GeoLocationByCellsEntity>(_dbContext);
                return geoLocationsRepository;
            }
        }
        public IRepository<GpsResponseEntity, int> GpsResponses
        {
            get
            {
                if (gpsResponse == null) gpsResponse = new Repository<GpsResponseEntity>(_dbContext);
                return gpsResponse;
            }
        }

        // public IRepository<OAuthUserEntity, int> OAuthUsers
        // {
        //     get
        //     {
        //         if (oAuthUserRepository == null) oAuthUserRepository = new Repository<OAuthUserEntity>(_dbContext);
        //         return oAuthUserRepository;
        //     }
        // }

        // public IRepository<UserProfileEntity, int> UserProfiles
        // {
        //     get
        //     {
        //         if (userProfilesRepository == null) userProfilesRepository = new Repository<UserProfileEntity>(_dbContext);
        //         return userProfilesRepository;
        //     }
        // }

        // public IRepository<UserRoleEntity, int> UserRoles
        // {
        //     get
        //     {
        //         if(userRolesRepository == null) userRolesRepository =  new Repository<UserRoleEntity>(_dbContext);
        //         return userRolesRepository;
        //     }
        // }

        #endregion
    }
}
