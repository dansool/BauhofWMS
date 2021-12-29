using System;
using System.Collections.Generic;
using System.Text;

namespace BauhofWMS.StackPanelOperations
{
    public class CollapseAllStackPanels
    {
        public void Collapse(BauhofWMS.MainPage mp)
        {
            //mp.ShowKeyBoard.Hide(mp);
            mp.stkUpdate.IsVisible = false;
            mp.stkPassword.IsVisible = false;
            mp.stkOperations.IsVisible = false;
            mp.stkSettings.IsVisible = false;
            mp.stkStockTake.IsVisible = false;
            mp.stkTransfer.IsVisible = false;
            mp.stkSelectItem.IsVisible = false;
            mp.stkItemInfo.IsVisible = false;
            mp.stkStockTakeAddedRowsView.IsVisible = false;
            mp.stkTransferAddedRowsView.IsVisible = false;
            mp.stkItemInfoBinsView.IsVisible = false;
        }
    }
}
