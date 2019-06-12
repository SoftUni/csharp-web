using SIS.MvcFramework.Attributes.Validation;

namespace Musaca.App.BindingModels.Products
{
    public class ProductOrderBindingModel
    {
        [RequiredSis]
        public string Product { get; set; }
    }
}
