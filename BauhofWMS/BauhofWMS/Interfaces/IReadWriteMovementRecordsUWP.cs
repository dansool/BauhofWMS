using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    public interface IReadWriteMovementRecordsUWP
    {
        Task<string> ReadMovementRecordsAsync();
        Task<string> WriteMovementRecordsAsync(string data);
    }
}
