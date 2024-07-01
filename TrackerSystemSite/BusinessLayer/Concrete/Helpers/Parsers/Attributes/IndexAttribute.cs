using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLayer.Parsers.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexAttribute : Attribute
    {
        public int Index { get; set; }

        public IndexAttribute(int ind)
        {
            Index = ind;
        }
    }
}