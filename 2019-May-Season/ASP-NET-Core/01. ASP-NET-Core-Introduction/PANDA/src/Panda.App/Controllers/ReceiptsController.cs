using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panda.App.Models.Receipt;
using Panda.Data;
using Panda.Domain;
using Panda.Services;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Panda.App.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsService receiptsService;        

        public ReceiptsController(IReceiptsService receiptsService)
        {            
            this.receiptsService = receiptsService;
        }

        [Authorize]
        public IActionResult My()
        {
            List<ReceiptMyViewModel> myReceipts = this.receiptsService.GetAllReceiptsWithRecipient()                
                .Where(receipt => receipt.Recipient.UserName == this.User.Identity.Name)
                .Select(receipt => new ReceiptMyViewModel
                {
                    Id = receipt.Id,
                    Fee = receipt.Fee,
                    IssuedOn = receipt.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Recipient = receipt.Recipient.UserName
                })
                .ToList();

            return View(myReceipts);
        }

        [HttpGet("/Receipts/Details/{id}")]
        [Authorize]
        public IActionResult Details(string id)
        {
            Receipt receiptFromDb = this.receiptsService.GetAllReceiptsWithRecipientAndPackage()
                .Where(receipt => receipt.Id == id)                
                .SingleOrDefault();

            ReceiptDetailsViewModel viewModel = new ReceiptDetailsViewModel
            {
                Id = receiptFromDb.Id,
                Total = receiptFromDb.Fee,
                Recipient = receiptFromDb.Recipient.UserName,
                DeliveryAddress = receiptFromDb.Package.ShippingAddress,
                PackageWeight = receiptFromDb.Package.Weight,
                PackageDescription = receiptFromDb.Package.Description,
                IssuedOn = receiptFromDb.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            };

            return this.View(viewModel);
        }
    }
}