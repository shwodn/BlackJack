using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Dealer : Character , IDraw
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

        public void Draw(Deck inputDeck)
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
            Console.SetCursorPosition(80, 10);
            Console.WriteLine("                 ");
            Console.SetCursorPosition(80, 10);
            Console.WriteLine($"{dealerCardOnHand[0]}({temp})");
            if(dealerCardOnHand.Count == 2)
            {
                Console.SetCursorPosition(80, 11);
                Console.WriteLine("                 ");
                Console.SetCursorPosition(80, 11);
                Console.WriteLine("???");
            }
            

        }

        public void PrintDealerCard(Deck inputDeck)
        {
            int cardCount = 0;
            int temp = 0;
            foreach (var input in dealerCardOnHand)
            {
                temp = inputDeck.ConvertToNumber(input, dealerCardState);
                Console.SetCursorPosition(80, 10 + cardCount);
                Console.WriteLine("                 ");
                Console.SetCursorPosition(80, 10 + cardCount);
                Console.WriteLine($"{input}({temp}) ");
                cardCount++;
            }
        }

        public void StartDealerTurn(Deck inputDeck)
        {
            PrintDealerCard(inputDeck);

            if (inputDeck.CalCard(dealerCardOnHand, dealerCardState) < 17)
            {
                Draw(inputDeck);
                StartDealerTurn(inputDeck);
            }
        }
    }
}
