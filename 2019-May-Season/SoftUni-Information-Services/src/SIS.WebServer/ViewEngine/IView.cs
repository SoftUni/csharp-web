using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.ViewEngine
{
    public interface IView
    {
        string GetHtml(object model);
    }
}
