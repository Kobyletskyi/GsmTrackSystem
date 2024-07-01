using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using BusinessLayer.Parsers.Attributes;

namespace BusinessLayer.Parsers
{
    public class SeparValuesParser<TItem> : ISeparValuesParser<TItem> where TItem : class, new()
    {
        public TItem Parse(string str)
        {
            TItem item = null;
            if (!String.IsNullOrWhiteSpace(str))
            {
                Type type = typeof(TItem);
                SeparatorAttribute separatorAttr = Attribute.GetCustomAttribute(type, typeof(SeparatorAttribute)) as SeparatorAttribute;
                if (separatorAttr != null)
                {
                    item = new TItem();
                    var parts = str.Split(separatorAttr.Separator);
                    PropertyInfo[] props = type.GetProperties().Where(p => Attribute.IsDefined(p, typeof(IndexAttribute))).ToArray();
                    foreach (PropertyInfo prop in props)
                    {
                        IndexAttribute indexAttr = prop.GetCustomAttribute(typeof(IndexAttribute)) as IndexAttribute;
                        if (!String.IsNullOrWhiteSpace(parts[indexAttr.Index]))
                        {
                            prop.SetValue(item, Convert.ChangeType(parts[indexAttr.Index], prop.PropertyType, CultureInfo.InvariantCulture));
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            return item;
        }
    }
}