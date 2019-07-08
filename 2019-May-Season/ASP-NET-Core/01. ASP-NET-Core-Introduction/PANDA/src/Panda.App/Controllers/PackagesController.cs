using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panda.App.Models.Package;
using Panda.Data;
using Panda.Domain;
using Panda.Services;
using System;
using System.Globalization;
using System.Linq;

namespace Panda.App.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPackagesService packagesService;
        private readonly IReceiptsService receiptsService;       

        public PackagesController(IUsersService usersService, IPackagesService packagesService, IReceiptsService receiptsService)
        {
            this.usersService = usersService;
            this.packagesService = packagesService;
            this.receiptsService = receiptsService;            
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            this.ViewData["Recipients"] = this.usersService.GetAllUsers();

            return this.View(new PackageCreateBindingModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(PackageCreateBindingModel bindingModel)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View(bindingModel ?? new PackageCreateBindingModel());
            }

            Package package = new Package
            {
                Description = bindingModel.Description,
                Recipient = this.usersService.GetUser(bindingModel.Recipient),
                ShippingAddress = bindingModel.ShippingAddress,
                Weight = bindingModel.Weight,
                Status = this.packagesService.GetPackageStatus("Pending")
            };

            this.packagesService.CreatePackage(package);

            return this.Redirect("/Packages/Pending");
        }

        [HttpGet("/Packages/Details/{id}")]
        [Authorize]
        public IActionResult Details(string id)
        {
            Package package = this.packagesService.GetPackage(id);

            PackageDetailsViewModel viewModel = new PackageDetailsViewModel
            {
                Description = package.Description,
                Recipient = package.Recipient.UserName,
                ShippingAddress = package.ShippingAddress,
                Weight = package.Weight,
                Status = package.Status.Name
            };

            if(package.Status.Name == "Pending")
            {
                viewModel.EstimatedDeliveryDate = "N/A";
            }
            else if(package.Status.Name == "Shipped")
            {
                viewModel.EstimatedDeliveryDate = package.EstimatedDeliveryDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                viewModel.EstimatedDeliveryDate = "Delivered";
            }

            return this.View(viewModel);
        }

        [HttpGet("/Packages/Ship/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Ship(string id)
        {
            Package package = this.packagesService.GetPackage(id);
            package.Status = this.packagesService.GetPackageStatus("Shipped");
            package.EstimatedDeliveryDate = DateTime.Now.AddDays(new Random().Next(20, 40));
            this.packagesService.UpdatePackage(package);

            return this.Redirect("/Packages/Shipped");
        }

        [HttpGet("/Packages/Deliver/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Deliver(string id)
        {
            Package package = this.packagesService.GetPackage(id);
            package.Status = this.packagesService.GetPackageStatus("Delivered");
            this.packagesService.UpdatePackage(package);

            return this.Redirect("/Packages/Delivered");
        }

        [HttpGet("/Packages/Acquire/{id}")]
        [Authorize]
        public IActionResult Acquire(string id)
        {
            Package package = this.packagesService.GetPackage(id);
            package.Status = this.packagesService.GetPackageStatus("Acquired");
            this.packagesService.UpdatePackage(package);

            Receipt receipt = new Receipt
            {
                Fee = (decimal)(2.67 * package.Weight),
                IssuedOn = DateTime.Now,
                Package = package,
                Recipient = this.usersService.GetUser(this.User.Identity.Name)
            };

            this.receiptsService.CreateReceipt(receipt);

            return this.Redirect("/Home/Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Pending()
        {
            return this.View(this.packagesService.GetPackagesWithRecipientAndStatus()
                .Where(package => package.Status.Name == "Pending")
                .ToList().Select(package =>
            {
                return new PackagePendingViewModel
                {
                    Id = package.Id,
                    Description = package.Description,
                    Weight = package.Weight,
                    ShippingAddress = package.ShippingAddress,
                    Recipient = package.Recipient.UserName
                };
            }).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Shipped()
        {
            return this.View(this.packagesService.GetPackagesWithRecipientAndStatus()
                .Where(package => package.Status.Name == "Shipped")
                .ToList().Select(package =>
                {
                    return new PackageShippedViewModel
                    {
                        Id = package.Id,
                        Description = package.Description,
                        Weight = package.Weight,
                        EstimatedDeliveryDate = package.EstimatedDeliveryDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Recipient = package.Recipient.UserName
                    };
                }).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delivered()
        {
            return this.View(this.packagesService.GetPackagesWithRecipientAndStatus()
                .Where(package => package.Status.Name == "Delivered" || package.Status.Name == "Acquired")
                .ToList().Select(package =>
                {
                    return new PackageDeliveredViewModel
                    {
                        Id = package.Id,
                        Description = package.Description,
                        Weight = package.Weight,
                        ShippingAddress = package.ShippingAddress,
                        Recipient = package.Recipient.UserName
                    };
                }).ToList());
        }
    }
}