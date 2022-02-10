using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Honeywell.AIDC.CrossPlatform;
using BauhofWMS.Utils;
using BauhofWMS.StackPanelOperations;
using BauhofWMSDLL.ListDefinitions;
using Newtonsoft.Json;

namespace BauhofWMS.Utils
{
    public class WritePurchaseReceiveRecordsToExportFile
    {
        public async Task<Tuple<bool, string>> Write(MainPage mp, string data, string exportFile, string shopID, string deviceID)
        {
            try
            {

                if (Device.RuntimePlatform == Device.Android)
                {
                    Debug.WriteLine("data " + data);
                    Debug.WriteLine("exportFile " + exportFile);
                    var settingsRead = await DependencyService.Get<IWritePurchaseReceiveRecordsToExportFileAndroid>().WritePurchaseReceiveRecordsToExportFileAsync(data, exportFile, shopID, deviceID);
                    Debug.WriteLine("settingsRead");
                    if (string.IsNullOrEmpty(settingsRead))
                    {
                        Debug.WriteLine("respnse");
                        return new Tuple<bool, string>(true, settingsRead);
                    }
                    return new Tuple<bool, string>(false, settingsRead);
                }

                if (Device.RuntimePlatform == Device.UWP)
                {
                    var settingsWrite = await DependencyService.Get<IWritePurchaseReceiveRecordsToExportFileUWP>().WritePurchaseReceiveRecordsToExportFileAsync(data, exportFile);
                    if (string.IsNullOrEmpty(settingsWrite))
                    {
                        return new Tuple<bool, string>(true, settingsWrite);
                    }
                    return new Tuple<bool, string>(false, settingsWrite);
                }

                return new Tuple<bool, string>(false, null);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "WritePurchaseReceiveRecordsToExportFile " + ex.Message);
            }

        }
    }
}

