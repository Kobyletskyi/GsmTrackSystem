using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Dto;

namespace Repositories.DataBase {  
    public class GeoLocationByCellsMap: BaseMap {
        public GeoLocationByCellsMap(EntityTypeBuilder<GeoLocationByCellsEntity> eb)
            : base(eb){

            eb.Property(t => t.Longitude).IsRequired(); 
            eb.Property(t => t.Latitude).IsRequired();  
            eb.Property(t => t.Accuracy).IsRequired();  
            eb.Property(t => t.TrackId).IsRequired();
            
            eb.HasOne(e => e.Track)
            .WithMany(p => p.LocationsByCells)
            .HasForeignKey(p => p.TrackId);
            //.HasConstraintName("ForeignKey_LocationsByCells_Track");

            eb.HasMany(e => e.CellInfos)
            .WithOne(p => p.GeoLocation)
            .HasForeignKey(p => p.LocationId);
            //.HasConstraintName("ForeignKey_LocationByCells_CellInfos");
            
        }  
    }  
}  