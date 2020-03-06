namespace ForumSystem.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new List<(string Name, string ImageUrl)>
            {
                ("Sport", "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0c/Sport_balls.svg/1200px-Sport_balls.svg.png"),
                ("Coronavirus", "https://cdn.wccftech.com/wp-content/uploads/2020/01/https___cdn.cnn_.com_cnnnext_dam_assets_200108214800-coronavirus.jpg"),
                ("News", "https://www.northampton.ac.uk/wp-content/uploads/2018/11/news.jpg"),
                ("Music", "https://www.bensound.com/bensound-img/happyrock.jpg"),
                ("Programming", "https://learnworthy.net/wp-content/uploads/2019/12/Why-programming-is-the-skill-you-have-to-learn.jpg"),
                ("Cats", "https://www.rd.com/wp-content/uploads/2019/11/cat-10-e1573844975155-1024x692.jpg"),
                ("Dogs", "https://cdn.cnn.com/cnnnext/dam/assets/191114120109-dog-aging-project-1-super-tease.jpg"),
            };
            foreach (var category in categories)
            {
                await dbContext.Categories.AddAsync(new Category
                {
                    Name = category.Name,
                    Description = category.Name,
                    Title = category.Name,
                    ImageUrl = category.ImageUrl,
                });
            }
        }
    }
}
