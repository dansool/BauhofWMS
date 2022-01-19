﻿using System;
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
using BauhofWMSDLL.Utils;
using BauhofWMS.Keyboard;
using BauhofWMS.Scanner;
using BauhofWMS.Utils.Parsers;
using ProgressRingControl.Forms.Plugin;
using Newtonsoft.Json.Linq;

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
        public ReaddbShopRelationRecords ReaddbShopRelationRecords = new ReaddbShopRelationRecords();
        public VersionCheck VersionCheck = new VersionCheck();
        public CharacterReceived CharacterReceived = new CharacterReceived();
        public VirtualKeyboardTypes VirtualKeyboardTypes = new VirtualKeyboardTypes();
        public BackKeyPress BackKeyPress = new BackKeyPress();
        public WriteSettings WriteSettings = new WriteSettings();
        public GetCurrentVersion GetCurrentVersion = new GetCurrentVersion();
        public TryParseDateTime TryParseDateTime = new TryParseDateTime();
        public TryParseDecimal TryParseDecimal = new TryParseDecimal();
        public WriteInvRecords WriteInvRecords = new WriteInvRecords();
        public ReadMovementRecords ReadMovementRecords = new ReadMovementRecords();
        public WriteMovementRecords WriteMovementRecords = new WriteMovementRecords();
        public WriteInvRecordsToExportFile WriteInvRecordsToExportFile = new WriteInvRecordsToExportFile();
        public WriteMovementRecordsToExportFile WriteMovementRecordsToExportFile = new WriteMovementRecordsToExportFile();
        public VersionCheckLocal VersionCheckLocal = new VersionCheckLocal();
        public ReadPurchaseReceiveRecords ReadPurchaseReceiveRecords = new ReadPurchaseReceiveRecords();
        public ReadTransferReceiveRecords ReadTransferReceiveRecords = new ReadTransferReceiveRecords();
        public ReadPurchaseOrderPickedQuantitiesRecords ReadPurchaseOrderPickedQuantitiesRecords = new ReadPurchaseOrderPickedQuantitiesRecords();
        public WritePurchaseOrderPickedQuantitiesRecords WritePurchaseOrderPickedQuantitiesRecords = new WritePurchaseOrderPickedQuantitiesRecords();
        public WriteTransferOrderPickedQuantitiesRecords WriteTransferOrderPickedQuantitiesRecords = new WriteTransferOrderPickedQuantitiesRecords();
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

        public int invRecordID = 0;
        public int transferRecordID = 0;
        public bool complete;

        public int purchReceiveRecordID = 0;
        public int transferReceiveRecordID = 0;
        


        #endregion
        #region lists
        public List<ListOfSettings> lstSettings = new List<ListOfSettings>();
        public List<ListOfSettings> lstSet = new List<ListOfSettings>();
        public List<ListOfOperationsRecords> lstOperationsRecords = new List<ListOfOperationsRecords>();
        public List<ListOfdbRecords> lstInternalRecordDB = new List<ListOfdbRecords>();
        public List<ListOfInvRecords> lstInternalInvDB = new List<ListOfInvRecords>();
        public List<ListOfInvToExport> lstInvToExport = new List<ListOfInvToExport>();

        public List<ListOfdbRecords> lstStockTakeInfo = new List<ListOfdbRecords>();

        public List<ListOfMovementRecords> lstInternalMovementDB = new List<ListOfMovementRecords>();
        public List<ListOfdbRecords> lstTransferInfo = new List<ListOfdbRecords>();
        public List<ListOfMovementToExport> lstInternalMovementToExport = new List<ListOfMovementToExport>();

        public List<ListOfdbRecords> lstItemInfo = new List<ListOfdbRecords>();

        public List<ListOfShopRelations> lstShopRelations = new List<ListOfShopRelations>();

        public List<ListOfSKU> lstBins = new List<ListOfSKU>();

        public List<ListOfdbRecords> lstResultItemInfo = new List<ListOfdbRecords>();

        public List<ListOfPurchaseReceive> lstInternalPurchaseReceiveDB = new List<ListOfPurchaseReceive>();
        public List<ListOfPurchaseReceive> lstPurchaseReceiveToExport = new List<ListOfPurchaseReceive>();
        public List<ListOfPurchaseReceive> lstPurchaseOrders = new List<ListOfPurchaseReceive>();
        public List<ListOfPurchaseReceive> lstPurchaseOrderLines = new List<ListOfPurchaseReceive>();
        public List<ListOfPurchaseReceive> lstPurchaseOrderQuantityInsertInfo = new List<ListOfPurchaseReceive>();
        public List<ListOfSHRCVToExport> lstPurchaseOrderQuantityInsertQuantities = new List<ListOfSHRCVToExport>();
        public List<ListOfSHRCVToExport> lstPurchaseOrderPickedQuantities = new List<ListOfSHRCVToExport>();
        public List<ListOfSHRCVToExport> lstPurchaseOrderPickedQuantitiesToExport = new List<ListOfSHRCVToExport>();


        public List<ListOfTransferReceive> lstInternalTransferReceiveDB = new List<ListOfTransferReceive>();
        public List<ListOfTransferReceive> lstPurchaseTransferToExport = new List<ListOfTransferReceive>();

        public List<ListOfTransferReceive> lstTransferOrders = new List<ListOfTransferReceive>();
        public List<ListOfTransferReceive> lstTransferOrderLines = new List<ListOfTransferReceive>();
        public List<ListOfTransferReceive> lstTransferOrderQuantityInsertInfo = new List<ListOfTransferReceive>();
        public List<ListOfTRFRCVToExport> lstTransferOrderQuantityInsertQuantities = new List<ListOfTRFRCVToExport>();
        public List<ListOfTRFRCVToExport> lstTransferOrderPickedQuantities = new List<ListOfTRFRCVToExport>();
        public List<ListOfTRFRCVToExport> lstTransferOrderPickedQuantitiesToExport = new List<ListOfTRFRCVToExport>();








        #endregion


        #region MainPage operations

        public MainPage()
        {
            InitializeComponent();
            obj.mp = this;
            grdProgressBar.IsVisible = true;
           
            StartMainPage();
        }

     
        public async Task startRing()
        {
            double i = 0.0;
            while (true)
            {
                if (!complete)
                {
                    i = i + 0.05;
                    if (i > 1)
                    {
                        prgRing.Progress = 0;
                        i = 0.0;
                    }
                    else
                    {
                        Debug.WriteLine(i);
                        prgRing.Progress = i;
                    }
                    await Task.Delay(70);
                }
                else
                {
                    break;
                }
            }
            

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
                  
                    if (Device.RuntimePlatform == Device.UWP) { obj.operatingSystem = "UWP"; UWP(); }
                    if (Device.RuntimePlatform == Device.Android) { obj.operatingSystem = "Android"; Android(); }

                    ScannedValueReceive();
                    var resultSettings = await ReadSettings.Read(this);
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        GetShopRelations();
                    }

                    if (resultSettings.Item1)
                    {
                        lstSettings = resultSettings.Item3;
                        lstSettings.First().shopLocationCode = lstSettings.First().shopLocationCode.Replace("\"", "");
                       
                        obj.wcfAddress = lstSettings.First().wmsAddress;
                        obj.shopLocationCode = !string.IsNullOrEmpty(lstSettings.First().shopLocationCode) ? lstSettings.First().shopLocationCode.ToUpper() : "";
                        obj.showInvQty = lstSettings.First().showInvQty; 
                        obj.showPurchaseReceiveQty = lstSettings.First().showPurchaseReceiveQty;
                        obj.showTransferReceiveQty = lstSettings.First().showTransferReceiveQty;
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

                        if (obj.showInvQty)
                        {
                            rbtnInvQtyYes.IsChecked = true;
                        }
                        else
                        {
                            rbtnInvQtyNo.IsChecked = true;
                        }

                        if (obj.showPurchaseReceiveQty)
                        {
                            rbtnPurchaseReceiveQtyYes.IsChecked = true;
                        }
                        else
                        {
                            rbtnPurchaseReceiveQtyNo.IsChecked = true;
                        }


                        if (obj.showTransferReceiveQty)
                        {
                            rbtnTransferReceiveQtyYes.IsChecked = true;
                        }
                        else
                        {
                            rbtnTransferReceiveQtyNo.IsChecked = true;
                        }

                        if (!string.IsNullOrEmpty(obj.shopLocationCode))
                        {
                            shopLocationCode.Text = obj.shopLocationCode;
                        }
                    }
                    else
                    {
                        proceed = false;
                        grdMain.IsVisible = true;

                        string prefix = "";
                        PrepareSettings();
                    }

                    if (lstSettings.Any())
                    {
                        Debug.WriteLine("SHOPLOCATION: " + lstSettings.First().shopLocationCode);
                        if (!string.IsNullOrEmpty(lstSettings.First().shopLocationCode))
                        {
                            grdProgressBar.IsVisible = true;
                            //prgRing = new  ProgressRing { RingThickness = 20, Progress = 0.5f };
                            progressBarActive = true;
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
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("=====resultReaddbRecords.Item2  ERROR " + resultReaddbRecords.Item2);
                                }

                                

                                grdProgressBar.IsVisible = false;
                                progressBarActive = false;
                                complete = true;
                            }

                            if (Device.RuntimePlatform == Device.Android)
                            {

                                await GetShopRelations();
                                string prefix = "";
                                Debug.WriteLine("lstShopRelations: " + lstShopRelations.Count());
                                if (lstShopRelations.Any())
                                {
                                    if (lstSettings.Any())
                                    {
                                        
                                        var r = lstShopRelations.Where(x => x.shopName.ToUpper() == lstSettings.First().shopLocationCode.ToUpper());
                                        if (r.Any())
                                        {
                                            prefix = r.First().shopID;
                                            obj.shopLocationID = prefix;
                                            if (prefix.Length == 2)
                                            {
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    startRing();
                                                });


                                                var resultReadPurchaseReceivedbRecords = await ReadPurchaseReceiveRecords.Read(this);
                                                if (resultReadPurchaseReceivedbRecords.Item1)
                                                {
                                                    Debug.WriteLine(resultReadPurchaseReceivedbRecords.Item2);
                                                    if (!string.IsNullOrEmpty(resultReadPurchaseReceivedbRecords.Item2))
                                                    {
                                                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                                        lstInternalPurchaseReceiveDB = JsonConvert.DeserializeObject<List<ListOfPurchaseReceive>>(resultReadPurchaseReceivedbRecords.Item2, jSONsettings);
                                                        progressBarActive = false;
                                                    }
                                                }
                                                var resultReadTransferReceivedbRecords = await ReadTransferReceiveRecords.Read(this);
                                                if (resultReadTransferReceivedbRecords.Item1)
                                                {
                                                    Debug.WriteLine(resultReadTransferReceivedbRecords.Item2);
                                                    if (!string.IsNullOrEmpty(resultReadTransferReceivedbRecords.Item2))
                                                    {
                                                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                                        lstInternalTransferReceiveDB = JsonConvert.DeserializeObject<List<ListOfTransferReceive>>(resultReadTransferReceivedbRecords.Item2, jSONsettings);
                                                        progressBarActive = false;
                                                        complete = true;
                                                    }
                                                }
                                                var resultReaddbRecords = await ReaddbRecords.Read(this);
                                                if (resultReaddbRecords.Item1)
                                                {
                                                    Debug.WriteLine(resultReaddbRecords.Item2);
                                                    if (!string.IsNullOrEmpty(resultReaddbRecords.Item2))
                                                    {
                                                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                                        lstInternalRecordDB = JsonConvert.DeserializeObject<List<ListOfdbRecords>>(resultReaddbRecords.Item2, jSONsettings);
                                                        progressBarActive = false;
                                                        complete = true;
                                                    }
                                                }

                                                
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DisplayAlert("VIGA", "KAUPLUSE NIMI ON SEADISTAMATA!", "OK");
                                        grdProgressBar.IsVisible = false;
                                        progressBarActive = false;
                                        PrepareOperations();
                                    }
                                }

                                Debug.WriteLine("SHOP: " + prefix);


                                
                                grdProgressBar.IsVisible = false;
                                progressBarActive = false;


                            }

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
                           
                            var resultReadMovementRecords = await ReadMovementRecords.Read(this);
                            if (resultReadMovementRecords.Item1)
                            {
                                if (!string.IsNullOrEmpty(resultReadMovementRecords.Item2))
                                {
                                    Debug.WriteLine(resultReadMovementRecords.Item2);
                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    lstInternalMovementDB = JsonConvert.DeserializeObject<List<ListOfMovementRecords>>(resultReadMovementRecords.Item2, jSONsettings);
                                    progressBarActive = false;
                                }
                            }

                            var resultPurchaseOrderPickedQuantities = await ReadPurchaseOrderPickedQuantitiesRecords.Read(this);
                            if (resultPurchaseOrderPickedQuantities.Item1)
                            {
                                if (!string.IsNullOrEmpty(resultPurchaseOrderPickedQuantities.Item2))
                                {
                                    Debug.WriteLine(resultReadMovementRecords.Item2);
                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    lstPurchaseOrderPickedQuantities = JsonConvert.DeserializeObject<List<ListOfSHRCVToExport>>(resultPurchaseOrderPickedQuantities.Item2, jSONsettings);
                                    progressBarActive = false;
                                }
                            }
                            


                            Debug.WriteLine("lstInternalRecordDB + " + lstInternalRecordDB.Count());

                           
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
                                else
                                {
                                    Debug.WriteLine("VERSIOONI EI SAADUD!");
                                }
                                PrepareOperations();

                            }
                        }
                    }
                    else
                    {
                        string prefix = "";
                        PrepareSettings();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("VIIGA " + ex.Message);
            }
        }

        public void UWP()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete " + arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); obj.deviceSerial = (arg.Length > 20 ? arg.Take(19).ToString() : arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "isDeviceHandheld", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { obj.isDeviceHandheld = Convert.ToBoolean(arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "backPressed", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { BackKeyPress.Press(this); }); });
            MessagingCenter.Subscribe<App, string>(this, "KeyboardListener", (sender, args) => { CharacterReceived.Receive(null, args, this); });
           
            
        }

        public void Android()
        {
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "exception", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("VIGA", arg, "OK"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannerInitStatus", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { Debug.WriteLine("Scanner initialization is complete"); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadUpdateProgress", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblUpdate.Text = "UUE VERSIOONI LAADIMINE " + arg + "%"; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "downloadComplete", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { CollapseAllStackPanels.Collapse(this); stkOperations.IsVisible = true; }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "isDeviceHandheld", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { obj.isDeviceHandheld = Convert.ToBoolean(arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "deviceSerial", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { lblDeviceSerial.Text = arg; Debug.WriteLine(arg); obj.deviceSerial = (arg.Length > 20 ? arg.Take(19).ToString() : arg); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "backPressed", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { BackKeyPress.Press(this); }); });
            MessagingCenter.Subscribe<App, string>((App)Application.Current, "erro", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { DisplayAlert("err", arg, "OK"); }); });
            

        }

        public async Task GetShopRelations()
        {
            Debug.WriteLine("Import alusta1");
            var resultShoprelations = await ReaddbShopRelationRecords.Read(this);
            if (resultShoprelations.Item1)
            {
                Debug.WriteLine("Import alusta2");
                if (!string.IsNullOrEmpty(resultShoprelations.Item2))
                {
                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                    lstShopRelations = JsonConvert.DeserializeObject<List<ListOfShopRelations>>(resultShoprelations.Item2, jSONsettings);
                    Debug.WriteLine("Import done " + lstShopRelations.Count());
                    foreach (var r in lstShopRelations)
                    {
                        r.shopID = r.shopID.Replace("\"", "");
                        r.shopName = r.shopName.Replace("\"", "");
                    }
                }
            }
            Debug.WriteLine("Import alusta3 " + lstShopRelations.Count());
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

        public string GetShopName(string sku)
        {
            string result = "";
            var shopNameLine = lstShopRelations.Where(x => x.shopID == sku);
            if (shopNameLine.Any())
            {
                result = sku + "  " + shopNameLine.First().shopName;
            }
            return result;
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


        private void rbtnInvQtyYes_CheckChanged(object sender, EventArgs e)
        {
            rbtnInvQtyNo.IsChecked = false;
            obj.showInvQty = true;
        }

        private void rbtnInvQtyNo_CheckChanged(object sender, EventArgs e)
        {
            rbtnInvQtyYes.IsChecked = false;
            obj.showInvQty = false;
        }
        private void EnvironmentColorChange(Color environmentColor)
        {
            grdMain.BackgroundColor = environmentColor;
            this.BackgroundColor = environmentColor;
        }

        private async void btnSettingsOK_Clicked(object sender, EventArgs e)
        {
            await GetShopRelations();
            bool pEnv = false;
            if (rbtnProduction.IsChecked == false && rbtnTest.IsChecked == false)
            {
                rbtnProduction.IsChecked = true;
                pEnv = true;
            }
            else
            {
                pEnv = rbtnProduction.IsChecked ? true : false;
            }
            var p = lstShopRelations.Where(x => x.shopName == shopLocationCode.Text);
            if (p.Any())
            {
                obj.shopLocationCode = shopLocationCode.Text;
                obj.shopLocationID = p.First().shopID;                


                var result = await WriteSettings.Write(pEnv, ediAddress.Text, obj.shopLocationCode, obj.showInvQty, obj.showPurchaseReceiveQty, obj.showTransferReceiveQty, this);
                if (result.Item1)
                {
                    DisplaySuccessMessage("SALVESTATUD!");
                    obj.pEnv = pEnv;
                    obj.deviceSerial = null;
                    ShowKeyBoard.Hide(this);
                    StartMainPage();
                    //BackKeyPress.Press(this);
                }
                else
                {
                    DisplayFailMessage(result.Item2);
                }
            }
            else
            {
                DisplayAlert("VIGANE KAUPLUSE NIMI", shopLocationCode.Text + " EI LEITUD KAUPLUSTE NIMEKIRJAST!", "OK");
            }
            
        }

        private void rbtnPurchaseReceiveQtyYes_CheckChanged(object sender, EventArgs e)
        {
            rbtnPurchaseReceiveQtyNo.IsChecked = false;
            obj.showPurchaseReceiveQty = true;
        }

        private void rbtnPurchaseReceiveQtyNo_CheckChanged(object sender, EventArgs e)
        {
            rbtnPurchaseReceiveQtyYes.IsChecked = false;
            obj.showPurchaseReceiveQty = false;
        }

        private void rbtnTransferReceiveQtyYes_CheckChanged(object sender, EventArgs e)
        {
            rbtnTransferReceiveQtyNo.IsChecked = false;
            obj.showTransferReceiveQty = true;
        }

        private void rbtnTransferReceiveQtyNo_CheckChanged(object sender, EventArgs e)
        {
            rbtnTransferReceiveQtyYes.IsChecked = false;
            obj.showTransferReceiveQty = false;
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
            frmOperationsPurchaseReceive.IsVisible = true;
            lblOperationspurchaseReceive.IsVisible = true;
            frmOperationsTransferReceive.IsVisible = true;
            lblOperationsTransferReceive.IsVisible = true;

            frmOperationsExport.IsVisible = false;
            lblOperationsExport.IsVisible = false;

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
            //var version = await VersionCheck.Check(obj.companyName, obj.pin, this);
            var version = await VersionCheckLocal.Check(obj.companyName, obj.pin, this);

           
            Debug.WriteLine("version true/false : " + version.Item1);
            Debug.WriteLine("version error:  " + version.Item2);
            Debug.WriteLine("version update: " + version.Item3);
            Debug.WriteLine("version current: " + version.Item4);


            Debug.WriteLine("=================lstInternalRecordDB count " + lstInternalRecordDB.Count());
            if (lstInternalRecordDB.Any())
            {
                lstOperationsRecords = new List<ListOfOperationsRecords>();
                lstOperationsRecords.Add(new ListOfOperationsRecords
                {
                    inventoryRecords = lstInternalInvDB.Any() ? lstInternalInvDB.Count() : 0,
                    transferRecords = lstInternalMovementDB.Any() ? lstInternalMovementDB.Count() : 0,
                    purchaseReceiveRecords = lstPurchaseOrderPickedQuantities.Any() ? lstPurchaseOrderPickedQuantities.Count() : 0,
                    transferReceiveRecords = lstTransferOrderPickedQuantities.Any() ? lstTransferOrderPickedQuantities.Count() : 0,
                    dbRecords = lstInternalRecordDB.Any() ? lstInternalRecordDB.Count() : 0,
                    locationCode = obj.shopLocationCode,
                    dbRecordsUpdateDate = lstInternalRecordDB.First().fileDate
                });
            }

            LstvOperationsRecordInfo.ItemsSource = null;
            LstvOperationsRecordInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcOperationsRecords)) : new DataTemplate(typeof(vcOperationsRecords));
            LstvOperationsRecordInfo.ItemsSource = lstOperationsRecords;
            if (lstInternalInvDB.Any() || lstInternalMovementDB.Any())
            {
                if (lstInternalInvDB.Count > 0)
                {
                    frmOperationsExport.IsVisible = true;
                    lblOperationsExport.IsVisible = true;
                }
                if (lstInternalMovementDB.Count > 0)
                {
                    frmOperationsExport.IsVisible = true;
                    lblOperationsExport.IsVisible = true;
                }
               
            }
            
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

        private void btnOperationsItemInfo_Clicked(object sender, EventArgs e)
        {
            string scannedCode = "";
            PrepareItemInfo(scannedCode);
        }

        private async void btnOperationsExport_Clicked(object sender, EventArgs e)
        {
            
            if (await YesNoDialog("EKSPORT", "KAS OLED KINDEL, ET KÕIK ANDMED ON KOGUTUD JA VÕIB JÄTKATA EKSPORDIGA?", false))
            {
               
                bool proceed = true;
                string exportFileNameStamp = "";
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
                string day = DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
                string hour = DateTime.Now.Hour.ToString().Length == 1 ? "0" + DateTime.Now.Hour.ToString() : DateTime.Now.Hour.ToString();
                string minute = DateTime.Now.Minute.ToString().Length == 1 ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString();
                string second = DateTime.Now.Second.ToString().Length == 1 ? "0" + DateTime.Now.Second.ToString() : DateTime.Now.Second.ToString();

                exportFileNameStamp = year + month + day + "_" + hour + minute + second;
                if (lstInternalInvDB.Any())
                {
                    string dataRowInv = "";
                    foreach (var p in lstInternalInvDB)
                    {
                        string barCode = "";
                        if (!string.IsNullOrEmpty(p.barCode))
                        {
                            var parseSKU = p.barCode.Split(new[] { ";" }, StringSplitOptions.None);
                            if (parseSKU.Any())
                            {
                                barCode = parseSKU[0];
                            }
                            else
                            {
                                barCode = p.barCode;
                            }
                        }
                        dataRowInv = dataRowInv + p.itemCode + ";" + barCode + ";" + p.quantity + ";" + p.uom + "\r\n";
                    }

                    
                        var write = await WriteInvRecordsToExportFile.Write(this, dataRowInv, exportFileNameStamp);
                        if (write.Item1)
                        {
                            lstInternalInvDB = new List<ListOfInvRecords>();

                            PrepareOperations();
                        }
                        else
                        {
                            proceed = false;
                            DisplayFailMessage(write.Item2);
                        }
                    
                    
                }
                if (lstInternalMovementDB.Any())
                {
                    string dataRowMovement = "";
                    foreach (var p in lstInternalMovementDB)
                    {
                        string barCode = "";
                        if (!string.IsNullOrEmpty(p.barCode))
                        {
                            var parseSKU = p.barCode.Split(new[] { ";" }, StringSplitOptions.None);
                            if (parseSKU.Any())
                            {
                                barCode = parseSKU[0];
                            }
                            else
                            {
                                barCode = p.barCode;
                            }
                        }

                        //Muudetud Märdi ettepanekul 12.01.22
                        dataRowMovement = dataRowMovement + p.itemCode + ";" + barCode + ";" + (0 - p.quantity) + ";" + p.uom + "\r\n";
                    }
                    var write = await WriteMovementRecordsToExportFile.Write(this, dataRowMovement, exportFileNameStamp);
                    if (write.Item1)
                    {
                        lstInternalMovementDB = new List<ListOfMovementRecords>();

                        PrepareOperations();
                    }
                    else
                    {
                        proceed = false;
                        DisplayFailMessage(write.Item2);
                    }
                }
                if (proceed)
                {
                    DisplaySuccessMessage("SALVESTATUD!");
                }
            }
        }

        private void btnOperationsTransferReceive_Clicked(object sender, EventArgs e)
        {
            PrepareTransferReceiveOrders();
        }

       
        private void btnOperationsPurchaseReceive_Clicked(object sender, EventArgs e)
        {
            PreparePurchaseReceiveOrders();
        }
        

        #endregion

        #region stkStockTake
        public async void PrepareStockTake()
        {
            frmbtnStockTakeAddedRowsDelete.IsVisible = false;
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
            lblStockTakeQuantityUOM.Text = "";

            frmbtnStockTakeQuantityOK.IsVisible = false;
            btnStockTakeQuantityOK.IsVisible = false;
            lblStockTakeQuantityUOM.IsVisible = false;
            frmentStockTakeQuantity.IsVisible = false;
            entStockTakeQuantity.IsVisible = false;
           


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
            lstStockTakeInfo = new List<ListOfdbRecords>();
            LstvStockTakeInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfoStockTake)) : new DataTemplate(typeof(vcItemInfoStockTake));
            if (lstStockTakeInfo.Any())
            {
                lstStockTakeInfo.First().showInvQty = obj.showInvQty;
            }
            LstvStockTakeInfo.ItemsSource = null;
            LstvStockTakeInfo.ItemsSource = lstStockTakeInfo;


            focusedEditor = "entStockTakeReadCode";
            entStockTakeReadCode.BackgroundColor = Color.Yellow;
            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
        }

        private async void btnStockTakeAddedRowsDelete_Clicked(object sender, EventArgs e)
        {
            if (await YesNoDialog("INVENTUUR", "JÄTKAMISEL KUSTUTATAKSE KIRJE TÄIELIKULT!", false))
            {
                var record = lstInternalInvDB.Where(x => x.recordID == invRecordID);
                if (record.Any())
                {
                    lstInternalInvDB.Remove(record.Take(1).First());
                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                    string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);

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
            }
        }

        private async void btnStockTakeQuantityOK_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool proceed = true;
                decimal quantity = 0;
                if (!string.IsNullOrEmpty(entStockTakeQuantity.Text))
                {
                    quantity = TryParseDecimal.Parse(entStockTakeQuantity.Text);
                }
                if (string.IsNullOrEmpty(lstStockTakeInfo.First().itemCode))
                {
                    proceed = false;
                    DisplayFailMessage("KAUPA POLE VALITUD!");
                }
                //if (quantity == 0)
                //{
                //    if (!await YesNoDialog("INVENTUUR", "SISESTATUD KOGUS ON 0. KAS JÄTKATA KIRJE TEKITAMISEGA?", false))
                //    {
                //        proceed = false;
                //    }
                //}

                if (proceed)
                {
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
                                barCode = lstStockTakeInfo.First().barCode,
                                itemCode = lstStockTakeInfo.First().itemCode,
                                itemDesc = lstStockTakeInfo.First().itemDesc,
                                quantity = quantity,
                                recordDate = DateTime.Now,
                                uom = lblStockTakeQuantityUOM.Text,
                                recordID = lastRecordID + 1
                            });

                            JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                            string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);

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
                            //if (quantity == 0)
                            //{
                            //    var record = lstInternalInvDB.Where(x => x.recordID == invRecordID);
                            //    if (record.Any())
                            //    {
                            //        lstInternalInvDB.Remove(record.Take(1).First());
                            //        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                            //        string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);

                            //        var writeInvDbToFile = await WriteInvRecords.Write(this, data);
                            //        if (writeInvDbToFile.Item1)
                            //        {
                            //            DisplaySuccessMessage("SALVESTATUD2!");
                            //            PrepareStockTake();
                            //        }
                            //        else
                            //        {
                            //            DisplayFailMessage(writeInvDbToFile.Item2);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                                var record = lstInternalInvDB.Where(x => x.recordID == invRecordID);
                                if (record.Any())
                                {
                                    record.First().quantity = quantity;
                                    record.First().recordDate = DateTime.Now;

                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);

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
                            //}
                        }
                    }
                    else
                    {
                        DisplayFailMessage("KOGUS PEAB OLEMA NUMBER!");
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private  void btnStockTakeReadCode_Clicked(object sender, EventArgs e)
        {
            SearchEntStockTakeReadCode();

        }

        public async void SearchEntStockTakeReadCode()
        {

            try
            {
                lstStockTakeInfo = new List<ListOfdbRecords>(); 
                if (!string.IsNullOrEmpty(entStockTakeReadCode.Text))
                {
                    if (entStockTakeReadCode.Text.Length > 4)
                    {
                        lstStockTakeInfo = new List<ListOfdbRecords>();
                        LstvStockTakeInfo.ItemsSource = null;
                        LstvStockTakeInfo.ItemsSource = lstStockTakeInfo;
                        frmbtnStockTakeQuantityOK.IsVisible = false;
                        btnStockTakeQuantityOK.IsVisible = false;
                        lblStockTakeQuantityUOM.IsVisible = false;
                        frmentStockTakeQuantity.IsVisible = false;
                        entStockTakeQuantity.IsVisible = false;
                        

                        var result = lstInternalRecordDB.Where(x =>
                           x.itemCode.Contains(entStockTakeReadCode.Text)
                        || x.itemDesc.ToUpper().Contains(entStockTakeReadCode.Text.ToUpper())
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
                                        obj.isScanAllowed = false;
                                        if (await YesNoDialog("INVENTUUR", "KAUP ON JUBA INVENTEERTUD. KAS SOOVID PARANDADA?", false))
                                        {
                                            obj.isScanAllowed = true;
                                            frmbtnStockTakeAddedRowsDelete.IsVisible = true;
                                            invRecordID = s.First().recordID;
                                            entStockTakeQuantity.Text = (s.First().quantity).ToString().Replace(".0", "");
                                            lblStockTakeQuantityUOM.Text = result.First().itemMagnitude;

                                            focusedEditor = "entStockTakeQuantity";
                                            entStockTakeQuantity.BackgroundColor = Color.Yellow;
                                            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
                                            entStockTakeReadCode.BackgroundColor = Color.White;

                                            frmbtnStockTakeQuantityOK.IsVisible = true;
                                            btnStockTakeQuantityOK.IsVisible = true;
                                            lblStockTakeQuantityUOM.IsVisible = true;
                                            frmentStockTakeQuantity.IsVisible = true;
                                            entStockTakeQuantity.IsVisible = true;
                                          

                                            lstStockTakeInfo = result;
                                            if (lstStockTakeInfo.Any())
                                            {
                                                lstStockTakeInfo.First().showInvQty = obj.showInvQty;
                                            }

                                            int SKUBinCount = 0;
                                            string sKUs = "";
                                            string sKUs2 = "";
                                            var parseSKU = lstStockTakeInfo.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
                                            if (parseSKU.Any())
                                            {
                                                foreach (var a in parseSKU)
                                                {
                                                    var uniqueSKU = a.Split(new[] { "###" }, StringSplitOptions.None);
                                                    if (uniqueSKU.Any())
                                                    {
                                                        if (uniqueSKU.Count() > 0)
                                                        {
                                                            if (uniqueSKU[0] == obj.shopLocationID)
                                                            {
                                                                SKUBinCount = SKUBinCount + 1;

                                                                if (SKUBinCount < 5)
                                                                {
                                                                    sKUs = sKUs + "\r\n" + uniqueSKU[1];
                                                                }
                                                                else
                                                                {
                                                                    sKUs2 = sKUs2 + "\r\n" + uniqueSKU[1];
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            lstStockTakeInfo.First().SKUBin = sKUs.TrimStart().TrimEnd();
                                            lstStockTakeInfo.First().SKUBin2 = sKUs2.TrimStart().TrimEnd();
                                            LstvStockTakeInfo.ItemsSource = null;
                                            LstvStockTakeInfo.ItemsSource = lstStockTakeInfo;
                                        }
                                        else
                                        {
                                            invRecordID = 0;
                                            entStockTakeReadCode.Text = "";
                                            entStockTakeQuantity.Text = "";
                                            lblStockTakeQuantityUOM.Text = "";
                                            focusedEditor = "entStockTakeReadCode";
                                            entStockTakeReadCode.BackgroundColor = Color.Yellow;
                                            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
                                            entStockTakeQuantity.BackgroundColor = Color.White;
                                            frmbtnStockTakeAddedRowsDelete.IsVisible = false;

                                        }
                                    }
                                    else
                                    {
                                        Debug.WriteLine("SIIJN");
                                        invRecordID = 0;
                                        entStockTakeQuantity.Text = "";
                                        lblStockTakeQuantityUOM.Text = result.First().itemMagnitude;
                                        focusedEditor = "entStockTakeQuantity";
                                        entStockTakeQuantity.BackgroundColor = Color.Yellow;
                                        ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
                                        entStockTakeReadCode.BackgroundColor = Color.White;

                                        frmbtnStockTakeAddedRowsDelete.IsVisible = false;
                                        frmbtnStockTakeQuantityOK.IsVisible = true;
                                        btnStockTakeQuantityOK.IsVisible = true;
                                        lblStockTakeQuantityUOM.IsVisible = true;
                                        frmentStockTakeQuantity.IsVisible = true;
                                        entStockTakeQuantity.IsVisible = true;
                                   
                                        lstStockTakeInfo = result;
                                        if (lstStockTakeInfo.Any())
                                        {
                                            lstStockTakeInfo.First().showInvQty = obj.showInvQty;
                                        }

                                        int SKUBinCount = 0;
                                        string sKUs = "";
                                        string sKUs2 = "";
                                        var parseSKU = lstStockTakeInfo.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
                                        if (parseSKU.Any())
                                        {
                                            foreach (var a in parseSKU)
                                            {
                                                var uniqueSKU = a.Split(new[] { "###" }, StringSplitOptions.None);
                                                if (uniqueSKU.Any())
                                                {
                                                    if (uniqueSKU.Count() > 0)
                                                    {
                                                        if (uniqueSKU[0] == obj.shopLocationID)
                                                        {
                                                            SKUBinCount = SKUBinCount + 1;

                                                            if (SKUBinCount < 5)
                                                            {
                                                                sKUs = sKUs + "\r\n" + uniqueSKU[1];
                                                            }
                                                            else
                                                            {
                                                                sKUs2 = sKUs2 + "\r\n" + uniqueSKU[1];
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        lstStockTakeInfo.First().SKUBin = sKUs.TrimStart().TrimEnd();
                                        lstStockTakeInfo.First().SKUBin2 = sKUs2.TrimStart().TrimEnd();
                                        LstvStockTakeInfo.ItemsSource = null;
                                        LstvStockTakeInfo.ItemsSource = lstStockTakeInfo;
                                    }
                                }
                                else
                                {
                                    invRecordID = 0;
                                    lblStockTakeQuantityUOM.Text = result.First().itemMagnitude;
                                    focusedEditor = "entStockTakeQuantity";
                                    entStockTakeQuantity.BackgroundColor = Color.Yellow;
                                    ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
                                    entStockTakeReadCode.BackgroundColor = Color.White;
                                    frmbtnStockTakeQuantityOK.IsVisible = true;
                                    btnStockTakeQuantityOK.IsVisible = true;
                                    lblStockTakeQuantityUOM.IsVisible = true;
                                    frmentStockTakeQuantity.IsVisible = true;
                                    entStockTakeQuantity.IsVisible = true;
                                    
                                   frmbtnStockTakeAddedRowsDelete.IsVisible = false;

                                    lstStockTakeInfo = new List<ListOfdbRecords>();
                                    lstStockTakeInfo = result;
                                    if (lstStockTakeInfo.Any())
                                    {
                                        lstStockTakeInfo.First().showInvQty = obj.showInvQty;
                                    }

                                    int SKUBinCount = 0;
                                    string sKUs = "";
                                    string sKUs2 = "";
                                    var parseSKU = lstStockTakeInfo.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
                                    if (parseSKU.Any())
                                    {
                                        foreach (var a in parseSKU)
                                        {
                                            var uniqueSKU = a.Split(new[] { "###" }, StringSplitOptions.None);
                                            if (uniqueSKU.Any())
                                            {
                                                if (uniqueSKU.Count() > 0)
                                                {
                                                    if (uniqueSKU[0] == obj.shopLocationID)
                                                    {
                                                        SKUBinCount = SKUBinCount + 1;

                                                        if (SKUBinCount < 5)
                                                        {
                                                            sKUs = sKUs + "\r\n" + uniqueSKU[1];
                                                        }
                                                        else
                                                        {
                                                            sKUs2 = sKUs2 + "\r\n" + uniqueSKU[1];
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    lstStockTakeInfo.First().SKUBin = sKUs.TrimStart().TrimEnd();
                                    lstStockTakeInfo.First().SKUBin2 = sKUs2.TrimStart().TrimEnd();
                                    LstvStockTakeInfo.ItemsSource = null;
                                    LstvStockTakeInfo.ItemsSource = lstStockTakeInfo;
                                }
                                obj.isScanAllowed = true;
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        private void btnStockTakeAddedRowsView_Clicked(object sender, EventArgs e)
        {
            PrepareStockTakeAddedRowsView();
        }

        private void btnStockTakeReadCodeClear_Clicked(object sender, EventArgs e)
        {
            entStockTakeReadCode.Text = "";
        }
        #endregion

        #region stkTransfer
        public async void PrepareTransfer()
        {
            frmbtnTransferAddedRowsDelete.IsVisible = false;
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
            entTransferReadCode.Text = "";
            entTransferQuantity.Text = "";

           
            frmentTransferQuantity.IsVisible = false;
            entTransferQuantity.IsVisible = false;
            lblTransferQuantityUOM.IsVisible = false;
            frmbtnTransferQuantityOK.IsVisible = false;
            btnTransferQuantityOK.IsVisible = false;

            var resultReadMovementRecords = await ReadMovementRecords.Read(this);
            if (resultReadMovementRecords.Item1)
            {
                if (!string.IsNullOrEmpty(resultReadMovementRecords.Item2))
                {
                    Debug.WriteLine(resultReadMovementRecords.Item2);
                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                    lstInternalMovementDB = JsonConvert.DeserializeObject<List<ListOfMovementRecords>>(resultReadMovementRecords.Item2, jSONsettings);
                    progressBarActive = false;
                }
            }
            lblTransferAddedRowsValue.Text = "0";
            if (lstInternalMovementDB.Any())
            {
                lblTransferAddedRowsValue.Text = lstInternalMovementDB.Count.ToString();
            }
            lstTransferInfo = new List<ListOfdbRecords>();
            LstvTransferInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfo)) : new DataTemplate(typeof(vcItemInfo));
            LstvTransferInfo.ItemsSource = null;
            LstvTransferInfo.ItemsSource = lstTransferInfo;


            focusedEditor = "entTransferReadCode";
            entTransferReadCode.BackgroundColor = Color.Yellow;
            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
        }

        private async void btnTransferAddedRowsDelete_Clicked(object sender, EventArgs e)
        {
            if (await YesNoDialog("LIIKUMINE", "JÄTKAMISEL KUSTUTATAKSE KIRJE TÄIELIKULT!", false))
            {
                var record = lstInternalMovementDB.Where(x => x.recordID == transferRecordID);
                if (record.Any())
                {
                    lstInternalMovementDB.Remove(record.Take(1).First());
                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                    string data = JsonConvert.SerializeObject(lstInternalMovementDB, jSONsettings);
                    Debug.WriteLine(data);
                    var writeMovementDbToFile = await WriteMovementRecords.Write(this, data);
                    if (writeMovementDbToFile.Item1)
                    {
                        DisplaySuccessMessage("SALVESTATUD!");
                        PrepareTransfer();
                    }
                    else
                    {
                        DisplayFailMessage(writeMovementDbToFile.Item2);
                    }
                }
            }
        }
        private async void btnTransferQuantityOK_Clicked(object sender, EventArgs e)
        {
            bool proceed = true;
            decimal quantity = 0;
            if (!string.IsNullOrEmpty(entTransferQuantity.Text))
            {
                quantity = TryParseDecimal.Parse(entTransferQuantity.Text);
            }
            if (quantity == -1)
            {
                proceed = false;
                DisplayFailMessage("SISESTATUD KOGUS SISALDAB TÄHTI!");

            }
            if (proceed)
            {
                if (string.IsNullOrEmpty(lstTransferInfo.First().itemCode))
                {
                    proceed = false;
                    DisplayFailMessage("KAUPA POLE VALITUD!");
                }
            }
            //if (quantity == 0)
            //{
            //    if (transferRecordID != 0)
            //    {
            //        if (!await YesNoDialog("LIIKUMINE", "SISESTATUD KOGUS ON 0. JÄTKAMISEL TÜHISTATAKSE VAREM SISESTATUD KOGUS. KAS JÄTKATA?", false))
            //        {
            //            proceed = false;
            //        }
            //    }
            //    else
            //    {
            //        DisplayFailMessage("0 EI SAA KOGUSENA SISESTADA!");
            //    }
            //}
            if (proceed)
            {
                if (quantity > -1)
                {
                    Debug.WriteLine("X2");
                    if (transferRecordID == 0)
                    {
                        int lastRecordID = 0;
                        if (lstInternalMovementDB.Any())
                        {
                            lastRecordID = lstInternalMovementDB.OrderBy(x => x.recordID).Take(1).First().recordID;
                        }
                        lstInternalMovementDB.Add(new ListOfMovementRecords
                        {
                            barCode = lstTransferInfo.First().barCode,
                            itemCode = lstTransferInfo.First().itemCode,
                            itemDesc = lstTransferInfo.First().itemDesc,
                            quantity = quantity,
                            recordDate = DateTime.Now,
                            uom = lstTransferInfo.First().itemMagnitude,
                            recordID = lastRecordID + 1
                        });

                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                        string data = JsonConvert.SerializeObject(lstInternalMovementDB, jSONsettings);
                        Debug.WriteLine(data);
                        var writeMovementDbToFile = await WriteMovementRecords.Write(this, data);
                        //await DisplayAlert("writeMovementDbToFile", writeMovementDbToFile.Item1 + "  " + writeMovementDbToFile.Item2, "OK");
                        if (writeMovementDbToFile.Item1)
                        {
                            DisplaySuccessMessage("SALVESTATUD!");
                            PrepareTransfer();
                        }
                        else
                        {
                            DisplayFailMessage(writeMovementDbToFile.Item2);
                        }
                    }
                    else
                    {
                        
                        //else
                        //{
                            var record = lstInternalMovementDB.Where(x => x.recordID == transferRecordID);
                            if (record.Any())
                            {
                                record.First().quantity = quantity;
                                record.First().recordDate = DateTime.Now;

                                JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                string data = JsonConvert.SerializeObject(lstInternalMovementDB, jSONsettings);
                                Debug.WriteLine(data);
                                var writeMovementDbToFile = await WriteMovementRecords.Write(this, data);
                                //await DisplayAlert("writeMovementDbToFile", writeMovementDbToFile.Item1 + "  " + writeMovementDbToFile.Item2, "OK");
                                if (writeMovementDbToFile.Item1)
                                {
                                    DisplaySuccessMessage("SALVESTATUD!");
                                    PrepareTransfer();
                                }
                                else
                                {
                                    DisplayFailMessage(writeMovementDbToFile.Item2);
                                }
                            }
                        //}
                    }
                }
                else
                {
                    DisplayFailMessage("KOGUS PEAB OLEMA NUMBER!");
                }
            }

        }

        private void btnTransferReadCode_Clicked(object sender, EventArgs e)
        {
            SearchEntTransferReadCode();
        }

        private  void btnTransferInternalCode_Clicked(object sender, EventArgs e)
        {
        }

        private  void btnTransferBarCode_Clicked(object sender, EventArgs e)
        {
        }

        private  void btnTransferAddedRowsView_Clicked(object sender, EventArgs e)
        {
            PrepareTransferAddedRowsView();
        }

        public async void SearchEntTransferReadCode()
        {
            if (!string.IsNullOrEmpty(entTransferReadCode.Text))
            {
                if (entTransferReadCode.Text.Length > 4)
                {
                    var result = lstInternalRecordDB.Where(x =>
                       x.itemCode.Contains(entTransferReadCode.Text)
                    || x.itemDesc.ToUpper().Contains(entTransferReadCode.Text.ToUpper())
                    || x.barCode.Contains(entTransferReadCode.Text)).ToList();
                    if (result.Any())
                    {
                        if (result.Count() == 1)
                        {
                            if (lstInternalMovementDB.Any())
                            {
                               
                                frmentTransferQuantity.IsVisible = false;
                                entTransferQuantity.IsVisible = false;
                                lblTransferQuantityUOM.IsVisible = false;
                                frmbtnTransferQuantityOK.IsVisible = false;
                                btnTransferQuantityOK.IsVisible = false;

                                var s = lstInternalMovementDB.Where(x => x.itemCode == result.First().itemCode && x.barCode == result.First().barCode).ToList();
                                if (s.Any())
                                {
                                    obj.isScanAllowed = false;
                                    if (await YesNoDialog("LIIKUMINE", "KAUP ON JUBA LIIGUTATUD. KAS SOOVID PARANDADA?", false))
                                    {
                                        obj.isScanAllowed = true;
                                        frmbtnTransferAddedRowsDelete.IsVisible = true;
                                        transferRecordID = s.First().recordID;
                                        entTransferQuantity.Text = (s.First().quantity).ToString().Replace(".0", "");
                                        lstTransferInfo = new List<ListOfdbRecords>();

                                        int SKUBinCount = 0;
                                        string sKUs = "";
                                        string sKUs2 = "";
                                        decimal sKUqty = 0;
                                        var parseSKU = result.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
                                        if (parseSKU.Any())
                                        {
                                            foreach (var a in parseSKU)
                                            {
                                                var uniqueSKU = a.Split(new[] { "###" }, StringSplitOptions.None);
                                                if (uniqueSKU.Any())
                                                {
                                                    if (uniqueSKU.Count() > 0)
                                                    {
                                                        if (uniqueSKU[0] == obj.shopLocationID)
                                                        {
                                                            SKUBinCount = SKUBinCount + 1;
                                                            sKUqty = Convert.ToDecimal(uniqueSKU[2]);
                                                            if (SKUBinCount < 4)
                                                            {
                                                                sKUs = sKUs + "\r\n" + uniqueSKU[1];
                                                            }
                                                            else
                                                            {
                                                                sKUs2 = sKUs2 + "\r\n" + uniqueSKU[1];
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        sKUs = sKUs.TrimStart().TrimEnd();
                                        sKUs2 = sKUs2.TrimStart().TrimEnd();

                                        lstTransferInfo.Add(new ListOfdbRecords
                                        {
                                            barCode = result.First().barCode,
                                            itemCode = result.First().itemCode,
                                            itemDesc = result.First().itemDesc,
                                            itemMagnitude = result.First().itemMagnitude,
                                            meistriklubihind = result.First().meistriklubihind,
                                            price = result.First().price,
                                            profiklubihind = result.First().profiklubihind,
                                            SKU = result.First().SKU,
                                            SKUqty = sKUqty,
                                            SKUBin = sKUs,
                                            SKUBin2 = sKUs2,
                                            soodushind = result.First().soodushind,
                                            sortiment = result.First().sortiment,

                                        });
                                        lblTransferQuantityUOM.Text = result.First().itemMagnitude;
                                        LstvTransferInfo.ItemsSource = null;
                                        LstvTransferInfo.ItemsSource = lstTransferInfo;
                                        focusedEditor = "entTransferQuantity";
                                        entTransferQuantity.BackgroundColor = Color.Yellow;
                                        ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
                                        entTransferReadCode.BackgroundColor = Color.White;

                                    
                                        frmentTransferQuantity.IsVisible = true;
                                        entTransferQuantity.IsVisible = true;
                                        lblTransferQuantityUOM.IsVisible = true;
                                        frmbtnTransferQuantityOK.IsVisible = true;
                                        btnTransferQuantityOK.IsVisible = true;
                                    }
                                    else
                                    {
                                        transferRecordID = 0;
                                        entTransferQuantity.Text = "";
                                        entTransferReadCode.Text = "";
                                        lblTransferQuantityUOM.Text = "";
                                        lstTransferInfo = new List<ListOfdbRecords>();
                                        LstvTransferInfo.ItemsSource = null;
                                        LstvTransferInfo.ItemsSource = lstTransferInfo;
                                        focusedEditor = "entTransferReadCode";
                                        entTransferReadCode.BackgroundColor = Color.Yellow;
                                        ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
                                        entTransferQuantity.BackgroundColor = Color.White;
                                        frmbtnTransferAddedRowsDelete.IsVisible = false;
                                    }
                                }
                                else
                                {
                                    int SKUBinCount = 0;
                                    string sKUs = "";
                                    string sKUs2 = "";
                                    decimal sKUqty = 0;
                                    var parseSKU = result.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
                                    if (parseSKU.Any())
                                    {
                                        foreach (var a in parseSKU)
                                        {
                                            var uniqueSKU = a.Split(new[] { "###" }, StringSplitOptions.None);
                                            if (uniqueSKU.Any())
                                            {
                                                if (uniqueSKU.Count() > 0)
                                                {
                                                    if (uniqueSKU[0] == obj.shopLocationID)
                                                    {
                                                        SKUBinCount = SKUBinCount + 1;
                                                        sKUqty = Convert.ToDecimal(uniqueSKU[2]);
                                                        if (SKUBinCount < 4)
                                                        {
                                                            sKUs = sKUs + "\r\n" + uniqueSKU[1];
                                                        }
                                                        else
                                                        {
                                                            sKUs2 = sKUs2 + "\r\n" + uniqueSKU[1];
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    sKUs = sKUs.TrimStart().TrimEnd();
                                    sKUs2 = sKUs2.TrimStart().TrimEnd();
                                    transferRecordID = 0;
                                    lstTransferInfo.Add(new ListOfdbRecords
                                    {
                                        barCode = result.First().barCode,
                                        itemCode = result.First().itemCode,
                                        itemDesc = result.First().itemDesc,
                                        itemMagnitude = result.First().itemMagnitude,
                                        meistriklubihind = result.First().meistriklubihind,
                                        price = result.First().price,
                                        profiklubihind = result.First().profiklubihind,
                                        SKU = result.First().SKU,
                                        SKUqty = sKUqty,
                                        SKUBin = sKUs,
                                        SKUBin2 = sKUs2,
                                        soodushind = result.First().soodushind,
                                        sortiment = result.First().sortiment,

                                    });
                                    lblTransferQuantityUOM.Text = result.First().itemMagnitude;
                                    LstvTransferInfo.ItemsSource = null;
                                    LstvTransferInfo.ItemsSource = lstTransferInfo;
                                    focusedEditor = "entTransferQuantity";
                                    entTransferQuantity.BackgroundColor = Color.Yellow;
                                    ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
                                    entTransferReadCode.BackgroundColor = Color.White;

                                   
                                    frmentTransferQuantity.IsVisible = true;
                                    entTransferQuantity.IsVisible = true;
                                    lblTransferQuantityUOM.IsVisible = true;
                                    frmbtnTransferQuantityOK.IsVisible = true;
                                    btnTransferQuantityOK.IsVisible = true;
                                    frmbtnTransferAddedRowsDelete.IsVisible = false;
                                }
                            }
                            else
                            {
                                transferRecordID = 0;
                                entTransferQuantity.Text = "";
                                lstTransferInfo = new List<ListOfdbRecords>();

                                int SKUBinCount = 0;
                                string sKUs = "";
                                string sKUs2 = "";
                                decimal sKUqty = 0;
                                var parseSKU = result.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
                                if (parseSKU.Any())
                                {
                                    foreach (var a in parseSKU)
                                    {
                                        var uniqueSKU = a.Split(new[] { "###" }, StringSplitOptions.None);
                                        if (uniqueSKU.Any())
                                        {
                                            if (uniqueSKU.Count() > 0)
                                            {
                                                if (uniqueSKU[0] == obj.shopLocationID)
                                                {
                                                    SKUBinCount = SKUBinCount + 1;
                                                    sKUqty = Convert.ToDecimal(uniqueSKU[2]);
                                                    if (SKUBinCount < 4)
                                                    {
                                                        sKUs = sKUs + "\r\n" + uniqueSKU[1];
                                                    }
                                                    else
                                                    {
                                                        sKUs2 = sKUs2 + "\r\n" + uniqueSKU[1];
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                sKUs = sKUs.TrimStart().TrimEnd();
                                sKUs2 = sKUs2.TrimStart().TrimEnd();

                                transferRecordID = 0;
                                lstTransferInfo.Add(new ListOfdbRecords
                                {
                                    barCode = result.First().barCode,
                                    itemCode = result.First().itemCode,
                                    itemDesc = result.First().itemDesc,
                                    itemMagnitude = result.First().itemMagnitude,
                                    meistriklubihind = result.First().meistriklubihind,
                                    price = result.First().price,
                                    profiklubihind = result.First().profiklubihind,
                                    SKU = result.First().SKU,
                                    SKUqty = sKUqty,
                                    SKUBin = sKUs,
                                    SKUBin2 = sKUs2,
                                    soodushind = result.First().soodushind,
                                    sortiment = result.First().sortiment,

                                });
                                lblTransferQuantityUOM.Text = result.First().itemMagnitude;
                                LstvTransferInfo.ItemsSource = null;
                                LstvTransferInfo.ItemsSource = lstTransferInfo;
                                focusedEditor = "entTransferQuantity";
                                entTransferQuantity.BackgroundColor = Color.Yellow;
                                ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
                                entTransferReadCode.BackgroundColor = Color.White;

                               
                                frmentTransferQuantity.IsVisible = true;
                                entTransferQuantity.IsVisible = true;
                                lblTransferQuantityUOM.IsVisible = true;
                                frmbtnTransferQuantityOK.IsVisible = true;
                                btnTransferQuantityOK.IsVisible = true;
                                frmbtnTransferAddedRowsDelete.IsVisible = false;
                            }
                            obj.isScanAllowed = true;
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

        private void btnTransferReadCodeClear_Clicked(object sender, EventArgs e)
        {
            entTransferReadCode.Text = "";
        }
        #endregion

        #region stkSelectItem

        public void PrepareSelectItem()
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
                   || x.itemDesc.ToUpper().Contains(entSelectItemReadCode.Text.ToUpper())
                   || x.barCode.Contains(entSelectItemReadCode.Text)).ToList();

                LstvSelectItem.ItemsSource = null;
                LstvSelectItem.ItemsSource = result;
            }
        }
       

        private void LstvSelectItem_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ListOfdbRecords;

            if (obj.previousLayoutName == "StockTake")
            {
                CollapseAllStackPanels.Collapse(this);
                stkStockTake.IsVisible = true;
                obj.currentLayoutName = "StockTake";
                obj.previousLayoutName = "";
                entStockTakeReadCode.Text = item.barCode;
                SearchEntStockTakeReadCode();
            }
            if (obj.previousLayoutName == "Transfer")
            {
                CollapseAllStackPanels.Collapse(this);
                stkTransfer.IsVisible = true;
                obj.currentLayoutName = "Transfer";
                obj.previousLayoutName = "";
                entTransferReadCode.Text = item.barCode;
                SearchEntTransferReadCode();
            }
        }

        private void btnSelectItemReadCode_Clicked(object sender, EventArgs e)
        {
            SearchEntSelectItemReadCode();
        }

        public void SearchEntSelectItemReadCode()
        {
            LstvSelectItem.ItemsSource = null;

            if (!string.IsNullOrEmpty(entSelectItemReadCode.Text))
            {
                if (entSelectItemReadCode.Text.Length > 4)
                {
                    var result = lstInternalRecordDB.Where(x =>
                       x.itemCode.Contains(entSelectItemReadCode.Text)
                    || x.itemDesc.ToUpper().Contains(entSelectItemReadCode.Text.ToUpper())
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

        #region stkItemInfo

        public void PrepareItemInfo(string scannedCode)
        {
            lstBins = new List<ListOfSKU>();
            frmbtnItemInfoBins.IsVisible = false;
            CollapseAllStackPanels.Collapse(this);
            stkItemInfo.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "ItemInfo";
            lblItemInfoHeader.Text = "KAUBA INFO";
            LstvItemInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfo)) : new DataTemplate(typeof(vcItemInfo));
            LstvItemInfoItems.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfoItems)) : new DataTemplate(typeof(vcItemInfoItems));
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
            LstvItemInfo.ItemsSource = null;
            LstvItemInfoItems.ItemsSource = null;
            if (!string.IsNullOrEmpty(scannedCode))
            {
                entItemInfoReadCode.Text = scannedCode;
                SearchEntItemInfoReadCode();
            }

            
            focusedEditor = "entItemInfoReadCode";
            entItemInfoReadCode.BackgroundColor = Color.Yellow;
            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
        }

        private void btnItemInfoReadCode_Clicked(object sender, EventArgs e)
        {
            entItemInfoReadCode.BackgroundColor = Color.White;
            SearchEntItemInfoReadCode();
        }

        public void DisplayItemInfoSKU(ListOfdbRecords row)
        {
            lstBins = new List<ListOfSKU>();
            if (!string.IsNullOrEmpty(row.SKU))
            {
                var parseSKU = row.SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
                if (parseSKU.Any())
                {
                    foreach (var s in parseSKU)
                    {
                        var uniqueSKU = s.Split(new[] { "###" }, StringSplitOptions.None);
                        if (uniqueSKU.Any())
                        {
                            if (!string.IsNullOrEmpty(uniqueSKU[0]))
                            {
                                Debug.WriteLine(uniqueSKU[0] + "  " + uniqueSKU[1] + "  " + uniqueSKU[2]);

                                row.SKUqty = Convert.ToDecimal(uniqueSKU[2]);
                                lstBins.Add(new ListOfSKU
                                {
                                    SKU = uniqueSKU[0],
                                    SKUBin = uniqueSKU[1],
                                    SKUqty = Convert.ToDecimal(uniqueSKU[2]),
                                    SKUShopName = GetShopName(uniqueSKU[0]),
                                    itemMagnitude = row.itemMagnitude
                                });
                            }
                        }
                    }
                }
            }

            var skuBin = "";
            var skuBin2 = "";
            int skuBinCount = 0;


            foreach (var p in lstBins)
            {
                if (p.SKU == obj.shopLocationID)
                {
                    row.SKUqty = p.SKUqty;
                    p.SKUCurrentShop = 1;
                    skuBinCount = skuBinCount + 1;
                    if (skuBinCount < 4)
                    {
                        skuBin = skuBin + p.SKUBin + "\r\n";
                    }
                    else
                    {
                        skuBin2 = skuBin2 + p.SKUBin + "\r\n";
                    }
                }
            }

            row.SKUBin = skuBin.TrimStart().TrimEnd();
            row.SKUBin2 = skuBin2.TrimStart().TrimEnd();

            var lst = new List<ListOfdbRecords>();
            lst.Add(row);
            LstvItemInfo.ItemsSource = null;
            LstvItemInfo.ItemsSource = lst;
            if (lst.Any())
            {
                frmbtnItemInfoBins.IsVisible = true;
            }
            else
            {
                frmbtnItemInfoBins.IsVisible = false;
            }
        }

        public void SearchEntItemInfoReadCode()
        {
            try
            {
                ShowKeyBoard.Hide(this);
                LstvItemInfoItems.ItemsSource = null;
                lstItemInfo = new List<ListOfdbRecords>();
                LstvItemInfo.ItemsSource = null;
                if (!string.IsNullOrEmpty(entItemInfoReadCode.Text))
                {
                    if (entItemInfoReadCode.Text.Length > 4)
                    {
                        lstResultItemInfo = new List<ListOfdbRecords>();
                        var result = lstInternalRecordDB.Where(x => (x.itemCode.Contains(entItemInfoReadCode.Text) || x.itemDesc.ToUpper().Contains(entItemInfoReadCode.Text.ToUpper()) || x.barCode.Contains(entItemInfoReadCode.Text))).ToList();
                        if (result.Any())
                        {
                            Debug.WriteLine("result count " + "  " + result.Count());
                            lstItemInfo = result;
                            LstvItemInfoItems.ItemsSource = null;
                            LstvItemInfoItems.ItemsSource = lstItemInfo;
                            if (lstItemInfo.Count == 1)
                            {
                                DisplayItemInfoSKU(lstItemInfo.First());
                            }
                        }
                        else
                        {
                            DisplayFailMessage("EI LEITUD MIDAGI!");
                            focusedEditor = "entItemInfoReadCode";
                            entItemInfoReadCode.BackgroundColor = Color.Yellow;
                            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
                        }
                    }
                    else
                    {
                        DisplayFailMessage("SISESTA VÄHEMALT 5 TÄHEMÄRKI!");
                        focusedEditor = "entItemInfoReadCode";
                        entItemInfoReadCode.BackgroundColor = Color.Yellow;
                        ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LstvItemInfoItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstItemInfo = new List<ListOfdbRecords>();
            var item = e.Item as ListOfdbRecords;
            DisplayItemInfoSKU(item);
            //lstItemInfo.Add(item);
            //LstvItemInfo.ItemsSource = lstItemInfo;
        }

        private void btnItemInfoReadCodeClear_Clicked(object sender, EventArgs e)
        {
            entItemInfoReadCode.Text = "";
        }


        private void btnItemInfoBins_Clicked(object sender, EventArgs e)
        {
            PrepareItemInfoBinsView();
        }

        #endregion

        #region stkStockTakeAddedRowsView

        public void PrepareStockTakeAddedRowsView()
        {
            ShowKeyBoard.Hide(this);
            CollapseAllStackPanels.Collapse(this);
            stkStockTakeAddedRowsView.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "StockTakeAddedRowsView";
            lblStockTakeAddedRowsViewHeader.Text = "INVENTUURIKANDED";
            LstvStockTakeAddedRowsView.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcInsertedRecords)) : new DataTemplate(typeof(vcInsertedRecords));
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
            LstvStockTakeAddedRowsView.ItemsSource = null;
            LstvStockTakeAddedRowsView.ItemsSource = lstInternalInvDB;
        }
        private async void LstvStockTakeAddedRowsView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private async void btnStockTakeAddedRowsViewClear_Clicked(object sender, EventArgs e)
        {
            if (await YesNoDialog("INVENTUUR", "JÄTKAMISEL KUSTUTATAKSE KÕIK INVENTUURI KIRJED!", false))
            {
                lstInternalInvDB = new List<ListOfInvRecords>();
                //JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                //string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);

                var writeInvDbToFile = await WriteInvRecords.Write(this, "");
                if (writeInvDbToFile.Item1)
                {
                    DisplaySuccessMessage("KUSTUTATUD!");
                    PrepareStockTake();
                }
                else
                {
                    DisplayFailMessage(writeInvDbToFile.Item2);
                }
            }
        }
        #endregion

        #region stkTransferAddedRowsView

        public  void PrepareTransferAddedRowsView()
        {
            ShowKeyBoard.Hide(this);
            CollapseAllStackPanels.Collapse(this);
            stkTransferAddedRowsView.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "TransferAddedRowsView";
            lblTransferAddedRowsViewHeader.Text = "LIIKUMISKANDED";
            LstvTransferAddedRowsView.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcInsertedRecords)) : new DataTemplate(typeof(vcInsertedRecords));
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
            LstvTransferAddedRowsView.ItemsSource = null;
            LstvTransferAddedRowsView.ItemsSource = lstInternalMovementDB;
        }
        private async void LstvTransferAddedRowsView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
        private async void btnTransferAddedRowsViewClear_Clicked(object sender, EventArgs e)
        {
            if (await YesNoDialog("LIIKUMINE", "JÄTKAMISEL KUSTUTATAKSE KÕIK LIIMUSTE KIRJED!", false))
            {
                lstInternalMovementDB = new List<ListOfMovementRecords>();
                //JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                //string data = JsonConvert.SerializeObject(lstInternalMovementDB, jSONsettings);
                //Debug.WriteLine(data);
                var writeMovementDbToFile = await WriteMovementRecords.Write(this, "");
                if (writeMovementDbToFile.Item1)
                {
                    DisplaySuccessMessage("KUSTUTATUD!");
                    PrepareTransfer();
                }
                else
                {
                    DisplayFailMessage(writeMovementDbToFile.Item2);
                }
            }
        }





        #endregion

        #region stkItemInfoBinsView
        public void PrepareItemInfoBinsView()
        {
            ShowKeyBoard.Hide(this);
            CollapseAllStackPanels.Collapse(this);
            stkItemInfoBinsView.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "ItemInfoBinsView";
            obj.previousLayoutName = null;
            obj.nextLayoutName = null;
            obj.mainOperation = null;

            lblItemInfoBinsViewHeader.Text = "RIIULID";
            LstvItemInfoBinsView.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfoBinsView)) : new DataTemplate(typeof(vcItemInfoBinsView));
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

            var lstToDisplay = lstBins.GroupBy(x => x.SKU).Select(s => new ListOfSKU
            {
                SKU = s.First().SKU,
                SKUBin = s.First().SKUBin,
                SKUqty = s.First().SKUqty,
                itemMagnitude = s.First().itemMagnitude,
                SKUCurrentShop = s.First().SKU == obj.shopLocationID ? 1 : 0,
                SKUShopName = s.First().SKUShopName
            }).ToList();

            
            foreach (var s in lstToDisplay)
            {
                string sku = "";
                string sku2 = "";
                int skucount = 0;
                var bins = lstBins.Where(x => x.SKU == s.SKU);
                if (bins.Any())
                {
                    skucount = skucount + 1;
                    foreach (var p in bins)
                    {                        
                        if (skucount < 5)
                        {
                            sku = sku + p.SKUBin + "\r\n";
                        }
                        else
                        {
                            sku2 = sku2 + p.SKUBin + "\r\n";
                        }
                    }
                }
                s.SKUBin = sku.TrimStart().TrimEnd();
                s.SKUBin2 = sku2.TrimStart().TrimEnd();
            }
            
            lblItemInfoBinsViewCount.Text = "KOGUS KÕIGIS POODIDES KOKKU: " + lstToDisplay.Sum(x => x.SKUqty);

            Debug.WriteLine("BINSARE lstToDisplay :" + lstToDisplay.First().SKUBin);
            LstvItemInfoBinsView.ItemsSource = null;
            LstvItemInfoBinsView.ItemsSource = lstToDisplay;
        }
        private async void LstvItemInfoBinsView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        public string SplitBins(string bin)
        {
            string result = "";
            var parseSKU = bin.Split(new[] { ";" }, StringSplitOptions.None);
            if (parseSKU.Any())
            {
                foreach (var p in parseSKU)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result = p;
                    }
                    else
                    {
                        result = result + "\r\n" + p;
                    }
                }
            }
            return result;
        }






        #endregion

        #region stkPurchaseReceiveOrders

        public void PreparePurchaseReceiveOrders()
        {
            CollapseAllStackPanels.Collapse(this);
            stkPurchaseReceiveOrders.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "PurchaseReceiveOrders";
            lblPurchaseReceiveOrdersHeader.Text = "OSTUTARNED";
           
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
            LstvPurchaseReceiveOrders.ItemsSource = null;
            LstvPurchaseReceiveOrders.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrders)) : new DataTemplate(typeof(vcPurchaseOrders));

            lstPurchaseOrders = lstInternalPurchaseReceiveDB.Where(x =>x.shop == obj.shopLocationID).ToList().GroupBy(x => x.docNo).Select(s => new ListOfPurchaseReceive
            {
                docNo = s.First().docNo,
                vendorCode = s.First().vendorCode,
                vendorName = s.First().vendorName,
                vendorReference = s.First().vendorReference,
                shop = s.First().shop,
                shipmentDate = s.First().shipmentDate,
            }).ToList().OrderBy(x => x.shipmentDate).ToList();
            LstvPurchaseReceiveOrders.ItemsSource = lstPurchaseOrders;
            focusedEditor = "entPurchaseReceiveOrders";
           
            
            //entPurchaseReceiveOrders.BackgroundColor = Color.Yellow;
            //ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
        }

        private void btnPurchaseReceiveOrdersSearch_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine(entPurchaseReceiveOrders.Text);
            if (!string.IsNullOrEmpty(entPurchaseReceiveOrders.Text))
            {
                string searchValue = entPurchaseReceiveOrders.Text.ToUpper();
                var result = lstPurchaseOrders.Where(x =>
                x.docNo.ToUpper().Contains(searchValue) ||
                x.vendorCode.ToUpper().Contains(searchValue) ||
                x.vendorName.ToUpper().Contains(searchValue) ||
                x.vendorReference.ToUpper().Contains(searchValue));
                if (!result.Any())
                {
                    DisplayFailMessage("OTSITUD VÄÄRTUST EI LEITUD!");
                }
                LstvPurchaseReceiveOrders.ItemsSource = result;
            }
            else
            {
                LstvPurchaseReceiveOrders.ItemsSource = lstPurchaseOrders;
            }
        }

        private void LstvPurchaseReceiveOrders_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ListOfPurchaseReceive;
            lstPurchaseOrderLines = lstInternalPurchaseReceiveDB.Where(x => x.docNo == item.docNo && x.shop == obj.shopLocationID).ToList();
            if (lstPurchaseOrderLines.Any())
            {
                foreach (var p in lstPurchaseOrderLines)
                {
                    var itemInfo = lstInternalRecordDB.Where(x => x.itemCode == p.itemCode).ToList();
                    if (itemInfo.Any())
                    {
                        p.itemDesc = itemInfo.First().itemDesc;
                        p.barCode = itemInfo.First().barCode;
                    }
                }
                PreparePurchaseReceiveOrderLines();
            }

        }
        private void btnPurchaseReceiveOrdersReadCodeClear_Clicked(object sender, EventArgs e)
        {
            entPurchaseReceiveOrders.Text = "";
        }


        #endregion

        #region stkPurchaseReceiveOrderLines
        public void PreparePurchaseReceiveOrderLines()
        {
            CollapseAllStackPanels.Collapse(this);
            stkPurchaseReceiveOrderLines.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "PurchaseReceiveOrderLines";
            lblPurchaseReceiveOrderLinesHeader.Text = "OSTUTARNE READ";

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
            LstvPurchaseReceiveOrderLines.ItemsSource = null;
            LstvPurchaseReceiveOrderLines.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrderLines)) : new DataTemplate(typeof(vcPurchaseOrderLines));
            LstvPurchaseReceiveOrderLines.ItemsSource = lstPurchaseOrderLines;
            focusedEditor = "entPurchaseReceiveOrderLines";


            //entPurchaseReceiveOrders.BackgroundColor = Color.Yellow;
            //ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
        }
        private void LstvPurchaseReceiveOrderLines_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as ListOfPurchaseReceive;
                lstPurchaseOrderQuantityInsertInfo = new List<ListOfPurchaseReceive>();
                lstPurchaseOrderQuantityInsertInfo.Add(item);
                PreparePurchaseOrderQuantityInsert();
            }
            catch (Exception ex)
            {
                DisplayAlert("LstvPurchaseReceiveOrderLines_ItemTapped", ex.Message, "OK");
            }
        }

        private void btnPurchaseReceiveOrderLinesSearch_Clicked(object sender, EventArgs e)
        {
            SearchPurchaseReceiveOrderLines();
        }

        public void SearchPurchaseReceiveOrderLines()
        {
            Debug.WriteLine(entPurchaseReceiveOrderLines.Text);
            if (!string.IsNullOrEmpty(entPurchaseReceiveOrderLines.Text))
            {
                string searchValue = entPurchaseReceiveOrderLines.Text.ToUpper();
                var result = lstPurchaseOrderLines.Where(x =>
                x.itemCode.ToUpper().Contains(searchValue) ||
                x.itemDesc.ToUpper().Contains(searchValue) ||
                x.barCode.ToUpper().Contains(searchValue));

                if (result.Any())
                {
                    DisplayFailMessage("OTSITUD VÄÄRTUST EI LEITUD!");
                }
                {
                    if (result.Count() == 1)
                    {

                    }
                }
                LstvPurchaseReceiveOrderLines.ItemsSource = result;
                
            }
            else
            {
                LstvPurchaseReceiveOrderLines.ItemsSource = lstPurchaseOrderLines;
            }
        }

        private void btnPurchaseReceiveOrderLinesReadCodeClear_Clicked(object sender, EventArgs e)
        {
            entPurchaseReceiveOrderLines.Text = "";
        }
        #endregion

        #region stkPurchaseOrderQuantityInsert
        public async void PreparePurchaseOrderQuantityInsert()
        {
            bool proceed = true;
            decimal previouslyReadQty = 0;
            if (lstPurchaseOrderPickedQuantities.Any())
            {
                var previousRead = lstPurchaseOrderPickedQuantities.Where(x => x.dokno == lstPurchaseOrderQuantityInsertInfo.First().docNo && x.dokreanr == lstPurchaseOrderQuantityInsertInfo.First().docLineNo && x.pood == obj.shopLocationID);
                if (previousRead.Any())
                {
                    if (!await YesNoDialog("OSTU VASTUVÕTT", lstPurchaseOrderQuantityInsertInfo.First().docNo + " RIDA " + lstPurchaseOrderQuantityInsertInfo.First().docLineNo + " ON JUBA LOETUD - KAS SOOVID PARANDADA?", false))
                    {
                        proceed = false;
                    }
                    else
                    {
                        previouslyReadQty = previousRead.First().pickedQty;
                        purchReceiveRecordID = previousRead.First().recordID;
                    }
                }
            }
            if (proceed)
            {
                CollapseAllStackPanels.Collapse(this);
                stkPurchaseOrderQuantityInsert.IsVisible = true;
                obj.mainOperation = "";
                obj.currentLayoutName = "PurchaseOrderQuantityInsert";
                lblPurchaseOrderQuantityInsertHeader.Text = "OSTUTARNE REA KOGUS";

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
                LstvPurchaseOrderQuantityInsertInfo.ItemsSource = null;
                LstvPurchaseOrderQuantityInsertInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrderQuantityInsertInfo)) : new DataTemplate(typeof(vcPurchaseOrderQuantityInsertInfo));
                LstvPurchaseOrderQuantityInsertInfo.ItemsSource = lstPurchaseOrderQuantityInsertInfo;
                focusedEditor = "entPurchaseOrderQuantityInsertQuantity";

                lblPurchaseOrderQuantityInsertQuantityUOM.Text = lstPurchaseOrderQuantityInsertInfo.First().magnitude;




                lstPurchaseOrderQuantityInsertQuantities = new List<ListOfSHRCVToExport>();
                var row = new ListOfSHRCVToExport
                {
                    initialQty = lstPurchaseOrderQuantityInsertInfo.First().initialQty,
                    pickedQty = previouslyReadQty,
                    remainingQty = (lstPurchaseOrderQuantityInsertInfo.First().initialQty - previouslyReadQty),
                    magnitude = lstPurchaseOrderQuantityInsertInfo.First().magnitude
                };
                lstPurchaseOrderQuantityInsertQuantities.Add(row);
                LstvPurchaseOrderQuantityInsertQuantityInfo.ItemsSource = null;
                if (obj.showPurchaseReceiveQty)
                {
                    LstvPurchaseOrderQuantityInsertQuantityInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrderQuantityInsertQuantityInfoFull)) : new DataTemplate(typeof(vcPurchaseOrderQuantityInsertQuantityInfoFull));
                }
                else
                {
                    LstvPurchaseOrderQuantityInsertQuantityInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrderQuantityInsertQuantityInfo)) : new DataTemplate(typeof(vcPurchaseOrderQuantityInsertQuantityInfo));
                }
                LstvPurchaseOrderQuantityInsertQuantityInfo.ItemsSource = lstPurchaseOrderQuantityInsertQuantities;

                if (previouslyReadQty > 0)
                {
                    entPurchaseOrderQuantityInsertQuantity.Text = (String.Format("{0:0.00}", previouslyReadQty)).Replace(".00", "");
                }
                else
                {
                    entPurchaseOrderQuantityInsertQuantity.Text = "";
                }
                entPurchaseOrderQuantityInsertQuantity.BackgroundColor = Color.Yellow;
                ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
            }
        }
        
        private async void btnPurchaseOrderQuantityInsertQuantityOK_Clicked(object sender, EventArgs e)
        {
            //lstPurchaseOrderPickedQuantities = new List<ListOfSHRCVToExport>();
            try
            {
                bool proceed = true;
                decimal quantity = 0;
                if (!string.IsNullOrEmpty(entPurchaseOrderQuantityInsertQuantity.Text))
                {
                    quantity = TryParseDecimal.Parse(entPurchaseOrderQuantityInsertQuantity.Text);
                }
                if (string.IsNullOrEmpty(lstPurchaseOrderQuantityInsertInfo.First().itemCode))
                {
                    proceed = false;
                    DisplayFailMessage("KAUPA POLE VALITUD!");
                }
                if (quantity == 0)
                {
                    if (!await YesNoDialog("INVENTUUR", "SISESTATUD KOGUS ON 0. KAS JÄTKATA KIRJE TEKITAMISEGA?", false))
                    {
                        proceed = false;
                    }
                }

                if (proceed)
                {
                    if (quantity > -1)
                    {
                        Debug.WriteLine("X2");
                        if (purchReceiveRecordID == 0)
                        {
                            int lastRecordID = 0;
                            if (lstPurchaseOrderPickedQuantities.Any())
                            {
                                lastRecordID = lstPurchaseOrderPickedQuantities.OrderBy(x => x.recordID).Take(1).First().recordID;
                            }
                            lstPurchaseOrderPickedQuantities.Add(new ListOfSHRCVToExport
                            {
                                dokno = lstPurchaseOrderQuantityInsertInfo.First().docNo,
                                dokreanr = lstPurchaseOrderQuantityInsertInfo.First().docLineNo,
                                initialQty = lstPurchaseOrderQuantityInsertInfo.First().initialQty,
                                pickedQty = quantity,
                                recordDate = DateTime.Now,
                                pood = lstPurchaseOrderQuantityInsertInfo.First().shop,
                                recordID = lastRecordID + 1
                            });

                            JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                            string data = JsonConvert.SerializeObject(lstPurchaseOrderPickedQuantities, jSONsettings);

                            var writePurchaseOrderPickedQuantitiesDbToFile = await WritePurchaseOrderPickedQuantitiesRecords.Write(this, data);
                            if (writePurchaseOrderPickedQuantitiesDbToFile.Item1)
                            {
                                DisplaySuccessMessage("SALVESTATUD!");
                                PreparePurchaseReceiveOrderLines();
                            }
                            else
                            {
                                DisplayFailMessage(writePurchaseOrderPickedQuantitiesDbToFile.Item2);
                            }
                        }
                        else
                        {
                            if (quantity == 0)
                            {
                                var record = lstPurchaseOrderPickedQuantities.Where(x => x.recordID == purchReceiveRecordID);
                                if (record.Any())
                                {
                                    lstPurchaseOrderPickedQuantities.Remove(record.Take(1).First());
                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    string data = JsonConvert.SerializeObject(lstPurchaseOrderPickedQuantities, jSONsettings);

                                    var writePurchaseOrderPickedQuantitiesDbToFile = await WritePurchaseOrderPickedQuantitiesRecords.Write(this, data);
                                    if (writePurchaseOrderPickedQuantitiesDbToFile.Item1)
                                    {
                                        purchReceiveRecordID = 0;
                                        DisplaySuccessMessage("SALVESTATUD2!");
                                        PreparePurchaseReceiveOrderLines();
                                    }
                                    else
                                    {
                                        DisplayFailMessage(writePurchaseOrderPickedQuantitiesDbToFile.Item2);
                                    }
                                }
                            }
                            else
                            {
                                Debug.WriteLine("X3 " + purchReceiveRecordID);
                                Debug.WriteLine("AA " + lstPurchaseOrderPickedQuantities.First().recordID);
                                var record = lstPurchaseOrderPickedQuantities.Where(x => x.recordID == purchReceiveRecordID);
                                if (record.Any())
                                {
                                    record.First().pickedQty = quantity;
                                    record.First().recordDate = DateTime.Now;

                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    string data = JsonConvert.SerializeObject(lstPurchaseOrderPickedQuantities, jSONsettings);

                                    var writePurchaseOrderPickedQuantitiesDbToFile = await WritePurchaseOrderPickedQuantitiesRecords.Write(this, data);
                                    if (writePurchaseOrderPickedQuantitiesDbToFile.Item1)
                                    {                                        
                                        DisplaySuccessMessage("SALVESTATUD3!");
                                        PreparePurchaseReceiveOrderLines();
                                    }
                                    else
                                    {
                                        DisplayFailMessage(writePurchaseOrderPickedQuantitiesDbToFile.Item2);
                                    }
                                }
                                //}
                            }
                        }
                    }
                    else
                    {
                        DisplayFailMessage("KOGUS PEAB OLEMA NUMBER!");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion

        #region stkTransferReceiveOrders
        public void PrepareTransferReceiveOrders()
        {
            CollapseAllStackPanels.Collapse(this);
            stkTransferReceiveOrders.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "TransferReceiveOrders";
            lblTransferReceiveOrdersHeader.Text = "ÜLEVIIMISTARNED";

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
            LstvTransferReceiveOrders.ItemsSource = null;
            LstvTransferReceiveOrders.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrders)) : new DataTemplate(typeof(vcTransferOrders));

            lstTransferOrders = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID).ToList().GroupBy(x => x.docNo).Select(s => new ListOfTransferReceive
            {
                docNo = s.First().docNo,
                shop = s.First().shop,
                shipmentDate = s.First().shipmentDate,
            }).ToList().OrderBy(x => x.shipmentDate).ToList();
            LstvTransferReceiveOrders.ItemsSource = lstTransferOrders;
            focusedEditor = "entTransferReceiveOrders";
        }

        private void btnTransferReceiveOrdersSearch_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine(entTransferReceiveOrders.Text);
            if (!string.IsNullOrEmpty(entTransferReceiveOrders.Text))
            {
                string searchValue = entTransferReceiveOrders.Text.ToUpper();
                var result = lstTransferOrders.Where(x =>
                x.docNo.ToUpper().Contains(searchValue));            
                if (!result.Any())
                {
                    DisplayFailMessage("OTSITUD VÄÄRTUST EI LEITUD!");
                }
                LstvTransferReceiveOrders.ItemsSource = result;
            }
            else
            {
                LstvTransferReceiveOrders.ItemsSource = lstTransferOrders;
            }
        }

        private void LstvTransferReceiveOrders_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ListOfPurchaseReceive;
            lstTransferOrderLines = lstInternalTransferReceiveDB.Where(x => x.docNo == item.docNo && x.shop == obj.shopLocationID).ToList();
            if (lstTransferOrderLines.Any())
            {
                foreach (var p in lstTransferOrderLines)
                {
                    var itemInfo = lstInternalRecordDB.Where(x => x.itemCode == p.itemCode).ToList();
                    if (itemInfo.Any())
                    {
                        p.itemDesc = itemInfo.First().itemDesc;
                        p.barCode = itemInfo.First().barCode;
                    }
                }
                //PrepareTransferReceiveOrderLines();
            }

        }
        private void btnTransferReceiveOrdersReadCodeClear_Clicked(object sender, EventArgs e)
        {
            entTransferReceiveOrders.Text = "";
        }



        #endregion

        #region stkTransferReceiveOrderLines
        public void PrepareTransferReceiveOrderLines()
        {
            CollapseAllStackPanels.Collapse(this);
            stkTransferReceiveOrderLines.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "TransferReceiveOrderLines";
            lblTransferReceiveOrderLinesHeader.Text = "ÜLEVIIMISTARNE READ";

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
            LstvTransferReceiveOrderLines.ItemsSource = null;
            LstvTransferReceiveOrderLines.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrderLines)) : new DataTemplate(typeof(vcTransferOrderLines));
            LstvTransferReceiveOrderLines.ItemsSource = lstTransferOrderLines;
            focusedEditor = "entTransferReceiveOrderLines";


            //entPurchaseReceiveOrders.BackgroundColor = Color.Yellow;
            //ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
        }
        private void LstvTransferReceiveOrderLines_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as ListOfTransferReceive;
                lstTransferOrderQuantityInsertInfo = new List<ListOfTransferReceive>();
                lstTransferOrderQuantityInsertInfo.Add(item);
                PreparePurchaseOrderQuantityInsert();
            }
            catch (Exception ex)
            {
                DisplayAlert("LstvTransferReceiveOrderLines_ItemTapped", ex.Message, "OK");
            }
        }

        private void btnTransferReceiveOrderLinesSearch_Clicked(object sender, EventArgs e)
        {
            SearchTransferReceiveOrderLines();
        }

        public void SearchTransferReceiveOrderLines()
        {
            Debug.WriteLine(entTransferReceiveOrderLines.Text);
            if (!string.IsNullOrEmpty(entTransferReceiveOrderLines.Text))
            {
                string searchValue = entTransferReceiveOrderLines.Text.ToUpper();
                var result = lstTransferOrderLines.Where(x =>
                x.itemCode.ToUpper().Contains(searchValue) ||
                x.itemDesc.ToUpper().Contains(searchValue) ||
                x.barCode.ToUpper().Contains(searchValue));

                if (result.Any())
                {
                    DisplayFailMessage("OTSITUD VÄÄRTUST EI LEITUD!");
                }
                {
                    if (result.Count() == 1)
                    {

                    }
                }
                LstvTransferReceiveOrderLines.ItemsSource = result;

            }
            else
            {
                LstvTransferReceiveOrderLines.ItemsSource = lstTransferOrderLines;
            }
        }

        private void btnTransferReceiveOrderLinesReadCodeClear_Clicked(object sender, EventArgs e)
        {
            entTransferReceiveOrderLines.Text = "";
        }
        #endregion

        #region stkTransferOrderQuantityInsert
        public async void PrepareTransferOrderQuantityInsert()
        {
            bool proceed = true;
            decimal previouslyReadQty = 0;
            if (lstPurchaseOrderPickedQuantities.Any())
            {
                var previousRead = lstTransferOrderPickedQuantities.Where(x => x.dokno == lstTransferOrderQuantityInsertInfo.First().docNo && x.dokreanr == lstTransferOrderQuantityInsertInfo.First().docLineNo && x.pood == obj.shopLocationID);
                if (previousRead.Any())
                {
                    if (!await YesNoDialog("OSTU VASTUVÕTT", lstTransferOrderQuantityInsertInfo.First().docNo + " RIDA " + lstTransferOrderQuantityInsertInfo.First().docLineNo + " ON JUBA LOETUD - KAS SOOVID PARANDADA?", false))
                    {
                        proceed = false;
                    }
                    else
                    {
                        previouslyReadQty = previousRead.First().pickedQty;
                        purchReceiveRecordID = previousRead.First().recordID;
                    }
                }
            }
            if (proceed)
            {
                CollapseAllStackPanels.Collapse(this);
                stkTransferOrderQuantityInsert.IsVisible = true;
                obj.mainOperation = "";
                obj.currentLayoutName = "TransferOrderQuantityInsert";
                lblTransferOrderQuantityInsertHeader.Text = "ÜLEVIIMISE REA KOGUS";

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
                LstvTransferOrderQuantityInsertInfo.ItemsSource = null;
                LstvTransferOrderQuantityInsertInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrderQuantityInsertInfo)) : new DataTemplate(typeof(vcTransferOrderQuantityInsertInfo));
                LstvTransferOrderQuantityInsertInfo.ItemsSource = lstPurchaseOrderQuantityInsertInfo;
                focusedEditor = "entPurchaseOrderQuantityInsertQuantity";

                lblTransferOrderQuantityInsertQuantityUOM.Text = lstTransferOrderQuantityInsertInfo.First().magnitude;




                lstTransferOrderQuantityInsertQuantities = new List<ListOfTRFRCVToExport>();
                var row = new ListOfTRFRCVToExport
                {
                    initialQty = lstTransferOrderQuantityInsertInfo.First().initialQty,
                    pickedQty = previouslyReadQty,
                    remainingQty = (lstTransferOrderQuantityInsertInfo.First().initialQty - previouslyReadQty),
                    magnitude = lstTransferOrderQuantityInsertInfo.First().magnitude
                };
                lstTransferOrderQuantityInsertQuantities.Add(row);
                LstvTransferOrderQuantityInsertQuantityInfo.ItemsSource = null;
                if (obj.showTransferReceiveQty)
                {
                    LstvTransferOrderQuantityInsertQuantityInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrderQuantityInsertQuantityInfoFull)) : new DataTemplate(typeof(vcTransferOrderQuantityInsertQuantityInfoFull));
                }
                else
                {
                    LstvTransferOrderQuantityInsertQuantityInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrderQuantityInsertQuantityInfo)) : new DataTemplate(typeof(vcTransferOrderQuantityInsertQuantityInfo));
                }
                LstvTransferOrderQuantityInsertQuantityInfo.ItemsSource = lstTransferOrderQuantityInsertQuantities;

                if (previouslyReadQty > 0)
                {
                    entTransferOrderQuantityInsertQuantity.Text = (String.Format("{0:0.00}", previouslyReadQty)).Replace(".00", "");
                }
                else
                {
                    entTransferOrderQuantityInsertQuantity.Text = "";
                }
                entTransferOrderQuantityInsertQuantity.BackgroundColor = Color.Yellow;
                ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
            }
        }

        private async void btnTransferOrderQuantityInsertQuantityOK_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool proceed = true;
                decimal quantity = 0;
                if (!string.IsNullOrEmpty(entTransferOrderQuantityInsertQuantity.Text))
                {
                    quantity = TryParseDecimal.Parse(entTransferOrderQuantityInsertQuantity.Text);
                }
                if (string.IsNullOrEmpty(lstTransferOrderQuantityInsertInfo.First().itemCode))
                {
                    proceed = false;
                    DisplayFailMessage("KAUPA POLE VALITUD!");
                }
                if (quantity == 0)
                {
                    if (!await YesNoDialog("ÜLEVIIMISE TARNE", "SISESTATUD KOGUS ON 0. KAS JÄTKATA KIRJE TEKITAMISEGA?", false))
                    {
                        proceed = false;
                    }
                }

                if (proceed)
                {
                    if (quantity > -1)
                    {
                        Debug.WriteLine("X2");
                        if (transferReceiveRecordID == 0)
                        {
                            int lastRecordID = 0;
                            if (lstTransferOrderPickedQuantities.Any())
                            {
                                lastRecordID = lstTransferOrderPickedQuantities.OrderBy(x => x.recordID).Take(1).First().recordID;
                            }
                            lstTransferOrderPickedQuantities.Add(new ListOfTRFRCVToExport
                            {
                                dokno = lstTransferOrderQuantityInsertInfo.First().docNo,
                                dokreanr = lstTransferOrderQuantityInsertInfo.First().docLineNo,
                                initialQty = lstTransferOrderQuantityInsertInfo.First().initialQty,
                                pickedQty = quantity,
                                recordDate = DateTime.Now,
                                pood = lstTransferOrderQuantityInsertInfo.First().shop,
                                recordID = lastRecordID + 1
                            });

                            JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                            string data = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                            var writeTransferOrderPickedQuantitiesDbToFile = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data);
                            if (writeTransferOrderPickedQuantitiesDbToFile.Item1)
                            {
                                DisplaySuccessMessage("SALVESTATUD!");
                                PrepareTransferReceiveOrderLines();
                            }
                            else
                            {
                                DisplayFailMessage(writeTransferOrderPickedQuantitiesDbToFile.Item2);
                            }
                        }
                        else
                        {
                            if (quantity == 0)
                            {
                                var record = lstTransferOrderPickedQuantities.Where(x => x.recordID == transferReceiveRecordID);
                                if (record.Any())
                                {
                                    lstTransferOrderPickedQuantities.Remove(record.Take(1).First());
                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    string data = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                    var writeTransferOrderPickedQuantitiesDbToFile = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data);
                                    if (writeTransferOrderPickedQuantitiesDbToFile.Item1)
                                    {
                                        transferReceiveRecordID = 0;
                                        DisplaySuccessMessage("SALVESTATUD!");
                                        PrepareTransferReceiveOrderLines();
                                    }
                                    else
                                    {
                                        DisplayFailMessage(writeTransferOrderPickedQuantitiesDbToFile.Item2);
                                    }
                                }
                            }
                            else
                            {
                                Debug.WriteLine("X3 " + transferReceiveRecordID);
                                Debug.WriteLine("AA " + lstTransferOrderPickedQuantities.First().recordID);
                                var record = lstTransferOrderPickedQuantities.Where(x => x.recordID == transferReceiveRecordID);
                                if (record.Any())
                                {
                                    record.First().pickedQty = quantity;
                                    record.First().recordDate = DateTime.Now;

                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    string data = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                    var writeTransferOrderPickedQuantitiesDbToFile = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data);
                                    if (writeTransferOrderPickedQuantitiesDbToFile.Item1)
                                    {
                                        DisplaySuccessMessage("SALVESTATUD!");
                                        PrepareTransferReceiveOrderLines();
                                    }
                                    else
                                    {
                                        DisplayFailMessage(writeTransferOrderPickedQuantitiesDbToFile.Item2);
                                    }
                                }
                                //}
                            }
                        }
                    }
                    else
                    {
                        DisplayFailMessage("KOGUS PEAB OLEMA NUMBER!");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
    