using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public interface IServiceCollection
    {
        void Add<TSource, TDestination>()
            where TDestination : TSource;

        object CreateInstance(Type type);

        T CreateInstance<T>();
    }
}
