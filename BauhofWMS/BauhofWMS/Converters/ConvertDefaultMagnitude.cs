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
    public class ConvertDefaultMagnitude : IValueConverter
    {
        private App obj = App.Current as App;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
           
            if (val)
            {
                return "(PÕHIMÕÕTÜHIK)";
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
