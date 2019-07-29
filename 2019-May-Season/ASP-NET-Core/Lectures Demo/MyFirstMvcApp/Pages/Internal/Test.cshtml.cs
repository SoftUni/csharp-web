using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyFirstMvcApp.Pages.Internal
{
    public class TestModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        public void OnGet()
        {
        }
    }
}