using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Mapping;
using Stopify.Services.Models;

namespace Stopify.Services
{
    public class OrderService : IOrderService
    {
        private readonly StopifyDbContext context;

        public OrderService(StopifyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CompleteOrder(string orderId)
        {
            Order orderFromDb = await this.context.Orders
                .SingleOrDefaultAsync(order => order.Id == orderId);

            if(orderFromDb == null || orderFromDb.Status.Name != "Active")
            {
                throw new ArgumentException(nameof(orderFromDb));
            }

            orderFromDb.Status = await this.context.OrderStatuses
                .SingleOrDefaultAsync(orderStatus => orderStatus.Name == "Completed");

            this.context.Update(orderFromDb);
            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateOrder(OrderServiceModel orderServiceModel)
        {
            Order order = orderServiceModel.To<Order>();

            order.Status = await context.OrderStatuses
                .SingleOrDefaultAsync(orderStatus => orderStatus.Name == "Active");

            order.IssuedOn = DateTime.UtcNow;

            this.context.Orders.Add(order);
            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<OrderServiceModel> GetAll()
        {
            return this.context.Orders.To<OrderServiceModel>();
        }

        public async Task<bool> IncreaseQuantity(string orderId)
        {
            Order orderFromDb = await this.context.Orders
                .SingleOrDefaultAsync(order => order.Id == orderId);

            if (orderFromDb == null)
            {
                throw new ArgumentNullException(nameof(orderFromDb));
            }

            orderFromDb.Quantity++;

            this.context.Update(orderFromDb);
            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ReduceQuantity(string orderId)
        {
            Order orderFromDb = await this.context.Orders
                .SingleOrDefaultAsync(order => order.Id == orderId);

            if (orderFromDb == null)
            {
                throw new ArgumentNullException(nameof(orderFromDb));
            }

            if (orderFromDb.Quantity == 1)
            {
                return false;
            }

            orderFromDb.Quantity--;

            this.context.Update(orderFromDb);
            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task SetOrdersToReceipt(Receipt receipt)
        {
            List<Order> ordersFromDb = await this.context.Orders
                .Where(order => order.IssuerId == receipt.RecipientId && order.Status.Name == "Active").ToListAsync();

            receipt.Orders = ordersFromDb;
        }
    }
}
