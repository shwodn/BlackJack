using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Dealer
    {
        private List<Deck.DeckInfo> dealerCardOnHand;

        public List<Deck.DeckInfo> DealerCardOnHand { get { return dealerCardOnHand; } private set { dealerCardOnHand = value; } }

        public Dealer()
        {
            dealerCardOnHand = new List<Deck.DeckInfo>();
        }

        public void DealerDraw(Deck inputDeck)
        {
            dealerCardOnHand.Add(inputDeck.DrawCard());
        }

        public void PrintDealerCard()
        {
            foreach (var input in dealerCardOnHand)
            {
                Console.WriteLine($"{input}({(int)input % 13})");
            }
        }
    }
}
