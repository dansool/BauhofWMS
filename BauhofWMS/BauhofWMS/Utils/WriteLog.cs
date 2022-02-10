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
    public class WriteLog
    {
        private App obj = App.Current as App;
        public async Task<Tuple<bool, string>> Write(MainPage mp, string message)
        {
            try
            {

                if (Device.RuntimePlatform == Device.Android)
                {
                    var settingsRead = await DependencyService.Get<IWriteLogAndroid>().Write(message, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB");
                    if (string.IsNullOrEmpty(settingsRead))
                    {
                        return new Tuple<bool, string>(true, settingsRead);
                    }
                    return new Tuple<bool, string>(false, settingsRead);
                }

                //if (Device.RuntimePlatform == Device.UWP)
                //{
                //    var settingsWrite = await DependencyService.Get<IWriteInvRecordsToExportFileUWP>().WriteInvRecordsToExportFileAsync(data, exportFile);
                //    if (string.IsNullOrEmpty(settingsWrite))
                //    {
                //        return new Tuple<bool, string>(true, settingsWrite);
                //    }
                //    return new Tuple<bool, string>(false, settingsWrite);
                //}

                return new Tuple<bool, string>(false, null);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "WriteInvRecordsToExportFile " + ex.Message);
            }

        }
    }
}

