using CardGame.UI;

namespace CardGame
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            do
            {
                Console.Clear();
                var game = new Game();
                game.PlayGame();

                Console.WriteLine("\nWould you like to play again? (Y/N)");
            } while (Console.ReadKey(true).Key == ConsoleKey.Y);

            Console.Clear();
            UserInterface.DisplayAnimation("Thanks for playing! Goodbye!", 100);
            Thread.Sleep(1000);
        }
    }
}
