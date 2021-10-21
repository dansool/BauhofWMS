using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.IO.IsolatedStorage;

[assembly: Xamarin.Forms.Dependency(typeof(BauhofWMS.UWP.Utils.ReadWriteMovementRecords))]

namespace BauhofWMS.UWP.Utils
{
    public class ReadWriteMovementRecords : IReadWriteMovementRecordsUWP
    {
        public async Task<string> WriteMovementRecordsAsync(string data)
        {
            try
            {
                Windows.Storage.StorageFolder _folder = await Windows.Storage.KnownFolders.DocumentsLibrary.CreateFolderAsync("Bauhof", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var p = await _folder.GetFilesAsync();
                foreach (var file in p)
                {
                    if (file.Name.ToUpper() == "MOVEMENTRECORDSDB.TXT")
                    {
                        await file.DeleteAsync();
                    }
                }

                Windows.Storage.StorageFile myFile = await _folder.CreateFileAsync("MOVEMENTRECORDSDB.TXT", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(myFile, data);
                return null;
            }
            catch (Exception ex)
            {
                return "WriteMovementRecordsAsync " + ex.Message;
            }
        }
        public async Task<string> ReadMovementRecordsAsync()
        {
            try
            {
                string result = "";
                Windows.Storage.StorageFolder _folder = await Windows.Storage.KnownFolders.DocumentsLibrary.CreateFolderAsync("Bauhof", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var p = await _folder.GetFilesAsync();
                foreach (var file in p)
                {
                    if (file.Name.ToUpper() == "MOVEMENTRECORDSDB.TXT")
                    {
                        result = await Windows.Storage.FileIO.ReadTextAsync(file);
                        return result;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return "ReadMovementRecordsAsync " + ex.Message;
            }
        }
    }
}
