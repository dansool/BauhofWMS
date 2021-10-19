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
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (isoStore.FileExists("invRecordsDB.txt"))
                {
                    isoStore.DeleteFile("invRecordsDB.txt");
                }
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("invRecordsDB.txt", FileMode.CreateNew, isoStore))
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
                return "WriteInvRecordsAsync " + ex.Message;
            }
        }
        public async Task<string> ReadInvRecordsAsync()
        {
            try
            {

                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (isoStore.FileExists("invRecordsDB.txt"))
                {
                    string result = "";
                    using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("invRecordsDB.txt", FileMode.Open, isoStore))
                    {
                        using (var reader = new StreamReader(isoStream, true))
                        {
                            string line;
                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                result = result + line;
                            }
                            
                        }
                        //using (StreamReader reader = new StreamReader(isoStream))
                        //{
                        //    result = result + reader.ReadLine();
                        //    return result;
                        //}
                    }
                    return result;
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
