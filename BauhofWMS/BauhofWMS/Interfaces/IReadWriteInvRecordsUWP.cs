using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    public interface IReadWriteInvRecordsUWP
    {
        Task<string> ReadInvRecordsAsync();
        Task<string> WriteInvRecordsAsync(string data);
    }
}
