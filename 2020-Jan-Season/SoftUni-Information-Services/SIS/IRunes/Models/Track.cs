using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IRunes.Models
{
    public class Track
    {
        public Track()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Link { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string AlbumId { get; set; }

        public Album Album { get; set; }
    }
}
