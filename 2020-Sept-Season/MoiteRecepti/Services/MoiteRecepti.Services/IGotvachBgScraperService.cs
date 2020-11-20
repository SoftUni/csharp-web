namespace MoiteRecepti.Services
{
    using System.Threading.Tasks;

    public interface IGotvachBgScraperService
    {
        Task ImportRecipesAsync(int fromId = 1, int toId = 10000);
    }
}
