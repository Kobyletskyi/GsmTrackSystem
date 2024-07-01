using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Repositories.Dto;

namespace Repositories.DataBase {
    public class DateTimeNowValueGenerator : ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;

        protected override object NextValue(EntityEntry entry)
        {
            return DateTime.UtcNow;
        }
    }
    public abstract class BaseMap {  
        public BaseMap(EntityTypeBuilder entityBuilder) {
            entityBuilder.HasKey("Id");
            entityBuilder.Property("Id");//.ValueGeneratedOnAdd(); 
            entityBuilder.Property("CreatedUtcDate");//.HasDefaultValueSql("getdate()");;
            entityBuilder.Property("UpdatedUtcDate");
        }  
    }  
}  