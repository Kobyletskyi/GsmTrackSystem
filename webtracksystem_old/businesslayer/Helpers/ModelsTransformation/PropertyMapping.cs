using System.Collections.Generic;

namespace BusinessLayer.Transformation
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping 
    {
        public Dictionary<string, PropertyMappingValue> mappingDictionary { get; private set; }
        public PropertyMapping(Dictionary<string, PropertyMappingValue> value) {
            mappingDictionary = value;
        }
    }
}