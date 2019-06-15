using System;
using System.Collections.Generic;
using System.Text;

namespace Musaca.Models
{
    public class OrderProducts
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
