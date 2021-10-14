using System;
using System.Collections.Generic;
using System.Text;
using BauhofWMSDLL.ListDefinitions;
using BauhofWMSDLL.Utils;
using System.Threading.Tasks;
using System.Diagnostics;
using BauhofWMS.Utils;


namespace BauhofWMS.Utils
{
    public class GetPublishedVersion
    {
        CheckBauhofWMSVersion CheckBauhofWMSVersion = new CheckBauhofWMSVersion();
        public async Task<Tuple<bool, string, string>> Get()
        {
            try
            {
                var result = await CheckBauhofWMSVersion.Get();
                return new Tuple<bool, string, string>(true, null, result);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, string>(false, "GetPublishedVersion " + ex.Message, null);
            }
        }
    }
}
