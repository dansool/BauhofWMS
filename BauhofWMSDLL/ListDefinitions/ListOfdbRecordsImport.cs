﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BauhofWMSDLL.ListDefinitions
{
    public class ListOfdbRecordsImport
    {
        public string fileName { get; set; }
        public DateTime fileDate { get; set; }
        public string itemCode { get; set; }
        public string barCode { get; set; }
        public string itemDesc { get; set; }
        public string itemMagnitude { get; set; }
        public string SKU { get; set; }
        public string SKUqty { get; set; }
        public string SKUBin { get; set; }
        public string price { get; set; }
        public string meistriklubihind { get; set; }
        public string profiklubihind { get; set; }
        public string sortiment { get; set; }
        public string soodushind { get; set; }

    }
}
