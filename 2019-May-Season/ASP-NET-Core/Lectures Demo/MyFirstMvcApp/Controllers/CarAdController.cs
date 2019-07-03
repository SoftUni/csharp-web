using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyFirstMvcApp.Models.CarAd;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Controllers
{
    public class CarAdController : Controller
    {
        public CarAdController()
        {
        }

        public IActionResult Create()
        {
            var model = new CreateInputModel();
            model.Description = "initial value";
            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            return this.Json(input);
        }
    }
}
