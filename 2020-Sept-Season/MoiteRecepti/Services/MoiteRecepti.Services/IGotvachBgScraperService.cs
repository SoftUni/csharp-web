namespace MoiteRecepti.Services
{
    using System.Threading.Tasks;

    public interface IGotvachBgScraperService
    {
        Task PopulateDbWithRecipesAsync(int recipesCount);
    }
}
