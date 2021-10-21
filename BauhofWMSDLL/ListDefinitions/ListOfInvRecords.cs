using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMSDLL.ListDefinitions
{
    public class ListOfInvRecords
    {
        public int recordID { get; set; }
        public DateTime recordDate { get; set; }
        public string itemCode { get; set; }
        public string itemDesc { get; set; }
        public string barCode { get; set; }
        public decimal quantity { get; set; }
        public string uom { get; set; }
        
    }
}
