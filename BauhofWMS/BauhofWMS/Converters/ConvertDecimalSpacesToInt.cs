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
    public class ConvertDecimalSpacesToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string numberToReturn = "0";
            if (value != null)
            {
                
                string docNo = (string)value;
                numberToReturn = docNo;
                if (numberToReturn.Contains(","))
                {
                    numberToReturn = docNo.Split(new[] { "," }, StringSplitOptions.None)[0];
                }
            }
            return numberToReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
