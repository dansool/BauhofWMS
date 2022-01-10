using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BauhofWMSDLL.ListDefinitions;
using BauhofOffline.Utils;
using System.Diagnostics;

namespace BauhofOffline.Utils
{
    public class ParseArguments
    {
        private SplitString SplitString = new SplitString();

        internal Tuple<bool, string, List<ListOfStartupArgs>> Parse(MainWindow mp, string argument)
        {
            try
            {
                if (argument != string.Empty)
                {
                    List<ListOfStartupArgs> result = new List<ListOfStartupArgs>();
                    ListOfStartupArgs row = new ListOfStartupArgs();

                    string[] splittedArgument = SplitString.SplitBy(argument, "%%%");
                    foreach (string arg in splittedArgument)
                    {
                        if (arg.ToUpper().StartsWith("SHOWUI"))
                        {
                            Debug.WriteLine(arg.ToUpper());
                            row.showUI = Convert.ToBoolean(Convert.ToInt32(arg.ToUpper().Replace("SHOWUI=", "")));
                            mp.ui = row.showUI;
                        }
                        if (arg.ToUpper().StartsWith("CONVERTFILES"))
                        {
                            Debug.WriteLine(arg.ToUpper());
                            row.ConvertFiles = Convert.ToBoolean(Convert.ToInt32(arg.ToUpper().Replace("CONVERTFILES=", "")));
                        }
                    }
                    result.Add(row);
                    return new Tuple<bool, string, List<ListOfStartupArgs>>(true, null, result);
                }
            }
            catch(Exception ex)
            {
                string error = "ParseArguments " + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null);
                mp.WriteError(error);
                return new Tuple<bool, string, List<ListOfStartupArgs>>(false, error, new List<ListOfStartupArgs>());
            }
            return new Tuple<bool, string, List<ListOfStartupArgs>>(false, "EI ÕNNESTUNUD PARSIDA ARGUMENTE", new List<ListOfStartupArgs>());
        }
    }
}
