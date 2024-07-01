using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Dto;

namespace Repositories.DataBase {  
    public class DeviceMap: BaseMap {
        public DeviceMap(EntityTypeBuilder<DeviceEntity> eb)
            : base(eb){
            
            eb.Property(e => e.Title).IsRequired();
            eb.Property(e => e.Description).IsRequired();
            eb.Property(e => e.SoftwareVersion).IsRequired();
            
            eb.Property(e => e.IMEI)
            .HasMaxLength(20)
            .IsRequired();
            eb.HasIndex(e => e.IMEI)
            .IsUnique();
            
            eb.Property(e => e.OwnerId).IsRequired();
            eb.HasOne(e => e.Owner)
            .WithMany(p => p.Devices)
            .HasForeignKey(e => e.OwnerId);

            eb.HasOne(e => e.AuthCode)
            .WithOne(p => p.Device)
            .HasForeignKey<DeviceEntity>(d => d.Id);

            // eb.Property(e => e.CreatorId).IsRequired();
            // eb.HasOne(e => e.Creator)
            // .WithMany(p => p.Devices)
            // .HasForeignKey(p => p.CreatorId);
        }  
    }  
}  