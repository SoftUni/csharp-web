namespace ForumSystem.Data.Models
{
    using ForumSystem.Data.Common.Models;
    using System.Collections.Generic;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
