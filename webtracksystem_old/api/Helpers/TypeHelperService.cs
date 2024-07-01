using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Api{
    public class TypeHelperService : ITypeHelperService {
        public bool TypeHasProperties<TSource>(string fields) {

            if(String.IsNullOrWhiteSpace(fields)){
                return true;
            }
                var fieldsAfterSplit = fields.Split(',');
                foreach (var field in fieldsAfterSplit)
                {
                    string propertyName = field.Trim();
                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if(propertyInfo==null) {
                        return false;
                    }
                }

            return true;
        }
    }
}