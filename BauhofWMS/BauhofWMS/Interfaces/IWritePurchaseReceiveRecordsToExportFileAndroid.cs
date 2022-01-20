using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    public interface IWritePurchaseReceiveRecordsToExportFileAndroid
    {
        Task<string> WritePurchaseReceiveRecordsToExportFileAsync(string data, string exportFile);
    }
}
