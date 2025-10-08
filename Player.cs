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
            foreach( var input in playerCardOnHand )
            {
                temp = inputDeck.ConvertToNumber(input, playerCardState);
                Console.WriteLine($"{input}({temp})");
            }
        }

        public void StartPlayerTurn(Deck inputDeck)
        {
            Console.WriteLine("현재 플레이어의 턴입니다. 다음 중 원하는 선택지를 입력해주세요. \n1. Stay 2. DoubleDown 3. Hit 4. Surrender");
            input = Game.PreventInputExceptions(4);

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
                Console.WriteLine("현재 보유한 칩이 부족합니다. \n올인하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
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
                Console.WriteLine("현재 배팅한 칩이 보유한 칩의 개수와 같습니다. \n올인하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
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
            Console.WriteLine($"현재 배팅한 칩의 개수 : {bettedChip}개");
        }

        public void DoHit(Deck inputDeck)
        {
            PlayerDraw(inputDeck);
            playerCardState = Game.UpdateCardState(inputDeck.CalCard(PlayerCardOnHand, playerCardState));

            if (playerCardState == Game.CardState.Normal)
            {
                Console.WriteLine("계속해서 Hit 하시겠습니까?( 1. 예, 2. 아니오)");
                input = Game.PreventInputExceptions(2);
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
