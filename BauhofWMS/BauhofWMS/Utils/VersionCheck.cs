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
    public class VersionCheck
    {
        MainPage mps;
        public async Task<Tuple<bool, string, string, string>> Check(string company, string pin, MainPage mp)
        {
            mps = mp;
            IDownloader downloader = DependencyService.Get<IDownloader>();

            var versionCheck = await mp.CheckNewVersion.Check(company, pin);
            if (!versionCheck.Item1)
            {
                mp.lblVersion.Text = "Versioon: " + versionCheck.Item4;
                Debug.WriteLine(versionCheck.Item2);
                if (versionCheck.Item2.StartsWith("LEITUD UUS"))
                {
                    var action = await mp.DisplayAlert("UUENDUS", "KAS INSTALLIDA UUS VERSIOON", "JAH", "EI");
                    if (action)
                    {
                        mp.CollapseAllStackPanels.Collapse(mp);
                        mp.stkPassword.IsVisible = true;
                        if (Device.RuntimePlatform == Device.UWP)
                        {
                            mp.currentVersion = await DependencyService.Get<IPlatformDetailsUWP>().DownloadAndInstall(versionCheck.Item3);
                        }
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            downloader.OnFileDownloaded += OnFileDownloaded;
                            Debug.WriteLine("http://www.develok.ee/BauhofWMS/Install/BauhofWMS" + ("." + versionCheck.Item3 + ".apk").Replace(".", ".x").Replace(".xapk", ".apk"));
                            downloader.DownloadFile("http://www.develok.ee/BauhofWMS/Install/BauhofWMS" + ("." + versionCheck.Item3 + ".apk").Replace(".", ".x").Replace(".xapk", ".apk"));

                        }
                    }
                    else
                    {
                        // mp.PreparePassword();
                    }
                }
                return new Tuple<bool, string, string, string>(true, null, versionCheck.Item3, versionCheck.Item4);
            }
            else
            {
                return new Tuple<bool, string, string, string>(true, null, versionCheck.Item3, versionCheck.Item4);
            }

        }

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (!e.FileSaved)
            {
                mps.DisplayAlert("UUENDAMINE", "INSTALLIFAILI EI LEITUD!", "OK");
            }
        }
    }
}
