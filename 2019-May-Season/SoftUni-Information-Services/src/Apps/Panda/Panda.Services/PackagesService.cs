using Panda.Data;
using Panda.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panda.Services
{
    public class PackagesService : IPackagesService
    {
        private readonly PandaDbContext db;
        private readonly IReceiptsService receiptsService;

        public PackagesService(PandaDbContext db, IReceiptsService receiptsService)
        {
            this.db = db;
            this.receiptsService = receiptsService;
        }

        public void Create(string description, decimal weight, string shippingAddress, string recipientName)
        {
            var userId = this.db.Users.Where(x => x.Username == recipientName).Select(x => x.Id).FirstOrDefault();
            if (userId == null)
            {
                return;
            }

            var package = new Package
            {
                Description = description,
                Weight = weight,
                Status = PackageStatus.Pending,
                ShippingAddress = shippingAddress,
                RecipientId = userId,
            };

            this.db.Packages.Add(package);
            this.db.SaveChanges();
        }

        public void Deliver(string id)
        {
            var package = this.db.Packages.FirstOrDefault(x => x.Id == id);
            if (package == null)
            {
                return;
            }

            package.Status = PackageStatus.Delivered;
            this.db.SaveChanges();

            this.receiptsService.CreateFromPackage(package.Weight, package.Id, package.RecipientId);
        }

        public IQueryable<Package> GetAllByStatus(PackageStatus status)
        {
            var packages = this.db.Packages.Where(x => x.Status == status); // x.Recipient.Username == username
            return packages;
        }
    }
}
