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

[assembly: Dependency(typeof(ReadWritedbRecords))]

namespace BauhofWMS.Droid.Utils
{
    public class ReadWritedbRecords : IReadWritedbRecordsAndroid
    {

        public async Task<string> ReaddbRecordsAsync(string prefix)
        {
            var result = "";
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            foreach (var r in dir.ListFiles())
            {
                if (r.Name.ToUpper().Contains(prefix + "_PDA_PRODUCTS") && r.Name.ToUpper().EndsWith(".TXT"))
                {
                    var backingFile = Path.Combine(dir.AbsolutePath, r.Name);

                    if (backingFile == null || !File.Exists(backingFile))
                    {
                        return null;
                    }

                    var test = File.OpenRead(backingFile);

                    var pStream = new ProgressStream(test);
                    pStream.BytesRead +=  new ProgressStreamReportDelegate(pStream_BytesRead);
                    
                    int bSize = 4320000;
                    byte[] buffer = new byte[bSize];
                    Stream s = new MemoryStream();
                    int n;
                    while ((n = pStream.Read(buffer, 0, bSize)) > 0)
                    {
                        result = result + (Encoding.UTF8.GetString(buffer, 0, n));
                    }
                    Console.WriteLine(prefix);
                    //Console.Write(result.Length.ToString());
                    //Console.ReadKey();


                    //using (var reader = new StreamReader(backingFile, true))
                    //{
                    //    string line;
                    //    while ((line = await reader.ReadLineAsync()) != null)
                    //    {
                    //        result = result + line;
                    //    }
                    //}
                }

                //if (r.Name.EndsWith(".txt"))
                //{
                //    var backingFile = Path.Combine(dir.AbsolutePath, r.Name);

                //    if (backingFile == null || !File.Exists(backingFile))
                //    {
                //        return null;
                //    }


                //    using (var reader = new StreamReader(backingFile, true))
                //    {
                //        string line;
                //        while ((line = await reader.ReadLineAsync()) != null)
                //        {
                //            result = result + r.Name + "%%%" + line + "???" + "\r\n";
                //        }
                //    }
                //    result = result;
                //}
            }
            return result;
        }

        static void pStream_BytesRead(object sender, ProgressStreamReportEventArgs args)
        {
            var PercentProgress = Convert.ToInt32((args.StreamPosition * 100) / args.StreamLength);

            Console.WriteLine(PercentProgress);
        }
    }
}