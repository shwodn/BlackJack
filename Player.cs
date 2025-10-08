using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Player
    {
        public enum PlayerState { Null = 0, Stay = 1, DoubleDown, Hit, Surrender }

        private int currentChip;
        private int bettedChip;
        private int bettedChipDouble;
        private int input = 0;

        private PlayerState playerState;
        private Game.CardState playerCardState;
        private List<Deck.DeckInfo> playerCardOnHand;

        public int CurrentChip { get { return currentChip; } set { currentChip = value; } }
        public int BettedChip { get { return bettedChip; }  set { bettedChip = value; } }
        public int BettedChipDouble { get { return bettedChipDouble; } }
        public int Input { get { return input; } set { input = value; } }


        public Game.CardState PlayerCardState { get { return playerCardState; } }
        public PlayerState PlayerCurrentState { get { return playerState; } }
        

        public List<Deck.DeckInfo> PlayerCardOnHand { get { return playerCardOnHand; } private set { playerCardOnHand = value; } }

        public Player()
        {
            currentChip = 50;
            bettedChip = 0;
            playerCardState = Game.CardState.Null;
            playerCardOnHand = new List<Deck.DeckInfo>();
        }

        public void Betting() 
        {
            Console.SetCursorPosition(30, 22);
            Console.WriteLine("                                                                 ");
            Console.SetCursorPosition(45, 22);
            Console.WriteLine("배팅할 칩의 개수를 정해주세요.");
            Console.SetCursorPosition(60, 25);
            Console.CursorVisible = true;
            if (int.TryParse(Console.ReadLine(), out bettedChip) != true)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(55, 25);
                Console.WriteLine("                 ");
                Betting();
            }

            Console.CursorVisible = false;
            Console.SetCursorPosition(55, 25);
            Console.WriteLine("                 ");

            if (bettedChip > currentChip)
            {
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(45, 22);
                Console.WriteLine("현재 보유한 칩이 부족합니다.");
                Console.SetCursorPosition(40, 23);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(40, 23);
                Console.WriteLine("올인하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(30, 23);
                Console.WriteLine("                                                                 ");
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
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(38, 22);
                Console.WriteLine("현재 배팅한 칩이 보유한 칩의 개수와 같습니다.");
                Console.SetCursorPosition(30, 23);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(40, 23);
                Console.WriteLine("올인하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(30, 23);
                Console.WriteLine("                                                                 ");
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
            Console.SetCursorPosition(30, 22);
            Console.WriteLine("                                                                 ");
            Console.SetCursorPosition(45, 22);
            Console.WriteLine($"현재 배팅한 칩의 개수 : {bettedChip}개");
            Thread.Sleep(1000);
            Console.SetCursorPosition(30, 22);
            Console.WriteLine("                                                                 ");

        }

        public void ResetRoundPlayer()
        {
            BettedChip = 1;
            playerState = PlayerState.Null;
        }

        public void ResetPlayerCard()
        {
            playerCardOnHand.Clear();
            playerCardState = Game.CardState.Null;
        }

        public void PlayerDraw(Deck inputDeck)
        {
            playerCardOnHand.Add(inputDeck.DrawCard());
            PrintPlayerCard(inputDeck);
            playerCardState = Game.UpdateCardState(inputDeck.CalCard(PlayerCardOnHand, playerCardState));
        }

        public void PlayerAdd( Deck.DeckInfo inputCard)
        {
            PlayerCardOnHand.Add(inputCard);
        }

        public void PrintPlayerCard(Deck inputDeck)
        {
            int temp = 0;
            int cardCount = 0;
            foreach ( var input in playerCardOnHand )
            {
                temp = inputDeck.ConvertToNumber(input, playerCardState);
                Console.SetCursorPosition(30, 10 + cardCount);
                Console.WriteLine("                 ");
                Console.SetCursorPosition(30, 10 + cardCount);
                Console.WriteLine($"{input}({temp})");
                cardCount++;
            }
        }

        public void StartPlayerTurn(Deck inputDeck)
        {
            Console.SetCursorPosition(30, 22);
            Console.WriteLine("                                                                 ");
            Console.SetCursorPosition(30, 22);
            Console.WriteLine("현재 플레이어의 턴입니다. 다음 중 원하는 선택지를 입력해주세요.");
            Console.SetCursorPosition(30, 23);
            Console.WriteLine("                                                                 ");
            Console.SetCursorPosition(38, 23);
            Console.WriteLine("1. Stay 2. DoubleDown 3. Hit 4. Surrender");
            input = Game.PreventInputExceptions(4);
            Console.SetCursorPosition(30, 22);
            Console.WriteLine("                                                                 ");
            Console.SetCursorPosition(30, 23);
            Console.WriteLine("                                                                 ");

            switch ((PlayerState)input)
            {
                case PlayerState.Stay:
                    playerState = PlayerState.Stay;
                    break;

                case PlayerState.DoubleDown:
                    DoDoubleDown(inputDeck);
                    break;

                case PlayerState.Hit:
                    DoHit(inputDeck);
                    break;

                case PlayerState.Surrender:
                    playerState = PlayerState.Surrender;
                    break;
            }
        }

        public void DoDoubleDown(Deck inputDeck)
        {
            PlayerDraw(inputDeck);
            bettedChipDouble = bettedChip * 2;
            if (bettedChipDouble > currentChip)
            {
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(45, 22);
                Console.WriteLine("현재 보유한 칩이 부족합니다.");
                Console.SetCursorPosition(40, 23);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(40, 23);
                Console.WriteLine("올인하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(30, 23);
                Console.WriteLine("                                                                 ");
                switch (input)
                {
                    case 1:
                        bettedChip = currentChip;
                        break;
                    case 2:
                        StartPlayerTurn(inputDeck);
                        break;
                }
            }
            else if(bettedChipDouble == currentChip)
            {
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(38, 22);
                Console.WriteLine("현재 배팅한 칩이 보유한 칩의 개수와 같습니다.");
                Console.SetCursorPosition(30, 23);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(40, 23);
                Console.WriteLine("올인하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(30, 23);
                Console.WriteLine("                                                                 ");
                switch (input)
                {
                    case 1:
                        bettedChip = currentChip;
                        break;
                    case 2:
                        StartPlayerTurn(inputDeck);
                        break;
                }
            }
            else
            {
                bettedChip *= 2;
            }

            playerCardState = Game.UpdateCardState(inputDeck.CalCard(PlayerCardOnHand, playerCardState));
            Console.SetCursorPosition(30, 22);
            Console.WriteLine("                                                                 ");
            Console.SetCursorPosition(45, 22);
            Console.WriteLine($"현재 배팅한 칩의 개수 : {bettedChip}개");
            Thread.Sleep(1000);
            Console.SetCursorPosition(30, 22);
            Console.WriteLine("                                                                 ");
        }

        public void DoHit(Deck inputDeck)
        {
            PlayerDraw(inputDeck);
            playerCardState = Game.UpdateCardState(inputDeck.CalCard(PlayerCardOnHand, playerCardState));

            if (playerCardState == Game.CardState.Normal)
            {
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");
                Console.SetCursorPosition(38, 22);
                Console.WriteLine("계속해서 Hit 하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
                Console.SetCursorPosition(30, 22);
                Console.WriteLine("                                                                 ");

                switch (input)
                {
                    case 1:
                        DoHit(inputDeck);
                        break;
                    case 2:
                        break;
                }

            }
        }
    }
}
