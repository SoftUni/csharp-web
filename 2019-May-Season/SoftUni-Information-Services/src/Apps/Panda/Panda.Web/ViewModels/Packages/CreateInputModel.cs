using SIS.MvcFramework.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Panda.Web.ViewModels.Packages
{
    public class CreateInputModel
    {
        [RequiredSis]
        [StringLengthSis(5, 20, "Description should be between 5 and 20 characters.")]
        public string Description { get; set; }

        public decimal Weight { get; set; }

        public string ShippingAddress { get; set; }

        [RequiredSis]
        public string RecipientName { get; set; }
    }
}
