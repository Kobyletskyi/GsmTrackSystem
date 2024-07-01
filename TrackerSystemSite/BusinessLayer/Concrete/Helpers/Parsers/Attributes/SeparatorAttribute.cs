using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLayer.Parsers.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SeparatorAttribute: Attribute
    {
        public char Separator { get; set; }

        public SeparatorAttribute(char sepr)
        {
            Separator = sepr;
        }
    }
}