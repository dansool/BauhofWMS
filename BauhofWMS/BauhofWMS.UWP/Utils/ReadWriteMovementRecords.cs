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
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (isoStore.FileExists("movementRecordsDB.txt"))
                {
                    isoStore.DeleteFile("movementRecordsDB.txt");
                }
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("movementRecordsDB.txt", FileMode.CreateNew, isoStore))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        writer.Write(data);
                    }
                }
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
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (isoStore.FileExists("movementRecordsDB.txt"))
                {
                    string result = "";
                    using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("movementRecordsDB.txt", FileMode.Open, isoStore))
                    {
                        using (var reader = new StreamReader(isoStream, true))
                        {
                            string line;
                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                result = result + line;
                            }

                        }
                    }
                    return result;
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