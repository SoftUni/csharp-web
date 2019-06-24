using System;

namespace Panda.Domain
{
    public class Package
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public double Weight { get; set; }

        public string ShippingAddress { get; set; }

        public string StatusId { get; set; }

        public PackageStatus Status { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public string RecipientId { get; set; }

        public PandaUser Recipient { get; set; }
    }
}
