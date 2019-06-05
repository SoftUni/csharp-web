using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using IRunes.App.ViewModels;
using IRunes.Models;
using SIS.MvcFramework;
using SIS.MvcFramework.Extensions;
using SIS.MvcFramework.Mapping;

namespace IRunes.App
{
    public static class Program
    {
        public static void Main()
        {
            WebHost.Start(new Startup());
        }
    }
}
