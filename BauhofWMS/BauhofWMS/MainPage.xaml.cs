using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Honeywell.AIDC.CrossPlatform;
using BauhofWMS.Utils;
using BauhofWMS.StackPanelOperations;
using BauhofWMSDLL.ListDefinitions;
using Newtonsoft.Json;
using BauhofWMS.Templates;
//using BauhofWMSDLL.WCF;
using BauhofWMSDLL.Utils;
using BauhofWMS.Keyboard;
using BauhofWMS.Scanner;
using BauhofWMS.Utils.Parsers;
using System.Threading;
//using BauhofWMS.Utils.Parsers;


namespace BauhofWMS
{
    public partial class MainPage : ContentPage
    {
        #region Utils
        public CollapseAllStackPanels CollapseAllStackPanels = new CollapseAllStackPanels();
        public ShowKeyBoard ShowKeyBoard = new ShowKeyBoard();
        public CheckNewVersion CheckNewVersion = new CheckNewVersion();
        public KeyBoardButtonPress KeyBoardButtonPress = new KeyBoardButtonPress();
        public ScannedDataProcess ScannedDataProcess = new ScannedDataProcess();
        public ReadSettings ReadSettings = new ReadSettings();
        public ReaddbRecords ReaddbRecords = new ReaddbRecords();
        public ReadInvRecords ReadInvRecords = new ReadInvRecords();
        public VersionCheck VersionCheck = new VersionCheck();
        public CharacterReceived CharacterReceived = new CharacterReceived();
        public VirtualKeyboardTypes VirtualKeyboardTypes = new VirtualKeyboardTypes();
        public BackKeyPress BackKeyPress = new BackKeyPress();
        public WriteSettings WriteSettings = new WriteSettings();
        public GetCurrentVersion GetCurrentVersion = new GetCurrentVersion();
        public TryParseDateTime TryParseDateTime = new TryParseDateTime();
        public TryParseDecimal TryParseDecimal = new TryParseDecimal();
        public WriteInvRecords WriteInvRecords = new WriteInvRecords();
        #endregion
        #region Variables
        protected override bool OnBackButtonPressed() => true;
        private App obj = App.Current as App;
        public string ProfileFileName = "HoneywellDecoderSettingsV2.exm";
        public string currentScannedValue = "";
        public string currentVersion = "";
        public bool YesNoResult = false;
        public bool YesNoStarted = false;
        public string focusedEditor = "";
        public string passWord = "";
        public bool wcfDebug = false;
        public bool messageBoxLock;
        public Color productionColor = Color.FromHex("#FF0C154F");
        public Color testColor = Color.FromHex("#FFB24646");
        public string salesOrderNoSelected = "";
        public string salesOrderVendorSelected = "";
        public string salesSelectedeANCode = "";

        public string purchaseOrderNoSelected = "";
        public int purchaseOrderLineNoSelected = 0;
        public string purchaseOrderVendorSelected = "";
        public string purchaseSelectedeANCode = "";
        public bool overRideLotNo;
        public string binPrefix = "";

        public string stockTakeLocationSelected = "";
        public string stockTakeBinNoSelected = "";
        public int binUsed = 0;
        public string stockTakeSelectedeANCode = "";

        public string binContentsLocationsSelected = "";
        public string binContentsBinNoSelected = "";
        public string binContentsItemSelected = "";

        public int transferMovementType = 0;
        public string transferJournalBatch = "";
        public string transferLocationCode = "";
        public int transferBinUsed = 0;
        public string transferBinNo = "";

        public bool transferManualSelection = false;
        public string transfereANCode = "";
        public bool progressBarActive = false;
        string tempString = "";

        public int invRecordID = 0;
        public int movementRecordID = 0;
        #endregion
        #region lists
        public List<ListOfSettings> lstSettings = new List<ListOfSettings>();
        public List<ListOfSettings> lstSet = new List<ListOfSettings>();
        public List<ListOfOperationsRecords> lstOperationsRecords = new List<ListOfOperationsRecords>();
        public List<ListOfdbRecords> lstInternalRecordDB = new List<ListOfdbRecords>();
        public List<ListOfInvRecords> lstInternalInvDB = new List<ListOfInvRecords>();
        public List<ListOfInvToExport> lstInvToExport = new List<ListOfInvToExport>();
        #endregion
        

        #region MainPage operations

        public MainPage()
        {
            InitializeComponent();
            obj.mp = this;
            StartMainPage();
        }

        public async void StartMainPage()
        {

            try
            {
                if (string.IsNullOrEmpty(obj.deviceSerial))
                {
                    bool proceed = true;
                    obj.currentLayoutName = "MainPage";
                    obj.mainOperation = "";
                    obj.nextLayoutName = "";
                    obj.previousLayoutName = "";
                    CollapseAllStackPanels.Collapse(this);
                    grdMain.IsVisible = false;

                    Device.SetFlags(new string[] { "RadioButton_Experimental" });
                    grdMain.IsVisible = true;
                    grdProgressBar.IsVisible = true;
                    progressBarActive = true;
                    if (Device.RuntimePlatform == Device.UWP) { obj.operatingSystem = "UWP"; UWP(); }
                    if (Device.RuntimePlatform == Device.Android) { obj.operatingSystem = "Android"; Android(); }

                    ScannedValueReceive();


                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        var resultReaddbRecords = await ReaddbRecords.Read(this);
                        if (resultReaddbRecords.Item1)
                        {
                            if (!string.IsNullOrEmpty(resultReaddbRecords.Item2))
                            {
                                JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                lstInternalRecordDB = JsonConvert.DeserializeObject<List<ListOfdbRecords>>(resultReaddbRecords.Item2, jSONsettings);
                                progressBarActive = false;
                                Debug.WriteLine("lstInternalRecordDB2 + " + lstInternalRecordDB.Count());
                            }
                        }
                        else
                        {
                            Debug.WriteLine("=====resultReaddbRecords.Item2  ERROR " + resultReaddbRecords.Item2);
                        }

                        grdProgressBar.IsVisible = false;
                        progressBarActive = false;
                        
                    }

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        var resultReaddbRecords = await ReaddbRecords.Read(this);
                        if (resultReaddbRecords.Item1)
                        {
                            if (!string.IsNullOrEmpty(resultReaddbRecords.Item2))
                            {
                                JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                lstInternalRecordDB = JsonConvert.DeserializeObject<List<ListOfdbRecords>>(resultReaddbRecords.Item2, jSONsettings);
                                progressBarActive = false;
                            }
                        }
                        grdProgressBar.IsVisible = false;
                        progressBarActive = false;
                    }
                    Debug.WriteLine("lstInternalRecordDB + " + lstInternalRecordDB.Count());

                    var resultSettings = await ReadSettings.Read(this);
                    
                    if (resultSettings.Item1)
                    {
                        lstSettings = resultSettings.Item3;
                        obj.wcfAddress = lstSettings.First().wmsAddress;
                        obj.shopLocationCode = !string.IsNullOrEmpty(lstSettings.First().shopLocationCode) ? lstSettings.First().shopLocationCode.ToUpper() : "";

                        obj.pEnv = lstSettings.First().pEnv;
                        obj.companyName = null;
                        if (!string.IsNullOrEmpty(lstSettings.First().wmsAddress))
                        {
                            obj.companyName = lstSettings.First().wmsAddress.Contains("/") ? lstSettings.First().wmsAddress.Split(new[] { "/" }, StringSplitOptions.None).Last().ToUpper().Replace("WMS", "") : null;
                        }

                        ediAddress.Text = obj.wcfAddress;
                        if (obj.pEnv)
                        {
                            rbtnProduction.IsChecked = true;
                            EnvironmentColorChange(productionColor);
                        }
                        else
                        {
                            rbtnTest.IsChecked = true;
                            EnvironmentColorChange(testColor);
                        }
                        
                        //if (string.IsNullOrEmpty(obj.wcfAddress))
                        //{
                        //    proceed = false;
                        //    grdMain.IsVisible = true;
                        //    PrepareSettings();
                        //}
                        if (!string.IsNullOrEmpty(obj.shopLocationCode))
                        {
                            shopLocationCode.Text = obj.shopLocationCode;
                        }
                    }
                    else
                    {

                        proceed = false;
                        grdMain.IsVisible = true;
                        PrepareSettings();
                    }
                    if (proceed)
                    {
                        grdMain.IsVisible = true;
                        var version = await GetCurrentVersion.Get();
                        if (!string.IsNullOrEmpty(version))
                        {
                            if (lstSettings.Any())
                            {
                                lstSettings.First().currentVersion = version;
                                lblVersion.Text = "Versioon: " + lstSettings.First().currentVersion;
                            }
                            else
                            {
                                lstSettings = new List<ListOfSettings>();
                                lstSettings.Add(new ListOfSettings { currentVersion = version });
                                lblVersion.Text = "Versioon: " + lstSettings.First().currentVersion;
                            }
                        }
                        PrepareOperations();

                    }
                    }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("VIIGA " + ex.Message);
            }
        }

        public async void UWP()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete " + arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); obj.deviceSerial = (arg.Length > 20 ? arg.Take(19).ToString() : arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "isDeviceHandheld", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { obj.isDeviceHandheld = Convert.ToBoolean(arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "backPressed", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { BackKeyPress.Press(this); }); });
            MessagingCenter.Subscribe<App, string>(this, "KeyboardListener", (sender, args) => { CharacterReceived.Receive(null, args, this); });


           
        }

        public async void Android()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadComplete", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { CollapseAllStackPanels.Collapse(this); stkOperations.IsVisible = true; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "isDeviceHandheld", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { obj.isDeviceHandheld = Convert.ToBoolean(arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); obj.deviceSerial = (arg.Length > 20 ? arg.Take(19).ToString() : arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "backPressed", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { BackKeyPress.Press(this); }); });

            
        }


      

        public async void DisplaySuccessMessage(string message)
        {
            try
            {
                lblDisplaySuccess.Text = message;
                grdDisplaySuccess.IsVisible = true;
                grdDisplaySuccess.Opacity = 1.0;
                for (double opacity = 1.0; opacity >= 0.0; opacity = opacity - .03)
                {
                    if (opacity >= 0.9)
                    {
                        await Task.Delay(150);
                        grdDisplaySuccess.Opacity = opacity;
                    }
                    else
                    {
                        if (opacity >= 0.1)
                        {
                            await Task.Delay(20);
                            grdDisplaySuccess.Opacity = opacity;
                        }
                        else
                        {
                            grdDisplaySuccess.Opacity = 0;
                            grdDisplaySuccess.IsVisible = false;
                            lblDisplaySuccess.Text = "";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        public async void DisplayFailMessage(string message)
        {
            try
            {
                lblDisplayFail.Text = message;
                grdDisplayFail.IsVisible = true;
                grdDisplayFail.Opacity = 1.0;
                for (double opacity = 1.0; opacity >= 0.0; opacity = opacity - .03)
                {
                    if (opacity >= 0.9)
                    {
                        await Task.Delay(150);
                        grdDisplayFail.Opacity = opacity;
                    }
                    else
                    {
                        if (opacity >= 0.1)
                        {
                            await Task.Delay(20);
                            grdDisplayFail.Opacity = opacity;
                        }
                        else
                        {
                            grdDisplayFail.Opacity = 0;
                            grdDisplayFail.IsVisible = false;
                            lblDisplayFail.Text = "";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<bool> YesNoDialog(string header, string message, bool exit)
        {
            try
            {
                var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                var modalPage = new YesNo(header, message, exit);
                modalPage.Disappearing += (sender2, e2) =>
                {
                    waitHandle.Set();
                };
                await this.Navigation.PushModalAsync(modalPage);
                await Task.Run(() => waitHandle.WaitOne());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return YesNoResult;
        }


       

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            BackKeyPress.Press(this);
        }
        #endregion

        #region ScannedValue operations
        public void ScannedValueReceive()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannedValue", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { ProcessScannedValue(arg); }); });
        }

        public async void ProcessScannedValue(string scannedValue)
        {
            string scannedData = "";
            string scannedSymbology = "";
            if (scannedValue.Contains("###"))
            {
                var split = scannedValue.Split(new[] { "###" }, StringSplitOptions.None);
                scannedData = split[0];
                scannedSymbology = split[1];
            }
            else
            {
                scannedData = scannedValue;
            }
            await ScannedDataProcess.DataReceived(scannedData, scannedSymbology, this);
        }
        #endregion

        #region KeyBoardOperations
        private void KeyboardButton_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("classID ei tea");
            Button btn = (Button)sender;
            Color btnColor = btn.BackgroundColor;
            btn.BackgroundColor = Color.Gold;
            Device.StartTimer(TimeSpan.FromSeconds(0.05), () => { btn.BackgroundColor = btnColor; return false; });
            var classID = (sender as Button).ClassId;
            Debug.WriteLine("classID " + classID);
            KeyBoardButtonPress.KeyPress(classID, this);
        }

        public bool CheckIfDefaultText(string entryText, string entryClassID)
        {
            if (entryText == "KOGUS" || entryText == "PARTII")
            {
                Application.Current.Properties[entryClassID] = entryText;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Entry_FocusedNumericWithSwitch(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;
            if (CheckIfDefaultText(current.Text, current.ClassId)) { current.Text = ""; }

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===
            if (focusedEditor.StartsWith("edi"))
            {
                if (focusedEditor != current.ClassId)
                {
                    Editor previous = this.FindByName<Editor>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(focusedEditor))
                {
                    if (focusedEditor != current.ClassId)
                    {
                        Entry previous = this.FindByName<Entry>(focusedEditor);
                        previous.BackgroundColor = Color.White;
                    }
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
        }

        private void Entry_FocusedNumeric(object sender, FocusEventArgs e)
        {

            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;
            if (CheckIfDefaultText(current.Text, current.ClassId)) { current.Text = ""; }

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===

            //if (!string.IsNullOrEmpty(focusedEditor))
            //{
            if (focusedEditor.StartsWith("edi"))
            {
                if (focusedEditor != current.ClassId)
                {
                    Editor previous = this.FindByName<Editor>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            else
            {
                if (focusedEditor != current.ClassId)
                {
                    Entry previous = this.FindByName<Entry>(focusedEditor);
                    if (previous != null)
                    {
                        previous.BackgroundColor = Color.White;
                    }
                }
            }
            //}
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
        }

        private void Entry_FocusedNumericWithPlusMinus(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;
            if (CheckIfDefaultText(current.Text, current.ClassId)) { current.Text = ""; }

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===
            if (focusedEditor.StartsWith("edi"))
            {
                if (focusedEditor != current.ClassId)
                {
                    Editor previous = this.FindByName<Editor>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(focusedEditor))
                {
                    if (focusedEditor != current.ClassId)
                    {
                        Entry previous = this.FindByName<Entry>(focusedEditor);
                        previous.BackgroundColor = Color.White;
                    }
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithPlusMinus, this);
        }

        private void Entry_FocusedNumericWithSwitchAndPlusMinus(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;
            if (CheckIfDefaultText(current.Text, current.ClassId)) { current.Text = ""; }

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===
            if (focusedEditor.StartsWith("edi"))
            {
                if (focusedEditor != current.ClassId)
                {
                    Editor previous = this.FindByName<Editor>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(focusedEditor))
                {
                    if (focusedEditor != current.ClassId)
                    {
                        Entry previous = this.FindByName<Entry>(focusedEditor);
                        previous.BackgroundColor = Color.White;
                    }
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitchAndPlusMinus, this);
        }

        private void Entry_FocusedKeyboard(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;
            if (CheckIfDefaultText(current.Text, current.ClassId)) { current.Text = ""; }

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===
            if (focusedEditor.StartsWith("edi"))
            {
                if (focusedEditor != current.ClassId)
                {
                    Editor previous = this.FindByName<Editor>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(focusedEditor))
                {
                    if (focusedEditor != current.ClassId)
                    {
                        Entry previous = this.FindByName<Entry>(focusedEditor);
                        previous.BackgroundColor = Color.White;
                    }
                }
            }
            focusedEditor = current.ClassId;

            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Keyboard, this);
        }

        private void Editor_FocusedKeyboardWithSwitch(object sender, FocusEventArgs e)
        {
            Editor current = sender as Editor;
            current.BackgroundColor = Color.Yellow;
            if (CheckIfDefaultText(current.Text, current.ClassId)) { current.Text = ""; }

            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===
            if (focusedEditor.StartsWith("edi"))
            {
                if (focusedEditor != current.ClassId)
                {
                    Editor previous = this.FindByName<Editor>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
                focusedEditor = current.ClassId;
                ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.KeyboardWithSwitch, this);
            }
            else
            {
                if (focusedEditor.StartsWith("shopLocationCode"))
                {
                    if (focusedEditor != current.ClassId)
                    {
                        Editor previous = this.FindByName<Editor>(focusedEditor);
                        previous.BackgroundColor = Color.White;
                    }
                    focusedEditor = current.ClassId;
                    ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.KeyboardWithSwitch, this);
                }
                else
                {
                    if (!string.IsNullOrEmpty(focusedEditor))
                    {
                        if (focusedEditor != current.ClassId)
                        {
                            Entry previous = this.FindByName<Entry>(focusedEditor);
                            previous.BackgroundColor = Color.White;

                        }
                    }
                }
                focusedEditor = current.ClassId;
                ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.KeyboardWithSwitch, this);
            }

        }

        private void Entry_FocusedKeyboardWithSwitch(object sender, FocusEventArgs e)
        {
            Entry current = sender as Entry;
            current.BackgroundColor = Color.Yellow;
            if (CheckIfDefaultText(current.Text, current.ClassId)) { current.Text = ""; }
            Debug.WriteLine("focusedEditor on " + focusedEditor);
            //Needed for Android
            current.Focus();
            current.Unfocus();
            //===
            if (focusedEditor.StartsWith("edi"))
            {
                if (focusedEditor != current.ClassId)
                {
                    Editor previous = this.FindByName<Editor>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(focusedEditor))
                {
                    if (focusedEditor != current.ClassId)
                    {
                        Entry previous = this.FindByName<Entry>(focusedEditor);
                        previous.BackgroundColor = Color.White;
                    }
                }
                focusedEditor = current.ClassId;
                ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.KeyboardWithSwitch, this);
            }

        }

        private void Entry_FocusedNoEntry(object sender, FocusEventArgs e)
        {
            if (focusedEditor.StartsWith("edi"))
            {
                Editor current = sender as Editor;
                current.BackgroundColor = Color.Yellow;

                //Needed for Android
                current.Focus();
                current.Unfocus();
                //===
                if (focusedEditor != current.ClassId)
                {
                    Editor previous = this.FindByName<Editor>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                }
                focusedEditor = current.ClassId;
                grdKeyBoards.IsVisible = false;
            }
            else
            {
                Entry current = sender as Entry;
                
                current.BackgroundColor = Color.Yellow;
                //Needed for Android
                current.Focus();
                current.Unfocus();
                //===

                if (!string.IsNullOrEmpty(focusedEditor))
                {
                    if (focusedEditor != current.ClassId)
                    {
                        Entry previous = this.FindByName<Entry>(focusedEditor);
                        previous.BackgroundColor = Color.White;
                    }
                }
                focusedEditor = current.ClassId;
                grdKeyBoards.IsVisible = false;
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                if (sender.GetType().ToString() == "CustomizedControl.CustomEntry" || sender.GetType().ToString() == "Xamarin.Forms.Editor")
                {
                    if (focusedEditor.StartsWith("edi"))
                    {
                        Editor current = sender as Editor;
                        if (focusedEditor != current.ClassId)
                        {
                            current.BackgroundColor = Color.White;
                            if (current.Text.Length == 0)
                            {
                                current.Text = (string)Application.Current.Properties[current.ClassId];
                            }
                        }
                    }
                    else
                    {

                        Entry current = sender as Entry;
                        if (focusedEditor != current.ClassId)
                        {
                            current.BackgroundColor = Color.White;
                            if (current.Text.Length == 0)
                            {
                                current.Text = (string)Application.Current.Properties[current.ClassId];
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(focusedEditor))
                    {
                        Entry previous = this.FindByName<Entry>(focusedEditor);
                        previous.BackgroundColor = Color.White;
                        if (previous.Text.Length == 0)
                        {
                            previous.Text = (string)Application.Current.Properties[previous.ClassId];
                        }
                        focusedEditor = "";
                    }
                    else
                    {
                        ShowKeyBoard.Hide(this);
                    }
                    Debug.WriteLine("UNFOCUS!!!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Control_Unfocused(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(focusedEditor))
                {
                    Entry previous = this.FindByName<Entry>(focusedEditor);
                    previous.BackgroundColor = Color.White;
                    if (previous.Text.Length == 0)
                    {
                        previous.Text = (string)Application.Current.Properties[previous.ClassId];
                    }
                    focusedEditor = "";
                    ShowKeyBoard.Hide(this);
                }
                else
                {
                    ShowKeyBoard.Hide(this);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ShowKeyBoard.Hide(this);
            }
        }
        #endregion

        #region stkPassword
        public void PreparePassword()
        {
            Debug.WriteLine("password started " + obj.currentLayoutName);
            CollapseAllStackPanels.Collapse(this);
            stkPassword.IsVisible = true;
            stkPassword.IsEnabled = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "Password";
            focusedEditor = "entPassword";
            entPassword.Text = "";
            passWord = "";
            entPassword.BackgroundColor = Color.Yellow;
            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Debug.WriteLine("ONAPPEARING : " + obj.currentLayoutName);
            if (obj.currentLayoutName == "Password")
            {
                entPassword.Focus();
            }
        }
        
            
        private async void btnPasswordOK_Clicked(object sender, EventArgs e)
        {
            string pass = passWord;
            passWord = "";
            await ScannedDataProcess.DataReceived(pass, "", this);
        }
        #endregion
        
        #region stkSettings

        public void PrepareSettings()
        {
            CollapseAllStackPanels.Collapse(this);
            stkSettings.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "Settings";
            //ediAddress.Text = "http://scanner.aptus.ee/AptusWMS";
        }


        private void rbtnProduction_CheckChanged(object sender, EventArgs e)
        {
            rbtnTest.IsChecked = false;
            EnvironmentColorChange(productionColor);
        }

        private void rbtnTest_CheckChanged(object sender, EventArgs e)
        {
            rbtnProduction.IsChecked = false;
            EnvironmentColorChange(testColor);
        }


        private void EnvironmentColorChange(Color environmentColor)
        {
            grdMain.BackgroundColor = environmentColor;
            this.BackgroundColor = environmentColor;
        }

        private async void btnSettingsOK_Clicked(object sender, EventArgs e)
        {

            bool pEnv = rbtnProduction.IsChecked ? true : false;
            //obj.wcfAddress = ediAddress.Text;
            obj.shopLocationCode = shopLocationCode.Text;
            //obj.companyName = obj.wcfAddress.Contains("/") ? obj.wcfAddress.Split(new[] { "/" }, StringSplitOptions.None).Last().ToUpper().Replace("WMS", "") : null;
            var result = await WriteSettings.Write(pEnv, ediAddress.Text, obj.shopLocationCode, this);
            if (result.Item1)
            {
                DisplaySuccessMessage("SALVESTATUD!");
                obj.pEnv = pEnv;
                BackKeyPress.Press(this);
            }
            else
            {
                DisplayFailMessage(result.Item2);
            }
        }

        void LstvSettings_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ListOfSettings;
            lstSet.ForEach(x => x.isSelected = false);
            item.isSelected = true;
        }
        #endregion

        #region stkOperations

        public async void PrepareOperations()
        {

            CollapseAllStackPanels.Collapse(this);
            stkOperations.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "Operations";
            lblOperationsHeader.Text = "VALI OPERATSIOON";

            if (obj.operatingSystem == "UWP")
            {
                stkOperations.Margin = new Thickness(-10, 0, 0, 0);
            }
            if (obj.operatingSystem == "Android")
            {
                grdMain.ScaleX = 1.0;
                grdMain.ScaleY = 1.0;
            }
            focusedEditor = "";
            Debug.WriteLine(obj.companyName);
            var version = await VersionCheck.Check(obj.companyName, obj.pin, this);

            Debug.WriteLine("=================lstInternalRecordDB count " + lstInternalRecordDB.Count());
            if (lstInternalRecordDB.Any())
            {
                lstOperationsRecords = new List<ListOfOperationsRecords>();
                lstOperationsRecords.Add(new ListOfOperationsRecords
                {
                    inventoryRecords = 0,
                    transferRecords = 0,
                    dbRecords = lstInternalRecordDB.Count(),
                    locationCode = obj.shopLocationCode,
                    dbRecordsUpdateDate = lstInternalRecordDB.First().fileDate
                });


                //JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                //string input = JsonConvert.SerializeObject(lstInternalRecordDB, jSONsettings);
                //Debug.WriteLine(input);
            }

            
            LstvOperationsRecordInfo.ItemsSource = null;
            LstvOperationsRecordInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcOperationsRecords)) : new DataTemplate(typeof(vcOperationsRecords));
            LstvOperationsRecordInfo.ItemsSource = lstOperationsRecords;
        }

        private async void btnExit_Clicked(object sender, EventArgs e)
        {
            if (await YesNoDialog("VÄLJUMINE", "KAS SULGEDA TARKVARA?", true))
            {
                var closer = DependencyService.Get<ICloseApplication>();
                closer?.closeApplication();
            }
        }

        private void btnSettings_Clicked(object sender, EventArgs e)
        {
            PreparePassword();
        }

        private void btnOperationsStocktake_Clicked(object sender, EventArgs e)
        {
            PrepareStockTake();
        }

        private void btnOperationsTransfer_Clicked(object sender, EventArgs e)
        {
            PrepareTransfer();
        }

        private void btnOperationsExport_Clicked(object sender, EventArgs e)
        {

        }

        #endregion

        #region stkStockTake
        public async void PrepareStockTake()
        {

            CollapseAllStackPanels.Collapse(this);
            stkStockTake.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "StockTake";
            lblStockTakeHeader.Text = "INVENTUUR -> " + obj.shopLocationCode;

            if (obj.operatingSystem == "UWP")
            {
                stkOperations.Margin = new Thickness(-10, 0, 0, 0);
            }
            if (obj.operatingSystem == "Android")
            {
                grdMain.ScaleX = 1.0;
                grdMain.ScaleY = 1.0;
            }
            focusedEditor = "";
            entStockTakeReadCode.Text = "";
            entStockTakeQuantity.Text = "";
            lblStockTakeBarCodeValue.Text = "";
            lblStockTakeInternalCodeValue.Text = "";
            lblStockTakeQuantityUOM.Text = "";
            lblStockTakeItemDesc.Text = "";

            var resultReadInvRecords = await ReadInvRecords.Read(this);
            if (resultReadInvRecords.Item1)
            {
                if (!string.IsNullOrEmpty(resultReadInvRecords.Item2))
                {
                    Debug.WriteLine(resultReadInvRecords.Item2);
                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                    lstInternalInvDB = JsonConvert.DeserializeObject<List<ListOfInvRecords>>(resultReadInvRecords.Item2, jSONsettings);
                    progressBarActive = false;
                }
            }
            lblStockTakeAddedRowsValue.Text = "0";
            if (lstInternalInvDB.Any())
            {
                lblStockTakeAddedRowsValue.Text = lstInternalInvDB.Count.ToString();
            }
            entStockTakeReadCode.Text = "438499239403";
        }

        private async void btnStockTakeQuantityOK_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("X1");
            decimal quantity = 0;
            if (!string.IsNullOrEmpty(entStockTakeQuantity.Text))
            {
                quantity = TryParseDecimal.Parse(entStockTakeQuantity.Text);
            }
            if (quantity > -1)
            {
                Debug.WriteLine("X2");
                if (invRecordID == 0)
                {
                    int lastRecordID = 0;
                    if (lstInternalInvDB.Any())
                    {
                        lastRecordID = lstInternalInvDB.OrderBy(x => x.recordID).Take(1).First().recordID;
                    }
                    lstInternalInvDB.Add(new ListOfInvRecords
                    {
                        barCode = lblStockTakeBarCodeValue.Text,
                        itemCode = lblStockTakeInternalCodeValue.Text,
                        quantity = quantity,
                        recordDate = DateTime.Now,
                        uom = lblStockTakeQuantityUOM.Text,
                        recordID = lastRecordID + 1
                    });

                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                    string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);
                    Debug.WriteLine(data);
                    var writeInvDbToFile = await WriteInvRecords.Write(this, data);
                    if (writeInvDbToFile.Item1)
                    {
                        DisplaySuccessMessage("SALVESTATUD!");
                        PrepareStockTake();
                    }
                    else
                    {
                        DisplayFailMessage(writeInvDbToFile.Item2);
                    }
                }
                else
                {
                    var record = lstInternalInvDB.Where(x => x.recordID == invRecordID);
                    if (record.Any())
                    {
                        record.First().quantity = quantity;
                        record.First().recordDate = DateTime.Now;

                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                        string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);
                        Debug.WriteLine(data);
                        var writeInvDbToFile = await WriteInvRecords.Write(this, data);
                        if (string.IsNullOrEmpty(writeInvDbToFile.Item2))
                        {
                            DisplaySuccessMessage("SALVESTATUD!");
                            PrepareStockTake();
                        }
                        else
                        {
                            DisplayFailMessage(writeInvDbToFile.Item2);
                        }
                    }
                }
            }
            else
            {
                DisplayFailMessage("KOGUS PEAB OLEMA NUMBER!");
            }


        }

        private async void btnStockTakeReadCode_Clicked(object sender, EventArgs e)
        {
            SearchEntStockTakeReadCode();

        }

        public async void SearchEntStockTakeReadCode()
        {
            if (!string.IsNullOrEmpty(entStockTakeReadCode.Text))
            {
                if (entStockTakeReadCode.Text.Length > 5)
                {
                    var result = lstInternalRecordDB.Where(x =>
                       x.itemCode.Contains(entStockTakeReadCode.Text)
                    || x.itemDesc.Contains(entStockTakeReadCode.Text)
                    || x.barCode.Contains(entStockTakeReadCode.Text)).ToList();
                    if (result.Any())
                    {
                        if (result.Count() == 1)
                        {
                            if (lstInternalInvDB.Any())
                            {
                                var s = lstInternalInvDB.Where(x => x.itemCode == result.First().itemCode && x.barCode == result.First().barCode).ToList();
                                if (s.Any())
                                {
                                    if (await YesNoDialog("INVENTUUR", "KAUP ON JUBA INVENTEERTUD. KAS SOOVID PARANDADA?", false))
                                    {
                                        invRecordID = s.First().recordID;
                                        entStockTakeQuantity.Text = (s.First().quantity).ToString().Replace(".0","");
                                        lblStockTakeBarCodeValue.Text = result.First().barCode;
                                        lblStockTakeInternalCodeValue.Text = result.First().itemCode;
                                        lblStockTakeItemDesc.Text = result.First().itemDesc;
                                        lblStockTakeQuantityUOM.Text = result.First().itemMagnitude;
                                    }
                                    else
                                    {
                                        invRecordID = 0;
                                        lblStockTakeBarCodeValue.Text = "";
                                        lblStockTakeInternalCodeValue.Text = "";
                                        lblStockTakeItemDesc.Text = "";
                                        lblStockTakeQuantityUOM.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                lblStockTakeQuantity.Text = "";
                                lblStockTakeBarCodeValue.Text = result.First().barCode;
                                lblStockTakeInternalCodeValue.Text = result.First().itemCode;
                                lblStockTakeItemDesc.Text = result.First().itemDesc;
                                lblStockTakeQuantityUOM.Text = result.First().itemMagnitude;
                            }
                        }
                        else
                        {
                            obj.previousLayoutName = "StockTake";
                            PrepareSelectItem();
                        }
                    }
                    else
                    {
                        DisplayFailMessage("EI LEITUD MIDAGI!");

                    }
                }
                else
                {
                    DisplayFailMessage("SISESTA VÄHEMALT 5 TÄHEMÄRKI!");
                }
            }
        }
        

        private async void btnStockTakeAddedRowsView_Clicked(object sender, EventArgs e)
        {
        }

        #endregion

        #region stkTransfer
        public async void PrepareTransfer()
        {

            CollapseAllStackPanels.Collapse(this);
            stkTransfer.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "Transfer";
            lblTransferHeader.Text = "LIIKUMINE -> " + obj.shopLocationCode;

            if (obj.operatingSystem == "UWP")
            {
                stkOperations.Margin = new Thickness(-10, 0, 0, 0);
            }
            if (obj.operatingSystem == "Android")
            {
                grdMain.ScaleX = 1.0;
                grdMain.ScaleY = 1.0;
            }
            focusedEditor = "";
        }

        private async void btnTransferQuantityOK_Clicked(object sender, EventArgs e)
        {
        }

        private async void btnTransferReadCode_Clicked(object sender, EventArgs e)
        {
            SearchEntTransferReadCode();
        }

        private async void btnTransferInternalCode_Clicked(object sender, EventArgs e)
        {
        }

        private async void btnTransferBarCode_Clicked(object sender, EventArgs e)
        {
        }

        private async void btnTransferAddedRowsView_Clicked(object sender, EventArgs e)
        {
        }

        public void SearchEntTransferReadCode()
        {
            if (!string.IsNullOrEmpty(entTransferReadCode.Text))
            {
                if (entTransferReadCode.Text.Length > 5)
                {
                    var result = lstInternalRecordDB.Where(x =>
                       x.itemCode.Contains(entTransferReadCode.Text)
                    || x.itemDesc.Contains(entTransferReadCode.Text)
                    || x.barCode.Contains(entTransferReadCode.Text)).ToList();
                    if (result.Any())
                    {
                        if (result.Count() == 1)
                        {
                            lblTransferBarCodeValue.Text = result.First().barCode;
                            lblTransferInternalCodeValue.Text = result.First().itemCode;
                            lblTransferItemDesc.Text = result.First().itemDesc;
                            lblTransferQuantityUOM.Text = result.First().itemMagnitude;
                        }
                        else
                        {
                            obj.previousLayoutName = "Transfer";
                            PrepareSelectItem();
                        }
                    }
                    else
                    {
                        DisplayFailMessage("EI LEITUD MIDAGI!");

                    }
                }
                else
                {
                    DisplayFailMessage("SISESTA VÄHEMALT 5 TÄHEMÄRKI!");
                }
            }
        }


        #endregion

        #region stkSelectItem

        public async void PrepareSelectItem()
        {

            CollapseAllStackPanels.Collapse(this);
            stkSelectItem.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "SelectItem";
            lblSelectItemHeader.Text = "VALI KAUP";
            LstvSelectItem.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemSelect)) : new DataTemplate(typeof(vcItemSelect));
            if (obj.operatingSystem == "UWP")
            {
                stkOperations.Margin = new Thickness(-10, 0, 0, 0);
            }
            if (obj.operatingSystem == "Android")
            {
                grdMain.ScaleX = 1.0;
                grdMain.ScaleY = 1.0;
            }

            focusedEditor = "";
            if (obj.previousLayoutName == "StockTake")
            {
                entSelectItemReadCode.Text = entStockTakeReadCode.Text;
               
            }
            if (obj.previousLayoutName == "Transfer")
            {
                entSelectItemReadCode.Text = entTransferReadCode.Text;
            }
            if (!string.IsNullOrEmpty(entSelectItemReadCode.Text))
            {
                var result = lstInternalRecordDB.Where(x =>
                      x.itemCode.Contains(entSelectItemReadCode.Text)
                   || x.itemDesc.Contains(entSelectItemReadCode.Text)
                   || x.barCode.Contains(entSelectItemReadCode.Text)).ToList();
                LstvSelectItem.ItemsSource = null;
                LstvSelectItem.ItemsSource = result;
            }
        }

        private async void LstvSelectedItem_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ListOfdbRecords;
            if (obj.previousLayoutName == "StockTake")
            {
                entStockTakeReadCode.Text = entSelectItemReadCode.Text;
                lblStockTakeBarCodeValue.Text = item.barCode;
                lblStockTakeItemDesc.Text = item.itemDesc;
                lblStockTakeInternalCodeValue.Text = item.itemCode;

                var s = lstInternalInvDB.Where(x => x.itemCode == item.itemCode && x.barCode == item.barCode).ToList();
                if (s.Any())
                {
                    if (await YesNoDialog("INVENTUUR", "KAUP ON JUBA INVENTEERTUD. KAS SOOVID PARANDADA?", false))
                    {
                        invRecordID = s.First().recordID;
                        entStockTakeQuantity.Text = (s.First().quantity).ToString().Replace(".0","");
                        CollapseAllStackPanels.Collapse(this);
                        stkStockTake.IsVisible = true;
                    }
                }
                else
                {
                    invRecordID = 0;
                    entStockTakeQuantity.Text = "";
                    CollapseAllStackPanels.Collapse(this);
                    stkStockTake.IsVisible = true;
                }
            }
            if (obj.previousLayoutName == "Transfer")
            {
                entTransferReadCode.Text = entSelectItemReadCode.Text;
                lblTransferBarCodeValue.Text = item.barCode;
                lblTransferItemDesc.Text = item.itemDesc;
                lblTransferInternalCodeValue.Text = item.itemCode;
                CollapseAllStackPanels.Collapse(this);
                stkTransfer.IsVisible = true;
            }
        }

        private async void btnSelectItemReadCode_Clicked(object sender, EventArgs e)
        {
            SearchEntSelectItemReadCode();
        }

        public void SearchEntSelectItemReadCode()
        {
            LstvSelectItem.ItemsSource = null;
            
            if (!string.IsNullOrEmpty(entSelectItemReadCode.Text))
            {
                if (entSelectItemReadCode.Text.Length > 5)
                {
                    var result = lstInternalRecordDB.Where(x =>
                       x.itemCode.Contains(entSelectItemReadCode.Text)
                    || x.itemDesc.Contains(entSelectItemReadCode.Text)
                    || x.barCode.Contains(entSelectItemReadCode.Text)).ToList();
                    if (result.Any())
                    {
                        LstvSelectItem.ItemsSource = result;
                    }
                    else
                    {
                        DisplayFailMessage("EI LEITUD MIDAGI!");

                    }
                }
                else
                {
                    DisplayFailMessage("SISESTA VÄHEMALT 5 TÄHEMÄRKI!");
                }
            }
        }

        #endregion
    }
}
