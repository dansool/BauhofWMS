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

[assembly: Dependency(typeof(WriteInvRecordsToExportFile))]

namespace BauhofWMS.Droid.Utils
{
    public class WriteInvRecordsToExportFile : IWriteInvRecordsToExportFileAndroid
    {
        public async Task<string> WriteInvRecordsToExportFileAsync(string data, string exportFileStamp)
        {
            string exportFileName = "inv_" + exportFileStamp + ".txt";
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
                    if (r.Name.ToUpper() == "INVRECORDSDB.TXT")
                    {
                        var _folderBackupTimeStampFile = Path.Combine(_folderBackupTimeStamp, r.Name);
                        File.Move(r.Path, _folderBackupTimeStampFile);
                    };
                    if (r.Name.ToUpper().StartsWith("INV_"))
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
                return "WriteInvRecordsAsync " + ex.Message + "  " + exportFileStamp;
            }
        }
    }
}