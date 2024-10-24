using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using CardGame.Models;

namespace CardGame.UI
{
    public class UserInterface
    {
        private static readonly Dictionary<char, ConsoleColor> SymbolColors = new()
        {
            {'@', ConsoleColor.Cyan},
            {'#', ConsoleColor.Yellow},
            {'^', ConsoleColor.Green},
            {'*', ConsoleColor.Red}
        };

        private const int CARD_WIDTH = 7;  // Width of card including borders
        private const int CARD_HEIGHT = 5; // Height of card including borders
        private const int CARD_OVERLAP = 3; // How much cards overlap
        private const int LEFT_MARGIN = 2;  // Left margin for card display

        public static void DisplayTitle()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
╔═══════════════════════════════════════════════╗
║                 CARD GAME                     ║
║             Four Players Edition              ║
╚═══════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public static void DisplayCard(Card card, int xOffset = 0)
        {
            try
            {
                int left = LEFT_MARGIN + xOffset;
                int top = Console.CursorTop;

                // Safety check for console boundaries
                if (left + CARD_WIDTH > Console.WindowWidth)
                {
                    return; // Skip drawing if it would exceed console width
                }

                Console.SetCursorPosition(left, top);
                Console.Write("┌────┐");
                Console.SetCursorPosition(left, top + 1);
                Console.Write($"│{card.Value,-2}  │");
                Console.SetCursorPosition(left, top + 2);
                Console.Write("│    │");
                Console.SetCursorPosition(left, top + 3);
                Console.Write("│  ");
                Console.ForegroundColor = SymbolColors[card.Symbol];
                Console.Write(card.Symbol);
                Console.ResetColor();
                Console.Write(" │");
                Console.SetCursorPosition(left, top + 4);
                Console.Write("└────┘");
            }
            catch (ArgumentOutOfRangeException)
            {
                // Fallback to simple card display if positioning fails
                Console.Write($"{card.Value}{card.Symbol} ");
            }
        }

        public static void DisplayPlayerCards(Player player, bool isWinner = false)
        {
            Console.WriteLine();
            Console.Write(isWinner ? ">>> " : "   ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{player.Name}'s cards:");
            Console.ResetColor();
            Console.WriteLine();

            var cards = player.Cards.OrderBy(c => c.Value).ThenBy(c => c.Symbol).ToList();
            int availableWidth = Console.WindowWidth - LEFT_MARGIN;
            int cardsPerRow = Math.Max(1, (availableWidth - CARD_WIDTH) / (CARD_WIDTH - CARD_OVERLAP) + 1);

            for (int row = 0; row * cardsPerRow < cards.Count; row++)
            {
                // For each line of the card (cards are 5 lines tall)
                for (int cardLine = 0; cardLine < CARD_HEIGHT; cardLine++)
                {
                    Console.CursorLeft = LEFT_MARGIN;

                    // Draw the same line for each card in this row
                    for (int cardIndex = row * cardsPerRow;
                         cardIndex < Math.Min((row + 1) * cardsPerRow, cards.Count);
                         cardIndex++)
                    {
                        Card card = cards[cardIndex];
                        DrawCardLine(card, cardLine);

                        // Move cursor to position for next card with overlap
                        Console.CursorLeft = LEFT_MARGIN + (cardIndex % cardsPerRow + 1) * (CARD_WIDTH - CARD_OVERLAP);
                    }
                    Console.WriteLine(); // Move to next line
                }
                Console.WriteLine(); // Add space between rows of cards
            }
        }

        private static void DrawCardLine(Card card, int lineNumber)
        {
            switch (lineNumber)
            {
                case 0:
                    Console.Write("┌────┐");
                    break;
                case 1:
                    Console.Write($"│{card.Value,-2}  │");
                    break;
                case 2:
                    Console.Write("│    │");
                    break;
                case 3:
                    Console.Write("│  ");
                    Console.ForegroundColor = SymbolColors[card.Symbol];
                    Console.Write(card.Symbol);
                    Console.ResetColor();
                    Console.Write(" │");
                    break;
                case 4:
                    Console.Write("└────┘");
                    break;
            }
        }

        public static void DisplayAnimation(string text, int delay = 50)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }
}
