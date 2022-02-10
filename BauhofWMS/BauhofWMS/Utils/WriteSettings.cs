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
    public class WriteSettings
    {
        private App obj = App.Current as App;
        public async Task<Tuple<bool, string>> Write(bool pEnv, string address, string shopLocationCode, bool showInvQty, bool showPurchaseReceiveQty, bool showTransferReceiveQty, string shopID, string deviceID, MainPage mp)
        {
            try
            {
                mp.lstSettings = new List<ListOfSettings>();
                var row = new ListOfSettings
                {
                    pEnv = pEnv,
                    wmsAddress = address,
                    shopLocationCode = shopLocationCode,
                    currentVersion = obj.currentVersion,
                    showInvQty = showInvQty,
                    showPurchaseReceiveQty = showPurchaseReceiveQty,
                    showTransferReceiveQty = showTransferReceiveQty
                };
                mp.lstSettings.Add(row);
                var settings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
                string input = JsonConvert.SerializeObject(mp.lstSettings, settings);

                string settingsWrite = "";
                if (Device.RuntimePlatform == Device.Android)
                {
                    settingsWrite = await DependencyService.Get<IReadWriteSettingsAndroid>().SaveSettingsAsync(input, shopID, deviceID);
                }
                else
                {
                    settingsWrite = await DependencyService.Get<IReadWriteSettingsUWP>().SaveSettingsAsync(input);
                }
                if (string.IsNullOrEmpty(settingsWrite))
                {
                    return new Tuple<bool, string>(true, null);
                }
                else
                {
                    return new Tuple<bool, string>(false, settingsWrite);
                }
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "WriteSettings " + ex.Message);
            }

        }
    }
}
