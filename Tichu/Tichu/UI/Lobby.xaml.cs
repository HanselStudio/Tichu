using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TichOnConsole;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tichu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lobby : ContentPage
    {

        public string DisplayName { get; set; }
        bool status = false;
        Server server;
        Client client;
        bool ready = false;

        public Lobby(string name, string ip)
        {
            InitializeComponent();

            if (name == "host")
            {

                StatusLabel.Text = "Your the Host";
                server = new Server(this);
                server.start();
                Console.WriteLine(Dns.GetHostName());


                try { 
                IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (IPAddress addr in ips)
                {
                    if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        Console.WriteLine(addr);
                        IpLabel.Text = addr.ToString();
                        QRCode.BarcodeValue = addr.ToString();
                        }
                }
                }
                catch
                {
                    ipAddressIos();
                }


            }

            else {
                playButton.Text = "Ready UP!";

                QRCode.IsVisible = !QRCode.IsVisible;


                client = new Client(name, ip,this);
                if (status) StatusLabel.Text = "Connected";
            }

            
         }

        public Lobby(Server s)
        {
            InitializeComponent();
                StatusLabel.Text = "Your the Host";
            server = s;
            server.lob = this;


            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (IPAddress addr in ips)
                {
                    if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        Console.WriteLine(addr);
                        IpLabel.Text = addr.ToString();
                        QRCode.BarcodeValue = addr.ToString();
                    }
                }
            }
            catch
            {
                ipAddressIos();
            }


        }

        private void ipAddressIos() {
            
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            var ipAddress = addrInfo.Address;
                            IpLabel.Text += ipAddress.ToString();
                            QRCode.BarcodeValue = ipAddress.ToString();
                            return;


                        }
                    }
                }
            }
        }


        public void changeLable(string s) {
            PlayerLabel1.Text = s;
        }
        public void changeStatusLable(string s)
        {
            StatusLabel.Text = s;
        }

        public void changeStatus(bool conneced) {
            status = conneced;
            if (!conneced) StatusLabel.Text = "Not Connected";
            else StatusLabel.Text = "Connected";

        }

        public void updateAsHost() {

            StatusLabel.Text = "Connected";
            
            PlayerLabel1.Text = server.game.players[0].name;
            PlayerLabel2.Text = server.game.players[1].name;
            PlayerLabel3.Text = server.game.players[2].name;
            PlayerLabel4.Text = server.game.players[3].name;
            
        }
        public void updateAsClient()
        {
            PlayerLabel1.Text = client.names[0];
            PlayerLabel2.Text = client.names[1];
            PlayerLabel3.Text = client.names[2];
            PlayerLabel4.Text = client.names[3];

            for(int i = 0; i < client.readyStatus.Length; i++)
            {
                Label lab = FindByName("PlayerLabel" + (i + 1).ToString()) as Label;

                if (client.readyStatus[i]) lab.BackgroundColor = new Color(0, 255, 0);
                else lab.BackgroundColor = new Color(255, 0, 0);

            }

            for (int i = 0; i < client.ping.Length; i++)
            {
                Label lab = FindByName("Ping" + (i + 1).ToString()) as Label;

                lab.Text = client.ping[i].ToString();

                if(client.ping[i] < 100)
                {
                    lab.TextColor = new Color(0, 255, 0);
                }
                else if(client.ping[i] < 200)
                {
                    lab.TextColor = new Color(255, 167, 0);
                }
                else lab.TextColor = new Color(255, 0, 0);


            }

            for (int i = 0; i < client.teams.Length; i++)
            {
                Button but = FindByName("P" + (i + 1) + "T") as Button;

                but.Text = client.teams[i].ToString();
            } 
        
        }

        public void start(object sender, EventArgs args)
        {
            

            
            if (server != null)
            {

               // server.sendPlayerInfo();

                //server.checkClients();

                
                if (server.allPlayerReady())
                {



                    Navigation.PushAsync(new GamePage(server));
                    
                    Navigation.RemovePage(this);
                }
                else playButton.Text = "Not all players are ready!";
                
            }
            else if (ready) {
                playButton.BackgroundColor = new Color(255,0, 0);
                playButton.Text = "Ready UP!";
                ready = false;
                client.client.Send(Encoding.Unicode.GetBytes("NotReady!" + client.name));

            }
            else
            {
                playButton.BackgroundColor = new Color(0, 255, 0);
                ready = true;
                playButton.Text = "You are Ready!";
                client.client.Send(Encoding.Unicode.GetBytes("Ready!" + client.name));

            }
            

        }

        public void startAsClient() {
            

            PlayerLabel2.Text = "in 1 sec";
            ClientPage gp = new ClientPage(client);
            Navigation.PushAsync(gp);

            Navigation.RemovePage(this);
        }

        public void leave(object sender, EventArgs args)
        {

            if (server != null)
            {
                for (int i = 0; i < 4; i++) try { server.game.players[i].client.Disconnect(); } catch { }
                server.server.Stop();

            }
            else {
                client.close();
                
                

            }
            Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);

        }

        public IPAddress GetIPAddress(string hostName)
        {
            Ping ping = new Ping();
            var replay = ping.Send(hostName);

            if (replay.Status == IPStatus.Success)
            {
                return replay.Address;
            }
            return null;
        }

        public async Task OnAlertReconnected()
        {
            await DisplayAlert("Player Reconnected", "RESUME?", "Play");

        }

    }
}