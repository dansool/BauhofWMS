using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms;

namespace BauhofWMS.Keyboard
{
    public class ShowKeyBoard
    {
        private App obj = App.Current as App;

        public void Show(VirtualKeyboardTypes.VirtualKeyboardType type, MainPage mp)
        {
            try
            {
                Debug.WriteLine("isDeviceHandheld " + obj.isDeviceHandheld + " focusedEditor: " + mp.focusedEditor);
                if (mp.focusedEditor == "entPassword" && obj.currentLayoutName != "Password")
                {

                }
                else
                {
                    if (obj.isDeviceHandheld)
                    {
                        mp.grdKeyBoards.IsVisible = true;

                        mp.stkKeyboardKeyboard.IsVisible = false;
                        mp.stkKeyboardNumeric.IsVisible = false;


                        if (obj.operatingSystem == "Android")
                        {
                            mp.grdKeyBoards.Margin = new Thickness(0, 360, 0, 0);
                            mp.grdKeyBoards.ScaleX = 1.045;
                            mp.grdKeyBoards.ScaleY = 1.0;
                        }
                        if (obj.operatingSystem == "UWP")
                        {
                            mp.grdKeyBoards.Margin = new Thickness(0, 264, 0, 0);
                            mp.grdKeyBoards.ScaleX = 1.03;
                            mp.grdKeyBoards.ScaleY = 1.0;
                        }

                        switch (type)
                        {
                            case VirtualKeyboardTypes.VirtualKeyboardType.Keyboard:
                                {
                                    Debug.WriteLine("type: Keyboard");
                                    mp.stkKeyboardKeyboard.IsVisible = true;
                                    break;
                                }
                            case VirtualKeyboardTypes.VirtualKeyboardType.KeyboardWithSwitch:
                                {
                                    mp.stkKeyboardKeyboard.IsVisible = true;
                                    mp.btnKeyboardNumericSwitchToKeyboard.IsVisible = true;
                                    mp.btnKeyboardNumericSwitchToNumpad.IsVisible = true;
                                    mp.btnMinus.IsVisible = false;
                                    mp.btnPlus.IsVisible = false;
                                    break;
                                }
                            case VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitch:
                                {
                                    Debug.WriteLine("type: NumericWithSwitch");
                                    mp.stkKeyboardNumeric.IsVisible = true;
                                    mp.btnKeyboardNumericSwitchToKeyboard.IsVisible = true;
                                    mp.btnKeyboardNumericSwitchToNumpad.IsVisible = true;
                                    mp.btnMinus.IsVisible = false;
                                    mp.btnPlus.IsVisible = false;
                                    break;
                                }
                            case VirtualKeyboardTypes.VirtualKeyboardType.Numeric:
                                {
                                    Debug.WriteLine("type: Numeric");
                                    mp.stkKeyboardNumeric.IsVisible = true;
                                    mp.btnKeyboardNumericSwitchToKeyboard.IsVisible = false;
                                    mp.btnKeyboardAdvNumericSwitchToNumpad.IsVisible = false;
                                    mp.btnKeyboardNumericSwitchToNumpad.IsVisible = false;
                                    mp.btnMinus.IsVisible = false;
                                    mp.btnPlus.IsVisible = false;
                                    break;
                                }
                            case VirtualKeyboardTypes.VirtualKeyboardType.NumericWithSwitchAndPlusMinus:
                                {
                                    Debug.WriteLine("type: NumericWithSwitchAndPlusMinus");
                                    mp.stkKeyboardNumeric.IsVisible = true;
                                    mp.btnKeyboardNumericSwitchToKeyboard.IsVisible = true;
                                    mp.btnKeyboardAdvNumericSwitchToNumpad.IsVisible = true;
                                    mp.btnKeyboardNumericSwitchToNumpad.IsVisible = true;
                                    mp.btnMinus.IsVisible = true;
                                    mp.btnPlus.IsVisible = true;
                                    break;
                                }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ShowKeyBoard " + ex.Message);
            }
        }

        public void Hide(MainPage mp)
        {
            mp.grdKeyBoards.IsVisible = false;
        }
    }
}

