using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tichu
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

         void  StartLob(object sender, EventArgs args)
        {
             Navigation.PushAsync(new Lobby("host","host"));

        }
         void StartJoin(object sender, EventArgs args)
        {
             Navigation.PushAsync(new JoinGame());

        }
    }
}
