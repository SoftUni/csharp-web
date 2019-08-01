using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstMvcApp.Services;

namespace MyFirstMvcApp.Pages
{
    public class ContactFormModel : PageModel
    {
        private readonly ICounterService service;

        public ContactFormModel(ICounterService service)
        {
            this.service = service;
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        [BindProperty]
        public string Email { get; set; }

        [Required]
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [Required]
        [BindProperty(SupportsGet = true)]
        public string Title { get; set; }

        [Required]
        [BindProperty]
        public string Content { get; set; }

        public string InfoMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                this.InfoMessage = "Thank you for submitting the contact form.";
                // TODO: Send mail
                // TODO: Save to database
                return this.Redirect("/");
            }

            return Page();
        }

        public override async Task OnPageHandlerExecutionAsync(
            PageHandlerExecutingContext context,
            PageHandlerExecutionDelegate next)
        {
            // Before page handler
            var result = await next();
            // After page handler
        }
    }
}