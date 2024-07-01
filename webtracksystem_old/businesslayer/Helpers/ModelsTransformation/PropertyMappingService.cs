using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BusinessLayer.Mappers;
using BusinessLayer.Models;
using Repositories.Dto;

namespace BusinessLayer.Transformation
{
    public class PropertyMappingService : IPropertyMappingService 
    {
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>() {
            var matchingMappings = PropertiesMappings.PropertyMappings.OfType<PropertyMapping<TSource, TDestination>>(); 
            if(matchingMappings.Count() == 1){
                return matchingMappings.First().mappingDictionary;
            }
            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}>");
        }
    }
}