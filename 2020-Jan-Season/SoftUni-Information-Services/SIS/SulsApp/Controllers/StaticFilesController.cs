using SIS.HTTP;
using SIS.HTTP.Response;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SulsApp.Controllers
{
    public class StaticFilesController : Controller
    {
        public HttpResponse Bootstrap(HttpRequest request)
        {
            return this.CssFileView("bootstrap.min");
        }

        public HttpResponse Site(HttpRequest request)
        {
            return this.CssFileView("site");
        }

        public HttpResponse Reset(HttpRequest request)
        {
            return this.CssFileView("reset-css");
        }
    }
}
