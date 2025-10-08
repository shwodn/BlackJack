using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
                
namespace ConsoleProjectBlackJack
{
    internal class Deck
    {
        public enum DeckInfo
        {
             HeartAce = 1, Heart2, Heart3, Heart4, Heart5, Heart6, Heart7, Heart8, Heart9, Heart10, HeartJack, HeartQueen, HeartKing = 13,
            SpadeAce = 14, Spade2, Spade3, Spade4, Spade5, Spade6, Spade7, Spade8, Spade9, Spade10, SpadeJack, SpadeQueen, SpadeKing = 26,
            DiamondAce = 27, Diamond2, Diamond3, Diamond4, Diamond5, Diamond6, Diamond7, Diamond8, Diamond9, Diamond10, DiamondJack, DiamondQueen, DiamondKing = 39,
            CloverAce = 40, Clover2, Clover3, Clover4, Clover5, Clover6, Clover7, Clover8, Clover9, Clover10, CloverJack, CloverQueen, CloverKing = 52
        }

         private int temp;

         private List<int> currentDeck; 
         private List<int> usedCard;
         private Random deckRandom = new Random();

        public List<int> CurrentDeck {  get { return currentDeck; } set { currentDeck = value; } }


        public Deck()
        {
            currentDeck = new List<int>();
            usedCard = new List<int>();
            ResetDeck();

        }

        public void ResetDeck()
        {
            currentDeck.Clear();
            usedCard.Clear();
           for (int i = 1; i < 53; i++)
           {
                currentDeck.Add(i);
           }
           ShuffleDeck();
        }

        // 피셔 에이츠 알고리즘 활용
        public void ShuffleDeck()
        {

            for (int i = currentDeck.Count - 1; i > 0; i--)
            {
                temp = currentDeck[i];

                int j = deckRandom.Next(0, i + 1);
                currentDeck[i] = currentDeck[j];
                currentDeck[j] = temp;

            }

        }

        public void PrintDeck()
        {
            foreach (var deckInfo in currentDeck)
            {
                Console.Write(deckInfo + " ");
            }
        }

        public DeckInfo DrawCard()
        {
            temp = currentDeck[0];
            RemoveCard(temp);
            Thread.Sleep(1000);
            return (DeckInfo)temp;

        }

        public void RemoveCard(int input)
        {
            currentDeck.Remove(input);
            usedCard.Add(input);
        }


        public int CalCard(List<DeckInfo> inputCard, Game.CardState inputState)
        {
            int sum = 0;
            
            foreach(var input in inputCard)
            {
                sum += ConvertToNumber(input, inputState);
            }

            return sum;

        }
        public int ConvertToNumber(DeckInfo inputDeckInfo, Game.CardState inputState)
        {
            if((int) inputDeckInfo % 13 == 1)
            {
                if(inputState == Game.CardState.Bust)
                {
                    return 1;
                }
                else
                {
                    return 11;
                }
            }
            else if((int)inputDeckInfo % 13 > 10 && (int)inputDeckInfo % 13 < 13 || (int)inputDeckInfo % 13 == 0)
            {
                return 10;
            }
            else
            {
                return (int)inputDeckInfo % 13;
            }
        }
        
        
    }
}
