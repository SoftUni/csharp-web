namespace ForumSystem.Web.ViewModels.Posts
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryDropDownViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
