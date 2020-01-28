using System;
using System.ComponentModel.DataAnnotations;

namespace DemoApp
{
    public class Tweet
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string Creator { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
