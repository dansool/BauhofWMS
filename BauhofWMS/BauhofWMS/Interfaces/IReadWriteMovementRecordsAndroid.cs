using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    interface IReadWriteMovementRecordsAndroid
    {
        Task<string> ReadMovementRecordsAsync(string shopID, string deviceID);

        Task<string> WriteMovementRecordsAsync(string data, string shopID, string deviceID);
    }
}
