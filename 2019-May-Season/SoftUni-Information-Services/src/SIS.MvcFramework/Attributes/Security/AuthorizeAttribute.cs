using System;
using SIS.MvcFramework.Identity;

namespace SIS.MvcFramework.Attributes.Security
{
    public class AuthorizeAttribute : Attribute
    {
        public AuthorizeAttribute(string authority = "authorized")
        {
            this.authority = authority;
        }

        private readonly string authority;

        private bool IsLoggedIn(Principal principal)
        {
            return principal != null;
        }

        public bool IsInAuthority(Principal principal)
        {
            if (!this.IsLoggedIn(principal))
            {
                return this.authority == "anonymous";
            }
           
            return this.authority == "authorized" 
                   || principal.Roles.Contains(this.authority.ToLower());
        }
    }
}
