using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.IO.IsolatedStorage;

[assembly: Xamarin.Forms.Dependency(typeof(BauhofWMS.UWP.Utils.ReadWriteInvRecords))]

namespace BauhofWMS.UWP.Utils
{
    public class ReadWriteInvRecords : IReadWriteInvRecordsUWP
    {
        public async Task<string> WriteInvRecordsAsync(string data)
        {
            try
            {
                Windows.Storage.StorageFolder _folder = await Windows.Storage.KnownFolders.DocumentsLibrary.CreateFolderAsync("Bauhof", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var p = await _folder.GetFilesAsync();
                foreach (var file in p)
                {
                    if (file.Name.ToUpper() == "INVRECORDSDB.TXT")
                    {
                        await file.DeleteAsync();
                    }
                }

                Windows.Storage.StorageFile myFile = await _folder.CreateFileAsync("INVRECORDSDB.TXT", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(myFile, data);
                return null;
            }
            catch (Exception ex)
            {
                return "WriteInvRecordsAsync " + ex.Message;
            }
        }
        public async Task<string> ReadInvRecordsAsync()
        {
            try
            {
                string result = "";
                Windows.Storage.StorageFolder _folder = await Windows.Storage.KnownFolders.DocumentsLibrary.CreateFolderAsync("Bauhof", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var p = await _folder.GetFilesAsync();
                foreach (var file in p)
                {
                    if (file.Name.ToUpper() == "INVRECORDSDB.TXT")
                    {
                        result = await Windows.Storage.FileIO.ReadTextAsync(file);
                        return result;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return "ReadInvRecordsAsync " + ex.Message;
            }
        }
    }
}
