using System;
using System.Collections.Generic;
using System.Text;

namespace BauhofWMS.Utils.Parsers
{
    public class TryParseDecimal
    {
        public decimal Parse(string quantity)
        {
            decimal value;
            bool tryparse = Decimal.TryParse(quantity, out value);
            if (tryparse)
            {
                return value;
            }
            return 0;
        }
    }
}
