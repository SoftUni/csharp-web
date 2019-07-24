using System;

namespace Stopify.Data.Models
{
    public class Order : BaseModel<string>
    {
        public DateTime IssuedOn { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public string IssuerId { get; set; }

        public StopifyUser Issuer { get; set; }

        public int StatusId { get; set; }

        public OrderStatus Status { get; set; }
    }
}
