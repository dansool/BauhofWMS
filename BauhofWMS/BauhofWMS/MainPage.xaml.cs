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
using BauhofWMSDLL.Utils;
using BauhofWMS.Keyboard;
using BauhofWMS.Scanner;
using BauhofWMS.Utils.Parsers;
using ProgressRingControl.Forms.Plugin;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

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
        public ReadTransferOrderPickedQuantitiesRecords ReadTransferOrderPickedQuantitiesRecords = new ReadTransferOrderPickedQuantitiesRecords();
        public WritePurchaseOrderPickedQuantitiesRecords WritePurchaseOrderPickedQuantitiesRecords = new WritePurchaseOrderPickedQuantitiesRecords();
        public WriteTransferOrderPickedQuantitiesRecords WriteTransferOrderPickedQuantitiesRecords = new WriteTransferOrderPickedQuantitiesRecords();

        public WritePurchaseReceiveRecordsToExportFile WritePurchaseReceiveRecordsToExportFile = new WritePurchaseReceiveRecordsToExportFile();
        public WriteTransferReceiveRecordsToExportFile WriteTransferReceiveRecordsToExportFile = new WriteTransferReceiveRecordsToExportFile();
        public WriteLog WriteLog = new WriteLog();
		#endregion
		#region Variables

		private double _width = 0.0;
		private double _height = 0.0;
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

        public string currentPurchaseOrder = "";
        public string currentTransferOrder = "";

        public bool defaultvalueOverride;

        public string entPurchaseReceiveOrderLinesSearchValue;
        public string entTransferReceiveOrderLinesSearchValue;

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
        public List<ListOfMagnitudes> lstItemMagnitudes = new List<ListOfMagnitudes>();
        public List<ListOfBarcodes> lstBarcodes = new List<ListOfBarcodes>();
		#endregion

        #region MainPage operations
        public MainPage()
        {
            InitializeComponent();
            obj.mp = this;
            grdProgressBar.IsVisible = true;
			
			StartMainPage();
        }

		private void RotationChanges()
		{
			//if (obj.isZebra)
			//{
			//	Debug.WriteLine("true obj.isZebra " + obj.isZebra);
			//	Debug.WriteLine("true obj.isHoneywell " + obj.isHoneywell);
			//	grdKeyBoards.Margin = new Thickness(0, 340, 0, 0);
			//	grdKeyBoards.ScaleX = 1.0;
			//	grdKeyBoards.ScaleY = 1.0;
			//}
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			if (width != _width || height != _height)
			{
				_width = width;
				_height = height;
				RotationChanges();
			}
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
                Debug.WriteLine("StartMainPage StartMainPage");
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
                    if (Device.RuntimePlatform == Device.UWP) { obj.operatingSystem = "UWP"; UWP(); }
                    if (Device.RuntimePlatform == Device.Android) { obj.operatingSystem = "Android"; Android(); }

                    
                    ScannedValueReceive();


                    var resultSettings = await ReadSettings.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
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
                        Debug.WriteLine("lstSettings.First().showPurchaseReceiveQty " + lstSettings.First().showPurchaseReceiveQty);
                        obj.showPurchaseReceiveQty = lstSettings.First().showPurchaseReceiveQty;
                        obj.showPurchaseReceiveQtySum = lstSettings.First().showPurchaseReceiveQtySum;
                        obj.showTransferReceiveQty = lstSettings.First().showTransferReceiveQty;
                        obj.showTransferReceiveQtySum = lstSettings.First().showTransferReceiveQtySum;
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

                        if (obj.showPurchaseReceiveQtySum)
                        {
                            rbtnPurchaseReceiveQtySumYes.IsChecked = true;
                            rbtnPurchaseReceiveQtySumNo.IsChecked = false;
                        }
                        else
                        {
                            rbtnPurchaseReceiveQtySumYes.IsChecked = false;
                            rbtnPurchaseReceiveQtySumNo.IsChecked = true;
                        }

                        if (obj.showTransferReceiveQty)
                        {
                            rbtnTransferReceiveQtyYes.IsChecked = true;
                        }
                        else
                        {
                            rbtnTransferReceiveQtyNo.IsChecked = true;
                        }

                        if (obj.showTransferReceiveQtySum)
                        {
                            rbtnTransferReceiveQtySumYes.IsChecked = true;
                            rbtnTransferReceiveQtySumNo.IsChecked = false;
                        }
                        else
                        {
                            rbtnTransferReceiveQtySumYes.IsChecked = false;
                            rbtnTransferReceiveQtySumNo.IsChecked = true;
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
                        Debug.WriteLine("SHOPLOCATION A : " + lstSettings.First().shopLocationCode);
                        if (!string.IsNullOrEmpty(lstSettings.First().shopLocationCode))
                        {
                            grdProgressBar.IsVisible = true;
                            //prgRing = new  ProgressRing { RingThickness = 20, Progress = 0.5f };
                            progressBarActive = true;
                            if (Device.RuntimePlatform == Device.UWP)
                            {
                                var resultReaddbRecords = await ReaddbRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                if (resultReaddbRecords.Item1)
                                {
                                    if (!string.IsNullOrEmpty(resultReaddbRecords.Item2))
                                    {
                                        Stopwatch swlstInternalRecordDB = Stopwatch.StartNew();
                                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                        lstInternalRecordDB = JsonConvert.DeserializeObject<List<ListOfdbRecords>>(resultReaddbRecords.Item2, jSONsettings);
                                        progressBarActive = false;
                                        swlstInternalRecordDB.Stop();
                                        WriteLog.Write(this, "InternalRecordDB created - " + lstInternalRecordDB.Count().ToString() + " recods. Time elapsed : " + swlstInternalRecordDB.Elapsed.Milliseconds.ToString() + " milliseconds");
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


                                                var resultReadPurchaseReceivedbRecords = await ReadPurchaseReceiveRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                                if (resultReadPurchaseReceivedbRecords.Item1)
                                                {
                                                    //Debug.WriteLine(resultReadPurchaseReceivedbRecords.Item2);
                                                    if (!string.IsNullOrEmpty(resultReadPurchaseReceivedbRecords.Item2))
                                                    {
                                                        Stopwatch swlstInternalPurchaseReceiveDB = Stopwatch.StartNew();
                                                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                                        lstInternalPurchaseReceiveDB = JsonConvert.DeserializeObject<List<ListOfPurchaseReceive>>(resultReadPurchaseReceivedbRecords.Item2, jSONsettings);
                                                        progressBarActive = false;
                                                        swlstInternalPurchaseReceiveDB.Stop();
                                                        WriteLog.Write(this, "PurchaseReceiveDB created - " + lstInternalPurchaseReceiveDB.Count().ToString() + " recods. Time elapsed : " + swlstInternalPurchaseReceiveDB.Elapsed.Milliseconds.ToString() + " milliseconds");
                                                    }
                                                }

                                                var resultReadTransferReceivedbRecords = await ReadTransferReceiveRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                                if (resultReadTransferReceivedbRecords.Item1)
                                                {
                                                    //Debug.WriteLine(resultReadTransferReceivedbRecords.Item2);
                                                    if (!string.IsNullOrEmpty(resultReadTransferReceivedbRecords.Item2))
                                                    {
                                                        Stopwatch swlstInternalTransferReceiveDB = Stopwatch.StartNew();
                                                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                                        lstInternalTransferReceiveDB = JsonConvert.DeserializeObject<List<ListOfTransferReceive>>(resultReadTransferReceivedbRecords.Item2, jSONsettings);
                                                        progressBarActive = false;
                                                        complete = true;
                                                        swlstInternalTransferReceiveDB.Stop();
                                                        WriteLog.Write(this, "TransferReceiveDB created - " + lstInternalTransferReceiveDB.Count().ToString() + " recods. Time elapsed : " + swlstInternalTransferReceiveDB.Elapsed.Milliseconds.ToString() + " milliseconds");
                                                    }
                                                }
                                                var resultReaddbRecords = await ReaddbRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                                if (resultReaddbRecords.Item1)
                                                {
                                                    //Debug.WriteLine(resultReaddbRecords.Item2);
                                                    if (!string.IsNullOrEmpty(resultReaddbRecords.Item2))
                                                    {
                                                        Stopwatch swlstInternalRecordDB = Stopwatch.StartNew();
                                                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                                        lstInternalRecordDB = JsonConvert.DeserializeObject<List<ListOfdbRecords>>(resultReaddbRecords.Item2, jSONsettings);
                                                        progressBarActive = false;
                                                        complete = true;
                                                        swlstInternalRecordDB.Stop();
                                                        WriteLog.Write(this, "InternalRecordDB created - " + lstInternalRecordDB.Count().ToString() + " recods. Time elapsed : " + swlstInternalRecordDB.Elapsed.Milliseconds.ToString() + " milliseconds");
                                                    }
                                                }

                                                Stopwatch swlstBarcodes = Stopwatch.StartNew();
                                                foreach (var n in lstInternalRecordDB)
                                                {
                                                    if (n.barCode.Contains(";"))
                                                    {
                                                        var barCodes = n.barCode.Split(new[] { ";" }, StringSplitOptions.None);
                                                        if (barCodes.Any())
                                                        {
                                                            foreach (var p in barCodes)
                                                            {
                                                                var row = new ListOfBarcodes { barCode = p, itemCode = n.itemCode };
                                                                lstBarcodes.Add(row);
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        var row = new ListOfBarcodes { barCode = n.barCode, itemCode = n.itemCode };
                                                        lstBarcodes.Add(row);
                                                    }

                                                }
                                                swlstBarcodes.Stop();

                                                WriteLog.Write(this, "lstBarcodes created - " + lstBarcodes.Count().ToString() + " recods. Time elapsed : " + swlstBarcodes.Elapsed.Milliseconds.ToString() + " milliseconds");

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

                            var resultReadInvRecords = await ReadInvRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                            if (resultReadInvRecords.Item1)
                            {
                                if (!string.IsNullOrEmpty(resultReadInvRecords.Item2))
                                {
                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    lstInternalInvDB = JsonConvert.DeserializeObject<List<ListOfInvRecords>>(resultReadInvRecords.Item2, jSONsettings);
                                    progressBarActive = false;
                                }
                            }

                            var resultReadMovementRecords = await ReadMovementRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                            if (resultReadMovementRecords.Item1)
                            {
                                if (!string.IsNullOrEmpty(resultReadMovementRecords.Item2))
                                {
                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    lstInternalMovementDB = JsonConvert.DeserializeObject<List<ListOfMovementRecords>>(resultReadMovementRecords.Item2, jSONsettings);
                                    progressBarActive = false;
                                }
                            }

                            var resultPurchaseOrderPickedQuantities = await ReadPurchaseOrderPickedQuantitiesRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                            if (resultPurchaseOrderPickedQuantities.Item1)
                            {
                                if (!string.IsNullOrEmpty(resultPurchaseOrderPickedQuantities.Item2))
                                {
                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    lstPurchaseOrderPickedQuantities = JsonConvert.DeserializeObject<List<ListOfSHRCVToExport>>(resultPurchaseOrderPickedQuantities.Item2, jSONsettings);
                                    progressBarActive = false;
                                }
                            }

                            var resultTransferPickedQuantities = await ReadTransferOrderPickedQuantitiesRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                            if (resultTransferPickedQuantities.Item1)
                            {
                                if (!string.IsNullOrEmpty(resultTransferPickedQuantities.Item2))
                                {
                                    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                    lstTransferOrderPickedQuantities = JsonConvert.DeserializeObject<List<ListOfTRFRCVToExport>>(resultTransferPickedQuantities.Item2, jSONsettings);
                                    progressBarActive = false;
                                }
                            }

                            #region ostuasjad?
                            Stopwatch swPurchaseOrdersGroup = Stopwatch.StartNew();
                            lstPurchaseOrders = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID).ToList().GroupBy(x => x.docNo).Select(s => new ListOfPurchaseReceive
                            {
                                docNo = s.First().docNo,
                                vendorCode = s.First().vendorCode,
                                vendorName = s.First().vendorName,
                                vendorReference = s.First().vendorReference,
                                shop = s.First().shop,
                                shipmentDate = s.First().shipmentDate,
                                department = s.First().department
                            }).ToList().OrderBy(x => x.shipmentDate).ToList();
                            WriteLog.Write(this, "lstPurchaseOrders grouped - " + lstPurchaseOrders.Count().ToString() + " recods. Time elapsed : " + swPurchaseOrdersGroup.Elapsed.Milliseconds.ToString() + " milliseconds");
                            Stopwatch swPurchaseOrdersCalc = Stopwatch.StartNew();

                            var allPicked = false;
                            var purchaseRowCount = new List<ListOfPurchaseReceive>();
                            foreach (var r in lstPurchaseOrders)
                            {
                                purchaseRowCount = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo).ToList();
                                if (purchaseRowCount.Any())
                                {
                                    r.purchaseRowCount = purchaseRowCount.Count();
                                }

                            }
                            foreach (var s in lstPurchaseOrderPickedQuantities)
                            {

                                currentPurchaseOrder = s.docNo;
                                Debug.WriteLine(currentPurchaseOrder);
                                if (lstPurchaseOrders.Any())
                                {
                                    if (!string.IsNullOrEmpty(currentPurchaseOrder))
                                    {
                                        var currentRows = lstPurchaseOrders.Where(x => x.docNo == currentPurchaseOrder);
                                        if (currentRows.Any())
                                        {
                                            currentRows.First().purchasePickedRowCount = 0;
                                            var purchaseRowCountA = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == currentPurchaseOrder);
                                            if (purchaseRowCountA.Any())
                                            {
                                                currentRows.First().purchaseRowCount = purchaseRowCountA.Count();
                                            }
                                            Debug.WriteLine("alustame");
                                            if (purchaseRowCountA.Any())
                                            {
                                                Debug.WriteLine("alustame 1");
                                                var allPickedA = true;
                                                foreach (var p in purchaseRowCountA)
                                                {
                                                    Debug.WriteLine("alustame 2");
                                                    var purchasePickedCount = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == currentPurchaseOrder && x.docLineNo == p.docLineNo);
                                                    if (purchasePickedCount.Any())
                                                    {
                                                        Debug.WriteLine("alustame 3");
                                                        currentRows.First().purchasePickedRowCount = currentRows.First().purchasePickedRowCount + 1;
                                                        if (!((p.initialQty - purchasePickedCount.First().pickedQty) == 0))
                                                        {
                                                            allPickedA = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Debug.WriteLine("alustame 4");
                                                        allPickedA = false;
                                                    }
                                                }
                                                currentRows.First().purchaseOrderPicked = allPickedA;
                                                currentPurchaseOrder = "";
                                            }
                                        }
                                    }
                                }
                                currentPurchaseOrder = "";
                            }


                            //foreach (var r in lstPurchaseOrders)
                            //{
                            //	var purchaseRowCount = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo);
                            //	if (purchaseRowCount.Any())
                            //	{
                            //		r.purchaseRowCount = purchaseRowCount.Count();
                            //	}

                            //	var allPicked = true;

                            //	foreach (var p in purchaseRowCount)
                            //	{
                            //		var purchasePickedCount = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == p.docNo && x.docLineNo == p.docLineNo);
                            //		if (purchasePickedCount.Any())
                            //		{
                            //			r.purchasePickedRowCount = r.purchasePickedRowCount + 1;
                            //			if (!((p.initialQty - purchasePickedCount.First().pickedQty) == 0))
                            //			{
                            //				allPicked = false;
                            //			}
                            //		}
                            //		else
                            //		{
                            //			allPicked = false;
                            //		}
                            //		r.purchaseOrderPicked = allPicked;
                            //		currentPurchaseOrder = "";
                            //	}
                            //}
                            stkOperations.IsEnabled = true;
                            WriteLog.Write(this, "lstPurchaseOrders read records calculation - " + lstPurchaseOrders.Count().ToString() + " recods. Time elapsed : " + swPurchaseOrdersCalc.Elapsed.Milliseconds.ToString() + " milliseconds");
                            #endregion


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

                                Debug.WriteLine("lstSettings.First().showPurchaseReceiveQty " + lstSettings.First().showPurchaseReceiveQty);
                                Debug.WriteLine("obj.showPurchaseReceiveQty " + obj.showPurchaseReceiveQty);
                                PrepareOperations();

                            }
                        }
                        LoadTemplates();
                    }
                    else
                    {
                        LoadTemplates();
                        string prefix = "";
                        PrepareSettings();
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("VIIGA " + ex.Message);
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
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
			MessagingCenter.Subscribe<App, string>((App)Application.Current, "DeviceVendor", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { SetDeviceVendor(arg); }); });

		}

		public void SetDeviceVendor(string vendor)
		{
			if (vendor == "Zebra")
			{
				obj.isZebra = true;
				obj.isHoneywell = false;
			}
			if (vendor == "Honeywell")
			{
				obj.isHoneywell = true;
				obj.isZebra = false;
			}
		}

		public async Task GetShopRelations()
        {
            try
            {
                Debug.WriteLine("Import alusta1");
                var resultShoprelations = await ReaddbShopRelationRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
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
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
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
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
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
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
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

		public void LoadTemplates()
		{
			LstvStockTakeInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfoStockTake)) : new DataTemplate(typeof(vcItemInfoStockTake));
			LstvTransferInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfo)) : new DataTemplate(typeof(vcItemInfo));
			LstvSelectItem.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemSelect)) : new DataTemplate(typeof(vcItemSelect));
			LstvItemInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfo)) : new DataTemplate(typeof(vcItemInfo));
			LstvItemInfoItems.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfoItems)) : new DataTemplate(typeof(vcItemInfoItems));
			LstvStockTakeAddedRowsView.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcInsertedRecords)) : new DataTemplate(typeof(vcInsertedRecords));
			LstvTransferAddedRowsView.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcInsertedRecords)) : new DataTemplate(typeof(vcInsertedRecords));
			LstvItemInfoBinsView.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcItemInfoBinsView)) : new DataTemplate(typeof(vcItemInfoBinsView));
			LstvPurchaseReceiveOrders.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrders)) : new DataTemplate(typeof(vcPurchaseOrders));
			LstvPurchaseReceiveOrderLines.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrderLines)) : new DataTemplate(typeof(vcPurchaseOrderLines));
			LstvPurchaseReceiveOrderLinesInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrderLinesInfo)) : new DataTemplate(typeof(vcPurchaseOrderLinesInfo));
			LstvPurchaseOrderQuantityInsertInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrderQuantityInsertInfo)) : new DataTemplate(typeof(vcPurchaseOrderQuantityInsertInfo));
			LstvTransferReceiveOrders.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrders)) : new DataTemplate(typeof(vcTransferOrders));
			LstvTransferReceiveOrderLines.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrderLines)) : new DataTemplate(typeof(vcTransferOrderLines));
			LstvTransferReceiveOrderLinesInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrderLinesInfo)) : new DataTemplate(typeof(vcTransferOrderLinesInfo));
			LstvTransferOrderQuantityInsertInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrderQuantityInsertInfo)) : new DataTemplate(typeof(vcTransferOrderQuantityInsertInfo));
			LstvSelectMagnitude.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcSelectMagnitude)) : new DataTemplate(typeof(vcSelectMagnitude));
			LstvOperationsRecordInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcOperationsRecords)) : new DataTemplate(typeof(vcOperationsRecords));

			if (obj.showPurchaseReceiveQty)
			{
				LstvPurchaseOrderQuantityInsertQuantityInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrderQuantityInsertQuantityInfoFull)) : new DataTemplate(typeof(vcPurchaseOrderQuantityInsertQuantityInfoFull));
			}
			else
			{
				LstvPurchaseOrderQuantityInsertQuantityInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcPurchaseOrderQuantityInsertQuantityInfo)) : new DataTemplate(typeof(vcPurchaseOrderQuantityInsertQuantityInfo));
			}

			if (obj.showTransferReceiveQty)
			{
				LstvTransferOrderQuantityInsertQuantityInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrderQuantityInsertQuantityInfoFull)) : new DataTemplate(typeof(vcTransferOrderQuantityInsertQuantityInfoFull));
			}
			else
			{
				LstvTransferOrderQuantityInsertQuantityInfo.ItemTemplate = obj.operatingSystem == "UWP" ? new DataTemplate(typeof(vcTransferOrderQuantityInsertQuantityInfo)) : new DataTemplate(typeof(vcTransferOrderQuantityInsertQuantityInfo));
			}
		}
		#endregion

		#region ScannedValue operations
		public void ScannedValueReceive()
        {
			MessagingCenter.Subscribe<App, string>((App)Application.Current, "scannedValue", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { ProcessScannedValue(arg); }); });
			MessagingCenter.Subscribe<App, string>((App)Application.Current, "ScanBarcode", (sender, arg) => { Device.BeginInvokeOnMainThread(() => { ProcessScannedValue(arg); }); });
		}

        public async void ProcessScannedValue(string scannedValue)
        {
            string scannedData = "";
            string scannedSymbology = "";
			Debug.WriteLine("'" + scannedValue + "'");
            if (scannedValue.Contains("###"))
            {
                var split = scannedValue.Split(new[] { "###" }, StringSplitOptions.None);
                scannedData = split[0].Replace("\r\n","").TrimStart().TrimEnd();
                scannedSymbology = split[1].Replace("\r\n", "").TrimStart().TrimEnd();
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


                var result = await WriteSettings.Write(pEnv, ediAddress.Text, obj.shopLocationCode, obj.showInvQty, obj.showPurchaseReceiveQty, obj.showPurchaseReceiveQtySum, obj.showTransferReceiveQty, obj.showTransferReceiveQtySum, obj.shopLocationID ?? "SHOPID-PUUDUB", obj.deviceSerial ?? "DEVICEID-PUUDUB", this);
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

       
        void LstvSettings_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ListOfSettings;
            lstSet.ForEach(x => x.isSelected = false);
            item.isSelected = true;
        }

        private void btnSettingsOthers_Clicked(object sender, EventArgs e)
        {
            PrepareSettingsOthers();
        }


        #endregion

        #region stkSettingsOthers
        public void PrepareSettingsOthers()
        {
            CollapseAllStackPanels.Collapse(this);
            stkSettingsOthers.IsVisible = true;
            obj.mainOperation = "";
            obj.currentLayoutName = "SettingsOthers";
        }

        private void rbtnPurchaseReceiveQtySumYes_CheckChanged(object sender, EventArgs e)
        {
            rbtnPurchaseReceiveQtySumNo.IsChecked = false;
            obj.showPurchaseReceiveQtySum = true;
        }

        private void rbtnPurchaseReceiveQtySumNo_CheckChanged(object sender, EventArgs e)
        {
            rbtnPurchaseReceiveQtySumYes.IsChecked = false;
            obj.showPurchaseReceiveQtySum = false;
        }

        private void rbtnTransferReceiveQtySumYes_CheckChanged(object sender, EventArgs e)
        {
            rbtnTransferReceiveQtySumNo.IsChecked = false;
            obj.showTransferReceiveQtySum = true;
        }

        private void rbtnTransferReceiveQtySumNo_CheckChanged(object sender, EventArgs e)
        {
            rbtnTransferReceiveQtySumYes.IsChecked = false;
            obj.showTransferReceiveQtySum = false;
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

        private async void btnSettingsOthersOK_Clicked(object sender, EventArgs e)
        {
            var result = await WriteSettings.Write(obj.pEnv, ediAddress.Text, obj.shopLocationCode, obj.showInvQty, obj.showPurchaseReceiveQty, obj.showPurchaseReceiveQtySum, obj.showTransferReceiveQty, obj.showTransferReceiveQtySum, obj.shopLocationID ?? "SHOPID-PUUDUB", obj.deviceSerial ?? "DEVICEID-PUUDUB", this);
            if (result.Item1)
            {
                DisplaySuccessMessage("SALVESTATUD!");
                obj.deviceSerial = null;
                ShowKeyBoard.Hide(this);
                PrepareSettings();
                //BackKeyPress.Press(this);
            }
            else
            {
                DisplayFailMessage(result.Item2);
            }
            
        }
        #endregion

        #region stkOperations

        public async void PrepareOperations()
        {
			RotationChanges();
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
           
            LstvOperationsRecordInfo.ItemsSource = lstOperationsRecords;
            if (lstInternalInvDB.Any() || lstInternalMovementDB.Any() || lstPurchaseOrderPickedQuantities.Any() || lstTransferOrderPickedQuantities.Any())
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

                if (lstPurchaseOrderPickedQuantities.Count > 0)
                {
                    frmOperationsExport.IsVisible = true;
                    lblOperationsExport.IsVisible = true;
                }

                if (lstTransferOrderPickedQuantities.Count > 0)
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

                    
                        var write = await WriteInvRecordsToExportFile.Write(this, dataRowInv, exportFileNameStamp, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
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
                    var write = await WriteMovementRecordsToExportFile.Write(this, dataRowMovement, exportFileNameStamp, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
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
                if (lstPurchaseOrderPickedQuantities.Any())
                {
                    string dataRowPurchaseReceive = "";
                    foreach (var p in lstPurchaseOrderPickedQuantities)
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

                        dataRowPurchaseReceive = dataRowPurchaseReceive + p.docNo + ";" + p.docLineNo + ";" + p.itemCode + ";" + p.pickedQty + ";" + p.magnitude + "\r\n";
                    }
                    var write = await WritePurchaseReceiveRecordsToExportFile.Write(this, dataRowPurchaseReceive, exportFileNameStamp, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                    if (write.Item1)
                    {
                        lstPurchaseOrderPickedQuantities = new List<ListOfSHRCVToExport>();

                        PrepareOperations();
                    }
                    else
                    {
                        proceed = false;
                        DisplayFailMessage(write.Item2);
                    }
                }
                if (lstTransferOrderPickedQuantities.Any())
                {
                    string dataRowTransferReceive = "";
                    foreach (var p in lstTransferOrderPickedQuantities)
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

                        dataRowTransferReceive = dataRowTransferReceive + p.docNo + ";" + p.docLineNo + ";" + p.itemCode + ";" + p.pickedQty + ";" + p.magnitude + "\r\n";
                    }
                    var write = await WriteTransferReceiveRecordsToExportFile.Write(this, dataRowTransferReceive, exportFileNameStamp, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                    if (write.Item1)
                    {
                        lstTransferOrderPickedQuantities = new List<ListOfTRFRCVToExport>();

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
            btnOperationsTransferReceive.IsEnabled = false;
            lstTransferOrders = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID).ToList().GroupBy(x => x.docNo).Select(s => new ListOfTransferReceive
            {
                docNo = s.First().docNo,
                receivedFromShop = s.First().receivedFromShop,
                receivedFromName = s.First().receivedFromName,
                shop = s.First().shop,
                shipmentDate = s.First().shipmentDate,
            }).ToList().OrderBy(x => x.shipmentDate).ToList();
            //foreach (var r in lstTransferOrders)
            //{
            //    var transferRowCount = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo);
            //    if (transferRowCount.Any())
            //    {
            //        r.transferRowCount = transferRowCount.Count();
            //    }

            //    var transferPickedCount = lstTransferOrderPickedQuantities.Where(x => x.docNo == r.docNo);
            //    if (transferPickedCount.Any())
            //    {
            //        r.transferPickedRowCount = transferPickedCount.Count();
            //    }

            //    if (r.transferRowCount == r.transferPickedRowCount)
            //    {
            //        r.transferOrderPicked = true;
            //    }
            //    else
            //    {
            //        r.transferOrderPicked = false;
            //    }
            //    string shopName = lstShopRelations.Where(x => x.shopID == r.receivedFromShop).ToList().First().shopName;
            //    r.receivedFromName = shopName;
            //}
            PrepareTransferReceiveOrders();
        }

       
        private void btnOperationsPurchaseReceive_Clicked(object sender, EventArgs e)
        {
			//stkOperations.IsEnabled = false;
			//btnOperationsPurchaseReceive.IsEnabled = false;
   //         lstPurchaseOrders = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID).ToList().GroupBy(x => x.docNo).Select(s => new ListOfPurchaseReceive
   //         {
   //             docNo = s.First().docNo,
   //             vendorCode = s.First().vendorCode,
   //             vendorName = s.First().vendorName,
   //             vendorReference = s.First().vendorReference,
   //             shop = s.First().shop,
   //             shipmentDate = s.First().shipmentDate,
			//	department = s.First().department
   //         }).ToList().OrderBy(x => x.shipmentDate).ToList();
   //         foreach (var r in lstPurchaseOrders)
   //         {
   //             var purchaseRowCount = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo);
   //             if (purchaseRowCount.Any())
   //             {
   //                 r.purchaseRowCount = purchaseRowCount.Count();
   //             }

			//	var allPicked = true;

			//	foreach (var p in purchaseRowCount)
			//	{

			//		var purchasePickedCount = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == p.docNo && x.docLineNo == p.docLineNo);
			//		if (purchasePickedCount.Any())
			//		{
			//			r.purchasePickedRowCount = r.purchasePickedRowCount + 1;
			//			if (!((p.initialQty - purchasePickedCount.First().pickedQty) == 0))
			//			{
			//				allPicked = false;
			//			}
			//		}
			//		else
			//		{
			//			allPicked = false;
			//		}
			//		r.purchaseOrderPicked = allPicked;
			//		currentPurchaseOrder = "";
			//	}
   //         }
			//stkOperations.IsEnabled = true;
			PreparePurchaseReceiveOrders();
		}
        

        #endregion

        #region stkStockTake
        public async void PrepareStockTake()
        {
            try
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



                var resultReadInvRecords = await ReadInvRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
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
            catch(Exception ex)
            {
                DisplayAlert("PrepareStockTake", ex.Message, "OK");
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private async void btnStockTakeAddedRowsDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (await YesNoDialog("INVENTUUR", "JÄTKAMISEL KUSTUTATAKSE KIRJE TÄIELIKULT!", false))
                {
                    var record = lstInternalInvDB.Where(x => x.recordID == invRecordID);
                    if (record.Any())
                    {
                        lstInternalInvDB.Remove(record.Take(1).First());
                        JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                        string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);

                        var writeInvDbToFile = await WriteInvRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
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
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private async void btnStockTakeQuantityOK_Clicked(object sender, EventArgs e)
        {
            int step = 0;
            try
            {
                bool proceed = true;
                decimal quantity = 0;
                if (!string.IsNullOrEmpty(entStockTakeQuantity.Text))
                {
                    step = 1;
                    quantity = TryParseDecimal.Parse(entStockTakeQuantity.Text);
                    step = 2;
                }
                if (string.IsNullOrEmpty(lstStockTakeInfo.First().itemCode))
                {
                    step = 3;
                    proceed = false;
                    DisplayFailMessage("KAUPA POLE VALITUD!");
                }
                step = 4;
                if (proceed)
                {
                    step = 5;
                    if (quantity > -1)
                    {
                        step = 6;
                        if (invRecordID == 0)
                        {
                            step = 7;
                            int lastRecordID = 0;
                            if (lstInternalInvDB.Any())
                            {
                                step = 9;
                                lastRecordID = lstInternalInvDB.OrderByDescending(x => x.recordID).Take(1).First().recordID;
                            }
                            step = 10;
                            lstInternalInvDB.Add(new ListOfInvRecords
                            {   
                                barCode = lstStockTakeInfo.First().barCode,
                                itemCode = lstStockTakeInfo.First().itemCode,
                                itemDesc = lstStockTakeInfo.First().itemDesc,
                                quantity = quantity,
                                recordDate = DateTime.Now,
                                uom = lblStockTakeQuantityUOM.Text,
                                config = lstStockTakeInfo.First().config,
                                recordID = lastRecordID + 1
                            });
                            step = 11;
                            JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                            string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);
                            step = 12;
                            var writeInvDbToFile = await WriteInvRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                            if (writeInvDbToFile.Item1)
                            {
                                step = 13;
                                DisplaySuccessMessage("SALVESTATUD!");
                                PrepareStockTake();
                            }
                            else
                            {
                                step = 14;
                                DisplayFailMessage(writeInvDbToFile.Item2);
                            }
                            step = 15;
                        }
                        else
                        {
                            step = 16;
                            var record = lstInternalInvDB.Where(x => x.recordID == invRecordID);
                            if (record.Any())
                            {
                                step = 17;
                                record.First().quantity = quantity;
                                record.First().recordDate = DateTime.Now;
                                
                                JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                string data = JsonConvert.SerializeObject(lstInternalInvDB, jSONsettings);
                                step = 18;
                                var writeInvDbToFile = await WriteInvRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                if (writeInvDbToFile.Item1)
                                {
                                    step = 19;
                                    DisplaySuccessMessage("SALVESTATUD!");
                                    PrepareStockTake();
                                }
                                else
                                {
                                    step = 20;
                                    DisplayFailMessage(writeInvDbToFile.Item2);
                                }
                                step = 21;
                            }
                            step = 22;
                        }
                        step = 23;
                    }
                    else
                    {
                        step = 24;
                        DisplayFailMessage("KOGUS PEAB OLEMA NUMBER!");
                    }
                    step = 25;
                }
                step = 26;
            }
            catch(Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + " step: " + step + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private  void btnStockTakeReadCode_Clicked(object sender, EventArgs e)
        {
            SearchEntStockTakeReadCode();
        }

        public async void SearchEntStockTakeReadCode()
        {
            int step = 0;
            try
            {
                if (!string.IsNullOrEmpty(entStockTakeReadCode.Text))
                {
                    LstvStockTakeInfo.ItemsSource = null;
                    lstStockTakeInfo = new List<ListOfdbRecords>();
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

                        step = 1;

						var result = new List<ListOfdbRecords>();
						if (entStockTakeReadCode.Text.Length == 13 && IsDigitsOnly(entStockTakeReadCode.Text))
						{
							Stopwatch swlstInternalRecordDBSearch = Stopwatch.StartNew();
							Debug.WriteLine("lstBarcodes " + lstBarcodes.Count());
							if (lstBarcodes.Any())
							{
								var n = lstBarcodes.Where(x => x.barCode == entStockTakeReadCode.Text);
								if (n.Any())
								{
									Debug.WriteLine("n leitud " + n.Count());
									string itemCode = n.First().itemCode;
									var preResult = lstInternalRecordDB.Where(x => x.itemCode == itemCode).ToList();
									result = preResult.Where(x => x.barCode.Contains(entStockTakeReadCode.Text)).ToList();
								}
							}
							WriteLog.Write(this, "SearchEntStockTakeReadCodee searched value " + entStockTakeReadCode.Text + ". Found - " + (result.Any() ? result.Count().ToString() : "0") + " recods." + "\r\n" +
								"Time elapsed : " + swlstInternalRecordDBSearch.Elapsed.Milliseconds.ToString() + " milliseconds");
						}
						else
						{
							Stopwatch swlstInternalRecordDBSearch = Stopwatch.StartNew();
							result = lstInternalRecordDB.Where(x =>
							   x.itemCode.Contains(entStockTakeReadCode.Text)
							|| x.itemDesc.ToUpper().Contains(entStockTakeReadCode.Text.ToUpper())
							||
							x.barCode.Contains(entStockTakeReadCode.Text)).ToList();
							swlstInternalRecordDBSearch.Stop();
							WriteLog.Write(this, "SearchEntStockTakeReadCodee searched value " + entStockTakeReadCode.Text + ". Found - " + (result.Any() ? result.Count().ToString() : "0") + " recods." + "\r\n" +
								"Time elapsed : " + swlstInternalRecordDBSearch.Elapsed.Milliseconds.ToString() + " milliseconds");
						}

						//Stopwatch swlstInternalRecordDBSearch = Stopwatch.StartNew();

      //                  var result = lstInternalRecordDB.Where(x =>
      //                     x.itemCode.Contains(entStockTakeReadCode.Text)
      //                  || x.itemDesc.ToUpper().Contains(entStockTakeReadCode.Text.ToUpper())
      //                  || x.barCode.Contains(entStockTakeReadCode.Text)).ToList();

      //                  swlstInternalRecordDBSearch.Stop();
                        //WriteLog.Write(this, "SearchEntStockTakeReadCode searched value " + entStockTakeReadCode.Text + ". Found - " + (result.Any() ? result.Count().ToString() : "0") + " recods. Time elapsed : " + swlstInternalRecordDBSearch.Elapsed.Milliseconds.ToString() + " milliseconds");
                        step = 2;
                        if (result.Any())
                        {
                            step = 3;
                            if (result.Count() == 1)
                            {
                                step = 4;
                                if (lstInternalInvDB.Any())
                                {
                                    step = 5;
                                    var s = lstInternalInvDB.Where(x => x.itemCode == result.First().itemCode && x.barCode == result.First().barCode).ToList();
                                    if (s.Any())
                                    {
                                        step = 6;
                                        obj.isScanAllowed = false;
                                        if (await YesNoDialog("INVENTUUR", "KAUP ON JUBA INVENTEERTUD. KAS SOOVID PARANDADA?", false))
                                        {
                                            step = 7;
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

                                            step = 8;
                                            lstStockTakeInfo = result;
                                            if (lstStockTakeInfo.Any())
                                            {
                                                step = 9;
                                                lstStockTakeInfo.First().showInvQty = obj.showInvQty;
                                                step = 10;
                                            }
                                            step = 11;
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
                                            step = 12;
                                            lstStockTakeInfo.First().SKUBin = sKUs.TrimStart().TrimEnd();
                                            lstStockTakeInfo.First().SKUBin2 = sKUs2.TrimStart().TrimEnd();
                                            LstvStockTakeInfo.ItemsSource = null;
                                            LstvStockTakeInfo.ItemsSource = lstStockTakeInfo;
                                            step = 13;
                                        }
                                        else
                                        {
                                            step = 14;
                                            invRecordID = 0;
                                            entStockTakeReadCode.Text = "";
                                            entStockTakeQuantity.Text = "";
                                            lblStockTakeQuantityUOM.Text = "";
                                            focusedEditor = "entStockTakeReadCode";
                                            entStockTakeReadCode.BackgroundColor = Color.Yellow;
                                            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
                                            entStockTakeQuantity.BackgroundColor = Color.White;
                                            frmbtnStockTakeAddedRowsDelete.IsVisible = false;
                                            step = 15;
                                        }
                                    }
                                    else
                                    {
                                        step = 16;
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
                                        step = 17;
                                        lstStockTakeInfo = result;
                                        if (lstStockTakeInfo.Any())
                                        {
                                            lstStockTakeInfo.First().showInvQty = obj.showInvQty;
                                        }
                                        step = 18;
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
                                        step = 19;
                                        lstStockTakeInfo.First().SKUBin = sKUs.TrimStart().TrimEnd();
                                        lstStockTakeInfo.First().SKUBin2 = sKUs2.TrimStart().TrimEnd();
                                        LstvStockTakeInfo.ItemsSource = null;
                                        LstvStockTakeInfo.ItemsSource = lstStockTakeInfo;
                                        step = 20;
                                    }
                                }
                                else
                                {
                                    step = 21;
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
                                    step = 22;
                                    frmbtnStockTakeAddedRowsDelete.IsVisible = false;

                                    lstStockTakeInfo = new List<ListOfdbRecords>();
                                    lstStockTakeInfo = result;
                                    if (lstStockTakeInfo.Any())
                                    {
                                        step = 23;
                                        lstStockTakeInfo.First().showInvQty = obj.showInvQty;
                                        step = 24;
                                    }
                                    step = 25;
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
                                    step = 26;
                                    lstStockTakeInfo.First().SKUBin = sKUs.TrimStart().TrimEnd();
                                    lstStockTakeInfo.First().SKUBin2 = sKUs2.TrimStart().TrimEnd();
                                    LstvStockTakeInfo.ItemsSource = null;
                                    LstvStockTakeInfo.ItemsSource = lstStockTakeInfo;
                                    step = 27;
                                }
                                obj.isScanAllowed = true;
                            }
                            else
                            {
                                step = 28;
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
                WriteLog.Write(this, this.GetType().Name + " step: " + step + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
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
            try
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
				lstInternalMovementDB = new List<ListOfMovementRecords>();
				Debug.WriteLine("LOEN SISESTATUT0: ALUSTAME");
				var resultReadMovementRecords = await ReadMovementRecords.Read(this, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
				if (resultReadMovementRecords.Item1)
				{
					Debug.WriteLine("LOEN SISESTATUT1: " + resultReadMovementRecords.Item2);
					if (!string.IsNullOrEmpty(resultReadMovementRecords.Item2))
					{
						Debug.WriteLine(resultReadMovementRecords.Item2);
						JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
						lstInternalMovementDB = JsonConvert.DeserializeObject<List<ListOfMovementRecords>>(resultReadMovementRecords.Item2, jSONsettings);
						progressBarActive = false;
						Debug.WriteLine("LOEN SISESTATUT2: " + resultReadMovementRecords.Item2);
					}
				}
				lblTransferAddedRowsValue.Text = "0";
				if (lstInternalMovementDB.Any())
				{
					lblTransferAddedRowsValue.Text = lstInternalMovementDB.Count.ToString();
				}

				Debug.WriteLine("lblTransferAddedRowsValue.Text: " + lblTransferAddedRowsValue.Text);
				lstTransferInfo = new List<ListOfdbRecords>();
                
                LstvTransferInfo.ItemsSource = null;
                LstvTransferInfo.ItemsSource = lstTransferInfo;


                focusedEditor = "entTransferReadCode";
                entTransferReadCode.BackgroundColor = Color.Yellow;
                ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
            }
            catch(Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private async void btnTransferAddedRowsDelete_Clicked(object sender, EventArgs e)
        {
            try
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
                        var writeMovementDbToFile = await WriteMovementRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
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
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private async void btnTransferQuantityOK_Clicked(object sender, EventArgs e)
        {
            int step = 0;
            try
            {
                bool proceed = true;
                decimal quantity = 0;
                step = 1;
                if (!string.IsNullOrEmpty(entTransferQuantity.Text))
                {
                    step = 2;
                    quantity = TryParseDecimal.Parse(entTransferQuantity.Text);
                    step = 3;
                }
                step = 4;
                if (quantity == -1)
                {
                    step = 5;
                    proceed = false;
                    DisplayFailMessage("SISESTATUD KOGUS SISALDAB TÄHTI!");

                }
                step = 6;
                if (proceed)
                {
                    step = 7;
                    if (string.IsNullOrEmpty(lstTransferInfo.First().itemCode))
                    {
                        
                        proceed = false;
                        DisplayFailMessage("KAUPA POLE VALITUD!");
                    }
                    step = 8;
                }
                step = 9;
                if (proceed)
                {
                    step = 10;
                    if (quantity > -1)
                    {
                        step = 11;
                        Debug.WriteLine("X2");
                        if (transferRecordID == 0)
                        {
                            step = 12;
                            int lastRecordID = 0;
							if (lstInternalMovementDB.Any())
							{
								step = 13;
								lastRecordID = lstInternalMovementDB.OrderByDescending(x => x.recordID).Take(1).First().recordID;
								step = 14;
							}
							step = 15;
							lstInternalMovementDB.Add(new ListOfMovementRecords
							{
								barCode = lstTransferInfo.First().barCode,
								itemCode = lstTransferInfo.First().itemCode,
								itemDesc = lstTransferInfo.First().itemDesc,
								quantity = quantity,
								recordDate = DateTime.Now,
								uom = lstTransferInfo.First().itemMagnitude,
								config = lstTransferInfo.First().config,
								recordID = lastRecordID + 1
							});

							JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                            string data = JsonConvert.SerializeObject(lstInternalMovementDB, jSONsettings);
                            step = 16;
							var writeMovementDbToFile = await WriteMovementRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
							if (writeMovementDbToFile.Item1)
							{
								step = 17;
								DisplaySuccessMessage("SALVESTATUD!");
								PrepareTransfer();
							}
							else
							{
								step = 18;
								DisplayFailMessage(writeMovementDbToFile.Item2);
							}
							step = 19;
						}
                        else
                        {
                            step = 20;
                            var record = lstInternalMovementDB.Where(x => x.recordID == transferRecordID);
                            if (record.Any())
                            {
                                step = 21;
                                record.First().quantity = quantity;
                                record.First().recordDate = DateTime.Now;

                                JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                string data = JsonConvert.SerializeObject(lstInternalMovementDB, jSONsettings);
								step = 22;
								var writeMovementDbToFile = await WriteMovementRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
								if (writeMovementDbToFile.Item1)
								{
									step = 23;
									DisplaySuccessMessage("SALVESTATUD!");
									PrepareTransfer();
								}
								else
								{
									step = 24;
									DisplayFailMessage(writeMovementDbToFile.Item2);
								}
								step = 25;
							}
							step = 26;
                        }
                        step = 27;
                    }
                    else
                    {
                        step = 28;
                        DisplayFailMessage("KOGUS PEAB OLEMA NUMBER!");
                    }
                    step = 29;
                }
                step = 30;
            }
            catch (Exception ex)
            {
				Debug.WriteLine("AA " +this.GetType().Name + " step: " + step + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
                WriteLog.Write(this, this.GetType().Name + " step: " + step + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
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

		public bool IsDigitsOnly(string str)
		{
			foreach (char c in str)
			{
				if (c < '0' || c > '9')
					return false;
			}

			return true;
		}

		public async void SearchEntTransferReadCode()
        {
            int step = 0;
            try
            {
                if (!string.IsNullOrEmpty(entTransferReadCode.Text))
                {
                    step = 1;
                    LstvTransferInfo.ItemsSource = null;
                    lstTransferInfo = new List<ListOfdbRecords>();
                    if (entTransferReadCode.Text.Length > 4)
                    {
                        step = 2;
						var result = new List<ListOfdbRecords>();
						Debug.WriteLine("entTransferReadCode.Text.Length " + entTransferReadCode.Text.Length);
						if (entTransferReadCode.Text.Length == 13 && IsDigitsOnly(entTransferReadCode.Text))
						{
							Stopwatch swlstInternalRecordDBSearch = Stopwatch.StartNew();
							Debug.WriteLine("lstBarcodes " + lstBarcodes.Count());
							if (lstBarcodes.Any())
							{
								var n = lstBarcodes.Where(x => x.barCode == entTransferReadCode.Text);
								if (n.Any())
								{
									Debug.WriteLine("n leitud " + n.Count());
									string itemCode = n.First().itemCode;
									var preResult = lstInternalRecordDB.Where(x => x.itemCode == itemCode).ToList();
									result = preResult.Where(x => x.barCode.Contains(entTransferReadCode.Text)).ToList();
								}
							}
							WriteLog.Write(this, "SearchEntTransferReadCode searched value " + entTransferReadCode.Text + ". Found - " + (result.Any() ? result.Count().ToString() : "0") + " recods." + "\r\n" +
								"Time elapsed : " + swlstInternalRecordDBSearch.Elapsed.Milliseconds.ToString() + " milliseconds");
						}
						else
						{
							Stopwatch swlstInternalRecordDBSearch = Stopwatch.StartNew();
							result = lstInternalRecordDB.Where(x =>
							   x.itemCode.Contains(entTransferReadCode.Text)
							|| x.itemDesc.ToUpper().Contains(entTransferReadCode.Text.ToUpper())
							||
							x.barCode.Contains(entTransferReadCode.Text)).ToList();
							swlstInternalRecordDBSearch.Stop();
							WriteLog.Write(this, "SearchEntTransferReadCode searched value " + entTransferReadCode.Text + ". Found - " + (result.Any() ? result.Count().ToString() : "0") + " recods." + "\r\n" +
								"Time elapsed : " + swlstInternalRecordDBSearch.Elapsed.Milliseconds.ToString() + " milliseconds");
						}
                        step = 3;
						
						step = 4;
						Debug.WriteLine("result " + result.Count());
						
						if (result.Any())
                        {
                            step = 5;
                            if (result.Count() == 1)
                            {
                                step = 6;
								if (lstInternalMovementDB.Any())
								{
									step = 7;
									frmentTransferQuantity.IsVisible = false;
									entTransferQuantity.IsVisible = false;
									lblTransferQuantityUOM.IsVisible = false;
									frmbtnTransferQuantityOK.IsVisible = false;
									btnTransferQuantityOK.IsVisible = false;
									Debug.WriteLine("step " + step);
									var s = lstInternalMovementDB.Where(x => x.itemCode == result.First().itemCode && x.barCode == result.First().barCode).ToList();
									if (s.Any())
									{
										step = 8;
										Debug.WriteLine("step " + step);
										obj.isScanAllowed = false;
										if (await YesNoDialog("LIIKUMINE", "KAUP ON JUBA LIIGUTATUD. KAS SOOVID PARANDADA?", false))
										{

											step = 9;
											Debug.WriteLine("step " + step);
											obj.isScanAllowed = true;
											frmbtnTransferAddedRowsDelete.IsVisible = true;
											transferRecordID = s.First().recordID;
											step = 10;
											Debug.WriteLine("step " + step);
											entTransferQuantity.Text = (s.First().quantity).ToString().Replace(".0", "");
											lstTransferInfo = new List<ListOfdbRecords>();
											step = 11;
											Debug.WriteLine("step " + step);
											int SKUBinCount = 0;
											string sKUs = "";
											string sKUs2 = "";
											decimal sKUqty = 0;
											var parseSKU = result.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
											if (parseSKU.Any())
											{
												step = 12;
												Debug.WriteLine("step " + step);
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
												step = 13;
												Debug.WriteLine("step " + step);
											}
											step = 14;
											Debug.WriteLine("step " + step);
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
												config = result.First().config

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
											step = 15;
										}
										else
										{
											step = 16;
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
											step = 17;
											Debug.WriteLine("step " + step);
										}
										step = 18;
										Debug.WriteLine("step " + step);
									}
									else
									{
										
										step = 19;
										Debug.WriteLine("step " + step);
										int SKUBinCount = 0;
										string sKUs = "";
										string sKUs2 = "";
										decimal sKUqty = 0;
										var parseSKU = result.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
										if (parseSKU.Any())
										{
											step = 20;
											Debug.WriteLine("step " + step);
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
															sKUqty = Convert.ToDecimal(uniqueSKU[2].StartsWith(",") ? "0" + uniqueSKU[2] : uniqueSKU[2]);
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
											step = 21;
											Debug.WriteLine("step " + step);
										}
										step = 22;
										Debug.WriteLine("step " + step);
										sKUs = sKUs.TrimStart().TrimEnd();
										sKUs2 = sKUs2.TrimStart().TrimEnd();
										transferRecordID = 0;
										lstTransferInfo = new List<ListOfdbRecords>();

										
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
											config = result.First().config

										});
										lblTransferQuantityUOM.Text = result.First().itemMagnitude;
										if (lstTransferInfo.Any())
										{
											LstvTransferInfo.ItemsSource = null;
											LstvTransferInfo.ItemsSource = lstTransferInfo;
										}
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
										step = 23;
										Debug.WriteLine("step " + step);
									}
								}
								else
								{
									step = 24;
									Debug.WriteLine("step " + step);
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
														sKUqty = Convert.ToDecimal(uniqueSKU[2].StartsWith(",") ? "0" + uniqueSKU[2] : uniqueSKU[2]);
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
										config = result.First().config

									});
									
									lblTransferQuantityUOM.Text = result.First().itemMagnitude ?? "";
									LstvTransferInfo.ItemsSource = null;
									LstvTransferInfo.ItemsSource = lstTransferInfo;
									Debug.WriteLine("a3");
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
									step = 25;
									Debug.WriteLine("step " + step);

								}
                                obj.isScanAllowed = true;
                            }
                            else
                            {
                                step = 26;
								Debug.WriteLine("step " + step);
								obj.previousLayoutName = "Transfer";
                                PrepareSelectItem();
                            }
                            step = 27;
							Debug.WriteLine("step " + step);
						}
                        else
                        {
                            step = 28;
							Debug.WriteLine("step " + step);
							DisplayFailMessage("EI LEITUD MIDAGI!");

                        }
                        step = 29;
						Debug.WriteLine("step " + step);
					}
                    else
                    {
                        step = 30;
						Debug.WriteLine("step " + step);
						DisplayFailMessage("SISESTA VÄHEMALT 5 TÄHEMÄRKI!");
                    }
                    step = 31;
					Debug.WriteLine("step " + step);
				}
                step = 32;
				Debug.WriteLine("step " + step);
			}
            catch (Exception ex)
            {
				step = 33;
				Debug.WriteLine("step " + step + "  " + ex.Message);
				WriteLog.Write(this, this.GetType().Name + " step: " + step + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
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
            try
            {
                CollapseAllStackPanels.Collapse(this);
                stkSelectItem.IsVisible = true;
                obj.mainOperation = "";
                obj.currentLayoutName = "SelectItem";
                lblSelectItemHeader.Text = "VALI KAUP";
               
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
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }
       

        private void LstvSelectItem_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnSelectItemReadCode_Clicked(object sender, EventArgs e)
        {
            SearchEntSelectItemReadCode();
        }

        public void SearchEntSelectItemReadCode()
        {
            try
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
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        #endregion

        #region stkItemInfo

        public void PrepareItemInfo(string scannedCode)
        {
            try
            {
                lstBins = new List<ListOfSKU>();
                frmbtnItemInfoBins.IsVisible = false;
                CollapseAllStackPanels.Collapse(this);
                stkItemInfo.IsVisible = true;
                obj.mainOperation = "";
                obj.currentLayoutName = "ItemInfo";
                lblItemInfoHeader.Text = "KAUBA INFO";
               
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
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnItemInfoReadCode_Clicked(object sender, EventArgs e)
        {
            entItemInfoReadCode.BackgroundColor = Color.White;
            SearchEntItemInfoReadCode();
        }

        public void DisplayItemInfoSKU(ListOfdbRecords row)
        {
            try
            {
                Debug.WriteLine("step 1");
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
                                    var cultureInfo = CultureInfo.InvariantCulture;
                                    row.SKUqty = decimal.Parse(uniqueSKU[2].Replace(",", "."), cultureInfo);
                                    row.price = decimal.Parse(uniqueSKU[3].Replace(",", "."), cultureInfo);
                                    row.meistriklubihind = decimal.Parse(uniqueSKU[4].Replace(",", "."), cultureInfo);
                                    row.profiklubihind = decimal.Parse(uniqueSKU[5].Replace(",", "."), cultureInfo);
                                    row.soodushind = decimal.Parse(uniqueSKU[6].Replace(",", "."), cultureInfo);
                                    lstBins.Add(new ListOfSKU
                                    {
                                        SKU = uniqueSKU[0],
                                        SKUBin = uniqueSKU[1],
                                        SKUqty = row.SKUqty,
                                        SKUShopName = GetShopName(uniqueSKU[0]),
                                        itemMagnitude = row.itemMagnitude
                                    });
                                }
                            }
                        }
                    }
                }
                Debug.WriteLine("step 2");
                var skuBin = "";
                var skuBin2 = "";
                int skuBinCount = 0;

                row.SKUqty = 0;
                foreach (var p in lstBins)
                {
                    if (p.SKU == obj.shopLocationID)
                    {
                        row.SKUqty = p.SKUqty;
                        p.SKUCurrentShop = true;
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
                Debug.WriteLine("step 3");
                row.SKUBin = skuBin.TrimStart().TrimEnd();
                row.SKUBin2 = skuBin2.TrimStart().TrimEnd();
                Debug.WriteLine("step 4");
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        public void SearchEntItemInfoReadCode()
        {
            int step = 0;
            try
            {
                ShowKeyBoard.Hide(this);
                LstvItemInfoItems.ItemsSource = null;
                lstItemInfo = new List<ListOfdbRecords>();
                LstvItemInfo.ItemsSource = null;
                frmbtnItemInfoBins.IsVisible = false;
                step = 1;
                if (!string.IsNullOrEmpty(entItemInfoReadCode.Text))
                {
                    step = 2;
                    if (entItemInfoReadCode.Text.Length > 4)
                    {
                        step = 3;
                        lstResultItemInfo = new List<ListOfdbRecords>();


						var result = new List<ListOfdbRecords>();
						if (entItemInfoReadCode.Text.Length == 13 && IsDigitsOnly(entItemInfoReadCode.Text))
						{
							Stopwatch swlstInternalRecordDBSearch = Stopwatch.StartNew();
							Debug.WriteLine("lstBarcodes " + lstBarcodes.Count());
							if (lstBarcodes.Any())
							{
								var n = lstBarcodes.Where(x => x.barCode == entItemInfoReadCode.Text);
								if (n.Any())
								{
									Debug.WriteLine("n leitud " + n.Count());
									string itemCode = n.First().itemCode;
									var preResult = lstInternalRecordDB.Where(x => x.itemCode == itemCode).ToList();
									result = preResult.Where(x => x.barCode.Contains(entItemInfoReadCode.Text)).ToList();
								}
							}
							WriteLog.Write(this, "SearchEntItemInfoReadCode searched value " + entItemInfoReadCode.Text + ". Found - " + (result.Any() ? result.Count().ToString() : "0") + " recods." + "\r\n" +
								"Time elapsed : " + swlstInternalRecordDBSearch.Elapsed.Milliseconds.ToString() + " milliseconds");
						}
						else
						{
							Stopwatch swlstInternalRecordDBSearch = Stopwatch.StartNew();
							result = lstInternalRecordDB.Where(x =>
							   x.itemCode.Contains(entItemInfoReadCode.Text)
							|| x.itemDesc.ToUpper().Contains(entItemInfoReadCode.Text.ToUpper())
							||
							x.barCode.Contains(entItemInfoReadCode.Text)).ToList();
							swlstInternalRecordDBSearch.Stop();
							WriteLog.Write(this, "SearchEntItemInfoReadCode searched value " + entItemInfoReadCode.Text + ". Found - " + (result.Any() ? result.Count().ToString() : "0") + " recods." + "\r\n" +
								"Time elapsed : " + swlstInternalRecordDBSearch.Elapsed.Milliseconds.ToString() + " milliseconds");
						}

						//Stopwatch swlstInternalRecordDBSearch = Stopwatch.StartNew();
      //                  var result = lstInternalRecordDB.Where(x => (x.itemCode.Contains(entItemInfoReadCode.Text) || x.itemDesc.ToUpper().Contains(entItemInfoReadCode.Text.ToUpper()) || x.barCode.Contains(entItemInfoReadCode.Text))).ToList();
      //                  step = 4;
      //                  swlstInternalRecordDBSearch.Stop();
      //                  WriteLog.Write(this, "SearchEntItemInfoReadCode searched value "  + entItemInfoReadCode.Text + ". Found - " + (result.Any() ? result.Count().ToString() : "0") + " recods. Time elapsed : " + swlstInternalRecordDBSearch.Elapsed.Milliseconds.ToString() + " milliseconds");
                        step = 5;
                        if (result.Any())
                        {
                            step = 6;
                            Debug.WriteLine("result count " + "  " + result.Count());
                            lstItemInfo = result;
                            LstvItemInfoItems.ItemsSource = null;
                            LstvItemInfoItems.ItemsSource = lstItemInfo;
                            if (lstItemInfo.Count == 1)
                            {
                                DisplayItemInfoSKU(lstItemInfo.First());
                            }
                            step = 7;
                        }
                        else
                        {
                            step = 8;
                            DisplayFailMessage("EI LEITUD MIDAGI!");
                            focusedEditor = "entItemInfoReadCode";
                            entItemInfoReadCode.BackgroundColor = Color.Yellow;
                            ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
                        }
                        step = 9;
                    }
                    else
                    {
                        step = 10;
                        DisplayFailMessage("SISESTA VÄHEMALT 5 TÄHEMÄRKI!");
                        focusedEditor = "entItemInfoReadCode";
                        entItemInfoReadCode.BackgroundColor = Color.Yellow;
                        ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch, this);
                    }
                    step = 11;
                }
                step = 12;
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + " step: "+ step + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void LstvItemInfoItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstItemInfo = new List<ListOfdbRecords>();
            var item = e.Item as ListOfdbRecords;
            DisplayItemInfoSKU(item);
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
            try
            {
                ShowKeyBoard.Hide(this);
                CollapseAllStackPanels.Collapse(this);
                stkStockTakeAddedRowsView.IsVisible = true;
                obj.mainOperation = "";
                obj.currentLayoutName = "StockTakeAddedRowsView";
                lblStockTakeAddedRowsViewHeader.Text = "INVENTUURIKANDED";
                
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

				foreach (var p in lstInternalInvDB)
				{
					string item = p.itemCode;
					string conf = p.config;
					


					var getPrices = lstInternalRecordDB.Where(x => x.itemCode == item && x.config == conf);
					if (getPrices.Any())
					{
						lstBins = new List<ListOfSKU>();
						if (!string.IsNullOrEmpty(getPrices.First().SKU))
						{
							var parseSKU = getPrices.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
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
											var cultureInfo = CultureInfo.InvariantCulture;

											if (uniqueSKU[0] == obj.shopLocationID)
											{
												Debug.WriteLine("prices found");
												p.price = decimal.Parse(uniqueSKU[3].Replace(",", "."), cultureInfo);
												p.soodushind = decimal.Parse(uniqueSKU[6].Replace(",", "."), cultureInfo);
												p.profiklubihind = decimal.Parse(uniqueSKU[5].Replace(",", "."), cultureInfo);
												p.meistriklubihind = decimal.Parse(uniqueSKU[4].Replace(",", "."), cultureInfo);
											}
										}
									}
								}
							}
						}
					}
				}
                LstvStockTakeAddedRowsView.ItemsSource = lstInternalInvDB;
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }
        private async void LstvStockTakeAddedRowsView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private async void btnStockTakeAddedRowsViewClear_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (await YesNoDialog("INVENTUUR", "JÄTKAMISEL KUSTUTATAKSE KÕIK INVENTUURI KIRJED!", false))
                {
                    lstInternalInvDB = new List<ListOfInvRecords>();

                    var writeInvDbToFile = await WriteInvRecords.Write(this, "", obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
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
            catch(Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
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

			foreach (var p in lstInternalMovementDB)
			{
				string item = p.itemCode;
				string conf = p.config;

				var getPrices = lstInternalRecordDB.Where(x => x.itemCode == item && x.config == conf);
				if (getPrices.Any())
				{
					lstBins = new List<ListOfSKU>();
					if (!string.IsNullOrEmpty(getPrices.First().SKU))
					{
						var parseSKU = getPrices.First().SKU.Split(new[] { "%%%" }, StringSplitOptions.None);
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
										var cultureInfo = CultureInfo.InvariantCulture;

										if (uniqueSKU[0] == obj.shopLocationID)
										{
											Debug.WriteLine("prices found");
											p.price = decimal.Parse(uniqueSKU[3].Replace(",", "."), cultureInfo);
											p.soodushind = decimal.Parse(uniqueSKU[6].Replace(",", "."), cultureInfo);
											p.profiklubihind = decimal.Parse(uniqueSKU[5].Replace(",", "."), cultureInfo);
											p.meistriklubihind = decimal.Parse(uniqueSKU[4].Replace(",", "."), cultureInfo);
										}
									}
								}
							}
						}
					}
				}
			}
			LstvTransferAddedRowsView.ItemsSource = lstInternalMovementDB;
        }
        private async void LstvTransferAddedRowsView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
        private async void btnTransferAddedRowsViewClear_Clicked(object sender, EventArgs e)
        {
            if (await YesNoDialog("LIIKUMINE", "JÄTKAMISEL KUSTUTATAKSE KÕIK LIIKUMISTE KIRJED!", false))
            {
                lstInternalMovementDB = new List<ListOfMovementRecords>();
                //JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                //string data = JsonConvert.SerializeObject(lstInternalMovementDB, jSONsettings);
                //Debug.WriteLine(data);
                var writeMovementDbToFile = await WriteMovementRecords.Write(this, "", obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
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
            try
            {
                Debug.WriteLine("PrepareItemInfoBinsView " + lstBins.Count());

                LstvItemInfoBinsView.ItemsSource = null;
                ShowKeyBoard.Hide(this);
                CollapseAllStackPanels.Collapse(this);
                stkItemInfoBinsView.IsVisible = true;
                obj.mainOperation = "";
                obj.currentLayoutName = "ItemInfoBinsView";
                obj.previousLayoutName = null;
                obj.nextLayoutName = null;
                obj.mainOperation = null;

                lblItemInfoBinsViewHeader.Text = "RIIULID";
                
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
                var lstToDisplay = new List<ListOfSKU>();

                lstToDisplay = lstBins.GroupBy(x => x.SKU).Select(s => new ListOfSKU
                {
                    SKU = s.First().SKU,
                    SKUBin = s.First().SKUBin,
                    SKUqty = s.First().SKUqty,
                    itemMagnitude = s.First().itemMagnitude,
                    SKUCurrentShop = s.First().SKU == obj.shopLocationID ? true : false,
                    SKUShopName = s.First().SKUShopName,
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
                
                LstvItemInfoBinsView.ItemsSource = lstToDisplay;
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
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
            try
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
               


                //DisplayAlert("", currentPurchaseOrder, "OK");
                if (lstPurchaseOrders.Any())
                {
                    if (!string.IsNullOrEmpty(currentPurchaseOrder))
                    {
                        var currentRows = lstPurchaseOrders.Where(x => x.docNo == currentPurchaseOrder);
						currentRows.First().purchasePickedRowCount = 0;
						var purchaseRowCount = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == currentPurchaseOrder);
                        if (purchaseRowCount.Any())
                        {
                            currentRows.First().purchaseRowCount = purchaseRowCount.Count();
                        }
						Debug.WriteLine("alustame");
						var allPicked = true;
						foreach (var p in purchaseRowCount)
						{
							var purchasePickedCount = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == currentPurchaseOrder && x.docLineNo == p.docLineNo);
							if (purchasePickedCount.Any())
							{
								currentRows.First().purchasePickedRowCount = currentRows.First().purchasePickedRowCount + 1;
								if (!((p.initialQty - purchasePickedCount.First().pickedQty) == 0))
								{
									allPicked = false;
								}
							}
							else
							{
								allPicked = false;
							}
						}
						currentRows.First().purchaseOrderPicked = allPicked;
						currentPurchaseOrder = "";
						
					}
                }
                LstvPurchaseReceiveOrders.ItemsSource = lstPurchaseOrders.OrderBy(x => x.shipmentDate);
                focusedEditor = "entPurchaseReceiveOrders";

                btnOperationsPurchaseReceive.IsEnabled = true;
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnPurchaseReceiveOrdersSearch_Clicked(object sender, EventArgs e)
        {
            try
            {
				
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
                    foreach (var r in result)
                    {
                        var purchaseRowCount = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo);
                        if (purchaseRowCount.Any())
                        {
                            r.purchaseRowCount = purchaseRowCount.Count();
                        }

                        var purchasePickedCount = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == r.docNo);
                        if (purchasePickedCount.Any())
                        {
                            r.purchasePickedRowCount = purchasePickedCount.Count();
                        }

                        if (r.purchaseRowCount == r.purchasePickedRowCount)
                        {
                            r.purchaseOrderPicked = true;
                        }
                        else
                        {
                            r.purchaseOrderPicked = false;
                        }
                    }
                    LstvPurchaseReceiveOrders.ItemsSource = result;
                }
                else
                {
                    foreach (var r in lstPurchaseOrders)
                    {
                        var purchaseRowCount = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo);
                        if (purchaseRowCount.Any())
                        {
                            r.purchaseRowCount = purchaseRowCount.Count();
                        }

                        var purchasePickedCount = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == r.docNo);
                        if (purchasePickedCount.Any())
                        {
                            r.purchasePickedRowCount = purchasePickedCount.Count();
                        }

                        if (r.purchaseRowCount == r.purchasePickedRowCount)
                        {
                            r.purchaseOrderPicked = true;
                        }
                        else
                        {
                            r.purchaseOrderPicked = false;
                        }
                    }
                    LstvPurchaseReceiveOrders.ItemsSource = lstPurchaseOrders;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void LstvPurchaseReceiveOrders_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnPurchaseReceiveOrdersReadCodeClear_Clicked(object sender, EventArgs e)
        {
            entPurchaseReceiveOrders.Text = "";
        }

        public void SearchPurchaseReceiveOrders(string scannedCode)
        {
            var lstPurchaseOrdersFiltered = new List<ListOfPurchaseReceive>();

			var purchaseRowsForScanedItem = new List<ListOfdbRecords>();
			if (scannedCode.Length == 13 && IsDigitsOnly(scannedCode))
			{
				Debug.WriteLine("lstBarcodes " + lstBarcodes.Count());
				if (lstBarcodes.Any())
				{
					var n = lstBarcodes.Where(x => x.barCode == scannedCode);
					if (n.Any())
					{
						Debug.WriteLine("n leitud " + n.Count());
						string itemCode = n.First().itemCode;
						var preResult = lstInternalRecordDB.Where(x => x.itemCode == itemCode).ToList();
						purchaseRowsForScanedItem = preResult.Where(x => x.barCode.Contains(scannedCode)).ToList();
					}
				}
			}
			else
			{
				purchaseRowsForScanedItem = lstInternalRecordDB.Where(x => x.barCode.Contains(scannedCode)).ToList();
			}

			//var purchaseRowsForScanedItem = lstInternalRecordDB.Where(x=> x.barCode.Contains(scannedCode)).ToList();
            if (purchaseRowsForScanedItem.Any())
            {
                foreach (var p in purchaseRowsForScanedItem)
                {
                    Debug.WriteLine("p found: " + p.itemCode + "  " + p.barCode);
                    var lst = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.itemCode == p.itemCode);
                    foreach (var l in lst)
                    {
                        Debug.WriteLine("l found: " + l.itemCode + " " + l.docNo + " " + l.barCode);
                        var purchaseRowCount = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == l.docNo);
                        if (purchaseRowCount.Any())
                        {                            
                            l.purchaseRowCount = purchaseRowCount.Count();
                            Debug.WriteLine("l purchaseRowCount: " + purchaseRowCount.Count());
                        }

                        var purchasePickedCount = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == l.docNo);
                        if (purchasePickedCount.Any())
                        {
                            l.purchasePickedRowCount = purchasePickedCount.Count();
                            Debug.WriteLine("l purchasePickedRowCount: " + l.purchasePickedRowCount);
                        }

                        if (l.purchaseRowCount == l.purchasePickedRowCount)
                        {
                            l.purchaseOrderPicked = true;
                            Debug.WriteLine("l purchaseOrderPicked: " + l.purchaseOrderPicked);
                        }
                        else
                        {
                            l.purchaseOrderPicked = false;
                            Debug.WriteLine("l purchaseOrderPicked: " + l.purchaseOrderPicked);
                        }

                        if (lstPurchaseOrdersFiltered.Any())
                        {
                            var exists = lstPurchaseOrdersFiltered.Where(x => x.docNo == l.docNo);
                            if (!exists.Any())
                            {
                                Debug.WriteLine("exists add");
                                lstPurchaseOrdersFiltered.Add(l);
                            }
                        }
                        else
                        {
                            Debug.WriteLine("first add");
                            lstPurchaseOrdersFiltered.Add(l);
                        }
                    }
                }


                //foreach (var r in lstPurchaseOrdersFiltered)
                //{
                //    var purchaseRowCount = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo);
                //    if (purchaseRowCount.Any())
                //    {
                //        r.purchaseRowCount = purchaseRowCount.Count();
                //    }

                //    var purchasePickedCount = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == r.docNo);
                //    if (purchasePickedCount.Any())
                //    {
                //        r.purchasePickedRowCount = purchasePickedCount.Count();
                //    }

                //    if (r.purchaseRowCount == r.purchasePickedRowCount)
                //    {
                //        r.purchaseOrderPicked = true;
                //    }
                //    else
                //    {
                //        r.purchaseOrderPicked = false;
                //    }
                //}

                LstvPurchaseReceiveOrders.ItemsSource = null;
                LstvPurchaseReceiveOrders.ItemsSource = lstPurchaseOrdersFiltered;
            }
        }
        #endregion

        #region stkPurchaseReceiveOrderLines
        public void PreparePurchaseReceiveOrderLines()
        {
            try
            {
                CollapseAllStackPanels.Collapse(this);
                stkPurchaseReceiveOrderLines.IsVisible = true;
                obj.mainOperation = "";
                obj.currentLayoutName = "PurchaseReceiveOrderLines";
                lblPurchaseReceiveOrderLinesHeader.Text = "OSTUTARNE READ";
                currentPurchaseOrder = lstPurchaseOrderLines.First().docNo;
                //DisplayAlert("ost", currentPurchaseOrder, "OK");
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
                LstvPurchaseReceiveOrderLinesInfo.ItemsSource = null;

                var lstPurchaseOrderLinesInfo = lstPurchaseOrderLines.Take(1);
                if (!string.IsNullOrEmpty(currentPurchaseOrder))
                {
                    var purchaseRowCount = lstInternalPurchaseReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == currentPurchaseOrder);
                    if (purchaseRowCount.Any())
                    {
                        lstPurchaseOrderLinesInfo.First().purchaseRowCount = purchaseRowCount.Count();
                    }

                    var purchasePickedCount = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == currentPurchaseOrder);
                    if (purchasePickedCount.Any())
                    {
                        lstPurchaseOrderLinesInfo.First().purchasePickedRowCount = purchasePickedCount.Count();
                    }
                }


                LstvPurchaseReceiveOrderLinesInfo.ItemsSource = lstPurchaseOrderLinesInfo;
				Debug.WriteLine("department " + lstPurchaseOrderLinesInfo.First().department);


				foreach (var p in lstPurchaseOrderLines)
                {
                    var lineInfo = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == currentPurchaseOrder && x.docLineNo == p.docLineNo);
                    if (lineInfo.Any())
                    {
						Debug.WriteLine("lineInfo.First().pickedQty " + lineInfo.First().pickedQty);
						Debug.WriteLine("lineInfo.First().magnitude " + lineInfo.First().magnitude);
						Debug.WriteLine("p.initialQty " +( p.initialQty - lineInfo.First().pickedQty));

						p.pickedQty = lineInfo.First().pickedQty;
                        p.magnitude = lineInfo.First().magnitude;
                        p.remaininQty = p.initialQty - lineInfo.First().pickedQty;
                       
                        if (p.remaininQty == 0)
                        {
                            p.completelyPicked = 1;
                            //DisplayAlert("e", p.itemCode + " " +  p.remaininQty.ToString(), "OK");
                        }
                        else
                        {
                            p.completelyPicked = 0;
                        }
                    }
                    else
                    {
                        var magnitudeToAddOnScreen = p.magnitude;
                        if (p.magnitude.Contains("%%%"))
                        {
                            var magnitudes = p.magnitude.Split(new[] { "%%%" }, StringSplitOptions.None);
                            magnitudeToAddOnScreen = magnitudes[0];
                        }

                        p.magnitude = magnitudeToAddOnScreen;
                        p.pickedQty = 0;
                        p.completelyPicked = 0;
                        //p.remaininQty = (p.initialQty ?? 0 )- lineInfo.First().pickedQty;
                    }

                    p.showPurchaseReceiveQty = obj.showPurchaseReceiveQty;
                }


                LstvPurchaseReceiveOrderLines.ItemsSource = lstPurchaseOrderLines.OrderBy(x => x.docLineNo) ;
                focusedEditor = "entPurchaseReceiveOrderLines";
                ShowKeyBoard.Hide(this);
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
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
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnPurchaseReceiveOrderLinesSearch_Clicked(object sender, EventArgs e)
        {
            entPurchaseReceiveOrderLinesSearchValue = entPurchaseReceiveOrderLines.Text;
            SearchPurchaseReceiveOrderLines();
        }

        public void SearchPurchaseReceiveOrderLines()
        {
            try
            {
                if (!string.IsNullOrEmpty(entPurchaseReceiveOrderLines.Text))
                {
                    string searchValue = entPurchaseReceiveOrderLines.Text;
                    var result = lstPurchaseOrderLines.Where(x => 
                    !string.IsNullOrEmpty(x.barCode) && 
                    ( x.itemCode.Contains(searchValue) || 
                      x.itemDesc.Contains(searchValue) || 
                      x.barCode.Contains(searchValue))).ToList();
                    if (!result.Any())
                    {
                        DisplayFailMessage("OTSITUD VÄÄRTUST EI LEITUD!");
                    }
                    else
                    {
                        if (result.Count() == 1)
                        {
                            lstPurchaseOrderQuantityInsertInfo = new List<ListOfPurchaseReceive>();
                            lstPurchaseOrderQuantityInsertInfo = result;
                            PreparePurchaseOrderQuantityInsert();
                        }
                        else
                        {
                            DisplayAlert("OTSING", "LEITI ROHKEM KUI 1 RIDA. PALUN VALI KAUP KÄSITSI!", "OK");
                        }
                    }
                    LstvPurchaseReceiveOrderLines.ItemsSource = null;
                    LstvPurchaseReceiveOrderLines.ItemsSource = result;

                }
                else
                {
                    LstvPurchaseReceiveOrderLines.ItemsSource = null;
                    LstvPurchaseReceiveOrderLines.ItemsSource = lstPurchaseOrderLines;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("viga", ex.Message, "OK");
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
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
            try
            {
                bool proceed = true;
                decimal previouslyReadQty = 0;
                string previouslyReadMagnitude = "";
                if (lstPurchaseOrderPickedQuantities.Any())
                {
                    var previousRead = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == lstPurchaseOrderQuantityInsertInfo.First().docNo && x.docLineNo == lstPurchaseOrderQuantityInsertInfo.First().docLineNo && x.shop == obj.shopLocationID);
                    if (previousRead.Any())
                    {
                        var line = lstPurchaseOrderQuantityInsertInfo.First().docLineNo.Replace(".",",").Split(new[] { "," }, StringSplitOptions.None);
                        if (!obj.showPurchaseReceiveQtySum)
                        {
                            if (!await YesNoDialog("OSTU VASTUVÕTT", lstPurchaseOrderQuantityInsertInfo.First().docNo + " RIDA " + line[0] + " ON JUBA LOETUD - KAS SOOVID PARANDADA?", false))
                            {
                                proceed = false;
                            }
                            else
                            {
                                previouslyReadQty = previousRead.First().pickedQty;
                                purchReceiveRecordID = previousRead.First().recordID;
                                previouslyReadMagnitude = previousRead.First().magnitude;

                            }
                        }
                        else
                        {
                            previouslyReadQty = previousRead.First().pickedQty;
                            purchReceiveRecordID = previousRead.First().recordID;
                            previouslyReadMagnitude = previousRead.First().magnitude;
                        }
                    }
                }
                if (proceed)
                {
                    CollapseAllStackPanels.Collapse(this);
                    stkPurchaseOrderQuantityInsert.IsVisible = true;
                    obj.mainOperation = "";
                    obj.previousLayoutName = null;
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
                   
                    LstvPurchaseOrderQuantityInsertInfo.ItemsSource = lstPurchaseOrderQuantityInsertInfo;
                    focusedEditor = "entPurchaseOrderQuantityInsertQuantity";




                    lstItemMagnitudes = new List<ListOfMagnitudes>();
                    var magnitudeToAddOnScreen = lstPurchaseOrderQuantityInsertInfo.First().magnitude;
                    lstPurchaseOrderQuantityInsertQuantities = new List<ListOfSHRCVToExport>();

                    if (lstPurchaseOrderQuantityInsertInfo.First().magnitude.Contains("%%%"))
                    {
                        var magnitudes = lstPurchaseOrderQuantityInsertInfo.First().magnitude.Split(new[] { "%%%" }, StringSplitOptions.None);
                        magnitudeToAddOnScreen = magnitudes[0];
                        foreach (var m in magnitudes)
                        {
                            if (m == magnitudeToAddOnScreen)
                            {
                                var rowMagnitude = new ListOfMagnitudes { defaultMagnitude = true, magnitude = m };
                                lstItemMagnitudes.Add(rowMagnitude);
                            }
                            else
                            {
                                var rowMagnitude = new ListOfMagnitudes { defaultMagnitude = false, magnitude = m };
                                lstItemMagnitudes.Add(rowMagnitude);
                            }
                        }
                    }

                    if (lstItemMagnitudes.Any())
                    {
                        lblPurchaseOrderQuantityInsertQuantityUOM.IsVisible = false;
                        frmbtnPurchaseOrderQuantityInsertQuantityUOM.IsVisible = true;
                        btnPurchaseOrderQuantityInsertQuantityUOM.Text = !string.IsNullOrEmpty(previouslyReadMagnitude) ? previouslyReadMagnitude : magnitudeToAddOnScreen;
                    }
                    else
                    {
                        lblPurchaseOrderQuantityInsertQuantityUOM.IsVisible = true;
                        frmbtnPurchaseOrderQuantityInsertQuantityUOM.IsVisible = false;
                        lblPurchaseOrderQuantityInsertQuantityUOM.Text = !string.IsNullOrEmpty(previouslyReadMagnitude) ? previouslyReadMagnitude : magnitudeToAddOnScreen;
                    }

                    var row = new ListOfSHRCVToExport
                    {
                        initialQty = lstPurchaseOrderQuantityInsertInfo.First().initialQty,
                        pickedQty = previouslyReadQty,
                        remainingQty = (lstPurchaseOrderQuantityInsertInfo.First().initialQty - previouslyReadQty),
                        magnitude = previouslyReadQty == 0 ? "" : !string.IsNullOrEmpty(previouslyReadMagnitude) ? previouslyReadMagnitude : magnitudeToAddOnScreen,
                        barCode = lstPurchaseOrderQuantityInsertInfo.First().barCode
                    };
                    lstPurchaseOrderQuantityInsertQuantities.Add(row);
                    LstvPurchaseOrderQuantityInsertQuantityInfo.ItemsSource = null;
                    
                    LstvPurchaseOrderQuantityInsertQuantityInfo.ItemsSource = lstPurchaseOrderQuantityInsertQuantities;

                    if (previouslyReadQty > 0)
                    {
                        if (obj.showPurchaseReceiveQtySum)
                        {
                            entPurchaseOrderQuantityInsertQuantity.Text = "1";
                        }
                        else
                        {
                            entPurchaseOrderQuantityInsertQuantity.Text = (String.Format("{0:0.00}", previouslyReadQty)).Replace(".00", "");
                        }
                    }
                    else
                    {
                        entPurchaseOrderQuantityInsertQuantity.Text = "1";
                        //entPurchaseOrderQuantityInsertQuantity.Text = "";
                    }
                    entPurchaseOrderQuantityInsertQuantity.BackgroundColor = Color.Yellow;
                    ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
                }
                defaultvalueOverride = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return regex.IsMatch(text);
        }

        private async void btnPurchaseOrderQuantityInsertQuantityOK_Clicked(object sender, EventArgs e)
        {
            try
            {
                JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                bool proceed = true;
                decimal quantity = 0;
                
                if (IsTextAllowed(entPurchaseOrderQuantityInsertQuantity.Text))
                {
                    proceed = false;
                    DisplayFailMessage("SISESTA NUMBER!");
                }

                if (proceed)
                {
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
                        if (!obj.showPurchaseReceiveQtySum)
                        {
                            if (!await YesNoDialog("OSTUTARNE VASTUVÕTT", "SISESTATUD KOGUS ON 0. KAS JÄTKATA KIRJE TEKITAMISEGA?", false))
                            {
                                proceed = false;
                            }
                        }
                    }

                    if (proceed)
                    {
                        if (obj.showPurchaseReceiveQtySum)
                        {
                            int lastRecordID = 0;
                            if (lstPurchaseOrderPickedQuantities.Any())
                            {
                                var previousRead = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == lstPurchaseOrderQuantityInsertInfo.First().docNo && x.docLineNo == lstPurchaseOrderQuantityInsertInfo.First().docLineNo && x.shop == obj.shopLocationID);
                                if (previousRead.Any())
                                {
                                    lastRecordID = previousRead.First().recordID;
                                    var record = lstPurchaseOrderPickedQuantities.Where(x => x.recordID == purchReceiveRecordID);
                                    if (record.Any())
                                    {
                                        record.First().pickedQty = record.First().pickedQty + quantity;
                                        record.First().magnitude = lblPurchaseOrderQuantityInsertQuantityUOM.IsVisible ? lblPurchaseOrderQuantityInsertQuantityUOM.Text : btnPurchaseOrderQuantityInsertQuantityUOM.Text;
                                        record.First().recordDate = DateTime.Now;
                                        string data1 = JsonConvert.SerializeObject(lstPurchaseOrderPickedQuantities, jSONsettings);

                                        var writePurchaseOrderPickedQuantitiesDbToFile1 = await WritePurchaseOrderPickedQuantitiesRecords.Write(this, data1, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                        if (writePurchaseOrderPickedQuantitiesDbToFile1.Item1)
                                        {
                                            DisplaySuccessMessage("SALVESTATUD!");
                                            ShowKeyBoard.Hide(this);
                                            proceed = false;
                                            PreparePurchaseReceiveOrderLines();
                                        }
                                        else
                                        {
                                            DisplayFailMessage(writePurchaseOrderPickedQuantitiesDbToFile1.Item2);
                                        }
                                    }
                                }
                                else
                                {
                                    lastRecordID = lstPurchaseOrderPickedQuantities.OrderByDescending(x => x.recordID).Take(1).First().recordID;
                                }
                            }
                            if (proceed)
                            {

                                lstPurchaseOrderPickedQuantities.Add(new ListOfSHRCVToExport
                                {
                                    docNo = lstPurchaseOrderQuantityInsertInfo.First().docNo,
                                    docLineNo = lstPurchaseOrderQuantityInsertInfo.First().docLineNo,
                                    initialQty = lstPurchaseOrderQuantityInsertInfo.First().initialQty,
                                    pickedQty = lstPurchaseOrderQuantityInsertInfo.First().pickedQty + quantity,
                                    recordDate = DateTime.Now,
                                    shop = lstPurchaseOrderQuantityInsertInfo.First().shop,
                                    itemCode = lstPurchaseOrderQuantityInsertInfo.First().itemCode,
                                    barCode = lstPurchaseOrderQuantityInsertInfo.First().barCode,
                                    magnitude = lblPurchaseOrderQuantityInsertQuantityUOM.IsVisible ? lblPurchaseOrderQuantityInsertQuantityUOM.Text : btnPurchaseOrderQuantityInsertQuantityUOM.Text,
                                    recordID = lastRecordID + 1
                                });

                                //JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                string data = JsonConvert.SerializeObject(lstPurchaseOrderPickedQuantities, jSONsettings);

                                var writePurchaseOrderPickedQuantitiesDbToFile = await WritePurchaseOrderPickedQuantitiesRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                if (writePurchaseOrderPickedQuantitiesDbToFile.Item1)
                                {

                                    DisplaySuccessMessage("SALVESTATUD!");
                                    ShowKeyBoard.Hide(this);
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
                            if (quantity > -1)
                            {
                                int lastRecordID = 0;
                                if (lstPurchaseOrderPickedQuantities.Any())
                                {
                                    var previousRead = lstPurchaseOrderPickedQuantities.Where(x => x.docNo == lstPurchaseOrderQuantityInsertInfo.First().docNo && x.docLineNo == lstPurchaseOrderQuantityInsertInfo.First().docLineNo && x.shop == obj.shopLocationID);
                                    if (previousRead.Any())
                                    {
                                        lastRecordID = previousRead.First().recordID;
                                        var record = lstPurchaseOrderPickedQuantities.Where(x => x.recordID == purchReceiveRecordID);
                                        if (record.Any())
                                        {
                                            if (quantity == 0)
                                            {
                                                if (record.Any())
                                                {
                                                    lstPurchaseOrderPickedQuantities.Remove(record.Take(1).First());
                                                    string data = JsonConvert.SerializeObject(lstPurchaseOrderPickedQuantities, jSONsettings);

                                                    var writePurchaseOrderPickedQuantitiesDbToFile = await WritePurchaseOrderPickedQuantitiesRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                                    if (writePurchaseOrderPickedQuantitiesDbToFile.Item1)
                                                    {
                                                        purchReceiveRecordID = 0;
                                                        DisplaySuccessMessage("SALVESTATUD!");
                                                        ShowKeyBoard.Hide(this);
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
                                                record.First().pickedQty = quantity;
                                                record.First().magnitude = lblPurchaseOrderQuantityInsertQuantityUOM.IsVisible ? lblPurchaseOrderQuantityInsertQuantityUOM.Text : btnPurchaseOrderQuantityInsertQuantityUOM.Text;
                                                record.First().recordDate = DateTime.Now;
                                                string data1 = JsonConvert.SerializeObject(lstPurchaseOrderPickedQuantities, jSONsettings);

                                                var writePurchaseOrderPickedQuantitiesDbToFile1 = await WritePurchaseOrderPickedQuantitiesRecords.Write(this, data1, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                                if (writePurchaseOrderPickedQuantitiesDbToFile1.Item1)
                                                {
                                                    DisplaySuccessMessage("SALVESTATUD!");
                                                    ShowKeyBoard.Hide(this);
                                                    proceed = false;
                                                    PreparePurchaseReceiveOrderLines();
                                                }
                                                else
                                                {
                                                    DisplayFailMessage(writePurchaseOrderPickedQuantitiesDbToFile1.Item2);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lastRecordID = lstPurchaseOrderPickedQuantities.OrderByDescending(x => x.recordID).Take(1).First().recordID;
                                    }
                                }
                                if (proceed)
                                {
                                    if (quantity == 0)
                                    {

                                    }
                                    else
                                    {
                                        lstPurchaseOrderPickedQuantities.Add(new ListOfSHRCVToExport
                                        {
                                            docNo = lstPurchaseOrderQuantityInsertInfo.First().docNo,
                                            docLineNo = lstPurchaseOrderQuantityInsertInfo.First().docLineNo,
                                            initialQty = lstPurchaseOrderQuantityInsertInfo.First().initialQty,
                                            pickedQty = quantity,
                                            recordDate = DateTime.Now,
                                            shop = lstPurchaseOrderQuantityInsertInfo.First().shop,
                                            itemCode = lstPurchaseOrderQuantityInsertInfo.First().itemCode,
                                            barCode = lstPurchaseOrderQuantityInsertInfo.First().barCode,
                                            magnitude = lblPurchaseOrderQuantityInsertQuantityUOM.IsVisible ? lblPurchaseOrderQuantityInsertQuantityUOM.Text : btnPurchaseOrderQuantityInsertQuantityUOM.Text,
                                            recordID = lastRecordID + 1
                                        });

                                        //JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                        string data = JsonConvert.SerializeObject(lstPurchaseOrderPickedQuantities, jSONsettings);

                                        var writePurchaseOrderPickedQuantitiesDbToFile = await WritePurchaseOrderPickedQuantitiesRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                        if (writePurchaseOrderPickedQuantitiesDbToFile.Item1)
                                        {

                                            DisplaySuccessMessage("SALVESTATUD!");
                                            ShowKeyBoard.Hide(this);
                                            PreparePurchaseReceiveOrderLines();
                                        }
                                        else
                                        {
                                            DisplayFailMessage(writePurchaseOrderPickedQuantitiesDbToFile.Item2);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DisplayFailMessage("KOGUS PEAB OLEMA NUMBER!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnPurchaseOrderQuantityInsertQuantityUOM_Clicked(object sender, EventArgs e)
        {
            obj.previousLayoutName = "PurchaseOrderQuantityInsert";
            PrepareSelectMagnitude();
        }
        #endregion

        #region stkTransferReceiveOrders
        public void PrepareTransferReceiveOrders()
        {
            try
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
               

               

               // DisplayAlert("", currentTransferOrder, "OK");
                if (lstTransferOrders.Any())
                {
                    if (!string.IsNullOrEmpty(currentTransferOrder))
                    {
                        var currentRows = lstTransferOrders.Where(x => x.docNo == currentTransferOrder);
                        var transferRowCount = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == currentTransferOrder);
                        if (transferRowCount.Any())
                        {
                            currentRows.First().transferRowCount = transferRowCount.Count();
                        }

                        var transferPickedCount = lstTransferOrderPickedQuantities.Where(x => x.docNo == currentTransferOrder);
                        if (transferPickedCount.Any())
                        {
                            currentRows.First().transferPickedRowCount = transferPickedCount.Count();
                        }

                        if (currentRows.First().transferRowCount == currentRows.First().transferPickedRowCount)
                        {
                            currentRows.First().transferOrderPicked = true;
                        }
                        else
                        {
                            currentRows.First().transferOrderPicked = false;
                        }
                        currentTransferOrder = "";
                    }

                }

                LstvTransferReceiveOrders.ItemsSource = lstTransferOrders;
                //if (lstInternalTransferReceiveDB.Any())
                //{
                //    lstTransferOrders = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID).ToList().GroupBy(x => x.docNo).Select(s => new ListOfTransferReceive
                //    {
                //        docNo = s.First().docNo,
                //        shop = s.First().shop,
                //        receivedFromShop = s.First().receivedFromShop,
                //        shipmentDate = s.First().shipmentDate,
                //    }).ToList().OrderBy(x => x.shipmentDate).ToList();

                //    foreach (var p in lstTransferOrders)
                //    {
                //        Debug.WriteLine("receivedFromShop " + p.receivedFromShop);
                //        string shopName = lstShopRelations.Where(x => x.shopID == p.receivedFromShop).ToList().First().shopName;
                //        p.receivedFromName = shopName;
                //        Debug.WriteLine("receivedFromName " + p.receivedFromName);
                //    }
                //    LstvTransferReceiveOrders.ItemsSource = lstTransferOrders;
                //}
                //else
                //{
                //    DisplayFailMessage("ÜLEVIIMISTE ANDMEBAASI EI LEITUD!");
                //}
                focusedEditor = "entTransferReceiveOrders";
                btnOperationsTransferReceive.IsEnabled = true;
            }
            catch(Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnTransferReceiveOrdersSearch_Clicked(object sender, EventArgs e)
        {
            try
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
                    foreach (var r in result)
                    {
                        var transferRowCount = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo);
                        if (transferRowCount.Any())
                        {
                            r.transferRowCount = transferRowCount.Count();
                        }

                        var transferPickedCount = lstTransferOrderPickedQuantities.Where(x => x.docNo == r.docNo);
                        if (transferPickedCount.Any())
                        {
                            r.transferPickedRowCount = transferPickedCount.Count();
                        }

                        if (r.transferRowCount == r.transferPickedRowCount)
                        {
                            r.transferOrderPicked = true;
                        }
                        else
                        {
                            r.transferOrderPicked = false;
                        }
                        string shopName = lstShopRelations.Where(x => x.shopID == r.receivedFromShop).ToList().First().shopName;
                        r.receivedFromName = shopName;
                    }
                    LstvTransferReceiveOrders.ItemsSource = result;
                }
                else
                {
                    foreach (var r in lstTransferOrders)
                    {
                        var transferRowCount = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo);
                        if (transferRowCount.Any())
                        {
                            r.transferRowCount = transferRowCount.Count();
                        }

                        var transferPickedCount = lstTransferOrderPickedQuantities.Where(x => x.docNo == r.docNo);
                        if (transferPickedCount.Any())
                        {
                            r.transferPickedRowCount = transferPickedCount.Count();
                        }

                        if (r.transferRowCount == r.transferPickedRowCount)
                        {
                            r.transferOrderPicked = true;
                        }
                        else
                        {
                            r.transferOrderPicked = false;
                        }
                        string shopName = lstShopRelations.Where(x => x.shopID == r.receivedFromShop).ToList().First().shopName;
                        r.receivedFromName = shopName;
                    }
                    LstvTransferReceiveOrders.ItemsSource = lstTransferOrders;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void LstvTransferReceiveOrders_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as ListOfTransferReceive;
                Debug.WriteLine(item.docNo + "  " + obj.shopLocationID);
                lstTransferOrderLines = lstInternalTransferReceiveDB.Where(x => x.docNo == item.docNo && x.shop == obj.shopLocationID).ToList();
                if (lstTransferOrderLines.Any())
                {
                    foreach (var p in lstTransferOrderLines)
                    {
                        var itemInfo = lstInternalRecordDB.Where(x => x.itemCode == p.itemCode).ToList();
                        if (itemInfo.Any())
                        {
                            string shopName = lstShopRelations.Where(x => x.shopID == p.receivedFromShop).ToList().First().shopName;
                            Debug.WriteLine(shopName);
                            p.itemDesc = itemInfo.First().itemDesc;
                            p.barCode = itemInfo.First().barCode;
                            p.receivedFromName = shopName;
                        }
                    }
                    PrepareTransferReceiveOrderLines();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnTransferReceiveOrdersReadCodeClear_Clicked(object sender, EventArgs e)
        {
            entTransferReceiveOrders.Text = "";
        }


        public void SearchTransferReceiveOrders(string scannedCode)
        {
            var lstTransferOrdersFiltered = new List<ListOfTransferReceive>();

			var transferRowsForScanedItem = new List<ListOfdbRecords>();
			if (scannedCode.Length == 13 && IsDigitsOnly(scannedCode))
			{
				Debug.WriteLine("lstBarcodes " + lstBarcodes.Count());
				if (lstBarcodes.Any())
				{
					var n = lstBarcodes.Where(x => x.barCode == scannedCode);
					if (n.Any())
					{
						Debug.WriteLine("n leitud " + n.Count());
						string itemCode = n.First().itemCode;
						var preResult = lstInternalRecordDB.Where(x => x.itemCode == itemCode).ToList();
						transferRowsForScanedItem = preResult.Where(x => x.barCode.Contains(scannedCode)).ToList();
					}
				}
			}
			else
			{
				transferRowsForScanedItem = lstInternalRecordDB.Where(x => x.barCode.Contains(scannedCode)).ToList();
			}

			//var transferRowsForScanedItem = lstInternalRecordDB.Where(x => x.barCode.Contains(scannedCode)).ToList();
            if (transferRowsForScanedItem.Any())
            {
                foreach (var p in transferRowsForScanedItem)
                {
                    var lst = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID && x.itemCode == p.itemCode);
                    foreach (var l in lst)
                    {
                        if (lstTransferOrdersFiltered.Any())
                        {
                            var exists = lstTransferOrdersFiltered.Where(x => x.docNo == l.docNo);
                            if (!exists.Any())
                            {
                                lstTransferOrdersFiltered.Add(l);
                            }
                        }
                        else
                        {
                            lstTransferOrdersFiltered.Add(l);
                        }
                    }
                }

                foreach (var r in lstTransferOrdersFiltered)
                {
                    var transferRowCount = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == r.docNo);
                    if (transferRowCount.Any())
                    {
                        r.transferRowCount = transferRowCount.Count();
                    }

                    var transferPickedCount = lstTransferOrderPickedQuantities.Where(x => x.docNo == r.docNo);
                    if (transferPickedCount.Any())
                    {
                        r.transferPickedRowCount = transferPickedCount.Count();
                    }

                    if (r.transferRowCount == r.transferPickedRowCount)
                    {
                        r.transferOrderPicked = true;
                    }
                    else
                    {
                        r.transferOrderPicked = false;
                    }
                    string shopName = lstShopRelations.Where(x => x.shopID == r.receivedFromShop).ToList().First().shopName;
                    r.receivedFromName = shopName;
                }

                LstvTransferReceiveOrders.ItemsSource = null;
                LstvTransferReceiveOrders.ItemsSource = lstTransferOrdersFiltered;
            }
        }

        #endregion

        #region stkTransferReceiveOrderLines
        public void PrepareTransferReceiveOrderLines()
        {
            try
            {
                CollapseAllStackPanels.Collapse(this);
                stkTransferReceiveOrderLines.IsVisible = true;
                obj.mainOperation = "";
                obj.currentLayoutName = "TransferReceiveOrderLines";
                lblTransferReceiveOrderLinesHeader.Text = "ÜLEVIIMISTARNE READ";
                currentTransferOrder = lstTransferOrderLines.First().docNo;

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

             
                LstvTransferReceiveOrderLines.ItemsSource = lstTransferOrderLines;


                LstvTransferReceiveOrderLinesInfo.ItemsSource = null;
               
                var lstTransferOrderLinesInfo = lstTransferOrderLines.Take(1);

                if (!string.IsNullOrEmpty(currentTransferOrder))
                {
                    var transferRowCount = lstInternalTransferReceiveDB.Where(x => x.shop == obj.shopLocationID && x.docNo == currentTransferOrder);
                    if (transferRowCount.Any())
                    {
                        lstTransferOrderLinesInfo.First().transferRowCount = transferRowCount.Count();
                    }

                    var transferPickedCount = lstTransferOrderPickedQuantities.Where(x => x.docNo == currentTransferOrder);
                    if (transferPickedCount.Any())
                    {
                        lstTransferOrderLinesInfo.First().transferPickedRowCount = transferPickedCount.Count();
                    }
                }

                LstvTransferReceiveOrderLinesInfo.ItemsSource = lstTransferOrderLinesInfo;

                foreach (var p in lstTransferOrderLines)
                {
                    var lineInfo = lstTransferOrderPickedQuantities.Where(x => x.docNo == currentTransferOrder && x.docLineNo == p.docLineNo);
                    if (lineInfo.Any())
                    {
                        p.pickedQty = lineInfo.First().pickedQty;
                        p.magnitude = lineInfo.First().magnitude;
                        p.remaininQty = p.initialQty - lineInfo.First().pickedQty;
                        if (p.remaininQty == 0)
                        {
                            p.completelyPicked = 1;
                        }
                        else
                        {
                            p.completelyPicked = 0;
                        }
                    }
                    else
                    {
                        var magnitudeToAddOnScreen = p.magnitude;
                        if (p.magnitude.Contains("%%%"))
                        {
                            var magnitudes = p.magnitude.Split(new[] { "%%%" }, StringSplitOptions.None);
                            magnitudeToAddOnScreen = magnitudes[0];
                        }

                        p.magnitude = magnitudeToAddOnScreen;
                        p.pickedQty = 0;
                        //p.remaininQty = (p.initialQty ?? 0 )- lineInfo.First().pickedQty;
                    }

                    p.showTransferReceiveQty = obj.showTransferReceiveQty;
                }

                focusedEditor = "entTransferReceiveOrderLines";

                ShowKeyBoard.Hide(this);
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }
        private void LstvTransferReceiveOrderLines_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as ListOfTransferReceive;
                lstTransferOrderQuantityInsertInfo = new List<ListOfTransferReceive>();
                lstTransferOrderQuantityInsertInfo.Add(item);
                PrepareTransferOrderQuantityInsert();
            }
            catch (Exception ex)
            {
                DisplayAlert("LstvTransferReceiveOrderLines_ItemTapped", ex.Message, "OK");
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnTransferReceiveOrderLinesSearch_Clicked(object sender, EventArgs e)
        {
            SearchTransferReceiveOrderLines();
        }

        public void SearchTransferReceiveOrderLines()
        {
            try
            {
                Debug.WriteLine("entTransferReceiveOrderLines.Text " + entTransferReceiveOrderLines.Text);
                if (!string.IsNullOrEmpty(entTransferReceiveOrderLines.Text))
                {
                    string searchValue = entTransferReceiveOrderLines.Text.ToUpper();
                    var result = lstTransferOrderLines.Where(x => !string.IsNullOrEmpty(x.barCode) & (
                    x.itemCode.ToUpper().Contains(searchValue) ||
                    x.itemDesc.ToUpper().Contains(searchValue) ||
                    x.barCode.ToUpper().Contains(searchValue))).ToList();

                    if (!result.Any())
                    {
                        DisplayFailMessage("OTSITUD VÄÄRTUST EI LEITUD!");
                    }
                    {
                        if (result.Count() == 1)
                        {
                            lstTransferOrderQuantityInsertInfo = new List<ListOfTransferReceive>();
                            lstTransferOrderQuantityInsertInfo = result;
                            PrepareTransferOrderQuantityInsert();
                        }
                        else
                        {
                            DisplayAlert("OTSING", "LEITI ROHKEM KUI 1 RIDA. PALUN VALI KAUP KÄSITSI!", "OK");
                        }
                    }
                    LstvTransferReceiveOrderLines.ItemsSource = result;

                }
                else
                {
                    LstvTransferReceiveOrderLines.ItemsSource = lstTransferOrderLines;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
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
            try
            {
                Console.WriteLine("1");
                bool proceed = true;
                decimal previouslyReadQty = 0;
                string previouslyReadMagnitude = "";
                if (lstTransferOrderPickedQuantities.Any())
                {
                    Console.WriteLine("3");
                    var previousRead = lstTransferOrderPickedQuantities.Where(x => x.docNo == lstTransferOrderQuantityInsertInfo.First().docNo && x.docLineNo == lstTransferOrderQuantityInsertInfo.First().docLineNo && x.shop == obj.shopLocationID);
                    if (previousRead.Any())
                    {
                        var line = lstTransferOrderQuantityInsertInfo.First().docLineNo.Replace(".",",").Split(new[] { "," }, StringSplitOptions.None);
                        if (!obj.showTransferReceiveQtySum)
                        {
                            if (!await YesNoDialog("ÜLEVIIMISTARNE VASTUVÕTT", lstTransferOrderQuantityInsertInfo.First().docNo + " RIDA " + line[0] + " ON JUBA LOETUD - KAS SOOVID PARANDADA?", false))
                            {
                                proceed = false;
                            }
                            else
                            {
                                Console.WriteLine("5");
                                previouslyReadQty = previousRead.First().pickedQty;
                                transferReceiveRecordID = previousRead.First().recordID;
                                previouslyReadMagnitude = previousRead.First().magnitude;
                            }
                        }
                        else
                        {
                            Console.WriteLine("5");
                            previouslyReadQty = previousRead.First().pickedQty;
                            transferReceiveRecordID = previousRead.First().recordID;
                            previouslyReadMagnitude = previousRead.First().magnitude;
                        }
                    }
                }
                Console.WriteLine("6 " + proceed);
                if (proceed)
                {
                    CollapseAllStackPanels.Collapse(this);
                    stkTransferOrderQuantityInsert.IsVisible = true;
                    obj.mainOperation = "";
                    obj.previousLayoutName = null;
                    obj.currentLayoutName = "TransferOrderQuantityInsert";
                    lblTransferOrderQuantityInsertHeader.Text = "ÜLEVIIMISTARNE REA KOGUS";

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
                   
                    LstvTransferOrderQuantityInsertInfo.ItemsSource = lstTransferOrderQuantityInsertInfo;
                    focusedEditor = "entTransferOrderQuantityInsertQuantity";

                    lstItemMagnitudes = new List<ListOfMagnitudes>();
                    var magnitudeToAddOnScreen = lstTransferOrderQuantityInsertInfo.First().magnitude;
                    Debug.WriteLine("magnitudeToAddOnScreen " + magnitudeToAddOnScreen);
                    Debug.WriteLine("lstTransferOrderQuantityInsertInfo.First().magnitude " + lstTransferOrderQuantityInsertInfo.First().magnitude);

                    lstTransferOrderQuantityInsertQuantities = new List<ListOfTRFRCVToExport>();

                    if (lstTransferOrderQuantityInsertInfo.First().magnitude.Contains("%%%"))
                    {
                        var magnitudes = lstTransferOrderQuantityInsertInfo.First().magnitude.Split(new[] { "%%%" }, StringSplitOptions.None);
                        magnitudeToAddOnScreen = magnitudes[0];
                        Debug.WriteLine("magnitudeToAddOnScreen2 " + magnitudeToAddOnScreen);
                        foreach (var m in magnitudes)
                        {
                            if (m == magnitudeToAddOnScreen)
                            {
                                Debug.WriteLine("magnitudeToAddOnScreen default: " + magnitudeToAddOnScreen);
                                var rowMagnitude = new ListOfMagnitudes { defaultMagnitude = true, magnitude = m };
                                lstItemMagnitudes.Add(rowMagnitude);
                            }
                            else
                            {
                                Debug.WriteLine("magnitudeToAddOnScreen muu: " + magnitudeToAddOnScreen);
                                var rowMagnitude = new ListOfMagnitudes { defaultMagnitude = false, magnitude = m };
                                lstItemMagnitudes.Add(rowMagnitude);
                            }
                        }
                    }

                    if (lstItemMagnitudes.Any())
                    {
                        lblTransferOrderQuantityInsertQuantityUOM.IsVisible = false;
                        frmbtnTransferOrderQuantityInsertQuantityUOM.IsVisible = true;
                        btnTransferOrderQuantityInsertQuantityUOM.Text = !string.IsNullOrEmpty(previouslyReadMagnitude) ? previouslyReadMagnitude : magnitudeToAddOnScreen;
                    }
                    else
                    {
                        lblTransferOrderQuantityInsertQuantityUOM.IsVisible = true;
                        frmbtnTransferOrderQuantityInsertQuantityUOM.IsVisible = false;
                        lblTransferOrderQuantityInsertQuantityUOM.Text = !string.IsNullOrEmpty(previouslyReadMagnitude) ? previouslyReadMagnitude : magnitudeToAddOnScreen;
                    }




                    lstTransferOrderQuantityInsertQuantities = new List<ListOfTRFRCVToExport>();
                    var row = new ListOfTRFRCVToExport
                    {
                        initialQty = lstTransferOrderQuantityInsertInfo.First().initialQty,
                        pickedQty = previouslyReadQty,
                        remainingQty = (lstTransferOrderQuantityInsertInfo.First().initialQty - previouslyReadQty),
                        magnitude = previouslyReadQty == 0 ? "" : !string.IsNullOrEmpty(previouslyReadMagnitude) ? previouslyReadMagnitude : magnitudeToAddOnScreen,
                        barCode = lstTransferOrderQuantityInsertInfo.First().barCode
                    };

                    lstTransferOrderQuantityInsertQuantities.Add(row);
                    LstvTransferOrderQuantityInsertQuantityInfo.ItemsSource = null;
                   
                    LstvTransferOrderQuantityInsertQuantityInfo.ItemsSource = lstTransferOrderQuantityInsertQuantities;

                    if (previouslyReadQty > 0)
                    {
                        if (obj.showTransferReceiveQtySum)
                        {
                            entTransferOrderQuantityInsertQuantity.Text = "1";
                        }
                        else
                        {
                            entTransferOrderQuantityInsertQuantity.Text = (String.Format("{0:0.00}", previouslyReadQty)).Replace(".00", "");
                        }
                    }
                    else
                    {
                        entTransferOrderQuantityInsertQuantity.Text = "1";
                        //entTransferOrderQuantityInsertQuantity.Text = "";
                    }
                    entTransferOrderQuantityInsertQuantity.BackgroundColor = Color.Yellow;
                    ShowKeyBoard.Show(VirtualKeyboardTypes.VirtualKeyboardType.Numeric, this);
                }

                defaultvalueOverride = true;
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private async void btnTransferOrderQuantityInsertQuantityOK_Clicked(object sender, EventArgs e)
        {
            try
            {
                JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                bool proceed = true;
                decimal quantity = 0;

                if (IsTextAllowed(entTransferOrderQuantityInsertQuantity.Text))
                {
                    proceed = false;
                    DisplayFailMessage("SISESTA NUMBER!");
                }

                if (proceed)
                {
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
                        if (!obj.showTransferReceiveQtySum)
                        {
                            if (!await YesNoDialog("ÜLEVIIMISE TARNE", "SISESTATUD KOGUS ON 0. KAS JÄTKATA KIRJE TEKITAMISEGA?", false))
                            {
                                proceed = false;
                            }
                        }
                    }

                    if (proceed)
                    {
                        if (obj.showTransferReceiveQtySum)
                        {
                            int lastRecordID = 0;
                            if (lstTransferOrderPickedQuantities.Any())
                            {
                                var previousRead = lstTransferOrderPickedQuantities.Where(x => x.docNo == lstTransferOrderQuantityInsertInfo.First().docNo && x.docLineNo == lstTransferOrderQuantityInsertInfo.First().docLineNo && x.shop == obj.shopLocationID);
                                if (previousRead.Any())
                                {
                                    lastRecordID = previousRead.First().recordID;
                                    var record = lstTransferOrderPickedQuantities.Where(x => x.recordID == transferReceiveRecordID);
                                    if (record.Any())
                                    {
                                        record.First().pickedQty = record.First().pickedQty + quantity;
                                        record.First().magnitude = lblTransferOrderQuantityInsertQuantityUOM.IsVisible ? lblTransferOrderQuantityInsertQuantityUOM.Text : btnTransferOrderQuantityInsertQuantityUOM.Text;
                                        record.First().recordDate = DateTime.Now;
                                        string data1 = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                        var writeTransferOrderPickedQuantitiesDbToFile1 = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data1, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                        if (writeTransferOrderPickedQuantitiesDbToFile1.Item1)
                                        {
                                            DisplaySuccessMessage("SALVESTATUD!");
                                            ShowKeyBoard.Hide(this);
                                            proceed = false;
                                            PrepareTransferReceiveOrderLines();
                                        }
                                        else
                                        {
                                            DisplayFailMessage(writeTransferOrderPickedQuantitiesDbToFile1.Item2);
                                        }
                                    }
                                }
                                else
                                {
                                    lastRecordID = lstTransferOrderPickedQuantities.OrderByDescending(x => x.recordID).Take(1).First().recordID;
                                }
                            }
                            if (proceed)
                            {

                                lstTransferOrderPickedQuantities.Add(new ListOfTRFRCVToExport
                                {
                                    docNo = lstTransferOrderQuantityInsertInfo.First().docNo,
                                    docLineNo = lstTransferOrderQuantityInsertInfo.First().docLineNo,
                                    initialQty = lstTransferOrderQuantityInsertInfo.First().initialQty,
                                    pickedQty = lstTransferOrderQuantityInsertInfo.First().pickedQty + quantity,
                                    recordDate = DateTime.Now,
                                    shop = lstTransferOrderQuantityInsertInfo.First().shop,
                                    itemCode = lstTransferOrderQuantityInsertInfo.First().itemCode,
                                    barCode = lstTransferOrderQuantityInsertInfo.First().barCode,
                                    magnitude = lblTransferOrderQuantityInsertQuantityUOM.IsVisible ? lblTransferOrderQuantityInsertQuantityUOM.Text : btnTransferOrderQuantityInsertQuantityUOM.Text,
                                    recordID = lastRecordID + 1
                                });

                                //JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                string data = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                var writeTransferOrderPickedQuantitiesDbToFile = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                if (writeTransferOrderPickedQuantitiesDbToFile.Item1)
                                {

                                    DisplaySuccessMessage("SALVESTATUD!");
                                    ShowKeyBoard.Hide(this);
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
                            if (quantity > -1)
                            {
                                int lastRecordID = 0;
                                if (lstTransferOrderPickedQuantities.Any())
                                {
                                    var previousRead = lstTransferOrderPickedQuantities.Where(x => x.docNo == lstTransferOrderQuantityInsertInfo.First().docNo && x.docLineNo == lstTransferOrderQuantityInsertInfo.First().docLineNo && x.shop == obj.shopLocationID);
                                    if (previousRead.Any())
                                    {
                                        lastRecordID = previousRead.First().recordID;
                                        var record = lstTransferOrderPickedQuantities.Where(x => x.recordID == transferReceiveRecordID);
                                        if (record.Any())
                                        {
                                            if (quantity == 0)
                                            {
                                                if (record.Any())
                                                {
                                                    lstTransferOrderPickedQuantities.Remove(record.Take(1).First());
                                                    string data = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                                    var writeTransferOrderPickedQuantitiesDbToFile = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                                    if (writeTransferOrderPickedQuantitiesDbToFile.Item1)
                                                    {
                                                        transferReceiveRecordID = 0;
                                                        DisplaySuccessMessage("SALVESTATUD!");
                                                        ShowKeyBoard.Hide(this);
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
                                                record.First().pickedQty = quantity;
                                                record.First().magnitude = lblTransferOrderQuantityInsertQuantityUOM.IsVisible ? lblTransferOrderQuantityInsertQuantityUOM.Text : lblTransferOrderQuantityInsertQuantityUOM.Text;
                                                record.First().recordDate = DateTime.Now;
                                                string data1 = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                                var writeTransferOrderPickedQuantitiesDbToFile1 = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data1, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                                if (writeTransferOrderPickedQuantitiesDbToFile1.Item1)
                                                {
                                                    DisplaySuccessMessage("SALVESTATUD!");
                                                    ShowKeyBoard.Hide(this);
                                                    proceed = false;
                                                    PrepareTransferReceiveOrderLines();
                                                }
                                                else
                                                {
                                                    DisplayFailMessage(writeTransferOrderPickedQuantitiesDbToFile1.Item2);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lastRecordID = lstTransferOrderPickedQuantities.OrderByDescending(x => x.recordID).Take(1).First().recordID;
                                    }
                                }
                                if (proceed)
                                {
                                    if (quantity == 0)
                                    {

                                    }
                                    else
                                    {
                                        lstTransferOrderPickedQuantities.Add(new ListOfTRFRCVToExport
                                        {
                                            docNo = lstTransferOrderQuantityInsertInfo.First().docNo,
                                            docLineNo = lstTransferOrderQuantityInsertInfo.First().docLineNo,
                                            initialQty = lstTransferOrderQuantityInsertInfo.First().initialQty,
                                            pickedQty = quantity,
                                            recordDate = DateTime.Now,
                                            shop = lstTransferOrderQuantityInsertInfo.First().shop,
                                            itemCode = lstTransferOrderQuantityInsertInfo.First().itemCode,
                                            barCode = lstTransferOrderQuantityInsertInfo.First().barCode,
                                            magnitude = lblTransferOrderQuantityInsertQuantityUOM.IsVisible ? lblTransferOrderQuantityInsertQuantityUOM.Text : btnTransferOrderQuantityInsertQuantityUOM.Text,
                                            recordID = lastRecordID + 1
                                        });

                                        //JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                        string data = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                        var writeTransferOrderPickedQuantitiesDbToFile = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                        if (writeTransferOrderPickedQuantitiesDbToFile.Item1)
                                        {

                                            DisplaySuccessMessage("SALVESTATUD!");
                                            ShowKeyBoard.Hide(this);
                                            PrepareTransferReceiveOrderLines();
                                        }
                                        else
                                        {
                                            DisplayFailMessage(writeTransferOrderPickedQuantitiesDbToFile.Item2);
                                        }
                                    }
                                }



                                //Debug.WriteLine("X2");
                                //if (transferReceiveRecordID == 0)
                                //{
                                //    int lastRecordID = 0;
                                //    if (lstTransferOrderPickedQuantities.Any())
                                //    {
                                //        lastRecordID = lstTransferOrderPickedQuantities.OrderByDescending(x => x.recordID).Take(1).First().recordID;
                                //    }
                                //    lstTransferOrderPickedQuantities.Add(new ListOfTRFRCVToExport
                                //    {
                                //        docNo = lstTransferOrderQuantityInsertInfo.First().docNo,
                                //        docLineNo = lstTransferOrderQuantityInsertInfo.First().docLineNo,
                                //        initialQty = lstTransferOrderQuantityInsertInfo.First().initialQty,
                                //        pickedQty = quantity,
                                //        recordDate = DateTime.Now,
                                //        shop = lstTransferOrderQuantityInsertInfo.First().shop,
                                //        recordID = lastRecordID + 1,
                                //        barCode = lstTransferOrderQuantityInsertInfo.First().barCode,
                                //        itemCode = lstTransferOrderQuantityInsertInfo.First().itemCode,
                                //        magnitude = lblTransferOrderQuantityInsertQuantityUOM.IsVisible ? lblTransferOrderQuantityInsertQuantityUOM.Text : btnTransferOrderQuantityInsertQuantityUOM.Text,
                                //    });

                                //    JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                //    string data = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                //    var writeTransferOrderPickedQuantitiesDbToFile = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                //    if (writeTransferOrderPickedQuantitiesDbToFile.Item1)
                                //    {
                                //        DisplaySuccessMessage("SALVESTATUD!");
                                //        ShowKeyBoard.Hide(this);
                                //        PrepareTransferReceiveOrderLines();
                                //    }
                                //    else
                                //    {
                                //        DisplayFailMessage(writeTransferOrderPickedQuantitiesDbToFile.Item2);
                                //    }
                                //}
                                //else
                                //{
                                //    if (quantity == 0)
                                //    {
                                //        var record = lstTransferOrderPickedQuantities.Where(x => x.recordID == transferReceiveRecordID);
                                //        if (record.Any())
                                //        {
                                //            lstTransferOrderPickedQuantities.Remove(record.Take(1).First());
                                //            JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                //            string data = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                //            var writeTransferOrderPickedQuantitiesDbToFile = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                //            if (writeTransferOrderPickedQuantitiesDbToFile.Item1)
                                //            {
                                //                transferReceiveRecordID = 0;
                                //                DisplaySuccessMessage("SALVESTATUD!");
                                //                ShowKeyBoard.Hide(this);
                                //                PrepareTransferReceiveOrderLines();
                                //            }
                                //            else
                                //            {
                                //                DisplayFailMessage(writeTransferOrderPickedQuantitiesDbToFile.Item2);
                                //            }
                                //        }
                                //    }
                                //    else
                                //    {
                                //        Debug.WriteLine("X3 " + transferReceiveRecordID);
                                //        Debug.WriteLine("AA " + lstTransferOrderPickedQuantities.First().recordID);
                                //        var record = lstTransferOrderPickedQuantities.Where(x => x.recordID == transferReceiveRecordID);
                                //        if (record.Any())
                                //        {
                                //            record.First().pickedQty = quantity;
                                //            record.First().magnitude = lblTransferOrderQuantityInsertQuantityUOM.IsVisible ? lblTransferOrderQuantityInsertQuantityUOM.Text : btnTransferOrderQuantityInsertQuantityUOM.Text;
                                //            record.First().recordDate = DateTime.Now;

                                //            JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };
                                //            string data = JsonConvert.SerializeObject(lstTransferOrderPickedQuantities, jSONsettings);

                                //            var writeTransferOrderPickedQuantitiesDbToFile = await WriteTransferOrderPickedQuantitiesRecords.Write(this, data, obj.shopLocationID ?? "SHOPID-PUUDUB?", obj.deviceSerial ?? "DEVICEID-PUUDUB?");
                                //            if (writeTransferOrderPickedQuantitiesDbToFile.Item1)
                                //            {
                                //                DisplaySuccessMessage("SALVESTATUD!");
                                //                ShowKeyBoard.Hide(this);
                                //                PrepareTransferReceiveOrderLines();
                                //            }
                                //            else
                                //            {
                                //                DisplayFailMessage(writeTransferOrderPickedQuantitiesDbToFile.Item2);
                                //            }
                                //        }
                                //        //}
                                //    }
                                //}
                            }
                            else
                            {
                                DisplayFailMessage("KOGUS PEAB OLEMA NUMBER!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }

        private void btnTransferOrderQuantityInsertQuantityUOM_Clicked(object sender, EventArgs e)
        {
            obj.previousLayoutName = "TransferOrderQuantityInsert";
            PrepareSelectMagnitude();
        }

        #endregion

        #region stkSelectMagnitude

        public void PrepareSelectMagnitude()
        {
            try
            {
                CollapseAllStackPanels.Collapse(this);
                stkSelectMagnitude.IsVisible = true;
                obj.mainOperation = "";
                obj.currentLayoutName = "SelectMagnitude";
                lblSelectMagnitudeHeader.Text = "KAUBA MÕÕTÜHIKUD";

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
                LstvSelectMagnitude.ItemsSource = null;
               
                LstvSelectMagnitude.ItemsSource = lstItemMagnitudes;
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }


        private void LstvSelectMagnitude_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as ListOfMagnitudes;
                if (obj.previousLayoutName == "PurchaseOrderQuantityInsert")
                {
                    btnPurchaseOrderQuantityInsertQuantityUOM.Text = item.magnitude;
                }
                if (obj.previousLayoutName == "TransferOrderQuantityInsert")
                {
                    btnTransferOrderQuantityInsertQuantityUOM.Text = item.magnitude;
                }
                DisplaySuccessMessage(item.magnitude + " VALITUD!");
                BackKeyPress.Press(this);
            }
            catch (Exception ex)
            {
                WriteLog.Write(this, this.GetType().Name + "\r\n" + ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.ToString() : null));
            }
        }


        #endregion
    }
}
    