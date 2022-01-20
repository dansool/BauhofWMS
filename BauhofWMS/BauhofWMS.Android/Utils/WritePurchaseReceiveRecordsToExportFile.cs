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

[assembly: Dependency(typeof(WritePurchaseReceiveRecordsToExportFile))]
namespace BauhofWMS.Droid.Utils
{
    public class WritePurchaseReceiveRecordsToExportFile : IWritePurchaseReceiveRecordsToExportFileAndroid
    {
        public async Task<string> WritePurchaseReceiveRecordsToExportFileAsync(string data, string exportFileStamp)
        {
            string exportFileName = "PURCRCV_" + exportFileStamp + ".TXT";
            try
            {
                var _folder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                var _folderBackup = Path.Combine(_folder.AbsolutePath.ToString(), "Backup");
                var _folderExport = Path.Combine(_folder.AbsolutePath.ToString(), "Export");
                var _folderBackupTimeStamp = Path.Combine(_folderBackup, exportFileStamp);
                Directory.CreateDirectory(_folderBackup);
                Directory.CreateDirectory(_folderExport);
                Directory.CreateDirectory(_folderBackupTimeStamp);

                foreach (var r in _folder.ListFiles())
                {
                    if (r.Name.ToUpper() == "PURCRCVRECORDSDB.TXT")
                    {
                        var _folderBackupTimeStampFile = Path.Combine(_folderBackupTimeStamp, r.Name);
                        File.Move(r.Path, _folderBackupTimeStampFile);
                    };
                    if (r.Name.ToUpper().StartsWith("PURCRCV_"))
                    {
                        var _folderBackupTimeStampFile = Path.Combine(_folderBackupTimeStamp, r.Name);
                        File.Move(r.Path, _folderBackupTimeStampFile);
                    }
                }

                var exportFile = Path.Combine(_folderExport, exportFileName);
                using (StreamWriter writer = new StreamWriter(exportFile))
                {
                    writer.Write(data);
                }

                var _folderBackupTimeStampFile2 = Path.Combine(_folderBackupTimeStamp, exportFileName);
                File.Copy(exportFile, _folderBackupTimeStampFile2);
                return null;
            }
            catch (Exception ex)
            {
                return "WritePurchaseReceiveRecordsToExportFileAsync " + ex.Message + "  " + exportFileStamp;
            }
        }
    }
}