using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstAspNetCoreApplication.Models;
using MyFirstAspNetCoreApplication.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstAspNetCoreApplication.ViewModels.Recipes
{
    public class AddRecipeInputModel
    {
        [MinLength(5)]
        [MaxLength(10)]
        [Required]
        [Display(Name = "Name of recipe")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter.")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public RecipeType Type { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "First time the recipe is cooked")]
        public DateTime FirstCooked { get; set; }

        [CurrentYearMaxValue(1900)]
        public int Year { get; set; }

        public RecipeTimeInputModel Time { get; set; }

        public IFormFile Image { get; set; }
    }
}
