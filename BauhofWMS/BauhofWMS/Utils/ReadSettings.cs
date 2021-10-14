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
    public class ReadSettings
    {

        public async Task<Tuple<bool, string, List<ListOfSettings>>> Read(MainPage mp)
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var settingsRead = await DependencyService.Get<IReadWriteSettingsAndroid>().ReadSettingsAsync();
                    if (!string.IsNullOrEmpty(settingsRead))
                    {
                        var lstSettings = JsonConvert.DeserializeObject<List<ListOfSettings>>(settingsRead);
                        return new Tuple<bool, string, List<ListOfSettings>>(true, null, lstSettings);
                    }
                }

                if (Device.RuntimePlatform == Device.UWP)
                {
                    var settingsRead = await DependencyService.Get<IReadWriteSettingsUWP>().ReadSettingsAsync();
                    if (!string.IsNullOrEmpty(settingsRead))
                    {
                        var lstSettings = JsonConvert.DeserializeObject<List<ListOfSettings>>(settingsRead);
                        return new Tuple<bool, string, List<ListOfSettings>>(true, null, lstSettings);
                    }
                }
                return new Tuple<bool, string, List<ListOfSettings>>(false, null, new List<ListOfSettings>());
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, List<ListOfSettings>>(false, "ReadSettings " + ex.Message, new List<ListOfSettings>());
            }

        }
    }
}
