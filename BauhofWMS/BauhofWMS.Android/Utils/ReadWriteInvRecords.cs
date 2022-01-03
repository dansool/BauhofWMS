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

[assembly: Dependency(typeof(ReadWriteInvRecords))]

namespace BauhofWMS.Droid.Utils
{
    public class ReadWriteInvRecords : IReadWriteInvRecordsAndroid
    {
        public async Task<string> ReadInvRecordsAsync()
        {
            var result = "";
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            foreach (var r in dir.ListFiles())
            {
                if (r.Name.ToUpper().StartsWith("INVRECORDSDB.TXT"))
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

        public async Task<string> WriteInvRecordsAsync(string data)
        {
            var result = "";
            try
            {
                
                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                var dir1 = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                var dir2 = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                //var dir3 = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
                //var dir4 = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.RootDirectory.AbsolutePath);
                //foreach (var r in dir.ListFiles())
                //{
                //    if (r.Name.ToUpper().StartsWith("INVRECORDSDB.TXT"))
                //    {
                //        var backingFile = Path.Combine(dir.AbsolutePath, r.Name);
                //        File.Delete(backingFile);
                //        using (StreamWriter writer = new StreamWriter(backingFile))
                //        {
                //            writer.Write(data);
                //        }
                //    }
                //    else
                //    {
                //        var backingFile = Path.Combine(dir.AbsolutePath, "INVRECORDSDB.TXT");
                //        using (StreamWriter writer = new StreamWriter(backingFile))
                //        {
                //            writer.Write(data);
                //        }
                //    }
                //}

                var backingFile = Path.Combine(dir.AbsolutePath, "INVRECORDSDB.TXT");
                using (StreamWriter writer = new StreamWriter(backingFile))
                {
                    writer.Write(data);
                }


                //var backingFile1 = Path.Combine(dir1.AbsolutePath, "INVRECORDSDB.TXT");
                //using (StreamWriter writer = new StreamWriter(backingFile1))
                //{
                //    writer.Write(data);
                //}

                //var backingFile2 = Path.Combine(dir2.AbsolutePath, "INVRECORDSDB.TXT");
                //using (StreamWriter writer = new StreamWriter(backingFile2))
                //{
                //    writer.Write(data);
                //}

              
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }
    }
}