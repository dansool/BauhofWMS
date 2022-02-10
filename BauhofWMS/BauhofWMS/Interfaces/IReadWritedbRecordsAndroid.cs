using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace BauhofWMS
{
    public interface IReadWritedbRecordsAndroid
    {
        Task<string> ReaddbRecordsAsync(string shopID, string deviceID);
    }
}
