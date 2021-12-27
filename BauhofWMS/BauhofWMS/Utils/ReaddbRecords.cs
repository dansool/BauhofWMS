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
    public class ReaddbRecords
    {
        public async Task<Tuple<bool, string>> Read(MainPage mp, string prefix)
        {
            int x = 0;
            try
            {

                if (Device.RuntimePlatform == Device.Android)
                {
                    var settingsRead = await DependencyService.Get<IReadWritedbRecordsAndroid>().ReaddbRecordsAsync(prefix);
                    if (!string.IsNullOrEmpty(settingsRead))
                    {
                        return new Tuple<bool, string>(true, settingsRead);
                    }
                }
                

                if (Device.RuntimePlatform == Device.UWP)
                {
                    
                    var settingsRead = await DependencyService.Get<IReadWritedbRecordsUWP>().ReaddbRecordsAsync();
                    if (!string.IsNullOrEmpty(settingsRead))
                    {
                        return new Tuple<bool, string>(true, settingsRead);
                    }
                }
                return new Tuple<bool, string>(false, null);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "ReaddbRecords " + x + " " + ex.Message);
            }

        }
    }
}
