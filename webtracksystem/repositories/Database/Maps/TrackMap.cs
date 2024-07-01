using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Dto;

namespace Repositories.DataBase {  
    public class TrackMap: BaseMap {
        public TrackMap(EntityTypeBuilder<TrackEntity> eb)
            : base(eb){
            
            eb.Property(e => e.Title).IsRequired();
            eb.Property(e => e.Description).IsRequired();
            
            eb.Property(e => e.UniqCreatedTicks)
            .IsRequired();
            eb.HasIndex(e => e.UniqCreatedTicks)
            .IsUnique();
            
            eb.Property(e => e.DeviceId).IsRequired();
            eb.HasOne(e => e.Device)
            .WithMany(p => p.Tracks)
            .HasForeignKey(p => p.DeviceId);

            eb.Property(e => e.OwnerId).IsRequired();
            eb.HasOne(e => e.Owner)
            .WithMany(p => p.Tracks)
            .HasForeignKey(e => e.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

            eb.HasMany(e => e.GpsLocations)
            .WithOne(p => p.Track)
            .HasForeignKey(p => p.TrackId);

            eb.HasMany(e => e.LocationsByCells)
            .WithOne(p => p.Track)
            .HasForeignKey(p => p.TrackId);
        }  
    }  
}  