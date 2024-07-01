using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Dto;

namespace Repositories.DataBase {  
    public class AuthCodeMap: BaseMap {
        public AuthCodeMap(EntityTypeBuilder<AuthCodeEntity> eb)
            : base(eb){

            eb.HasKey(e => e.Id);
            eb.Property(e => e.Id).ValueGeneratedNever();  

            eb.Property(e => e.Code).IsRequired();
            eb.Property(e => e.Expiration).IsRequired();
            
            eb.Property(e => e.IMEI)
            .HasMaxLength(20)
            .IsRequired();
            eb.HasIndex(e => e.IMEI)
            .IsUnique();
            eb.HasOne(e => e.Device)
            .WithOne(p => p.AuthCode)
            .HasForeignKey<AuthCodeEntity>(d => d.Id);
        }  
    }  
}  