using System;

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