using Repositories.Dto;

namespace Repositories
{
    public interface IRepositories
    {
        IRepository<GpsResponseEntity, int> GpsResponses { get; }
        IRepository<DeviceEntity, int> Devices { get; }
        IRepository<AuthCodeEntity, int> AuthCodes { get; }
        IRepository<TrackEntity, int> Tracks { get; }
        IRepository<GeoLocationByCellsEntity, int> GeoLocations { get; }
        IRepository<NeighborCellInfoEntity, int> NeighborCellInfo { get; }
        IRepository<UserEntity, int> Users { get; }
        //IRepository<UserRoleEntity, int> UserRoles { get; }
        //IRepository<UserProfileEntity, int> UserProfiles { get; }
        //IRepository<OAuthUserEntity, int> OAuthUsers { get; }
    }
}
