using DbModels.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.DataBase
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DbScheme : DbContext
    {
        #region Constructors

        public DbScheme()
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            objectContext.SavingChanges += (sender, args) =>
            {
                var now = DateTime.UtcNow;
                foreach (var entry in this.ChangeTracker.Entries<BaseEntity<int>>())
                {
                    var entity = entry.Entity;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.CreatedUtcDate = now;
                            entity.UpdatedUtcDate = now;
                            break;
                        case EntityState.Modified:
                            entity.UpdatedUtcDate = now;
                            break;
                    }
                }
                this.ChangeTracker.DetectChanges();
            };
        }

        #endregion

        #region DbSets

        public DbSet<NeighborCellInfo> NeighborCellInfo { get; set; }
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<AuthCodeEntity> AuthCodes { get; set; }
        public DbSet<TrackEntity> Tracks { get; set; }
        public DbSet<GeoLocationByCellsEntity> GeoLocations { get; set; }
        public DbSet<GpsResponseEntity> GpsResponse { get; set; }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserProfileEntity> UserProfiles { get; set; }
        public DbSet<OAuthUserEntity> OAuthUsers { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        #endregion

        #region Events
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            //modelBuilder.Entity<User>()
            //    .HasOptional(x => x.ProfileEntity)
            //    .WithRequired();


            base.OnModelCreating(modelBuilder);
        }
        #endregion

    }
}
