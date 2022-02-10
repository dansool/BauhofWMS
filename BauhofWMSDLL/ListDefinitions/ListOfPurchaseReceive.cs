using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMSDLL.ListDefinitions
{
    public class ListOfPurchaseReceive
    {
        public string shop { get; set; }
        public string docNo { get; set; }
        public string docLineNo { get; set; }
        public string vendorCode { get; set; }
        public string vendorName { get; set; }
        public string vendorReference { get; set; }
        public DateTime shipmentDate { get; set; }
        public string itemCode { get; set; }
        public string itemDesc { get; set; }
        public string barCode { get; set; }
        public decimal initialQty { get; set; }
        public string magnitude { get; set; }
        public int purchasePickedRowCount { get; set; }
        public int purchaseRowCount { get; set; }
        public bool purchaseOrderPicked { get; set; }

        public int shipToday { get; set; }
        public decimal pickedQty { get; set; }
        public decimal remaininQty { get; set; }
        public bool showPurchaseReceiveQty { get; set; }
    }
}
