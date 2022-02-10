using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace BauhofWMS
{
    public interface  IReadWriteVersionAndroid
    {
        Task<string> ReadVersionAsync(string shopID, string deviceID);
        Task<string> WriteVersionAsync(string settings, string shopID, string deviceID);
    }
}