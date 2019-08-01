using Stopify.Services.Mapping;
using Stopify.Services.Models;

namespace Stopify.Web.ViewModels.Product.Delete
{
    public class ProductDeleteProductTypeViewModel : IMapFrom<ProductTypeServiceModel>
    {
        public string Name { get; set; }
    }
}