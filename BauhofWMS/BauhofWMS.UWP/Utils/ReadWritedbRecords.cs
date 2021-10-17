using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(BauhofWMS.UWP.Utils.ReadWritedbRecords))]
namespace BauhofWMS.UWP.Utils
{
    
    public class ReadWritedbRecords : IReadWritedbRecordsUWP
    {
        public async Task<string> ReaddbRecordsAsync()
        {
            try
            {
                var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/dbrecords_2021-10-14_13-34-33.txt"));
                String profileContent = await FileIO.ReadTextAsync(storageFile);
                return profileContent;
            }
            catch (Exception ex)
            {
                return "siin " + ex.Message + "  " + ex.InnerException;
            }

        }
    }
}
