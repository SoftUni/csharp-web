using ForumSystem.Data.Models;
using ForumSystem.Services.Mapping;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForumSystem.Web.ViewModels.Posts
{
    public class PostCreateInputModel : IMapTo<Post>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }
    }
}
