using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.ComponentModel;
using System.Globalization;


namespace BauhofWMS.Converters
{
    class ConvertDecimalSpaces : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string numberToReturn = "0";
            if (value != null)
            {
                decimal number = (decimal)value;
                numberToReturn = (String.Format("{0:0.00}", number)).Replace(".00", "");
            }
            return numberToReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
