using System.ComponentModel.DataAnnotations;

namespace Panda.App.Models.Package
{
    public class PackageCreateBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Description invalid.", MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Description invalid.", MinimumLength = 5)]
        public string ShippingAddress { get; set; }

        [Required]
        public string Recipient { get; set; }
    }
}
