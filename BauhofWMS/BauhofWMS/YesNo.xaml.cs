using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BauhofWMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YesNo : ContentPage
    {
        private App obj = App.Current as App;
        public YesNo(string header, string message, bool exit)
        {
            InitializeComponent();

            lblHeader.Text = header;
            lblMessage.Text = message;
            BackgroundColor = Color.DarkSlateBlue;

            if (!obj.isDeviceHandheld)
            {
                stkYesNO.WidthRequest = 600;
                stkYesNO.HeightRequest = 800;
            }

            if (exit)
            {
                BackgroundColor = Color.DarkRed;
                btnYes.Text = "VÄLJU";
                btnYes.BackgroundColor = Color.Red;

                btnNo.Text = "TÜHISTA";
                btnNo.BackgroundColor = Color.Green;
            }
        }

        private async void btnYes_Pressed(object sender, EventArgs e)
        {
            obj.mp.YesNoResult = true;
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void btnNo_Pressed(object sender, EventArgs e)
        {
            obj.mp.YesNoResult = false;
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}