using System;
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
using Android.Content;
using Xamarin.Essentials;
using System.Diagnostics;

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
	[IntentFilter(new[] { "com.darryncampbell.datawedge.xamarin.ACTION" }, Categories = new[] { Intent.CategoryDefault })]

	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
		private static string ACTION_DATAWEDGE_FROM_6_2 = "com.symbol.datawedge.api.ACTION";
		private static string EXTRA_CREATE_PROFILE = "com.symbol.datawedge.api.CREATE_PROFILE";
		private static string EXTRA_SET_CONFIG = "com.symbol.datawedge.api.SET_CONFIG";
		//private static string EXTRA_PROFILE_NAME = "Inventory DEMO";
		private static string EXTRA_PROFILE_NAME = "SaarioinenWMS";
		private DataWedgeReceiver _broadcastReceiver = null;


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
			var manufacturer = DeviceInfo.Manufacturer;
			if (manufacturer == "Zebra Technologies")
			{
				
				MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "DeviceVendor", "Zebra");
				_broadcastReceiver = new DataWedgeReceiver();

				_broadcastReceiver.scanDataReceived += (s, scanData) =>
				{
					System.Diagnostics.Debug.WriteLine(scanData);
					MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "ScanBarcode", scanData.TrimStart().TrimEnd());
				};
				CreateProfile();
			}
			else
			{
				MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "DeviceVendor", "Honeywell");
				ScannerInit.OpenBarcodeReader();
			}
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

		protected override void OnResume()
		{
			base.OnResume();
			var manufacturer = DeviceInfo.Manufacturer;
			if (manufacturer == "Zebra Technologies")
			{
				if (null != _broadcastReceiver)
				{
					// Register the broadcast receiver
					IntentFilter filter = new IntentFilter(DataWedgeReceiver.IntentAction);
					filter.AddCategory(DataWedgeReceiver.IntentCategory);
					Android.App.Application.Context.RegisterReceiver(_broadcastReceiver, filter);
				}
			}
		}

		protected override void OnPause()
		{
			var manufacturer = DeviceInfo.Manufacturer;
			if (manufacturer == "Zebra Technologies")
			{
				if (null != _broadcastReceiver)
				{
					// Unregister the broadcast receiver
					Android.App.Application.Context.UnregisterReceiver(_broadcastReceiver);
				}
			}
			base.OnStop();
		}

		private void CreateProfile()
		{
			String profileName = EXTRA_PROFILE_NAME;
			SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_CREATE_PROFILE, profileName);

			//  Now configure that created profile to apply to our application
			Bundle profileConfig = new Bundle();
			profileConfig.PutString("PROFILE_NAME", EXTRA_PROFILE_NAME);
			profileConfig.PutString("PROFILE_ENABLED", "true"); //  Seems these are all strings
			profileConfig.PutString("CONFIG_MODE", "UPDATE");
			Bundle barcodeConfig = new Bundle();
			barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
			barcodeConfig.PutString("RESET_CONFIG", "true"); //  This is the default but never hurts to specify
			Bundle barcodeProps = new Bundle();
			barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);
			profileConfig.PutBundle("PLUGIN_CONFIG", barcodeConfig);
			Bundle appConfig = new Bundle();
			appConfig.PutString("PACKAGE_NAME", this.PackageName);      //  Associate the profile with this app
			appConfig.PutStringArray("ACTIVITY_LIST", new String[] { "*" });
			profileConfig.PutParcelableArray("APP_LIST", new Bundle[] { appConfig });
			SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
			//  You can only configure one plugin at a time, we have done the barcode input, now do the intent output
			profileConfig.Remove("PLUGIN_CONFIG");
			Bundle intentConfig = new Bundle();
			intentConfig.PutString("PLUGIN_NAME", "INTENT");
			intentConfig.PutString("RESET_CONFIG", "true");
			Bundle intentProps = new Bundle();
			intentProps.PutString("intent_output_enabled", "true");
			intentProps.PutString("intent_action", DataWedgeReceiver.IntentAction);
			intentProps.PutString("intent_delivery", "2");
			intentConfig.PutBundle("PARAM_LIST", intentProps);
			profileConfig.PutBundle("PLUGIN_CONFIG", intentConfig);
			SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
		}

		private void SendDataWedgeIntentWithExtra(String action, String extraKey, Bundle extras)
		{
			Intent dwIntent = new Intent();
			dwIntent.SetAction(action);
			dwIntent.PutExtra(extraKey, extras);
			SendBroadcast(dwIntent);
		}

		private void SendDataWedgeIntentWithExtra(String action, String extraKey, String extraValue)
		{
			Intent dwIntent = new Intent();
			dwIntent.SetAction(action);
			dwIntent.PutExtra(extraKey, extraValue);
			SendBroadcast(dwIntent);
		}
	}
}