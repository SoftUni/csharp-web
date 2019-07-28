using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Web.ViewModels.Order.Cart;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stopify.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        private readonly IReceiptService receiptService;

        public OrderController(IOrderService orderService, IReceiptService receiptService)
        {
            this.orderService = orderService;
            this.receiptService = receiptService;
        }

        [HttpGet("Cart")]
        [Route("/Order/Cart")]
        public async Task<IActionResult> Cart()
        {
            List<OrderCartViewModel> orders = await this.orderService.GetAll()
                .Where(order => order.Status.Name == "Active"
                && order.IssuerId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .To<OrderCartViewModel>()
                .ToListAsync();

            return this.View(orders);
        }

        [HttpPost]
        [Route("/Order/{id}/Quantity/Reduce")]
        public async Task<IActionResult> Reduce(string id)
        {
            bool result = await this.orderService.ReduceQuantity(id);

            if(result)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        [HttpPost]
        [Route("/Order/{id}/Quantity/Increase")]
        public async Task<IActionResult> Increase(string id)
        {
            bool result = await this.orderService.IncreaseQuantity(id);

            if (result)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        [HttpPost]
        [Route("/Order/Cart/Complete")]
        public async Task<IActionResult> Complete()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            string receiptId = await this.receiptService.CreateReceipt(userId);

            return this.Redirect($"/Receipt/Details/{receiptId}");
        }
    }
}