using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace BauhofWMS
{
    public interface IWriteTransferReceiveRecordsToExportFileUWP
    {
        Task<string> WriteTransferReceiveRecordsToExportFileAsync(string data, string exportFile);
    }
}
