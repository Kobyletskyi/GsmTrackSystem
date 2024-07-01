using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BusinessLayer.Transformation
{
    public static class IQueryableExtensions
    {
        const char fieldsSplitter = ',';
        const char descendingSplitter = ' ';
        const string descendingValue = " desc";
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary) {
            if (source == null)
            {
                throw new System.ArgumentNullException(nameof(source));
            }

            if (mappingDictionary == null)
            {
                throw new System.ArgumentNullException(nameof(mappingDictionary));
            }
            
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            string[] orderByAfterSplit = orderBy.Split(fieldsSplitter);

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith(descendingValue);
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(descendingSplitter);
                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new System.ArgumentException($"Key mapping for {propertyName} is missing");
                }
                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                {
                    throw new System.ArgumentNullException(nameof(propertyMappingValue));
                }
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if(propertyMappingValue.Revert) {
                        orderDescending = !orderDescending;
                    }
                    source = source.OrderBy(destinationProperty + (orderDescending?" descending":" ascending"));
                }
            }
            return source;
        }
    }
}