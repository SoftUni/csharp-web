using MyFirstAspNetCoreApplication.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MinLength(10)]
        public string Name { get; set; }

        [MinLength(10)]
        public string Description { get; set; }

        [CurrentYearMaxValue(1900)]
        public DateTime ActiveFrom { get; set; }

        [Range(0, 10000)]
        public double Price { get; set; }
    }
}
