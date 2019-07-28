using Stopify.Data.Models;
using Stopify.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(OrderServiceModel orderServiceModel);

        IQueryable<OrderServiceModel> GetAll();

        Task SetOrdersToReceipt(Receipt receipt);

        Task<bool> CompleteOrder(string orderId);

        Task<bool> ReduceQuantity(string orderId);

        Task<bool> IncreaseQuantity(string orderId);
    }
}
