﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    public interface IWriteTransferReceiveRecordsToExportFileAndroid
    {
        Task<string> WriteTransferReceiveRecordsToExportFileAsync(string data, string exportFile, string shopID, string deviceID);
    }
}
