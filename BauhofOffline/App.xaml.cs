using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BauhofOffline
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string mArgs { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            foreach (var arg in e.Args)
            {
                mArgs = arg;
                //mArgs = "showUI=false convertfiles=true";
            }
            mArgs = "showUI=1%%%convertfiles=1";
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }
    }
}
