using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Game
    {
        // 메인 문 아래 작성한 코드도 있으니 참고 부탁드립니다

        static void Main(string[] args)
        {
            int round = 1;
            bool isPlaying = false;

            Player player = new Player();
            Dealer dealer = new Dealer();
            Deck usingDeck = new Deck();
            RoundState roundState = new RoundState();
            roundState = RoundState.Playing;
            Action resetCard = player.ResetPlayerCard;
            resetCard += dealer.ResetDealerCard;

            isPlaying = true;

            while (isPlaying)
            {
                if(usingDeck.CurrentDeck.Count < 6)
                {
                    Console.WriteLine("카드가 부족하여 다시 섞습니다.");
                    usingDeck.ResetDeck();
                }

                // 라운드 초기화
                resetCard();
                player.ResetRoundPlayer();
                roundState = RoundState.Playing;

                Console.WriteLine($"Round {round}");

                do
                {
                    resetCard();
                    Console.WriteLine("카드를 뽑는 중입니다.");
                    DrawCardFirst(player, dealer, usingDeck);
                } while (player.PlayerCardState == CardState.BlackJack || dealer.DealerCardState == CardState.BlackJack);

                player.Betting();

                player.StartPlayerTurn(usingDeck);

                if (player.PlayerCurrentState != Player.PlayerState.Surrender)
                {
                    dealer.StartDealerTurn(usingDeck);
                }

                roundState = IsPlayerWin(player, dealer, roundState, usingDeck);

                Console.WriteLine(roundState);
                CalChip(player, roundState);

                Console.WriteLine(player.CurrentChip);

                if (player.CurrentChip > 99)
                {
                    Console.WriteLine("Player Win");
                    isPlaying = false;
                }
                else if (player.CurrentChip < 1)
                {
                    Console.WriteLine("Player Lose");
                    isPlaying = false;
                }

                round++;
            }
            

        }
        

        public enum CardState { Null = 0, Normal = 1, BlackJack, Bust }
        public enum RoundState { Error = 0, Playing, PlayerWin, Push, PlayerLose}

        public static void DrawCardFirst(Player inputPlayer, Dealer inputDealer, Deck inputDeck)
        {
            inputPlayer.PlayerDraw(inputDeck);
            inputDealer.DealerDraw(inputDeck);
            inputPlayer.PlayerDraw(inputDeck);
            inputDealer.DealerDraw(inputDeck);

           
        }

        public static void CalChip(Player inputPlayer, RoundState inputRoundState)
        {

            if (inputRoundState == RoundState.PlayerLose && inputPlayer.PlayerCurrentState == Player.PlayerState.Surrender)
            {
                inputPlayer.CurrentChip -= inputPlayer.BettedChip / 2;
            }
            else if(inputRoundState == RoundState.PlayerLose)
            {
                inputPlayer.CurrentChip -= inputPlayer.BettedChip;
            }
            else if(inputRoundState == RoundState.PlayerWin && inputPlayer.PlayerCardState == CardState.BlackJack)
            {
                inputPlayer.CurrentChip += inputPlayer.BettedChip * 2;
            }
            else if(inputRoundState == RoundState.PlayerWin)
            {
                inputPlayer.CurrentChip += inputPlayer.BettedChip;
            }
        }

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
                        return RoundState.PlayerWin;
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

        // 예외 방지 매서드(1부터 인자로 받은 숫자이하를 제외한 나머지는 예외로 간주)
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
