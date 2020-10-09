using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMvcApp.Controllers
{
    public class CardsController : Controller
    {
        // GET /cards/add
        public HttpResponse Add(HttpRequest request)
        {
            return this.View();
        }

        // /cards/all
        public HttpResponse All(HttpRequest request)
        {
            return this.View();
        }

        // /cards/collection
        public HttpResponse Collection(HttpRequest request)
        {
            return this.View();
        }
    }
}
