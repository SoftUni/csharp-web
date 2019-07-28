using Stopify.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public interface IReceiptService
    {
        Task<string> CreateReceipt(string recipientId);

        IQueryable<ReceiptServiceModel> GetAll();

        IQueryable<ReceiptServiceModel> GetAllByRecipientId(string recipientId);
    }
}
