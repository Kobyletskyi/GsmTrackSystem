using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Repositories.Dto;

namespace BusinessLayer.Transformation
{
    public static class DataShapingExtensions 
    {
        private const char fieldsSplitter = ',';
        private const char propertiesSplitter = '.';
        public static Func<IQueryable<TDestination>, IQueryable<TDestination>> IncludeQuery<TSource, TDestination>(
            string fields, Dictionary<string, PropertyMappingValue> mappingDictionary) 
            where TDestination : BaseEntity<int> 
        {
            if(!String.IsNullOrWhiteSpace(fields)) {
                string[] fieldsAfterSplit = fields.Split(fieldsSplitter);
                HashSet<string> expandableProperties = new HashSet<string>();
                foreach (string field in fieldsAfterSplit)
                {
                    string propertyName = field.Trim();
                    if(propertyName.Contains(propertiesSplitter)) {
                        propertyName = propertyName.Split(propertiesSplitter).First();
                    }
                    if (!mappingDictionary.ContainsKey(propertyName))
                    {
                        throw new ArgumentException($"Key mapping for {propertyName} is missing");
                    }
                    var propertyMappingValue = mappingDictionary[propertyName];
                    if (propertyMappingValue == null)
                    {
                        throw new ArgumentNullException(nameof(propertyMappingValue));
                    }
                    foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                    {
                        Type entityType = typeof(TDestination);
                        var propertyInfo = entityType.GetProperty(destinationProperty, 
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if(propertyInfo == null){
                            throw new Exception($"Property {destinationProperty} wasn't found on {entityType}");
                        }
                        if(IsExpandable(propertyInfo)){
                            expandableProperties.Add(propertyInfo.Name);
                        }
                    }
                }
                Func<IQueryable<TDestination>, IQueryable<TDestination>> include = (query) => {
                    var result = query;
                    foreach (string propertyPath in expandableProperties)
                    {
                        result = result.Include(propertyPath);
                    }
                    return result;
                };
                return include;
            }
            return null;
        }
        public static Func<IQueryable<TSource>, IQueryable<TSource>> IncludeQuery<TSource>(string fields) where TSource : BaseEntity<int> {
            if(String.IsNullOrWhiteSpace(fields)) {
                return null;
            } else {
                Type sourceType = typeof(TSource);
                string[] fieldsAfterSplit = fields.Split(fieldsSplitter);
                HashSet<string> expandableProperties = new HashSet<string>();
                foreach (string field in fieldsAfterSplit)
                {
                    string propertyPath = field.Trim();
                    if(propertyPath.Contains(propertiesSplitter)) {
                        List<string> chainOfProperties  = new List<string>();
                        Type currentType = sourceType;
                        foreach (string childPropertyName in propertyPath.Split(propertiesSplitter))
                        {
                            if (currentType.IsGenericType 
                                && currentType.GetGenericTypeDefinition() == typeof(ICollection<>))
                            {
                                currentType = currentType.GetGenericArguments().First();
                            }
                            var propertyInfo = currentType.GetProperty(childPropertyName, 
                                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if(propertyInfo == null) {
                                throw new Exception($"Property {propertyPath} wasn't found on {sourceType}({childPropertyName} on {currentType})");
                            }
                            if(IsExpandable(propertyInfo)){
                                chainOfProperties.Add(propertyInfo.Name);
                            } else {
                                break;
                            }
                            currentType = propertyInfo.PropertyType;
                        }
                        if(chainOfProperties.Count > 0) {
                            expandableProperties.Add(String.Join(propertiesSplitter, chainOfProperties));
                        }
                    } else {
                        var propertyInfo = sourceType.GetProperty(propertyPath, 
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if(propertyInfo == null){
                            throw new Exception($"Property {propertyPath} wasn't found on {sourceType}");
                        }
                        if(IsExpandable(propertyInfo)){
                            expandableProperties.Add(propertyInfo.Name);
                        }
                    }
                }
                Func<IQueryable<TSource>, IQueryable<TSource>> include = (query) => {
                    var result = query;
                    foreach (string propertyPath in expandableProperties)
                    {
                        result = result.Include(propertyPath);
                    }
                    return result;
                };
                return include;
            }
        }
        private static bool IsExpandable(PropertyInfo propertyInfo){
            if(IsEntityType(propertyInfo.PropertyType)){
                return true;
            }
            if (propertyInfo.PropertyType.IsGenericType &&
                propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                Type itemType = propertyInfo.PropertyType.GetGenericArguments().First();
                if(IsEntityType(itemType)){
                    return true;
                }
            }
            return false;
        }
        private static bool IsEntityType(Type propertyType){
            return propertyType.IsSubclassOf(typeof(BaseEntity<int>));
        }
        public static IEnumerable<ExpandoObject> ShapeCollection<TSource>(this IEnumerable<TSource> source, string fields){
            if(source == null){
                throw new ArgumentNullException("source");
            }

            var expandoObjectList = new List<ExpandoObject>();
            PropertiesObject properties = GetPropertiesStructure(fields);
            foreach (TSource sourceObject in source)
            {
                expandoObjectList.Add(Transform(typeof(TSource), sourceObject, properties));
            }
            return expandoObjectList;
        }
        public static ExpandoObject ShapeObject<TSource>(this TSource source, string fields) {
            if(source == null){
                throw new ArgumentNullException("source");
            }
            PropertiesObject properties = GetPropertiesStructure(fields);
            return Transform(typeof(TSource), source, properties);
        }
        private static PropertiesObject GetPropertiesStructure(string fields){
            PropertiesObject properties = new PropertiesObject();
            if(!String.IsNullOrWhiteSpace(fields)){
                var fieldsAfterSplit = fields.Split(fieldsSplitter);
                foreach (var path in fieldsAfterSplit)
                {
                    GroupProperties(path, properties);
                }
            }
            return properties;
        }
        //ToDo: limit 
        private static ExpandoObject Transform(Type type, Object source, PropertiesObject properties){
            var data = new ExpandoObject();
            if(properties.Count() > 0){
                foreach (var property in properties)
                {
                    string propertyName = property.Key;
                    PropertiesObject childProperties = property.Value;
                    var propertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if(propertyInfo == null){
                        throw new Exception($"Property {propertyName} wasn't found on {type}");
                    }
                    var obj = GetPropertyValueObject(propertyInfo, source, childProperties);
                    ((IDictionary<string, object>)data).Add(propertyInfo.Name, obj);
                }
            } else {
                AddAllProperties(type, source, (IDictionary<string, object>)data);
            }
            return data;
        }
        private static Object GetPropertyValueObject(PropertyInfo propertyInfo, Object source, PropertiesObject childProperties){            
            var propertyValue = propertyInfo.GetValue(source);
            if( childProperties == null){
                return propertyValue;
            } else {
                Type collectionType = typeof(ICollection<>);
                if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == collectionType)
                {
                    var genericType = propertyInfo.PropertyType.GetGenericArguments().First();
                    var expandoObjectList = new List<ExpandoObject>();
                    foreach (var itemValue in propertyValue as IEnumerable)
                    {
                        var obj = Transform(genericType, itemValue, childProperties);
                        expandoObjectList.Add(obj);
                    }
                    return expandoObjectList;
                } else {
                    return Transform(propertyInfo.PropertyType, propertyValue, childProperties);
                }
            }
        }
        private static void AddAllProperties(Type type, Object sourceObject, IDictionary<string, object> data){
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                data.Add(propertyInfo.Name, propertyInfo.GetValue(sourceObject));
            }
        }
        private static void GroupProperties(string path, PropertiesObject parentShape){
            if(path.Contains(propertiesSplitter)) {
                var propertyName = path.Split(propertiesSplitter).First();
                var nextPath = path.Substring(path.IndexOf(propertiesSplitter) + 1);
                PropertiesObject nextShape;
                if(!parentShape.TryGetValue(propertyName, out nextShape)){
                    nextShape = new PropertiesObject();
                    parentShape.Add(propertyName, nextShape);
                }
                GroupProperties(nextPath, nextShape);
            } else {
                parentShape.Add(path, null);
            }
        }
    }
    public class PropertiesObject : Dictionary<string, PropertiesObject> { }
}