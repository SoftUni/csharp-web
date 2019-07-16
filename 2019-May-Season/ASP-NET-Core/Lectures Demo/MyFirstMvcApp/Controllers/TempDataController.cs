using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Controllers
{
    public class TempDataFormInputModel
    {
        public string Message { get; set; }

        public IEnumerable<IFormFile> Files { get; set; }
    }

    // Post-Redirect-Get
    public class TempDataController : Controller
    {
        private readonly IHostingEnvironment environment;

        public TempDataController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult SendForm(TempDataFormInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            System.IO.Directory.CreateDirectory(environment.WebRootPath + "/Files");
            using (var file = System.IO.File.OpenWrite(environment.WebRootPath + "/Files/test.txt"))
            {
                input.Files.First().CopyTo(file);
            }

            // Save to db
            this.TempData["message"] = input.Message;
            return this.Redirect("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return this.PhysicalFile(environment.WebRootPath + "/Files/test.txt", "text/plain");
        }
    }
}
