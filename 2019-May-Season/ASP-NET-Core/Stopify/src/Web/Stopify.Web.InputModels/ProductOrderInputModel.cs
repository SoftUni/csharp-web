using Stopify.Services.Mapping;
using Stopify.Services.Models;

namespace Stopify.Web.InputModels
{
    public class ProductOrderInputModel : IMapTo<OrderServiceModel>
    {
        public string ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
