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

[assembly: Dependency(typeof(WriteLog))]


namespace BauhofWMS.Droid.Utils
{
    public class WriteLog : IWriteLogAndroid
    {
        public async Task<string> Write(string message, string shopID, string scannerID)
        {
            message = "" + "\r\n" + String.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now) + "\r\n" + message + "\r\n" + "==============";
            string exportFileName = shopID + "_" + scannerID + "_" + String.Format("{0:dd-MM-yyyy}", DateTime.Now) + "_" + "LOG.TXT";
            try
            {
                var _folder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                var _folderLogs = Path.Combine(_folder.AbsolutePath.ToString(), "Logs");
                Directory.CreateDirectory(_folderLogs);
                var exportFile = Path.Combine(_folderLogs, exportFileName);
                using (FileStream fs = new FileStream(exportFile, FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(message);
                }
                return null;
            }
            catch (Exception ex)
            {
                return "WriteLog " + ex.Message;
            }
        }
    }
}