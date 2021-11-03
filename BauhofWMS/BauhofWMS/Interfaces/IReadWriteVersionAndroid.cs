using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace BauhofWMS
{
    public interface  IReadWriteVersionAndroid
    {
        Task<string> ReadVersionAsync();
        Task<string> WriteVersionAsync(string settings);
    }
}