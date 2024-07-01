using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BusinessLayer.Models;
using BusinessLayer.Transformation;
using Repositories.Dto;

namespace BusinessLayer.Mappers
{
    public static class PropertiesMappings
    {
        
        public static IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();
        static PropertiesMappings(){
            CreateMap<Device,DeviceEntity>(PropertiesMappings.deviceToEntity);
            CreateMap<Track,TrackEntity>();
            CreateMap<GpsLocation,GpsResponseEntity>();
        }
        private static Dictionary<string, PropertyMappingValue> deviceToEntity = 
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase) {
            //{ GetPropertyName<Device>(o => o.Tracks), new PropertyMappingValue(new []{GetPropertyName<DeviceEntity>(o => o.Tracks)})}
        };
        private static string GetPropertyName<TValue>(Expression<Func<TValue, object>> propertySelector) {
            var memberExpression = propertySelector.Body as MemberExpression;
            return memberExpression != null ? memberExpression.Member.Name : string.Empty;
        }
        private static void CreateMap<TSource, TDestination>(Dictionary<string, PropertyMappingValue> propetiesMapping = null) {
            var sourcePropertyNames = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p=>p.Name);
            if(propetiesMapping == null){
                propetiesMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase);
            }
            foreach (var propertyName in sourcePropertyNames)
            {
                if(!propetiesMapping.ContainsKey(propertyName))
                {
                    propetiesMapping.Add(propertyName, new PropertyMappingValue(new []{propertyName}));
                }
            }
            PropertyMappings.Add(new PropertyMapping<TSource, TDestination>(propetiesMapping));
        }
    }
}