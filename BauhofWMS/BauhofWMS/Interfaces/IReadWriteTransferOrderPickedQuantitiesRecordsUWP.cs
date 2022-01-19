using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    interface IReadWriteTransferOrderPickedQuantitiesRecordsUWP
    {
        Task<string> ReadTransferOrderPickedQuantitiesRecordsAsync();
        Task<string> WriteTransferOrderPickedQuantitiesRecordsAsync(string data);
    }
}
