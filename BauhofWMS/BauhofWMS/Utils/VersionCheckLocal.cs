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
    public class VersionCheckLocal
    {
        MainPage mps;
        public async Task<Tuple<bool, string, string, string>> Check(string company, string pin, MainPage mp)
        {
            mps = mp;
            IDownloader downloader = DependencyService.Get<IDownloader>();
            var latestVersion = "";

            string latestFilePath = "";
            //var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            //foreach (var r in dir.ListFiles())
            //{
            //    if (r.Name.ToUpper().StartsWith("BAUHOFWMS.") && r.Name.ToUpper().EndsWith(".APK"))
            //    {
            //        latestVersion = r.Name.ToUpper().Replace("BAUHOFWMS.", "").Replace("X", "").Replace(".APK", "");
            //        latestFilePath = r.Name;
            //    }
            //}



            var versionCheck = await mp.CheckNewVersion.Check(company, pin);
            if (versionCheck.Item1)
            {
                mp.lblVersion.Text = "Versioon: " + versionCheck.Item4;
                if (!string.IsNullOrEmpty(latestVersion))
                {
                    if (Convert.ToDecimal(latestVersion.Remove(0, 4)) > Convert.ToDecimal(versionCheck.Item4.Remove(0, 4)))
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
                                downloader.LaunchLocalFile(latestFilePath);
                            }
                        }
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

