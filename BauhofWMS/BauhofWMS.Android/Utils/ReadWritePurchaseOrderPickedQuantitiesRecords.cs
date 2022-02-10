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

[assembly: Dependency(typeof(ReadWritePurchaseOrderPickedQuantitiesRecords))]

namespace BauhofWMS.Droid.Utils
{
    public class ReadWritePurchaseOrderPickedQuantitiesRecords : IReadWritePurchaseOrderPickedQuantitiesRecordsAndroid
    {
        WriteLog WriteLog = new WriteLog();
        public async Task<string> ReadPurchaseOrderPickedQuantitiesRecordsAsync(string shopLocationID, string deviceSerial)
        {
            var result = "";
            try
            {
                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                foreach (var r in dir.ListFiles())
                {
                    if (r.Name.ToUpper().StartsWith("PURCRCVRECORDSDB.TXT"))
                    {
                        var backingFile = Path.Combine(dir.AbsolutePath, r.Name);

                        if (backingFile == null || !File.Exists(backingFile))
                        {
                            return null;
                        }
                        using (var reader = new StreamReader(backingFile, true))
                        {
                            string line;
                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                result = result + line;
                            }
                        }
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

        public async Task<string> WritePurchaseOrderPickedQuantitiesRecordsAsync(string data, string shopLocationID, string deviceSerial)
        {
            var result = "";
            try
            {
                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                foreach (var r in dir.ListFiles())
                {
                    if (r.Name.ToUpper().StartsWith("PURCRCVRECORDSDB.TXT"))
                    {
                        var backingFile = Path.Combine(dir.AbsolutePath, r.Name);
                        File.Move(Path.Combine(dir.AbsolutePath, r.Name), Path.Combine(dir.AbsolutePath, "BAK_" + r.Name));
                        using (StreamWriter writer = new StreamWriter(backingFile))
                        {
                            writer.Write(data);
                        }
                        File.Delete(Path.Combine(dir.AbsolutePath, "BAK_" + r.Name));
                    }
                    else
                    {
                        var backingFile = Path.Combine(dir.AbsolutePath, "PURCRCVRECORDSDB.TXT");
                        using (StreamWriter writer = new StreamWriter(backingFile))
                        {
                            writer.Write(data);
                        }
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