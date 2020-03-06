namespace ForumSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<IndexCategoryViewModel> Categories { get; set; }
    }
}
