using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BauhofOffline.Utils
{
    public class SplitString
    {
        internal string[] SplitBy(string value, string splitValue)
        {
            return value.Split(new string[] { splitValue }, StringSplitOptions.None);
        }
    }
}
