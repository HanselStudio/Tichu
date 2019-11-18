using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TichOnConsole;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Tichu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientPage : ContentPage
    {
        Client client;
        List<Card> cardList = new List<Card>();
        int maxCard = 14;
        int dis = 30;

        public ClientPage(Client cl)
        {
            InitializeComponent();

            client = cl;
            client.page = this;

            client.client.Send(Encoding.Unicode.GetBytes("GetCards!" + client.name));
            update();

        }

        public void update()
        {
            lable.Text = "";
            Name.Text = "";

            for (int i = 0 ; i < maxCard;i++)
            {
                ImageButton image = this.FindByName("c" + i) as ImageButton;
                
                image.Source = client.cards[i].name + ".png";
                lable.Text += client.cards[i].name + " ";


            }
        }

        public void imageClicked(object sender, EventArgs arg)
        {
            
            ImageButton b = sender as ImageButton;
            Card clickedCard = new Card(0);

            string s = b.Source.ToString();

            for(int i = 0; i < client.cards.Count; i++) { 
                if ("File: "+ client.cards[i].name+".png" == s) {
                    clickedCard = client.cards[i];
                     }
            } 
            if (cardList.Contains(clickedCard))
            {
                cardList.Remove(clickedCard);

                
                b.TranslationY += Math.Cos((2 * Math.PI * b.Rotation) / 360) * dis;
                b.TranslationX -= Math.Sin((2 * Math.PI * b.Rotation) / 360) * dis;
                

            }
            else
            {
                
                cardList.Add(clickedCard);

                
                b.TranslationY -= Math.Cos((2*Math.PI*b.Rotation)/360) * dis;
                b.TranslationX += Math.Sin((2 * Math.PI * b.Rotation) / 360) * dis;
                

            }
            
        }

        private void ImageButton_Pressed(object sender, EventArgs e)
        {
            ImageButton b = sender as ImageButton;

           show.Source = b.Source;

            
        }

        private void ImageButton_Released(object sender, EventArgs e)
        {
            show.Source = "";
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            List<ImageButton> iml = new List<ImageButton>();

            for(int i = 0; i < 14; i++)
            {
                iml.Add(FindByName("c" + i) as ImageButton);
            }

            string send = "Play!"+client.name+"!";

            foreach(Card c in cardList)
            {
                send += c.value + ",";
                client.cards.Remove(c);
                foreach (ImageButton im in iml)
                {
                    if("File: " + c.name + ".png" == im.Source.ToString()) { 
                    im.TranslationY += Math.Cos((2 * Math.PI * im.Rotation) / 360) * dis;
                    im.TranslationX -= Math.Sin((2 * Math.PI * im.Rotation) / 360) * dis;

                    }
                }


            }
            foreach (ImageButton im in iml)
            {
                im.Source = "";
            }
            update();
            await Task.Run(() => client.client.Send(Encoding.Unicode.GetBytes(send)));
            cardList = new List<Card>();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
           await Task.Run(() => client.client.Send(Encoding.Unicode.GetBytes("Pass!"+client.name)));
        }
    }
}