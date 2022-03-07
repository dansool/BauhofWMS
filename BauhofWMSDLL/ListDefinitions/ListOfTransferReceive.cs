using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMSDLL.ListDefinitions
{
    public class ListOfTransferReceive
    {
        public string shop { get; set; }
        public string docNo { get; set; }
        public string docLineNo { get; set; }
        public string receivedFromShop { get; set; }
        public string receivedFromName { get; set; }
        public DateTime shipmentDate { get; set; }
        public string itemCode { get; set; }
        public string itemDesc { get; set; }
        public string barCode { get; set; }
        public decimal initialQty { get; set; }
        public string magnitude { get; set; }
        public decimal remainingQty { get; set; }
        public bool showTransferReceiveQty { get; set; }

        public int transferPickedRowCount { get; set; }
        public int transferRowCount { get; set; }
        public bool transferOrderPicked { get; set; }
        public decimal pickedQty { get; set; }
        public decimal remaininQty { get; set; }
    }
}
