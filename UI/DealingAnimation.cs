using CardGame.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CardGame.UI
{
    public class DealingAnimation
    {
        // Position for deck of cards (center of screen)
        private static int DeckPositionLeft => Console.WindowWidth / 2 - 3;
        private static int DeckPositionTop => Console.WindowHeight / 2 - 2;

        // Positions for each player's card area
        private static readonly (int left, int top, string label)[] PlayerPositions = new[]
        {
            (Console.WindowWidth / 2 - 3, Console.WindowHeight - 8, "Player 1"),     // Bottom
            (5, Console.WindowHeight / 2 - 2, "Player 2"),                           // Left
            (Console.WindowWidth / 2 - 3, 5, "Player 3"),                           // Top
            (Console.WindowWidth - 15, Console.WindowHeight / 2 - 2, "Player 4")     // Right
        };

        public static void AnimateDealing(List<Player> players, List<Card> deck)
        {
            Console.Clear();
            DrawTable();
            DrawPlayerLabels();

            int cardsPerPlayer = deck.Count / 4;
            int currentCardIndex = 0;

            // Initial deck display
            DrawDeck(deck[currentCardIndex]);

            // Dealing animation
            for (int round = 0; round < cardsPerPlayer; round++)
            {
                for (int playerIndex = 0; playerIndex < 4; playerIndex++)
                {
                    var currentCard = deck[currentCardIndex];

                    // Animate current card
                    AnimateCardToPlayer(playerIndex, currentCard,
                        () => {
                            // Redraw deck with next card during animation
                            if (currentCardIndex + 1 < deck.Count)
                            {
                                DrawDeck(deck[currentCardIndex + 1]);
                            }
                        });

                    PlayDealSound();
                    Thread.Sleep(50);

                    currentCardIndex++;

                    // If all cards dealt, clear the deck completely
                    if (currentCardIndex >= deck.Count - 1)
                    {
                        ClearDeckCompletely();
                    }
                }
            }
        }

        private static void DrawPlayerLabels()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var (left, top, label) in PlayerPositions)
            {
                // Calculate label position relative to card position
                int labelLeft = left;
                int labelTop = top - 1; // Above the card position

                // Adjust label position based on player position
                if (top == 5) // Top player
                    labelTop = top - 2;
                else if (top == Console.WindowHeight - 8) // Bottom player
                    labelTop = top + 6;

                try
                {
                    Console.SetCursorPosition(labelLeft, labelTop);
                    Console.Write(label);
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
            }
            Console.ResetColor();
        }

        private static void DrawCard(int left, int top, Card card, bool isFinal)
        {
            try
            {
                string[] cardDisplay = new[]
                {
                    "┌────┐",
                    $"│{card.Value,-2}  │",
                    "│    │",
                    $"│  {card.Symbol} │",
                    "└────┘"
                };

                Console.ForegroundColor = isFinal ? GetSymbolColor(card.Symbol) : ConsoleColor.White;
                for (int i = 0; i < cardDisplay.Length; i++)
                {
                    Console.SetCursorPosition(left, top + i);
                    Console.Write(cardDisplay[i]);
                }
                Console.ResetColor();
            }
            catch (ArgumentOutOfRangeException)
            {
                // Ignore if position is out of bounds
            }
        }

        private static ConsoleColor GetSymbolColor(char symbol) => symbol switch
        {
            '@' => ConsoleColor.Cyan,
            '#' => ConsoleColor.Yellow,
            '^' => ConsoleColor.Green,
            '*' => ConsoleColor.Red,
            _ => ConsoleColor.White
        };

        private static void AnimateCardToPlayer(int playerIndex, Card card, Action redrawDeckAction)
        {
            var (targetLeft, targetTop, _) = PlayerPositions[playerIndex];
            var (startLeft, startTop) = (DeckPositionLeft, DeckPositionTop);

            int steps = 5;
            double deltaLeft = (targetLeft - startLeft) / (double)steps;
            double deltaTop = (targetTop - startTop) / (double)steps;

            for (int step = 0; step <= steps; step++)
            {
                int currentLeft = (int)(startLeft + deltaLeft * step);
                int currentTop = (int)(startTop + deltaTop * step);

                if (step > 0)
                {
                    ClearCardPosition(
                        (int)(startLeft + deltaLeft * (step - 1)),
                        (int)(startTop + deltaTop * (step - 1))
                    );
                }

                // Redraw deck first (below the moving card)
                redrawDeckAction();

                // Then draw the moving card
                DrawCard(currentLeft, currentTop, card, step == steps);
                Thread.Sleep(20);
            }
        }

        private static void DrawTable()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
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

        private static void DrawDeck(Card topCard)
        {
            // Draw deck thickness (3 cards behind)
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 2; i >= 0; i--)
            {
                for (int line = 0; line < 5; line++)
                {
                    Console.SetCursorPosition(DeckPositionLeft + i, DeckPositionTop + line);
                    Console.Write(new string[] {
                    "┌────┐",
                    "│████│",
                    "│████│",
                    "│████│",
                    "└────┘"
                }[line]);
                }
            }
            Console.ResetColor();

            // Draw the top card
            DrawCard(DeckPositionLeft, DeckPositionTop, topCard, false);
        }

        private static void ClearDeckCompletely()
        {
            // Clear main deck area and any potential shadows
            for (int offset = 2; offset >= 0; offset--)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.SetCursorPosition(DeckPositionLeft + offset, DeckPositionTop + i);
                    Console.Write("       "); // Clear card width
                }
            }
        }

        private static void ClearCardPosition(int left, int top)
        {
            try
            {
                for (int i = 0; i < 5; i++)
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
                    Console.Beep(2000, 25); // Shorter beep duration
                }
                catch
                {
                    // Ignore if beep fails
                }
            }
        }
    }
}