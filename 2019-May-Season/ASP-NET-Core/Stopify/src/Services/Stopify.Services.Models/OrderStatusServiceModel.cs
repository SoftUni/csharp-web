using Stopify.Data.Models;
using Stopify.Services.Mapping;

namespace Stopify.Services.Models
{
    public class OrderStatusServiceModel : IMapFrom<OrderStatus>
    {
        public string Name { get; set; }
    }
}
