using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;


namespace BauhofWMS.Utils
{
    public class GetCurrentVersion
    {
        public async Task<string> Get()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                return DependencyService.Get<IPlatformDetailsUWP>().GetPlatformName();
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                var build = DependencyService.Get<IAppVersion>().GetBuild();
                var version = DependencyService.Get<IAppVersion>().GetVersion();
                return build + ".0." + version;
            }
            return null;
        }
    }
}
