using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using BauhofWMSDLL.ListDefinitions;

namespace BauhofWMS.Utils
{
    public class CheckNewVersion
    {
        private App obj = App.Current as App;
        ParseVersionToList ParseVersionToList = new ParseVersionToList();
        public GetPublishedVersion GetPublishedVersion = new GetPublishedVersion();
        public async Task<Tuple<bool, string, string, string, string>> Check(string company, string pin)
        {
            try
            {
                string currentVersion = null;
                string publishedVersion = null;
                var p = 0;

                if (Device.RuntimePlatform == Device.UWP)
                {
                    currentVersion = DependencyService.Get<IPlatformDetailsUWP>().GetPlatformName();
                }
                if (Device.RuntimePlatform == Device.Android)
                {
                    var build = DependencyService.Get<IAppVersion>().GetBuild();
                    var version = DependencyService.Get<IAppVersion>().GetVersion();
                    currentVersion = build + ".0." + version;
                    p = 3;
                    var versionWrite = await DependencyService.Get<IReadWriteVersionAndroid>().WriteVersionAsync(currentVersion, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                    if (versionWrite.Any())
                    {
                        Debug.WriteLine("Check: " + versionWrite);
                    }
                }
                var lstCurrentVersion = ParseVersionToList.Get(currentVersion);
                return new Tuple<bool, string, string, string, string>(true, null, publishedVersion, currentVersion, null);
               
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Tuple<bool, string, string, string, string>(false, "AAAAA " + ex.Message, null, null, null);
            }
        }
    }

}