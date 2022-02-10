using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    interface IReadWritePurchaseOrderPickedQuantitiesRecordsAndroid
    {
        Task<string> ReadPurchaseOrderPickedQuantitiesRecordsAsync(string shopID, string deviceID);
        Task<string> WritePurchaseOrderPickedQuantitiesRecordsAsync(string data, string shopID, string deviceID);
    }
}
