using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Player
    {
        private int currentChip;
        private int bettedChip;
        private int input = 0;
        private Game.CardState playerState;
        private List<Deck.DeckInfo> playerCardOnHand;

        public int CurrentChip { get { return currentChip; } set { currentChip = value; } }
        public int BettedChip { get { return bettedChip; } private set { bettedChip = value; } }

        public int Input { get { return input; } set { input = value; } }

        public Game.CardState PlayerState { get { return playerState; } }
        

        public List<Deck.DeckInfo> PlayerCardOnHand { get { return playerCardOnHand; } private set { playerCardOnHand = value; } }

        public Player()
        {
            currentChip = 50;
            bettedChip = 0;
            playerState = Game.CardState.Null;
            playerCardOnHand = new List<Deck.DeckInfo>();
        }

        public void Betting() 
        {
            
            Console.WriteLine("배팅할 칩의 개수를 정해주세요.");
            if(int.TryParse(Console.ReadLine(), out bettedChip) != true)
            {
                Betting();
            }

            if (bettedChip > currentChip)
            {
                Console.WriteLine("현재 보유한 칩이 부족합니다. \n올인하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
                switch (input)
                {
                    case 1:
                        bettedChip = currentChip;
                        break;
                    case 2:
                        Betting();
                        break;
                }
            }

            if(bettedChip == currentChip)
            {
                Console.WriteLine("현재 배팅한 칩이 보유한 칩의 개수와 같습니다. \n올인하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
                switch (input)
                {
                    case 1:
                        bettedChip = currentChip;
                        break;
                    case 2:
                        Betting();
                        break;
                }
            }

            Console.WriteLine($"현재 배팅한 칩의 개수 : {bettedChip}개");


        }

        public void PlayerDraw(Deck inputDeck)
        {
            playerCardOnHand.Add(inputDeck.DrawCard());
        }

        public void PlayerAdd(Deck inputDeck, Deck.DeckInfo inputCard)
        {
            PlayerCardOnHand.Add(inputCard);
        }

        public void PrintPlayerCard(Deck inputDeck)
        {
            int temp = 0;
            foreach( var input in playerCardOnHand )
            {
                temp = inputDeck.ConvertToNumber(input, playerState);
                Console.WriteLine($"{input}({temp})");
            }
        }
    }
}
