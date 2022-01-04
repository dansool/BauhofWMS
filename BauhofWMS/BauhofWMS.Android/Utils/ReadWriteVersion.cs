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


[assembly: Dependency(typeof(ReadWriteVersion))]


namespace BauhofWMS.Droid.Utils
{
    public class ReadWriteVersion : IReadWriteVersionAndroid
    {
        public async Task<string> ReadVersionAsync()
        {
            var result = "";
            string DownloadsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);

            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            foreach (var r in dir.ListFiles())
            {
                if (r.Name.ToUpper().StartsWith("VERSION.TXT"))
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

        public async Task<string> WriteVersionAsync(string data)
        {
            try
            {
                var result = "";

                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                result = "leidsin dir " +  dir.ListFiles().Count();
                foreach (var r in dir.ListFiles())
                {
                    result = result +"\r\n" + r.Name;
                    if (r.Name.ToUpper().StartsWith("VERSION.TXT"))
                    {
                        var backingFile = Path.Combine(dir.AbsolutePath, r.Name);
                        File.Delete(backingFile);
                        using (StreamWriter writer = new StreamWriter(backingFile))
                        {
                            writer.Write(data);
                        }
                    }
                    else 
                    {
                        result = result +  "\r\n" + "leidsin, et pole faili";
                        var backingFile = Path.Combine(dir.AbsolutePath, "VERSION.TXT");
                        result = result + backingFile;
                        using (StreamWriter writer = new StreamWriter(backingFile))
                        {
                            writer.Write(data);
                        }
                        result = result + "\r\n" + "kirjutasin faili";
                    }
                }
                if (dir.ListFiles().Count() == 0)
                {
                    result = result + "\r\n" + "leidsin, et pole faili";
                    var backingFile = Path.Combine(dir.AbsolutePath, "VERSION.TXT");
                    using (StreamWriter writer = new StreamWriter(backingFile))
                    {
                        writer.Write(data);
                    }
                    result = result + "\r\n" + "kirjutasin faili";
                }

                return result;
            }
            catch(Exception ex)
            {
                return "WriteVersionAsync " + ex.Message;
            }
        }
    }
}