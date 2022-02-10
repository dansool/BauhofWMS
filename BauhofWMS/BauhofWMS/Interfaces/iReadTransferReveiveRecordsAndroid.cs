using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace BauhofWMS
{
    public interface iReadTransferReveiveRecordsAndroid
    {
        Task<string> ReadRecordsAsync(string shopID, string deviceID);
    }
}
