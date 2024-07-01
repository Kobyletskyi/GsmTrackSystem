using System;

namespace BusinessLayer.Parsers
{
    public interface ISeparValuesParser<TItem> where TItem: class, new()
    {
        TItem Parse(string str);
    }
}