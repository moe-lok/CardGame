using CardGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.UI
{
    public class CelebrationEffects
    {
        private static readonly string[] FireworkFrames = new string[]
        {
            @"       ",
            @"   .   ",
            @"   *   ",
            @" . * . ",
            @"* * * *",
            @" \'*'/ ",
            @"  \|/  ",
            @"   |   "
        };

        private static readonly string[] TrophyArt = new string[]
        {
            @"   ___________   ",
            @"  '._==_==_=_.'  ",
            @"  .-\:      /-.  ",
            @" | (|:.     |) | ",
            @"  '-|:.     |-'  ",
            @"    \::.    /    ",
            @"     '::. .'     ",
            @"       ) (       ",
            @"     _.' '._     ",
            @"    '-------'    "
        };

        public static void Celebrate(Player winner)
        {
            Console.Clear();
            DisplayFireworks();
            DisplayWinnerBanner(winner);
            DisplayTrophy();
            PlayVictoryTune();
        }

        private static void DisplayFireworks()
        {
            // Display multiple fireworks at different positions
            for (int i = 0; i < 3; i++)
            {
                int leftPosition = Random.Shared.Next(10, Console.WindowWidth - 20);
                AnimateFirework(leftPosition);
            }
        }

        private static void AnimateFirework(int leftPosition)
        {
            int topPosition = Console.WindowHeight - FireworkFrames.Length - 1;

            foreach (string frame in FireworkFrames)
            {
                try
                {
                    Console.SetCursorPosition(leftPosition, topPosition);
                    Console.ForegroundColor = (ConsoleColor)Random.Shared.Next(1, 15); // Random color except black
                    Console.Write(frame);
                    Thread.Sleep(100);
                    topPosition--;
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Skip if position is out of console bounds
                    continue;
                }
            }
            Console.ResetColor();
        }

        private static void DisplayWinnerBanner(Player winner)
        {
            string message = $" {winner.Name} WINS! ";
            string border = new string('=', message.Length);

            // Slide in animation
            for (int i = -message.Length; i < (Console.WindowWidth - message.Length) / 2; i++)
            {
                Console.Clear();
                if (i >= 0)
                {
                    Console.SetCursorPosition(i, Console.WindowHeight / 2 - 1);
                    Console.WriteLine(border);
                    Console.SetCursorPosition(i, Console.WindowHeight / 2);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(message);
                    Console.ResetColor();
                    Console.SetCursorPosition(i, Console.WindowHeight / 2 + 1);
                    Console.WriteLine(border);
                }
                Thread.Sleep(20);
            }
        }

        private static void DisplayTrophy()
        {
            int leftPosition = (Console.WindowWidth - TrophyArt[0].Length) / 2;
            int topPosition = (Console.WindowHeight - TrophyArt.Length) / 2 + 3;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (string line in TrophyArt)
            {
                try
                {
                    Console.SetCursorPosition(leftPosition, topPosition++);
                    Console.WriteLine(line);
                    Thread.Sleep(100);
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
            }
            Console.ResetColor();
        }

        private static void PlayVictoryTune()
        {
            if (OperatingSystem.IsWindows())
            {
                int[] notes = { 523, 523, 523, 659, 784 }; // C5, C5, C5, E5, G5
                int[] duration = { 100, 100, 100, 300, 500 };

                for (int i = 0; i < notes.Length; i++)
                {
                    Console.Beep(notes[i], duration[i]);
                }
            }
        }
    }
}
