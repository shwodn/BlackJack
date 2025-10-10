using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
            Console.CursorVisible = false;





            isPlaying = StartGame();

            while (isPlaying)
            {

                // 라운드 초기화
                resetCard();
                player.ResetRoundPlayer();
                roundState = RoundState.Playing;
                

                // 라운드 표시 화면
                Console.Clear();
                Console.SetCursorPosition(55, 5);
                Console.WriteLine($"Round {round}");
                Thread.Sleep(1000);

                if (usingDeck.CurrentDeck.Count < 6)
                {
                    // 카드 섞는 화면
                    Console.Clear();
                    Console.SetCursorPosition(43, 22);
                    Console.Write("카드가 부족하여 다시 섞습니다");
                    for(int i = 0; i < 3; i++)
                    {
                        Console.Write(". ");
                        Thread.Sleep(1000);
                    }
                    usingDeck.ResetDeck();
                }
                            
                // 게임 메인 화면
                Console.Clear();
                Console.SetCursorPosition(30, 8);
                Console.WriteLine("Player Card");
                Console.SetCursorPosition(80, 8);
                Console.WriteLine("Dealer Card");

                Console.SetCursorPosition(100, 25);
                Console.WriteLine("                 ");
                Console.SetCursorPosition(100, 25);
                Console.Write("보유한 칩 : ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(player.CurrentChip);
                Console.ResetColor();

                Console.SetCursorPosition(100, 27);
                Console.WriteLine("                 ");
                Console.SetCursorPosition(100, 27);
                Console.Write("배팅한 칩 : ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(player.BettedChip);
                Console.ResetColor();

                do
                {
                    resetCard();
                    Console.SetCursorPosition(47, 22);
                    Console.WriteLine("카드를 뽑는 중입니다.");
                    DrawCardFirst(player, dealer, usingDeck);
                } while (player.PlayerCardState == CardState.BlackJack || dealer.DealerCardState == CardState.BlackJack);

                player.Betting();

                PrintBettedChip(player);

                player.StartPlayerTurn(usingDeck);

                PrintBettedChip(player);

                if (player.PlayerCurrentState != Player.PlayerState.Surrender)
                {
                    dealer.StartDealerTurn(usingDeck);
                }
                Thread.Sleep(3000);

                roundState = IsPlayerWin(player, dealer, roundState, usingDeck);

                // 라운드 결과 화면
                Console.Clear();
                Console.SetCursorPosition(55, 5);
                Console.WriteLine(roundState);
                Thread.Sleep(3000);
                CalChip(player, roundState);

                if (player.CurrentChip > 99)
                {
                    // 게임 승리 화면
                    Console.Clear();
                    Console.SetCursorPosition(57, 5);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You Win");
                    Console.ResetColor();
                    Thread.Sleep(3000);
                    isPlaying = false;
                }
                else if (player.CurrentChip < 1)
                {
                    // 게임 패배 화면
                    Console.Clear();
                    Console.SetCursorPosition(55, 5);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You Lose");
                    Console.ResetColor();
                    Thread.Sleep(3000);
                    isPlaying = false;
                }

                round++;
            }
            

        }
        

        public enum CardState { Null = 0, Normal = 1, BlackJack, Bust }
        public enum RoundState { Error = 0, Playing, PlayerWin, Push, PlayerLose}

        public static bool StartGame()
        {
            int temp = 0;

            Console.SetCursorPosition(55, 5);
            Console.WriteLine($"Black Jack");
            Console.SetCursorPosition(30, 20);
            Console.WriteLine($"1. Game Start");
            Console.SetCursorPosition(85, 20);
            Console.WriteLine($"2. Exit");

            temp = PreventInputExceptions(2);

            switch(temp)
            {
                case 1:
                    return true;
                case 2:
                    return false;
            }

            //예상 외 입력시 거짓 반환
            return false;
        }

        public static void PrintBettedChip(Player inputPlayer)
        {
            Console.SetCursorPosition(100, 27);
            Console.WriteLine("                 ");
            Console.SetCursorPosition(100, 27);
            Console.Write("배팅한 칩 : ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(inputPlayer.BettedChip);
            Console.ResetColor();
        }
        public static void DrawCardFirst(Player inputPlayer, Dealer inputDealer, Deck inputDeck)
        {
            inputPlayer.Draw(inputDeck);
            inputDealer.Draw(inputDeck);
            inputPlayer.Draw(inputDeck);
            inputDealer.Draw(inputDeck);
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
            Console.CursorVisible = true;

            while (isException)
            {

                Console.SetCursorPosition(60, 25);
                if (int.TryParse(Console.ReadLine(), out temp))
                {
                    if(temp > 0 && temp < input + 1)
                    {
                        Console.SetCursorPosition(55, 25);
                        Console.WriteLine("                 ");
                        Console.CursorVisible = false;
                        return temp;
                    }
                    else
                    {
                        Console.SetCursorPosition(55, 25);
                        Console.WriteLine("                 ");
                        Console.CursorVisible = false;
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
