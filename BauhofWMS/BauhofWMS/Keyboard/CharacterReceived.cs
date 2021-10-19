using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using BauhofWMS.StackPanelOperations;
using BauhofWMS.Scanner;

namespace BauhofWMS.Keyboard
{
    public class CharacterReceived
    {
        private App obj = App.Current as App;
        private BackKeyPress BackKeyPress = new BackKeyPress();
        private ScannedDataProcess ScannedDataProcess = new ScannedDataProcess();
        public async void Receive(string classID, string keyCode, MainPage mp)
        {
            try
            {
                switch (keyCode)
                {
                    //enter
                    case "13":
                        {
                            if (classID == null)
                            {
                                Debug.WriteLine("ENTER PRESSED! " + mp.currentScannedValue);
                                switch (mp.focusedEditor)
                                {
                                    case "entPassword":
                                        {
                                            mp.currentScannedValue = "";
                                            string pass = mp.passWord;
                                            mp.passWord = "";
                                            await ScannedDataProcess.DataReceived(pass, "", mp);
                                        }
                                        break;
                                    case "entStockTakeReadCode":
                                        {
                                            mp.SearchEntStockTakeReadCode();
                                        }
                                        break;
                                    case "entTransferReadCode":
                                        {
                                            mp.SearchEntTransferReadCode();
                                        }
                                        break;
                                    
                                    case "entSelectItemReadCode":
                                        {
                                            mp.SearchEntSelectItemReadCode();
                                        }
                                        break;
                                    case "entItemInfoReadCode":
                                        {
                                            mp.PrepareItemInfo(mp.entItemInfoReadCode.Text);
                                        }
                                        break;
                                    default:
                                        {
                                            string value = mp.currentScannedValue;
                                            mp.currentScannedValue = "";
                                        }
                                        break;

                                        

                                }
                                if (mp.focusedEditor == "entPassword")
                                {
                                    string scannedValue = mp.currentScannedValue;
                                    mp.currentScannedValue = "";
                                }
                                
                                mp.currentScannedValue = "";
                            }
                            break;
                        }
                    //backspace
                    case "8":
                        {
                            switch (mp.focusedEditor)
                            {
                                case "entPassword":
                                    {
                                        Entry focusedEditor = mp.FindByName<Entry>(mp.focusedEditor);
                                        mp.passWord = mp.passWord.Length > 0 ? mp.passWord.Remove(mp.passWord.Length - 1) : "";
                                        focusedEditor.Text = focusedEditor.Text.Length > 0 ? focusedEditor.Text.Remove(focusedEditor.Text.Length - 1) : "";
                                    }
                                    break;
                                case "ediAddress":
                                    {
                                        Editor focusedEditor = mp.FindByName<Editor>(mp.focusedEditor);
                                        mp.passWord = mp.passWord.Length > 0 ? mp.passWord.Remove(mp.passWord.Length - 1) : "";
                                        focusedEditor.Text = focusedEditor.Text.Length > 0 ? focusedEditor.Text.Remove(focusedEditor.Text.Length - 1) : "";
                                    }
                                    break;
                                case "shopLocationCode":
                                    {
                                        Editor focusedEditor = mp.FindByName<Editor>(mp.focusedEditor);                                        
                                        focusedEditor.Text = focusedEditor.Text.Length > 0 ? (focusedEditor.Text.Remove(focusedEditor.Text.Length - 1)).ToUpper() : "";
                                    }
                                    break;
                                default:
                                    {
                                        Entry focusedEditor = mp.FindByName<Entry>(mp.focusedEditor);
                                        focusedEditor.Text = focusedEditor.Text.Length > 0 ? focusedEditor.Text.Remove(focusedEditor.Text.Length - 1) : "";
                                    }
                                    break;
                            }
                            break;
                        }
                    //escape
                    case "27":
                        {
                            Debug.WriteLine("mp.focusedEditor: ESC pressed");
                            try
                            {
                                await Application.Current.MainPage.Navigation.PopModalAsync();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                                if (obj.currentLayoutName == "Operations")
                                {
                                }

                                BackKeyPress.Press(mp);
                            }
                        }
                        break;
                    default:
                        {
                            Debug.WriteLine("mp.focusedEditor Default: " + mp.focusedEditor + " pressed");
                            char receivedChar = Convert.ToChar(Convert.ToInt32(keyCode));
                            if (!string.IsNullOrEmpty(mp.focusedEditor))
                            {
                                switch (mp.focusedEditor)
                                {
                                    case "entPassword":
                                        {
                                            Entry focusedEditor = mp.FindByName<Entry>(mp.focusedEditor);
                                            mp.passWord = mp.passWord + receivedChar.ToString();
                                            focusedEditor.Text = focusedEditor.Text + "*";
                                        }
                                        break;
                                    case "ediAddress":
                                        {
                                            Editor focusedEditor = mp.FindByName<Editor>(mp.focusedEditor);
                                            focusedEditor.Text = (focusedEditor.Text + receivedChar.ToString());
                                        }
                                        break;
                                    case "shopLocationCode":
                                        {
                                            Editor focusedEditor = mp.FindByName<Editor>(mp.focusedEditor);
                                            focusedEditor.Text = (focusedEditor.Text + receivedChar.ToString()).ToUpper();
                                        }
                                        break;
                                    default:
                                        {
                                            Entry focusedEditor = mp.FindByName<Entry>(mp.focusedEditor);
                                            focusedEditor.Text = focusedEditor.Text + receivedChar.ToString();
                                        }
                                        break;
                                }
                            }
                            mp.currentScannedValue = mp.currentScannedValue + receivedChar.ToString();
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
