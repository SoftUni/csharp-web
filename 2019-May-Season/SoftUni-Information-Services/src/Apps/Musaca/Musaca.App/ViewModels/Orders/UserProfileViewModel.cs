using System.Collections.Generic;

namespace Musaca.App.ViewModels.Orders
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel()
        {
            this.Orders = new List<OrderProfileViewModel>();
        }

        public List<OrderProfileViewModel> Orders { get; set; }
    }
}
