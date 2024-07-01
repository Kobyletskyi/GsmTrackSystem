using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLayer.Parsers
{
    public interface ISeparValuesParser<TItem> where TItem: class, new()
    {
        TItem Parse(string str);
    }
}