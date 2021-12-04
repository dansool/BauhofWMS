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
    class ConvertSKUColor : IValueConverter
    {
        private App obj = App.Current as App;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string)value;
            Color lime = Color.Lime;
            Color red = Color.Red;

            if (val == obj.shopLocationCode)
            {
                return lime;
            }
            else
            {
                return red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}