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
                
                if (r.Name.ToUpper().StartsWith("DBRECORDS") && r.Name.ToUpper().EndsWith(".TXT"))
                {
                    var backingFile = Path.Combine(dir.AbsolutePath, r.Name);

                    if (backingFile == null || !File.Exists(backingFile))
                    {
                        return null;
                    }

                    int i = 0;
                    Stopwatch sw = Stopwatch.StartNew();
                    const Int32 BufferSize = 81920;
                    using (var reader = new StreamReader(backingFile, Encoding.UTF8, true, BufferSize))
                    {
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            result = result + line;
                        }
                    }
                    sw.Stop();
                    System.Diagnostics.Debug.WriteLine("Time elapsed : " + sw.Elapsed.TotalMilliseconds.ToString() + " ms.");
                }
            }
            return result;
        }
    }
}