using System.ComponentModel.DataAnnotations;

namespace MyFirstAspNetCoreApplication.ViewModels.Home
{
    public class IndexViewModel
    {
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(2000, 2100)]
        public int Year { get; set; }

        public int UsersCount { get; set; }

        public int Processors { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
        public bool ReadPrivacy { get; internal set; }
    }
}
