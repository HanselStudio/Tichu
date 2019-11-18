using ObooltNet;
using System;
using Xamarin;
using System.Threading.Tasks;

namespace Tichu
{
    public class Player
    {

        bool tichu, bigTichu = false;
        public int collectedPoints = 0;
        public int team = 0, numberOfCards = 0;
        public string name;
        public string ip;
        public long ping = 0;
        private Server parent; 

        // change to false wenn start
        public bool ready = true;
        public NetConnection client = new NetConnection();
        public Card[] cards = new Card[14];
        public int[] schupf;
        public bool schupfed = false;
        public bool answered = true;

        public Player(string Name, int Team,Server server) {
            name = Name;
            team = Team;
            parent = server;

            client.OnDisconnect += Client_OnDisconnect;
        }

        private void Client_OnDisconnect(object sender, NetConnection connection)
        {
            Client_OnDisconnectAsync();
        }

        private async Task Client_OnDisconnectAsync()
        {
            if (parent.game.started) {
                bool exit = await parent.gp.OnAlertDisconnect();

                //replay
                if (!exit&&await parent.checkConnectionsAsync(this))
                {
                    parent.gp.OnAlertReconnected();
                }
                else
                {
                    parent.exitGame();
                }
            }
            //close client
            else
            {
                parent.lob.OnAlertReconnected();
                client.Disconnect();
                ready = false;
                client = new NetConnection();
                numberOfCards = 0;
                ip = "";
                collectedPoints = 0;
                name = "";
                answered = true;
                schupfed = false;
                parent.lob.updateAsHost();
            }

        }

        void addPoints(int Points) {
            collectedPoints += Points;
        }

        public void giveCards(Card[] Cards) {
            for (int i = 0; i < cards.Length; i++) {
                cards[i] = Cards[schupf[i]];
            }
        }

        public void giveCard(Card Card) {

            cards[numberOfCards] = Card;
            numberOfCards++;

        }

        public Card[] getCards() {
            Card[] c = new Card[3];

            for (int i = 0; i < c.Length; i++) c[i] = cards[schupf[i]];

            return c;
        }


        void playCard(int PositionOfCards) {

            cards[PositionOfCards] = null;
            numberOfCards -= 1;

        }

        void playCards(int[] PositionsOfCards) {

            

            for (int i = 0; i < PositionsOfCards.Length; i++) {
                cards[PositionsOfCards[i]] = null;
            }

            numberOfCards -= PositionsOfCards.Length;

        }

        void changeCardPosition(int CurrentPosition, int NewPosition) {

            Card tempCard = cards[NewPosition];
            cards[NewPosition] = cards[CurrentPosition];
            cards[CurrentPosition] = tempCard;

        }
    }
}
