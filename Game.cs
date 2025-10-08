using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Game
    {
        // 메인 문 아래 작성한 코드도 있으니 참고 부탁드립니다

        static void Main(string[] args)
        {
            

            Player player = new Player();
            Dealer dealer = new Dealer();
            Deck usingDeck = new Deck();
            RoundState roundState = new RoundState();
            roundState = RoundState.Playing;

            usingDeck.PrintDeck();

            Console.WriteLine();

            player.PlayerDraw(usingDeck);
            dealer.DealerDraw(usingDeck);
            player.PlayerDraw(usingDeck);
            dealer.DealerDraw(usingDeck);

            player.Betting();

            //player.PlayerAdd( Deck.DeckInfo.CloverAce);
            //player.PrintPlayerCard(usingDeck);
            //Console.WriteLine(usingDeck.CalCard(player.PlayerCardOnHand, player.PlayerCardState)); 

            player.StartPlayerTurn(usingDeck);

            //dealer.DealerAdd(Deck.DeckInfo.Clover6);
            //dealer.DealerAdd(Deck.DeckInfo.Clover10);

            if(player.PlayerCurrentState != Player.PlayerState.Surrender)
            {
                dealer.StartDealerTurn(usingDeck);
            }

            roundState = IsPlayerWin(player, dealer, roundState, usingDeck);

            Console.WriteLine(roundState);


        }
        

        public enum CardState { Null = 0, Normal = 1, BlackJack, Bust }
        public enum RoundState { Error = 0, Playing, PlayerWin, Push, PlayerLose}

        

        public static RoundState IsPlayerWin(Player inputPlayer, Dealer inputDealer, RoundState inputRoundState, Deck inputDeck)
        {
            if(inputPlayer.PlayerCurrentState == Player.PlayerState.Surrender)
            {
                return RoundState.PlayerLose;
            }

            if(inputRoundState == RoundState.Playing)
            {
                switch (IsBust(inputPlayer, inputDealer))
                {
                    case 1:
                        return RoundState.Push;
                    case 2:
                        return RoundState.PlayerLose;
                    case 3:
                        return RoundState.PlayerWin;
                    case 4:
                        break;
                }
            }

            if (inputRoundState == RoundState.Playing)
            {
                switch(CompareCard(inputPlayer, inputDealer, inputDeck))
                {
                    case 1:
                        return RoundState.PlayerLose;
                    case 2:
                        return RoundState.Push;
                    case 3:
                        break;
                }
            }

            return RoundState.Error;

        }

        public static int CompareCard(Player inputPlayer, Dealer inputDealer, Deck inputDeck)
        {
            if(inputDeck.CalCard(inputPlayer.PlayerCardOnHand, inputPlayer.PlayerCardState) < inputDeck.CalCard(inputDealer.DealerCardOnHand, inputDealer.DealerCardState))
            {
                return 1;
            }
            else if(inputDeck.CalCard(inputPlayer.PlayerCardOnHand, inputPlayer.PlayerCardState) == inputDeck.CalCard(inputDealer.DealerCardOnHand, inputDealer.DealerCardState))
            {
                return 2;
            }
            else { return 3; }
            
        }

        public static int IsBust(Player inputPlayer, Dealer inputDealer)
        {
            if (inputPlayer.PlayerCardState == CardState.Bust && inputDealer.DealerCardState == CardState.Bust)
            {
                return 1;
            }
            else if (inputPlayer.PlayerCardState == CardState.Bust)
            {
                return 2;
            }
            else if (inputDealer.DealerCardState == CardState.Bust)
            {
                return 3;
            }
            else return 4;
        }

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
