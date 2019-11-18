using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile;

namespace Tichu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinGame : ContentPage
    {
        public JoinGame()
        {
            InitializeComponent();
        }


        private void StartLob(object sender, EventArgs args)
        {
            if (NameEditor.Text != "" && IpEditor.Text != "")
            {
                try { Navigation.PushAsync(new Lobby(NameEditor.Text, IpEditor.Text)); }
                catch { IpEditor.Text = "Can't find Server"; }
            }
        }
        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            

        
            var options = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                CameraResolutionSelector = HandleCameraResolutionSelectorDelegate
            };

            var scanner = new MobileBarcodeScanner();
        
            scanner.AutoFocus();
            scanner.FlashButtonText = "Flash";
            scanner.CancelButtonText = "Cancel";
            scanner.Torch(true);
            //call scan with options created above
            var result = await scanner.Scan(options);
            HandleScanResult(result);
        }

        void HandleScanResult(ZXing.Result result)
        {
            if (result != null && !string.IsNullOrEmpty(result.Text)) {
                IpEditor.Text = result.Text;
                if(NameEditor.Text == "") {
                    NameEditor.Text = "Player";
                }
                StartLob(null,null);
            }
        }

        CameraResolution HandleCameraResolutionSelectorDelegate(List<CameraResolution> availableResolutions)
        {
            //Don't know if this will ever be null or empty
            if (availableResolutions == null || availableResolutions.Count < 1)
                return new CameraResolution() { Width = 800, Height = 600 };

            //Debugging revealed that the last element in the list
            //expresses the highest resolution. This could probably be more thorough.
            return availableResolutions[availableResolutions.Count - 1];
        }
    }
}