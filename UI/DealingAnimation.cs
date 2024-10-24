using CardGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.UI
{
    public class DealingAnimation
    {
        private static readonly string[] CardBack = new[]
        {
            "┌────┐",
            "│████│",
            "│████│",
            "│████│",
            "└────┘"
        };

        // Position for deck of cards (center-right of screen)
        private static int DeckPositionLeft => Console.WindowWidth - 20;
        private static int DeckPositionTop => Console.WindowHeight / 2;

        // Positions for each player's card area
        private static readonly (int left, int top)[] PlayerPositions = new[]
        {
            (Console.WindowWidth / 2, Console.WindowHeight - 8),     // Bottom (Player 1)
            (5, Console.WindowHeight / 2),                           // Left (Player 2)
            (Console.WindowWidth / 2, 5),                           // Top (Player 3)
            (Console.WindowWidth - 15, Console.WindowHeight / 2)     // Right (Player 4)
        };

        public static void AnimateDealing(List<Player> players, int totalCards)
        {
            Console.Clear();
            DrawTable();
            DrawDeck();

            // Track dealt cards for each player
            int cardsPerPlayer = totalCards / 4;
            int cardCount = 0;

            // Dealing animation
            for (int round = 0; round < cardsPerPlayer; round++)
            {
                for (int playerIndex = 0; playerIndex < 4; playerIndex++)
                {
                    AnimateCardToPlayer(playerIndex, cardCount++);
                    PlayDealSound();
                    Thread.Sleep(150); // Delay between deals
                }
            }
        }

        private static void DrawTable()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            // Draw a simple table border
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                for (int j = 0; j < Console.WindowHeight; j++)
                {
                    if (i == 0 || i == Console.WindowWidth - 1 || j == 0 || j == Console.WindowHeight - 1)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write("█");
                    }
                }
            }
            Console.ResetColor();
        }

        private static void DrawDeck()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < CardBack.Length; i++)
            {
                Console.SetCursorPosition(DeckPositionLeft, DeckPositionTop + i);
                Console.Write(CardBack[i]);
            }
            Console.ResetColor();
        }

        private static void AnimateCardToPlayer(int playerIndex, int cardNumber)
        {
            var (targetLeft, targetTop) = PlayerPositions[playerIndex];
            var (startLeft, startTop) = (DeckPositionLeft, DeckPositionTop);

            // Calculate path
            int steps = 10; // Number of animation steps
            double deltaLeft = (targetLeft - startLeft) / (double)steps;
            double deltaTop = (targetTop - startTop) / (double)steps;

            // Animate card movement
            for (int step = 0; step <= steps; step++)
            {
                int currentLeft = (int)(startLeft + deltaLeft * step);
                int currentTop = (int)(startTop + deltaTop * step);

                // Clear previous position if not first step
                if (step > 0)
                {
                    ClearCardPosition(
                        (int)(startLeft + deltaLeft * (step - 1)),
                        (int)(startTop + deltaTop * (step - 1))
                    );
                }

                // Draw card at new position
                DrawCard(currentLeft, currentTop, step == steps);
                Thread.Sleep(20); // Small delay for smooth animation
            }
        }

        private static void DrawCard(int left, int top, bool isFinal)
        {
            try
            {
                Console.ForegroundColor = isFinal ? ConsoleColor.White : ConsoleColor.Blue;
                for (int i = 0; i < CardBack.Length; i++)
                {
                    Console.SetCursorPosition(left, top + i);
                    Console.Write(CardBack[i]);
                }
                Console.ResetColor();
            }
            catch (ArgumentOutOfRangeException)
            {
                // Ignore if position is out of bounds
            }
        }

        private static void ClearCardPosition(int left, int top)
        {
            try
            {
                for (int i = 0; i < CardBack.Length; i++)
                {
                    Console.SetCursorPosition(left, top + i);
                    Console.Write("       "); // Clear card width
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // Ignore if position is out of bounds
            }
        }

        private static void PlayDealSound()
        {
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    Console.Beep(2000, 50); // High-pitched short beep
                }
                catch
                {
                    // Ignore if beep fails
                }
            }
        }
    }
}
