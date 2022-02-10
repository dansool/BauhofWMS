using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace BauhofWMS.Droid.Utils
{
   
    public class CloseApplication : ICloseApplication
    {
        private App obj = App.Current as App;
        public void closeApplication()
        {
            WriteLog WriteLog = new WriteLog();
            WriteLog.Write("Closing app!", obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB");
            var activity = (Activity)Forms.Context;            
            activity.FinishAndRemoveTask();
        }
    }

}