using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;
using System;
using System.Linq;

namespace Panda.Services
{
    public class PackagesService : IPackagesService
    {
        private readonly PandaDbContext pandaDbContext;

        public PackagesService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public void CreatePackage(Package package)
        {
            this.pandaDbContext.Packages.Add(package);
            this.pandaDbContext.SaveChanges();
        }

        public Package GetPackage(string id)
        {
            Package package = this.pandaDbContext.Packages
                .Where(packageDb => packageDb.Id == id)
                .Include(packageDb => packageDb.Recipient)
                .Include(packageDb => packageDb.Status)
                .SingleOrDefault();

            return package;
        }

        public PackageStatus GetPackageStatus(string status)
        {
            PackageStatus statusDb = this.pandaDbContext.PackageStatus.SingleOrDefault(statusEntity => statusEntity.Name == status);

            return statusDb;
        }

        public IQueryable<Package> GetPackagesWithRecipientAndStatus()
        {
            IQueryable<Package> packageWithRecipiuentAndStatusDb = pandaDbContext.Packages
                 .Include(package => package.Recipient)
                 .Include(package => package.Status);

            return packageWithRecipiuentAndStatusDb;
        }

        public void UpdatePackage(Package package)
        {
            this.pandaDbContext.Update(package);
            this.pandaDbContext.SaveChanges();
        }
    }
}
