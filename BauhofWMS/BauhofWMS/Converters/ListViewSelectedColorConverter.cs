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
    public class ListViewSelectedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Color rw = Color.White;
                Color redw = Color.LightSkyBlue;
                bool isSelected = (bool)value;
                if (isSelected)
                {
                    return redw;
                }
                else
                {
                    return rw;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}