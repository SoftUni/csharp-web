namespace MoiteRecepti.Services
{
    using System.Threading.Tasks;

    public interface IGotvachBgScraperService
    {
        Task ImportRecipesAsync(int recipesCount);
    }
}
