using System.ComponentModel.DataAnnotations;

namespace MyFirstMvcApp.Controllers
{
    public class FormInputModel
    {
        [Required]
        public string Search { get; set; }
    }
}
