using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    interface IReadWriteInvRecordsAndroid
    {
        Task<string> ReadInvRecordsAsync();
    }
}
