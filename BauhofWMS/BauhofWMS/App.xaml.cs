using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BauhofWMS
{
    public partial class App : Application
    {
        public static INavigation GlobalNavigation { get; private set; }

        public MainPage mp;
        public bool scannerInitComplete;
        public string mainOperation = "";
        public string previousLayoutName = "";
        public string currentLayoutName = "";
        public string nextLayoutName = "";
        public string shopLocationCode = "";
        public bool isDeviceHandheld;
        public bool pEnv;
        public string deviceSerial;
        public string operatingSystem;
        public string wcfAddress;
        public string companyName;
        public string currentVersion;
        public string pin;
        public bool isScanAllowed = true;
        public bool searchLocalShop = true;
        public string shopLocationID = "";
        public bool showInvQty = false;
        public bool showPurchaseReceiveQty = false;
        public bool showTransferReceiveQty = false;
        public bool showPurchaseReceiveQtySum = false;
        public bool showTransferReceiveQtySum = false;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
