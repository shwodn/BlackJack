using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Dealer
    {
        private Game.CardState dealerState;
        private List<Deck.DeckInfo> dealerCardOnHand;

        public Game.CardState DealerState {  get { return dealerState; } }
        public List<Deck.DeckInfo> DealerCardOnHand { get { return dealerCardOnHand; } private set { dealerCardOnHand = value; } }

        public Dealer()
        {
            dealerCardOnHand = new List<Deck.DeckInfo>();
            dealerState = Game.CardState.Null;
        }

        public void DealerDraw(Deck inputDeck)
        {
            dealerCardOnHand.Add(inputDeck.DrawCard());
        }

        public void PrintDealerCard(Deck inputDeck)
        {
            int temp = 0;
            foreach (var input in dealerCardOnHand)
            {
                temp = inputDeck.ConvertToNumber(input, dealerState);
                Console.WriteLine($"{input}({temp})");
            }
        }
    }
}
