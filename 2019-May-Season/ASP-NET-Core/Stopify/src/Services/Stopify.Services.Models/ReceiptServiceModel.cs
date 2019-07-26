using Stopify.Data.Models;
using Stopify.Services.Mapping;
using System;
using System.Collections.Generic;

namespace Stopify.Services.Models
{
    public class ReceiptServiceModel : IMapFrom<Receipt>
    {
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public List<OrderServiceModel> Orders { get; set; }

        public string RecipientId { get; set; }

        public StopifyUserServiceModel Recipient { get; set; }
    }
}
