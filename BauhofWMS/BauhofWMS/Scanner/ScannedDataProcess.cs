using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using BauhofWMSDLL.ListDefinitions;
using BauhofWMS.Keyboard;
using Xamarin.Forms;
using BauhofWMS.Utils;

namespace BauhofWMS.Scanner
{
    public class ScannedDataProcess
    {
        private App obj = App.Current as App;
       
        public async Task<bool> DataReceived(string scannedCode, string scannedSymbology, MainPage mp)
        {
            try
            {
                Debug.WriteLine("============================");
                Debug.WriteLine("   DataReceived obj.mainOperation  " + obj.mainOperation);
                Debug.WriteLine("   DataReceived obj.previousLayoutName " + obj.previousLayoutName);
                Debug.WriteLine("   DataReceived obj.currentLayoutName  " + obj.currentLayoutName);
                Debug.WriteLine("   DataReceived obj.nextLayoutName     " + obj.nextLayoutName);
                Debug.WriteLine("   scannedValue                        " + scannedCode);
                Debug.WriteLine("   scannedSymbology                    " + scannedSymbology);
                Debug.WriteLine("============================");


                if (scannedSymbology == null)
                {
                    scannedSymbology = "";
                }
                switch (obj.currentLayoutName)
                {
                    case "Password":
                        {
                            mp.entPassword.Text = "";
                            for (int i = 0; i < scannedCode.Length; i++)
                            {
                                mp.entPassword.Text = mp.entPassword.Text + "*";
                            }
                            Debug.WriteLine(scannedCode + " " + obj.deviceSerial + " " + obj.pEnv);
                            if (scannedCode == "5555")
                            {
                                obj.pin = scannedCode;
                                mp.entPassword.Text = "";
                                mp.PrepareSettings();
                            }
                            else
                            {
                                mp.entPassword.Text = "";
                                mp.DisplayFailMessage("VALE PIN!");
                            }
                        }
                        break;
                    case "Settings":
                        {
                            mp.ShowKeyBoard.Hide(mp);
                            mp.ediAddress.Text = scannedCode.StartsWith("\"") ? scannedCode.Replace("\"", "") : mp.ediAddress.Text;
                            mp.lblScanTestSymbologyData.Text = !string.IsNullOrEmpty(scannedSymbology) ? scannedSymbology : "unknown";
                            mp.lblScanTestData.Text = scannedCode;
                        }
                        break;
                    case "StockTake":
                        {
                            Debug.WriteLine("UPDATE LAYOUT");
                            mp.ShowKeyBoard.Hide(mp);
                            
                            mp.entStockTakeReadCode.Text = "";
                            mp.entStockTakeInternalCode.Text = "";
                            mp.entStockTakeQuantityQuantity.Text = "";
                            mp.entStockTakeBarCode.Text = "";
                            mp.lblItemDesc.Text = "";
                            var itemExists = mp.lstInternalRecordDB.Where(x => x.barCode == scannedCode);
                            if (itemExists.Any())
                            {
                                mp.DisplaySuccessMessage(scannedCode + "\r\n" +
                                    itemExists.First().itemCode + "\r\n" +
                                    itemExists.First().itemDesc
                                    );
                                mp.entStockTakeBarCode.Text = scannedCode;
                                mp.entStockTakeReadCode.Text = itemExists.First().itemCode;
                                mp.lblItemDesc.Text  = itemExists.First().itemDesc;
                            }
                            else
                            {
                               
                                mp.DisplayFailMessage("SKÄNNITUD " + scannedCode + " EI LEITUD REGISTRIST!");
                            }
                        }
                        break;
                        


                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.GetType().Name, "DataReceived", ex);
            }
            return true;
        }
    }
}
