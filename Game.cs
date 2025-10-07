using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Game
    {
        // 메인 문 아래 작성한 기능도 있으니 참고 부탁드립니다

        static void Main(string[] args)
        {
            

            Player player = new Player();
            Dealer dealer = new Dealer();
            Deck usingDeck = new Deck();

            usingDeck.PrintDeck();

            Console.WriteLine();

            player.Betting();
            player.PlayerDraw(usingDeck);
            player.PlayerDraw(usingDeck);
            //player.PlayerAdd(usingDeck, Deck.DeckInfo.CloverAce);
            player.PrintPlayerCard(usingDeck);
            Console.WriteLine(usingDeck.CalCard(player.PlayerCardOnHand, player.PlayerCardState)); 
            player.StartPlayerTurn(usingDeck);

        }
        
        public enum CardState { Null = 0, Normal = 1, BlackJack, Bust }
        public enum RoundState { Playing = 0, PlayerWin, Push, PlayerLose}

        public static CardState UpdateCardState(int input)
        {
            if(input < 21)
            {
                return CardState.Normal;
            }
            else if(input == 21)
            {
                return CardState.BlackJack;
            }
            else
            {
                return CardState.Bust;
            }
        }

        // 예외 방지 매서드
        public static int PreventInputExceptions(int input)
        {
            bool isException = true;
            int temp = 0;

            while (isException)
            {
                if (int.TryParse(Console.ReadLine(), out temp))
                {
                    if(temp > 0 && temp < input + 1)
                    {
                        return temp;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }

            //예상 못한 입력일 경우 0 반환
            return 0;
        }
    }
}
