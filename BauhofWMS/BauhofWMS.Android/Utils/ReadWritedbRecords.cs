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

                    //List<ListOfdbRecordsImport> values = File.ReadAllLines(backingFile).Skip(1).Select(v => FromCsv(v, r.Name, DateTime.Now)).ToList();
                    //Console.WriteLine("TOTAL " + values.Count.ToString());
                    //var test = File.OpenRead(backingFile);

                    //var pStream = new ProgressStream(test);
                    //pStream.BytesRead += new ProgressStreamReportDelegate(pStream_BytesRead);

                    //int bSize = 4320000;
                    //byte[] buffer = new byte[bSize];
                    //Stream s = new MemoryStream();
                    //int n;
                    //while ((n = pStream.Read(buffer, 0, bSize)) > 0)
                    //{
                    //    result = result + (Encoding.UTF8.GetString(buffer, 0, n));
                    //}
                    //Console.WriteLine(prefix);
                    //Console.Write(result.Length.ToString());
                    //Console.ReadKey();


                    using (var reader = new StreamReader(backingFile, true))
                    {
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            result = result + line;
                        }
                    }
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

        //public static ListOfdbRecordsImport FromCsv(string csvLine, string fileName, DateTime fileDate)
        //{
        //    ListOfdbRecordsImport lst = new ListOfdbRecordsImport();

        //    string[] values = csvLine.Split("\",");
        //    lst.fileName = fileName;
        //    lst.fileDate = fileDate;
        //    lst.itemCode = values[0];
        //    lst.itemDesc = values[1];
        //    lst.itemMagnitude = values[2];
        //    lst.price = !string.IsNullOrEmpty(values[3]) ? "0" : values[3];
        //    lst.SKU = values[4];
        //    lst.SKUqty = !string.IsNullOrEmpty(values[6]) ? "0" : values[6];
        //    lst.meistriklubihind = !string.IsNullOrEmpty(values[7]) ? "0" : values[7];
        //    lst.soodushind = !string.IsNullOrEmpty(values[8]) ? "0" : values[8];
        //    lst.profiklubihind = !string.IsNullOrEmpty(values[9]) ? "0" : values[9];
        //    lst.sortiment = values[10];

        //    //lst.SKUBin = values[12];
        //    lst.barCode = values[13];


        //    return lst;
        //}

        static void pStream_BytesRead(object sender, ProgressStreamReportEventArgs args)
        {
            var PercentProgress = Convert.ToInt32((args.StreamPosition * 100) / args.StreamLength);

            Console.WriteLine(PercentProgress);
        }
    }
}