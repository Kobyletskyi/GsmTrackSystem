using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Dto;

namespace Repositories.DataBase {  
    public class GpsResponseMap: BaseMap {
        public GpsResponseMap(EntityTypeBuilder<GpsResponseEntity> eb)
            : base(eb){
            eb.Property(e => e.UtcTime).IsRequired();
            eb.Property(e => e.Latitude).IsRequired();
            eb.Property(e => e.NorthOrSouth).HasMaxLength(1).IsRequired();
            eb.Property(e => e.Longitude).IsRequired();
            eb.Property(e => e.EastOrWest).HasMaxLength(1).IsRequired();
            eb.Property(e => e.NavigationStatus).IsRequired();
            eb.Property(e => e.HorizontalAccuracy).IsRequired();
            eb.Property(e => e.NavigationStatus).IsRequired();
            eb.Property(e => e.NavigationStatus).IsRequired();
            eb.Property(e => e.NavigationStatus).IsRequired();
            eb.Property(e => e.NavigationStatus).IsRequired();
            
            eb.HasOne(e => e.Track)
            .WithMany(p => p.GpsLocations)
            .HasForeignKey(e => e.TrackId);
        }  
    }  
}  