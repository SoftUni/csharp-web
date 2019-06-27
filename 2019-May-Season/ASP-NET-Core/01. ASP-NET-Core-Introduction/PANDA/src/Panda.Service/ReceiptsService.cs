using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panda.Services
{
    public class ReceiptsService : IReceiptsService
    {
        private readonly PandaDbContext pandaDbContext;

        public ReceiptsService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public void CreateReceipt(Receipt receipt)
        {
            this.pandaDbContext.Receipts.Add(receipt);

            this.pandaDbContext.SaveChanges();
            
        }

        public List<Receipt> GetAllReceiptsWithRecipient()
        {
            var receiptsWithRecipientDb = this.pandaDbContext.Receipts.Include(receipt => receipt.Recipient).ToList();

            return receiptsWithRecipientDb;
        }

        public List<Receipt> GetAllReceiptsWithRecipientAndPackage()
        {
            var receiptsAllDb = this.pandaDbContext.Receipts
                .Include(receipt => receipt.Recipient)
                .Include(receipt => receipt.Package)
                .ToList();

            return receiptsAllDb;
        }
    }
}
