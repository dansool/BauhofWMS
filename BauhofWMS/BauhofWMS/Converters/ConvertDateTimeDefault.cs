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
    class ConvertDateTimeDefault : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var DateValue = (DateTime)value;
            if (DateValue == System.Convert.ToDateTime("2100-01-01 00:00:00"))
            {
                return "";
            }
            else
            {
                var minDate = DateTime.Now.AddYears(-10);
                var maxDate = DateTime.Now.AddYears(10);
                if ((minDate < DateValue) && (maxDate > DateValue))
                {
               
                    return String.Format("{0:dd.MM.yyyy}", DateValue.ToLocalTime());
                }
                else
                {
                    return "";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}