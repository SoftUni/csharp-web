using Microsoft.AspNetCore.Mvc;
using MyFirstMvcApp.ModelBinders;
using MyFirstMvcApp.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Models.CarAd
{
    public class CarBrandAndModel : IValidatableObject
    {
        [Required]
        [MinLength(2)]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Brand == "Audi" && !this.Model.StartsWith("Q") && !this.Model.StartsWith("A"))
            {
                yield return new ValidationResult("Invalid Audi model");
            }
            if (this.Brand == "BMW" && !this.Model.StartsWith("M"))
            {
                yield return new ValidationResult("Invalid Audi model");
            }
        }
    }

    public class CreateInputModel
    {
        [Range(0, int.MaxValue)]
        public CarType Type { get; set; }

        [Required]
        public CarBrandAndModel Car { get; set; }

        [Display(Name = "Long descrption")]
        [DataType(DataType.MultilineText)]
        [MinLength(20)]
        [RegularExpression("[A-Z][a-z0-9 ]+")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        // TODO: To current year custom validator
        [BeforeCurrentYear(1900)]
        public int Year { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        // TODO
        public byte[] Image { get; set; }
    }
}
