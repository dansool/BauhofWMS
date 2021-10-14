using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMSDLL.ListDefinitions
{
    public class ListOfUser
    {
        public byte[] byteData { get; set; }
        public string scannerID { get; set; }
        public string pin { get; set; }
        public string username { get; set; }
        public string errorMessage { get; set; }
        public bool pEnv { get; set; }
        public string binPrefix { get; set; }
        public string visibleButtons { get; set; }
        public int printerRequired { get; set; }
    }
}
