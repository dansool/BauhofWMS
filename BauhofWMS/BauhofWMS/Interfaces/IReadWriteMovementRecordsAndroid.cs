using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    interface IReadWriteMovementRecordsAndroid
    {
        Task<string> ReadMovementRecordsAsync();

        Task<string> WriteMovementRecordsAsync(string data);
    }
}
