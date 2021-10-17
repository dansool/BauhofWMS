using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.IO.IsolatedStorage;
//using BauhofWMSDLL.ListDefinitions;

[assembly: Xamarin.Forms.Dependency(typeof(BauhofWMS.UWP.Utils.ReadWriteSettings))]

namespace BauhofWMS.UWP.Utils
{
    public class ReadWriteSettings : IReadWriteSettingsUWP
    {
        public async Task<string> SaveSettingsAsync(string settings)
        {
            try
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (isoStore.FileExists("BauhofWMSSettings.txt"))
                {
                    isoStore.DeleteFile("BauhofWMSSettings.txt");
                }
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("BauhofWMSSettings.txt", FileMode.CreateNew, isoStore))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine(settings);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return "SaveSettingsAsync " + ex.Message;
            }
        }

        public async Task<string> ReadSettingsAsync()
        {
            try
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (isoStore.FileExists("BauhofWMSSettings.txt"))
                {
                    using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("BauhofWMSSettings.txt", FileMode.Open, isoStore))
                    {
                        using (StreamReader reader = new StreamReader(isoStream))
                        {
                            string result = reader.ReadLine();
                            return result;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return "ReadSettingsAsync " + ex.Message;
            }
        }
    }
}