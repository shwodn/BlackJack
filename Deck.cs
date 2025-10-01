using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Deck
    {

        // public enum DeckInfo
        // {
        //     HeartAce = 1, Heart2, Heart3, Heart4, Heart5, Heart6, Heart7, Heart8, Heart9, Heart10, HeartJack, HeartQueen, HeartKing = 13,
        //     SpadeAce = 14, Spade2, Spade3, Spade4, Spade5, Spade6, Spade7, Spade8, Spade9, Spade10, SpadeJack, SpadeQueen, SpadeKing = 26,
        //     DiamondAce = 27, Diamond2, Diamond3, Diamond4, Diamond5, Diamond6, Diamond7, Diamond8, Diamond9, Diamond10, DiamondJack, DiamondQueenen, DiamondKing = 39,
        //     CloverAce = 40, Clover2, Clover3, Clover4, Clover5, Clover6, Clover7, Clover8, Clover9, Clover10, CloverJack, CloverQueen, CloverKing = 52
        // }
        private bool isUse = false;
        private List<int> currentDeck; 
        private Random deckRandom = new Random();

        public bool IsUse { get { return isUse; } set { isUse = value; } }

        public Deck()
        {
            currentDeck = new List<int>();
            resetDeck();
            isUse = true;
        }

        public void resetDeck()
        {
           for (int i = 1; i < 53; i++)
            {
                currentDeck.Add(i);
            }
           ShuffleDeck();
        }

        public void printDeck()
        {
            foreach (var deckInfo in currentDeck)
            {
                Console.Write(deckInfo + " ");
            }
        }

        // 피셔 에이츠 알고리즘 활용
        public void ShuffleDeck()
        {
            for(int i = currentDeck.Count - 1; i > 0; i--)
            {
                int temp = currentDeck[i];

                int j = deckRandom.Next(1, i + 1);
                currentDeck[i] = currentDeck[j];
                currentDeck[j] = temp;

            }
        }
    }
}
