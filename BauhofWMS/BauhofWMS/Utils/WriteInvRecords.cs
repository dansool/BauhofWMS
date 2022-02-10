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
    public class WriteInvRecords
    {
        public async Task<Tuple<bool, string>> Write(MainPage mp, string data, string shopID, string deviceID)
        {
            try
            {

                if (Device.RuntimePlatform == Device.Android)
                {
                    
                    var settingsRead = await DependencyService.Get<IReadWriteInvRecordsAndroid>().WriteInvRecordsAsync(data, shopID, deviceID);
                    if (string.IsNullOrEmpty(settingsRead))
                    {
                        Debug.WriteLine("settingsRead " + settingsRead);
                        return new Tuple<bool, string>(true, settingsRead);
                    }
                    Debug.WriteLine("false settingsRead " + settingsRead);
                    return new Tuple<bool, string>(false, settingsRead);
                }

                if (Device.RuntimePlatform == Device.UWP)
                {
                    var settingsWrite = await DependencyService.Get<IReadWriteInvRecordsUWP>().WriteInvRecordsAsync(data);
                    if (string.IsNullOrEmpty(settingsWrite))
                    {
                        return new Tuple<bool, string>(true, settingsWrite);
                    }
                    return new Tuple<bool, string>(false, settingsWrite);
                }


                if (Device.RuntimePlatform == Device.UWP)
                {

                }
                return new Tuple<bool, string>(false, null);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "WriteInvRecords " + ex.Message);
            }

        }
    }
}