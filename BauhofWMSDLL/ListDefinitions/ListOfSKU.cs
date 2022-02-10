using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMSDLL.ListDefinitions
{
    public class ListOfSKU
    {
        public string SKU { get; set; }
        public decimal SKUqty { get; set; }
        public string SKUBin { get; set; }
        public string SKUBin2 { get; set; }
        public string itemMagnitude { get; set; }
        public bool SKUCurrentShop { get; set; }
        public string SKUShopName { get; set; }
    }
}
