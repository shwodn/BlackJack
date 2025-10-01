using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjectBlackJack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Deck usingDeck = new Deck();
            usingDeck.resetDeck();
            usingDeck.printDeck();
        }
    }
}
