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

        public Game.CardState DealerCardState {  get { return dealerCardState; } }
        public List<Deck.DeckInfo> DealerCardOnHand { get { return dealerCardOnHand; } private set { dealerCardOnHand = value; } }

        public Dealer()
        {
            dealerCardOnHand = new List<Deck.DeckInfo>();
            dealerCardState = Game.CardState.Null;
        }

        public void ResetDealerCard()
        {
            dealerCardOnHand.Clear();
            dealerCardState = Game.CardState.Null;
        }

        public void DealerDraw(Deck inputDeck)
        {
            dealerCardOnHand.Add(inputDeck.DrawCard());
            if(dealerCardOnHand.Count < 3)
            {
                FirstPrintDealerCard(inputDeck);
                dealerCardState = Game.UpdateCardState(inputDeck.CalCard(dealerCardOnHand, dealerCardState));
            }
            else
            {
                PrintDealerCard(inputDeck);
                dealerCardState = Game.UpdateCardState(inputDeck.CalCard(dealerCardOnHand, dealerCardState));
            }
            
        }

        public void DealerAdd(Deck.DeckInfo inputCard)
        {
            dealerCardOnHand.Add(inputCard);
        }

        public void FirstPrintDealerCard(Deck inputDeck)
        {
            int temp = 0;

            temp = inputDeck.ConvertToNumber(dealerCardOnHand[0], dealerCardState);
            Console.WriteLine($"{dealerCardOnHand[0]}({temp})           ");
            Console.WriteLine("???          ");

        }

        public void PrintDealerCard(Deck inputDeck)
        {
            int temp = 0;
            foreach (var input in dealerCardOnHand)
            {
                temp = inputDeck.ConvertToNumber(input, dealerCardState);
                Console.WriteLine($"{input}({temp})         ");
            }
        }

        public void StartDealerTurn(Deck inputDeck)
        {
            PrintDealerCard(inputDeck);

            if (inputDeck.CalCard(dealerCardOnHand, dealerCardState) < 17)
            {
                DealerDraw(inputDeck);
                StartDealerTurn(inputDeck);
            }
        }
    }
}
