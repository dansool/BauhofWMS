using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.IO.IsolatedStorage;

[assembly: Xamarin.Forms.Dependency(typeof(BauhofWMS.UWP.Utils.WriteInvRecordsToExportFile))]

namespace BauhofWMS.UWP.Utils
{
    public class WriteInvRecordsToExportFile : IWriteInvRecordsToExportFileUWP
    {
        public async Task<string> WriteInvRecordsToExportFileAsync(string data, string exportFileStamp)
        {
            string exportFileName = "inv_" + exportFileStamp + ".txt";
            try
            {
                Windows.Storage.StorageFolder _folder = await Windows.Storage.KnownFolders.DocumentsLibrary.CreateFolderAsync("Bauhof", Windows.Storage.CreationCollisionOption.OpenIfExists);
                Windows.Storage.StorageFolder _folderBackup = await _folder.CreateFolderAsync("Backup", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var p = await _folder.GetFilesAsync();
                
                foreach (var file in p)
                {
                    if (file.Name == "invRecordsDB.txt")
                    {
                        Windows.Storage.StorageFolder _folderBackupTimeStamp = await _folderBackup.CreateFolderAsync(exportFileStamp, Windows.Storage.CreationCollisionOption.OpenIfExists);
                        await file.MoveAsync(_folderBackupTimeStamp);
                    };
                    if (file.Name.StartsWith("inv_"))
                    {
                        Windows.Storage.StorageFolder _folderBackupTimeStamp = await _folderBackup.CreateFolderAsync(exportFileStamp, Windows.Storage.CreationCollisionOption.OpenIfExists);
                        await file.MoveAsync(_folderBackupTimeStamp);
                    }
                }

                Windows.Storage.StorageFile myFile = await _folder.CreateFileAsync(exportFileStamp, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(myFile, data);
                return null;
            }
            catch (Exception ex)
            {
                return "WriteInvRecordsAsync " + ex.Message + "  " + exportFileStamp;
            }
        }
    }
}
