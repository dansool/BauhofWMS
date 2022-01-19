using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    interface IReadWritePurchaseOrderPickedQuantitiesRecordsAndroid
    {
        Task<string> ReadPurchaseOrderPickedQuantitiesRecordsAsync();
        Task<string> WritePurchaseOrderPickedQuantitiesRecordsAsync(string data);
    }
}
