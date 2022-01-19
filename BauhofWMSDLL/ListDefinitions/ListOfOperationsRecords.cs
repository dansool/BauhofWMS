using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMSDLL.ListDefinitions
{
    public class ListOfOperationsRecords
    {
        public int inventoryRecords { get; set; }
        public int transferRecords { get; set; }
        public int purchaseReceiveRecords { get; set; }
        public int transferReceiveRecords { get; set; }
        public int dbRecords { get; set; }
        public DateTime dbRecordsUpdateDate { get; set; }
        public string locationCode { get; set; }


    }
}
