using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Dto;

namespace Repositories.DataBase {  
    public class UserMap: BaseMap {
        public UserMap(EntityTypeBuilder<UserEntity> eb)
            : base(eb){
            eb.Property(t => t.UserName); 
            
            eb.HasMany(e => e.Devices)
            .WithOne(d => d.Owner)
            .HasForeignKey(d => d.OwnerId);

            eb.HasMany(e => e.Tracks)
            .WithOne(d => d.Owner)
            .HasForeignKey(d => d.OwnerId);
        }  
    }  
}  