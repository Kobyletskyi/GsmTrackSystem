using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositories.Dto;

namespace Repositories.DataBase
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DbScheme : DbContext
    {
        #region Constructors

        public DbScheme(DbContextOptions <DbScheme> options): base(options) { }

        #endregion

        #region DbSets
        public DbSet<NeighborCellInfoEntity> NeighborCellInfos { get; set; }
        public DbSet<GeoLocationByCellsEntity> GeoLocationsByCells { get; set; }
        public DbSet<GpsResponseEntity> GpsResponses { get; set; }
        public DbSet<TrackEntity> Tracks { get; set; }
        public DbSet<AuthCodeEntity> DeviceAuthCodes { get; set; }
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        #endregion

        #region Events
        protected override void OnModelCreating(ModelBuilder modelBuilder) {  
            base.OnModelCreating(modelBuilder);  
            new UserMap(modelBuilder.Entity<UserEntity> ());
            new DeviceMap(modelBuilder.Entity<DeviceEntity> ());
            new TrackMap(modelBuilder.Entity<TrackEntity> ());
            new AuthCodeMap(modelBuilder.Entity<AuthCodeEntity> ());
            new GpsResponseMap(modelBuilder.Entity<GpsResponseEntity> ());
            new GeoLocationByCellsMap(modelBuilder.Entity<GeoLocationByCellsEntity> ());
            new NeighborCellInfoMap(modelBuilder.Entity<NeighborCellInfoEntity> ());
        }  
        #endregion

        private void applyDates(){
            var entries =  this.ChangeTracker.Entries<BaseEntity<int>>();
            var toUpdateEntries = entries.Where(e=>e.State == EntityState.Added || e.State == EntityState.Modified);
            var now = DateTime.UtcNow;
            foreach (var entry in toUpdateEntries){
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
            if(toUpdateEntries.Count() > 0){
                //this.ChangeTracker.DetectChanges();
            }
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)){
            applyDates();
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges(){
            return SaveChangesAsync().Result;
        }
    }
}
