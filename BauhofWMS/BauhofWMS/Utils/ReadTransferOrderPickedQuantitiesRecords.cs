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
    public class ReadTransferOrderPickedQuantitiesRecords
    {
        public async Task<Tuple<bool, string>> Read(MainPage mp, string shopID, string deviceID)
        {
            try
            {

                if (Device.RuntimePlatform == Device.Android)
                {
                    var settingsRead = await DependencyService.Get<IReadWriteTransferOrderPickedQuantitiesRecordsAndroid>().ReadTransferOrderPickedQuantitiesRecordsAsync(shopID, deviceID);
                    if (!string.IsNullOrEmpty(settingsRead))
                    {
                        return new Tuple<bool, string>(true, settingsRead);
                    }
                }

                if (Device.RuntimePlatform == Device.UWP)
                {
                    var settingsRead = await DependencyService.Get<IReadWriteTransferOrderPickedQuantitiesRecordsUWP>().ReadTransferOrderPickedQuantitiesRecordsAsync();
                    if (!string.IsNullOrEmpty(settingsRead))
                    {
                        return new Tuple<bool, string>(true, settingsRead);
                    }
                }

                return new Tuple<bool, string>(false, null);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "ReadSettings " + ex.Message);
            }

        }
    }
}

