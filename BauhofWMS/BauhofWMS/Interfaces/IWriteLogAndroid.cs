using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    public interface IWriteLogAndroid
    {        
        Task<string> Write(string message, string shopID, string deviceID);
    }
}
