namespace Panda.Domain
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class PandaUser : IdentityUser
    {
        public PandaUser()
        {
            this.Packages = new List<Package>();
            this.Receipts = new List<Receipt>();
        }

        public PandaUserRole UserRole { get; set; }

        public ICollection<Package> Packages { get; set; }

        public ICollection<Receipt> Receipts { get; set; }
    }
}
