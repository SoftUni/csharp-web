using Panda.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Panda.Services
{
    public interface IReceiptsService
    {
        void CreateReceipt(Receipt receipt);

        List<Receipt> GetAllReceiptsWithRecipient();

        List<Receipt> GetAllReceiptsWithRecipientAndPackage();
    }
}
