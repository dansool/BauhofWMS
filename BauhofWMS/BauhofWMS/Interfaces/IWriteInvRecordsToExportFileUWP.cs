using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    public interface IWriteInvRecordsToExportFileUWP
    {
        Task<string> WriteInvRecordsToExportFileAsync(string data, string exportFile);
    }
}
