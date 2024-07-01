using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Api{
    public interface ITypeHelperService {
        bool TypeHasProperties<TSource>(string fields);
    }
}