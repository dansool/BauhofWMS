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
    public class ListViewPurchaseOrderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Color rw = Color.White;
                Color green = Color.LightGreen;
                int isPicked = (int)value;
                if (isPicked == 1)
                {
                    return green;
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