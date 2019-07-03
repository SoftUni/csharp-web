using System.Threading.Tasks;

namespace Eventures.Data.Seeding
{
    public interface ISeeder
    {
        Task Seed();
    }
}