using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMSDLL.ListDefinitions
{
    public class ListOfTRFRCVToExport
    {
        public int recordID { get; set; }
        public DateTime recordDate { get; set; }
        public string pood { get; set; }
        public string sihtpood { get; set; }
        public string dokno { get; set; }
        public string dokreanr { get; set; }
        public DateTime tarnekp { get; set; }
        public string kaup { get; set; }
        public decimal initialQty { get; set; }
        public decimal pickedQty { get; set; }
        public decimal remainingQty { get; set; }
        public string magnitude { get; set; }
    }
}
