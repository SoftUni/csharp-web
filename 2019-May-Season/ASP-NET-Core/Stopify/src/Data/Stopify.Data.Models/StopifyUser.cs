using Microsoft.AspNetCore.Identity;

namespace Stopify.Data.Models
{
    public class StopifyUser : IdentityUser
    {
        public StopifyUser()
        {
        }
        
        public string FullName { get; set; }
    }
}
