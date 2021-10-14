using System;
using System.Collections.Generic;
using System.Text;

namespace BauhofWMS
{
    public interface IAppVersion
    {
        string GetVersion();
        int GetBuild();
    }
}
