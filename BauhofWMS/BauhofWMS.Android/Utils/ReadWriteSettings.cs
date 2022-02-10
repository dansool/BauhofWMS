using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using BauhofWMS.Droid.Utils;

[assembly: Dependency(typeof(ReadWriteSettings))]

namespace BauhofWMS.Droid.Utils
{
    public class ReadWriteSettings : IReadWriteSettingsAndroid
    {
        WriteLog WriteLog = new WriteLog();
        public async Task<string> SaveSettingsAsync(string settings, string shopLocationID, string deviceSerial)
        {
            string result = "";
            try
            {
                Console.WriteLine("line settings: " + settings);
                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "BauhofWMSSettings.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    await writer.WriteLineAsync(settings);
                }
                return result;
            }
            catch (Exception ex)
            {
                result = this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteLog.Write(result, shopLocationID, deviceSerial);
                return result;
            }
        }

        public async Task<string> ReadSettingsAsync(string shopLocationID, string deviceSerial)
        {
            var result = "";
            try
            {
                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "BauhofWMSSettings.txt");

                if (backingFile == null || !File.Exists(backingFile))
                {
                    return null;
                }
                using (var reader = new StreamReader(backingFile, true))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        Console.WriteLine("line: " + line);
                        result = line;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                WriteLog.Write(result, shopLocationID, deviceSerial);
                return result;
            }
        }
    }
}