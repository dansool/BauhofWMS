using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMS
{
    public interface IReadWriteSettingsUWP
    {
        Task<string> SaveSettingsAsync(string settings);
        Task<string> ReadSettingsAsync();
    }
}

