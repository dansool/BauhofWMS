using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    interface IReadWriteTransferOrderPickedQuantitiesRecordsAndroid
    {
        Task<string> ReadTransferOrderPickedQuantitiesRecordsAsync(string shopID, string deviceID);
        Task<string> WriteTransferOrderPickedQuantitiesRecordsAsync(string data, string shopID, string deviceID);
    }
}
