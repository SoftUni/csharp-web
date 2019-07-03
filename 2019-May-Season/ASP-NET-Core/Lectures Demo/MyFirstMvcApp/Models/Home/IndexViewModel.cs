using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Models.Home
{
    public class IndexViewModel
    {
        [Display(Name = "Niki")]
        [DataType(DataType.MultilineText)]
        public string Input123 { get; set; }

        public IEnumerable<string> Usernames { get; set; }
    }
}
