using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.ModelBinders
{
    public class ExtractYearModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context?.Metadata?.Name?.ToLower() == "year"
                && context?.Metadata?.ModelType == typeof(int))
            {
                return new ExtractYearModelBinder();
            }

            return null;
        }
    }
}
