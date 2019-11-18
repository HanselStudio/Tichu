using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace Tichu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Scanner : ContentPage
    {
        public Scanner()
        {
            InitializeComponent();
        }

        private async void ZXingScannerView_OnOnScanResult(Result result)
        {
            zxing.IsAnalyzing = false;
            await DisplayAlert("Scanned Barcode", result.Text, "OK");
            await Navigation.PopAsync();
        }
        private void Overlay_OnFlashButtonClicked(Button sender, EventArgs e)
        {
            zxing.IsTorchOn = !zxing.IsTorchOn;
        }

    }
}