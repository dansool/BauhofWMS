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
using BauhofWMSDLL.ListDefinitions;

[assembly: Dependency(typeof(ReadWritedbRecords))]

namespace BauhofWMS.Droid.Utils
{
    public class ReadWritedbRecords : IReadWritedbRecordsAndroid
    {

        public async Task<string> ReaddbRecordsAsync()
        {
            var result = "";
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            foreach (var r in dir.ListFiles())
            {
                
                if (r.Name.ToUpper().Contains("DBRECORDS") && r.Name.ToUpper().EndsWith(".TXT"))
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
    }
}