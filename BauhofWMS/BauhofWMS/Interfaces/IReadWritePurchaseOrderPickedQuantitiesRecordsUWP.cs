using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    interface IReadWritePurchaseOrderPickedQuantitiesRecordsUWP
    {
        Task<string> ReadPurchaseOrderPickedQuantitiesRecordsAsync();
        Task<string> WritePurchaseOrderPickedQuantitiesRecordsAsync(string data);
    }
}
