using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.Models;
using CardGame.UI;

namespace CardGame
{
    public class Game
    {
        private readonly List<Card> deck;
        private readonly List<Player> players;
        private readonly Random random;

        public Game()
        {
            deck = InitializeDeck();
            players = InitializePlayers();
            random = new Random();
        }

        // InitializeDeck and InitializePlayers
        private List<Card> InitializeDeck()
        {
            var values = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            var symbols = new[] { '@', '#', '^', '*' };

            return values
                .SelectMany(v => symbols.Select(s => new Card(v, s)))
                .ToList();
        }

        private List<Player> InitializePlayers()
        {
            return Enumerable.Range(1, 4)
                .Select(i => new Player($"Player {i}"))
                .ToList();
        }

        public void PlayGame()
        {
            UserInterface.DisplayTitle();

            Console.WriteLine("\nPress any key to start the game...");
            Console.ReadKey(true);

            UserInterface.DisplayAnimation("Shuffling the deck...", 100);
            ShuffleDeck();
            Thread.Sleep(1000);

            // Add dealing animation
            DealingAnimation.AnimateDealing(players, deck.Count);
            DealCards();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
            Console.Clear();
            UserInterface.DisplayTitle();

            foreach (var player in players)
            {
                UserInterface.DisplayPlayerCards(player);
                Console.WriteLine("\nPress any key to see next player's cards...");
                Console.ReadKey(true);
                Console.Clear();
                UserInterface.DisplayTitle();
            }

            var winner = DetermineWinner();

            // Display all players' cards with winner highlighted
            foreach (var player in players)
            {
                UserInterface.DisplayPlayerCards(player, player == winner);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            UserInterface.DisplayAnimation($"\n*** {winner.Name} wins the game! ***", 100);
            Console.ResetColor();

            // Show winning combination
            var winningCards = winner.Cards
                .GroupBy(c => c.Value)
                .OrderByDescending(g => g.Count())
                .ThenByDescending(g => g.Max())
                .First();

            Console.WriteLine("\nWinning combination:");
            Console.WriteLine();
            foreach (var card in winningCards)
            {
                UserInterface.DisplayCard(card);
            }
            Console.WriteLine("\n");

            // Add celebration sequence
            Console.WriteLine("\nPress any key to celebrate the winner!");
            Console.ReadKey(true);
            CelebrationEffects.Celebrate(winner);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

        // Other methods (ShuffleDeck, DealCards, DetermineWinner)
        public void ShuffleDeck()
        {
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (deck[k], deck[n]) = (deck[n], deck[k]);
            }
        }

        public void DealCards()
        {
            for (int i = 0; i < deck.Count; i++)
            {
                players[i % 4].AddCard(deck[i]);
            }
        }

        public Player DetermineWinner()
        {
            var playerScores = players.Select(p =>
            {
                var groupedCards = p.Cards.GroupBy(c => c.Value)
                    .OrderByDescending(g => g.Count())
                    .ThenByDescending(g => g.Max());

                var bestGroup = groupedCards.First();
                return new
                {
                    Player = p,
                    Count = bestGroup.Count(),
                    HighestCard = bestGroup.Max()
                };
            }).OrderByDescending(s => s.Count)
                .ThenByDescending(s => s.HighestCard)
                .ToList();

            return playerScores.First().Player;
        }
    }
}
