using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tichu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        public Server server;


        public GamePage(Server serv)
        {
            InitializeComponent();

            server = serv;
            server.startGame(this);
            updatePoints();
        }
        public void exit()
        {
            Navigation.PushAsync(new Lobby(server));

            Navigation.RemovePage(this);
        }

        private void updatePoints() {

            T1P1.Text = server.game.players[0].name + ": " + server.game.players[0].numberOfCards + " Cards " + server.game.players[0].collectedPoints + "Points";
            T2P1.Text = server.game.players[1].name + ": " + server.game.players[1].numberOfCards + " Cards " + server.game.players[1].collectedPoints + "Points";
            T1P2.Text = server.game.players[2].name + ": " + server.game.players[2].numberOfCards + " Cards " + server.game.players[2].collectedPoints + "Points";
            T2P2.Text = server.game.players[3].name + ": " + server.game.players[3].numberOfCards + " Cards " + server.game.players[3].collectedPoints + "Points";

        }

        public void update()
        {
            updatePoints();
           // turnLable.Text = "";

            for(int i = 0; i< server.game.lastPlayedCards.Count; i++)
            {
                Image im = FindByName("Card" + i) as Image;

                im.Source = server.game.lastPlayedCards[server.game.lastPlayedCards.Count - 1 -i].name.ToString()+".png";
               // turnLable.Text += server.game.lastPlayedCards[server.game.lastPlayedCards.Count - 1 - i].name.ToString()+" ";
            }
        }

        public async Task<bool> OnAlertDisconnect()
        {
            return await DisplayAlert("A Player is Disconnected!", server.server.RemoteEndPoint.ToString() , "Exit Game","Wait for player to return");
            
        }
        public async Task OnAlertReconnected()
        {
            await DisplayAlert("Player Reconnected", "RESUME?", "Play");

        }
        public async Task OnAlertExit()
        {
             await DisplayAlert("EXIT GAME!!", "Player has not returned!", "Exit Game");

        }
    }
}