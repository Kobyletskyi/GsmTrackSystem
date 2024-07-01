using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Models;
using Repositories.Dto;

namespace BusinessLayer.Transformation
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}