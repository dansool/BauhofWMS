using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;

namespace BauhofWMS.Utils.Parsers
{
    public class TryParseDateTime
    {
        public Tuple<DateTime, string> Parse(string date)
        {
            if (date != null)
            {
                if (date != "")
                {
                    if (date.Length == 6)
                    {

                        string[] splittedDate = Split(date);
                        string finalDate = "20" + splittedDate[2] + "-" + splittedDate[1] + "-" + splittedDate[0] + " 00:00:00";
                        //Debug.WriteLine("finalDate " + finalDate);
                        bool tryparse = DateTime.TryParse(finalDate, CultureInfo.CurrentCulture, DateTimeStyles.None, out var value);
                        if (tryparse)
                        {
                            return new Tuple<DateTime, string>(value, null);
                        }
                        else
                        {
                            //mainPage.MessageBox("Kuupäev ei vasta formaadile MMyy või ddMMyy:" + "\r\n" + date);
                            return new Tuple<DateTime, string>(default(DateTime), "Kuupäev ei vasta formaadile MMyy või ddMMyy:" + "\r\n" + date);
                        }
                    }
                    else
                    {
                        if (date.Length == 4)
                        {
                            string[] splittedDate = Split(date + DateTime.Now.Year.ToString().Substring(2, 2));
                            string finalDate = "20" + splittedDate[2] + "-" + splittedDate[1] + "-" + splittedDate[0] + " 00:00:00";
                            bool tryparse = DateTime.TryParse(finalDate, CultureInfo.CurrentCulture, DateTimeStyles.None, out var value);
                            if (tryparse)
                            {
                                return new Tuple<DateTime, string>(value, null);
                            }
                            else
                            {
                                //mainPage.MessageBox("Kuupäev ei vasta formaadile MMyy või ddMMyy:" + "\r\n" + date);
                                return new Tuple<DateTime, string>(default(DateTime), "Kuupäev ei vasta formaadile MMyy või ddMMyy:" + "\r\n" + date);
                            }
                        }
                        else
                        {
                            DateTime value;
                            bool tryparse = DateTime.TryParse(date, out value);
                            if (tryparse)
                            {
                                return new Tuple<DateTime, string>(value, null);
                            }
                            return new Tuple<DateTime, string>(default(DateTime), "Viga " + this.GetType().Name + " klassi meetodis Parse" + "\r\n" + "Sisend " + date + " ei vasta standardile dd.mm.yy või ddmm või ddmmyy");
                        }
                    }
                }
            }
            return new Tuple<DateTime, string>(default(DateTime), null);
        }
        public string[] Split(string str)
        {
            string year = str.Substring(0, 2);
            string month = str.Substring(2, 2);
            string date = str.Substring(4, 2);
            return new string[] { date, month, year };
        }

    }
}
