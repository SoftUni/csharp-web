using SIS.MvcFramework.Attributes.Validation;

namespace Musaca.App.BindingModels.Products
{
    public class ProductCreateBindingModel
    {
        private const string NameErrorMessage = "Product Name must be between 5 and 20 symbols long.";

        private const string PriceErrorMessage = "Product Price must be greater than or equal to 0.01.";

        [RequiredSis]
        [StringLengthSis(5, 20, NameErrorMessage)]
        public string Name { get; set; }
       

        [RequiredSis]
        [RangeSis(typeof(decimal), "0,01", "100000000", PriceErrorMessage)]
        public decimal Price { get; set; }
    }
}
