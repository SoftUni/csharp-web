using System;
using System.Collections.Generic;
using System.Text;

namespace Panda.Web.ViewModels.Packages
{
    public class PackageViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public decimal Weight { get; set; }

        public string ShippingAddress { get; set; }

        public string RecipientName { get; set; }
    }
}
