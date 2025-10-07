using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Dealer
    {
        private Game.CardState dealerCardState;
        private List<Deck.DeckInfo> dealerCardOnHand;

        public Game.CardState DealerState {  get { return dealerCardState; } }
        public List<Deck.DeckInfo> DealerCardOnHand { get { return dealerCardOnHand; } private set { dealerCardOnHand = value; } }

        public Dealer()
        {
            dealerCardOnHand = new List<Deck.DeckInfo>();
            dealerCardState = Game.CardState.Null;
        }

        public void DealerDraw(Deck inputDeck)
        {
            dealerCardOnHand.Add(inputDeck.DrawCard());
            PrintDealerCard(inputDeck);
            dealerCardState = Game.UpdateCardState(inputDeck.CalCard( dealerCardOnHand, dealerCardState) );
        }

        public void PrintDealerCard(Deck inputDeck)
        {
            int temp = 0;
            foreach (var input in dealerCardOnHand)
            {
                temp = inputDeck.ConvertToNumber(input, dealerCardState);
                Console.WriteLine($"{input}({temp})");
            }
        }
    }
}
