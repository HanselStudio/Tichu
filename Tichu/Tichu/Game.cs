using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Tichu
{
   public class Game
    {
        int round = 1;
        public int playersTurn = 0;
        public Player[] players = new Player[4];
        public int[] teamPoints = new int[2];
        public int numberOfPlayers = 0;
        public bool started = false;
        Card[] allCards = new Card[56];



        public List<Card> lastPlayedCards = new List<Card>();

        int wish_value = 0;
        bool wish_aktive = false;
        public Game(Server server) {

            players[0] = new Player("", 1,server);
            players[1] = new Player("", 2,server);
            players[2] = new Player("", 1,server);
            players[3] = new Player("", 2,server);


            int i = 0;
            for (; i < allCards.Length; i++) allCards[i] = new TichuCard(i + 2);
            
            RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
            allCards = allCards.OrderBy(x => GetNextInt32(rnd)).ToArray();

            
            giveCardes();
            printCards(players[0].cards);
        }
        static int GetNextInt32(RNGCryptoServiceProvider rnd)
        {
            byte[] randomInt = new byte[4];
            rnd.GetBytes(randomInt);
            return Convert.ToInt32(randomInt[0]);
        }
        void giveCardes() {
                for (int j = 0; j < 56 ; j+=4)
                {
                for (int i = 0; i < 4; i++) {
                    players[i].giveCard(allCards[j+i]);
                }
                }
        }

        public void start()
        {
            //schupfen();
            started = true;

        }
        public void schupfen() {
            while (!players[0].schupfed && !players[1].schupfed && !players[2].schupfed && !players[3].schupfed) {
            
            }
            Card[][] cards = new Card[4][];

            cards[0] = players[0].getCards();
            cards[1] = players[1].getCards();
            cards[2] = players[2].getCards();
            cards[3] = players[3].getCards();

            for(int i = 0; i < 4; i++)
            {
                Card[] c = new Card[3];

                c[0] = cards[(i + 1) % 4][0];
                c[1] = cards[(i + 2) % 4][1];
                c[2] = cards[(i + 3) % 4][2];

                players[i].giveCards(c);
            }


        } 
        public bool checkCombo(int player,Card c = null, Card[] cardArray = null) {
            if (c != null && lastPlayedCards.Count == 1)
            {   

                if (c.value < 53 &&  (c.value%14 > lastPlayedCards[0].value) && !wish_aktive) { return true; }
                if (c.value == 56) return true;
                if (c.value == 55 && lastPlayedCards[0].value != 56) return true;
            }
            return false;
        }
        void printCards(Card[] cards) {

            foreach(Card c in cards)
            {
                Console.Write(c.name + " ");
            }
            Console.WriteLine();
        }

    }
}
