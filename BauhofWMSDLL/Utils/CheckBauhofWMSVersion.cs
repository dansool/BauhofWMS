using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;


namespace BauhofWMSDLL.Utils
{
    public class CheckBauhofWMSVersion
    {
        public async Task<string> Get()
        {
            System.Net.Http.HttpClient httpClient = new HttpClient();
            var httpResponse = await httpClient.GetStringAsync("http://www.develok.ee/BauhofWMS/Install/BauhofWMSVersion2.txt");
			Debug.WriteLine("POLE FAILI!");
            return httpResponse.ToString();
        }
    }
}
