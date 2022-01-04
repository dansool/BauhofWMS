﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BauhofWMS.StackPanelOperations
{
    public class BackKeyPress
    {
        private App obj = App.Current as App;
        public async void Press(MainPage mp)
        {
            Debug.WriteLine("   BackKeyPress obj.mainOperation  " + obj.mainOperation);
            Debug.WriteLine("   BackKeyPress obj.previousLayoutName " + obj.previousLayoutName);
            Debug.WriteLine("   BackKeyPress obj.currentLayoutName  " + obj.currentLayoutName);
            Debug.WriteLine("   BackKeyPress obj.nextLayoutName     " + obj.nextLayoutName);
            Debug.WriteLine("   BackKeyPress obj.deviceSerial     " + obj.deviceSerial);

            mp.ShowKeyBoard.Hide(mp);

            switch (obj.currentLayoutName)
            {
                case "Operations":
                    {
                        //mp.PreparePassword();
                    }
                    break;
                case "Settings":
                    {
                        mp.PrepareOperations();
                    }
                    break;
                case "Password":
                    {
                        mp.PrepareOperations();
                    }
                    break;
                case "StockTake":
                    {
                        mp.PrepareOperations();
                    }
                    break;
                case "Transfer":
                    {
                        mp.PrepareOperations();
                    }
                    break;
                case "SelectItem":
                    {
                        if (obj.previousLayoutName == "StockTake")
                        {
                            obj.previousLayoutName = "";
                            obj.currentLayoutName = "StockTake";
                            mp.CollapseAllStackPanels.Collapse(mp);
                            mp.stkStockTake.IsVisible = true;
                        }
                        if (obj.previousLayoutName == "Transfer")
                        {
                            obj.previousLayoutName = "";
                            obj.currentLayoutName = "Transfer";
                            
                            mp.CollapseAllStackPanels.Collapse(mp);
                            mp.stkTransfer.IsVisible = true;

                        }
                    }
                    break;
                case "ItemInfo":
                    {
                        mp.PrepareOperations();
                    }
                    break;
                case "StockTakeAddedRowsView":
                    {
                        obj.previousLayoutName = "";
                        obj.currentLayoutName = "StockTake";
                        mp.CollapseAllStackPanels.Collapse(mp);
                        mp.stkStockTake.IsVisible = true;
                    }
                    break;
                case "TransferAddedRowsView":
                    {
                        obj.previousLayoutName = "";
                        obj.currentLayoutName = "Transfer";
                        mp.CollapseAllStackPanels.Collapse(mp);
                        mp.stkTransfer.IsVisible = true;
                    }
                    break;
                case "ItemInfoBinsView":
                    {
                        obj.previousLayoutName = null;
                        obj.currentLayoutName = "ItemInfo";
                        mp.CollapseAllStackPanels.Collapse(mp);
                        mp.stkItemInfo.IsVisible = true;
                    }
                    break;
                    
            }
        }
    }
}
