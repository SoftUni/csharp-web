using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Models
{
    public enum RecipeType
    {
        Unknown = 0,
        [Display(Name = "test")]
        FastFood = 1,
        LongCookingMeal = 2,
    }
}
