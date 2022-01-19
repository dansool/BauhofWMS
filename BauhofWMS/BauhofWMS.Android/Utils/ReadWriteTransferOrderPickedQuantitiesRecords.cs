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

[assembly: Dependency(typeof(ReadWriteTransferOrderPickedQuantitiesRecords))]

namespace BauhofWMS.Droid.Utils
{
    public class ReadWriteTransferOrderPickedQuantitiesRecords : IReadWriteTransferOrderPickedQuantitiesRecordsAndroid
    {
        public async Task<string> ReadTransferOrderPickedQuantitiesRecordsAsync()
        {
            var result = "";
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            foreach (var r in dir.ListFiles())
            {
                if (r.Name.ToUpper().StartsWith("TRFRCVRECORDSDB.TXT"))
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

        public async Task<string> WriteTransferOrderPickedQuantitiesRecordsAsync(string data)
        {
            var result = "";
            try
            {

                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                foreach (var r in dir.ListFiles())
                {
                    if (r.Name.ToUpper().StartsWith("TRFRCVRECORDSDB.TXT"))
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
                        var backingFile = Path.Combine(dir.AbsolutePath, "TRFRCVRECORDSDB.TXT");
                        using (StreamWriter writer = new StreamWriter(backingFile))
                        {
                            writer.Write(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }
    }
}