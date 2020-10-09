using SUS.MvcFramework;
using System.Threading.Tasks;

namespace BattleCards
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // TODO: <Startup>
            await Host.CreateHostAsync(new Startup(), 80);
        }
    }
}
