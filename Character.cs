using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleProjectBlackJack.Player;

namespace ConsoleProjectBlackJack 
{
    internal class Character
    {
        private Game.CardState characterCardState;

        private List<Deck.DeckInfo> characterCardOnHand;

        public Game.CardState CharacterCardState { get { return characterCardState; } }

        public List<Deck.DeckInfo> CharacterCardOnHand { get { return characterCardOnHand; } private set { characterCardOnHand = value; } }
    }

    interface IDraw
    {
        void Draw(Deck inputDeck);
    }
}
