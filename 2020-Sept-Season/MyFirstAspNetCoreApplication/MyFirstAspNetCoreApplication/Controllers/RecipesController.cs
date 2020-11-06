using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyFirstAspNetCoreApplication.ViewModels.Recipes;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public RecipesController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Add()
        {

            var model = new AddRecipeInputModel
            {
                Type = Models.RecipeType.Unknown,
                FirstCooked = DateTime.UtcNow,
                Time = new RecipeTimeInputModel()
                {
                    CookingTime = 10,
                    PreparationTime = 5,
                }
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRecipeInputModel input)
        {
            if (!input.Image.FileName.EndsWith(".png"))
            {
                this.ModelState.AddModelError("Image", "Invalid file type.");
            }

            if (input.Image.Length > 10 * 1024 * 1024 )
            {
                this.ModelState.AddModelError("Image", "File too big.");
            }

            if (!ModelState.IsValid)
            {
                return this.View();
            }

            // TODO: Save data
            using (FileStream fs = new FileStream(
                this.webHostEnvironment.WebRootPath + "/user.png", FileMode.Create))
            {
                await input.Image.CopyToAsync(fs);
            }

            return this.RedirectToAction(nameof(ThankYou));
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }

        public IActionResult Image()
        {
            return this.PhysicalFile(this.webHostEnvironment.WebRootPath + "/user.png", "image/png");
        }
    }
}
