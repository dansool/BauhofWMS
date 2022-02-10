using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    public interface IWriteInvRecordsToExportFileAndroid
    {
        Task<string> WriteInvRecordsToExportFileAsync(string data, string exportFile, string shopID, string deviceID);
    }
}
