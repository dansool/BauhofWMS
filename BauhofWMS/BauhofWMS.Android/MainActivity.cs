﻿using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BauhofWMS.Droid;
using Android;
using Honeywell.AIDC.CrossPlatform;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using BauhofWMS.Droid.Scanner;
using BauhofWMS.Droid.Utils;
using Android.Content;
using Android.Views.InputMethods;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using System.IO;


[assembly: Xamarin.Forms.Dependency(typeof(BauhofWMS.Droid.Utils.PlatformDetailsAndroid))]
[assembly: Xamarin.Forms.Dependency(typeof(BauhofWMS.Droid.Utils.Version_Android))]
[assembly: Xamarin.Forms.Dependency(typeof(BauhofWMS.Droid.Utils.CloseApplication))]

namespace BauhofWMS.Droid
{
    [Activity(
        Label = "xx",
        Icon = "@drawable/bauhof",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]


    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region Lists
        #endregion

        #region Utilis
        GetDeviceSerial GetDeviceSerial = new GetDeviceSerial();
        public ScannerInit ScannerInit = new ScannerInit();

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            string[] perm = new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.WriteExternalStorage, Manifest.Permission.WriteSettings };
            RequestPermissions(perm, 1);
            Permission WESCheck2 = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.ReadExternalStorage);
            Permission WESCheck = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteExternalStorage);
            Permission WSCheck = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteSettings);
            Console.WriteLine("WESCheck " + WESCheck.ToString());
            Console.WriteLine("WESCheck2 " + WESCheck.ToString());
            Console.WriteLine("WSCheck " + WSCheck.ToString());
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            ScannerInit.OpenBarcodeReader();
            LaunchStart();
        }

        private void CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                    RequestPermissions(permissions, 1);
                }
            }
        }

        public async void LaunchStart()
        {
            var result = await GetDeviceSerial.Get();
            if (result.Item1)
            {
                string deviceSerial = result.Item3;
                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "deviceSerial", deviceSerial);
                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "isDeviceHandheld", "true");
            }
            else
            {
                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "deviceSerial", "ERROR " + result.Item2);
            }
        }

        public override void OnBackPressed()
        {
            MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "backPressed", "");
            base.OnBackPressed();
        }

        public void requestpermission()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 1);
            }


            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 1);
            }
        }
    }
}