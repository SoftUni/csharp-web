namespace ForumSystem.Web.ViewModels.Categories
{
    using System;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class PostInCategoryViewModel : IMapFrom<Post>
    {
        public DateTime CreatedOn { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ShortContent =>
            this.Content?.Length > 100
                ? this.Content?.Substring(0, 100) + "..."
                : this.Content;

        public string UserUserName { get; set; }

        public int CommentsCount { get; set; }
    }
}