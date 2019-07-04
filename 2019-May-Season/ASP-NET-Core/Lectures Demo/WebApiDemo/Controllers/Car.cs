using System.ComponentModel.DataAnnotations;

namespace WebApiDemo.Controllers
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Model { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        public Color Color { get; set; }
    }

    public enum Color
    {
        Black = 1,
        Red = 2,
        Blue = 3,
    }
}