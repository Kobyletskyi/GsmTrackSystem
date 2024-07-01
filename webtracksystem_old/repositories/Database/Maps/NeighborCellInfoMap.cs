using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Dto;

namespace Repositories.DataBase {  
    public class NeighborCellInfoMap: BaseMap {
        public NeighborCellInfoMap(EntityTypeBuilder<NeighborCellInfoEntity> eb)
            : base(eb){
            eb.Property(e => e.Arfcn).IsRequired();
            eb.Property(e => e.RxLevel).IsRequired();
            eb.Property(e => e.Bsic).IsRequired();
            eb.Property(e => e.CellId).IsRequired();
            eb.Property(e => e.Mcc).IsRequired();
            eb.Property(e => e.Mnc).IsRequired();
            eb.Property(e => e.Lac).IsRequired();
            
            eb.HasOne(e => e.GeoLocation)
            .WithMany(p => p.CellInfos)
            .HasForeignKey(p => p.LocationId);
        }  
    }  
}  