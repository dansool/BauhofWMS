using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    public interface IWriteMovementRecordsToExportFileAndroid
    {
        Task<string> WriteMovementRecordsToExportFileAsync(string data, string exportFile, string shopID, string deviceID);
    }
}
